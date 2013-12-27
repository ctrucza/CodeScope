using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;

namespace Churn
{
    class Program
    {
        private static ChurnContainer db;
        private static Project triangles;
        private static VersionControlServer versionControl;

        static void Main(string[] args)
        {

            //LoadTriangles();
            CalcDiff();
        }

        private static void CalcDiff()
        {
            DiffOptions diffOptions = new DiffOptions
            {
                UseThirdPartyTool = false, 
                OutputType = DiffOutputType.Binary
            };

            db = new ChurnContainer();

            foreach (Item item in db.Items.Where(item=>item.Project.Id == 7).ToList())
            {
                Console.WriteLine(item.ServerItem);
                if (item.Versions.First().Encoding != 65001)
                    continue;

                string previous = "";

                foreach (ItemVersion version in item.Versions.OrderBy(v=>v.Version))
                {
                    Console.WriteLine(version.Version);
                    string current = version.Stream;

                    Stream previousStream = StringToStream(previous);
                    Stream currentStream = StringToStream(current);

                    DiffSummary summary = DiffUtil.Diff(previousStream, Encoding.UTF8, currentStream, Encoding.UTF8, diffOptions, true);
                    //Console.Write("{0} +{1} ~{2} -{3} {4}",
                    //    summary.OriginalLineCount,
                    //    summary.TotalLinesAdded,
                    //    summary.TotalLinesModified,
                    //    summary.TotalLinesDeleted,
                    //    summary.ModifiedLineCount);

                    Change change = db.Changes.Single(c => c.Changeset.ChangesetId == version.Version && c.ItemVersion.Item.Id == version.Item.Id);
                    change.DiffStats.InitialLineCount = summary.OriginalLineCount;
                    change.DiffStats.Added = summary.TotalLinesAdded;
                    change.DiffStats.Modified = summary.TotalLinesModified;
                    change.DiffStats.Deleted = summary.TotalLinesDeleted;
                    change.DiffStats.FinalLineCount = summary.ModifiedLineCount;

                    db.SaveChanges();

                    previous = current;
                }
            }
        }

        private static Stream StringToStream(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private static void HackItem(ItemVersion version)
        {
            
        }

        private static void HackChangeset(long changesetId)
        {
            Microsoft.TeamFoundation.VersionControl.Client.Changeset changeset = versionControl.GetChangeset((int)changesetId);
            Changeset c = triangles.Changesets.Single(cs => cs.ChangesetId == changesetId);
            Console.WriteLine("{0} ({1}) {2}", 
                changeset.CreationDate, 
                changeset.Changes.Count(),
                changeset.CommitterDisplayName);
            foreach (Microsoft.TeamFoundation.VersionControl.Client.Change change in changeset.Changes)
            {
                Item item = triangles.Items.SingleOrDefault(i => i.ServerItem == change.Item.ServerItem);
                if (item == null)
                {
                    item = new Item
                    {
                        ServerItem = change.Item.ServerItem,
                        ItemType = change.Item.ItemType
                    };
                    triangles.Items.Add(item);
                }

                Change ch = new Change
                {
                    ChangeType = change.ChangeType
                };

                c.Changes.Add(ch);

                ItemVersion version = new ItemVersion
                {
                    Version = change.Item.ChangesetId,
                    Stream = "",
                    Encoding = change.Item.Encoding
                };
                item.Versions.Add(version);

                if (item.ItemType == ItemType.File)
                {
                    Console.WriteLine("reading {0}", item.ServerItem);
                    Stream currentStream = change.Item.DownloadFile();
                    StreamReader reader = new StreamReader(currentStream);
                    version.Stream = reader.ReadToEnd();
                }

                ch.ItemVersion = version;


                DiffStat diff2 = new DiffStat();
                diff2.InitialLineCount = 42;
                diff2.Added = 20;
                diff2.Modified = 10;
                diff2.Deleted = 10;
                diff2.FinalLineCount = 52;

                ch.DiffStats = diff2;

                HackItem(version);

                //Microsoft.TeamFoundation.VersionControl.Client.Item previousVersion = items[key];
                //Microsoft.TeamFoundation.VersionControl.Client.Item currentVersion = change.Item;
                //Stream previousStream = streams[key];

                //DiffSummary summary = DiffUtil.Diff(previousStream, Encoding.ASCII, currentStream, Encoding.ASCII, diffOptions, true);
                //Console.Write("{0} +{1} ~{2} -{3} {4}",
                //    summary.OriginalLineCount,
                //    summary.TotalLinesAdded,
                //    summary.TotalLinesModified,
                //    summary.TotalLinesDeleted,
                //    summary.ModifiedLineCount);

                //if (!diff.ContainsKey(key))
                //{
                //    diff[key] = new Dictionary<int, DiffSummary>();
                //}
                //diff[key][currentVersion.ChangesetId] = summary;
            
            
            }

            db.SaveChanges();
        }

        private static void LoadTriangles()
        {
            OpenTfs();

            db = new ChurnContainer();
            triangles = new Project();
            triangles.Path = "$/triangles";
            db.Projects.Add(triangles);
            db.SaveChanges();

            Console.WriteLine("reading history...");
            IEnumerable history = versionControl.QueryHistory("$/triangles/src/triangles",
                VersionSpec.Latest,
                0,
                RecursionType.Full,
                null,
                new ChangesetVersionSpec(1),
                VersionSpec.Latest,
                int.MaxValue,
                true,
                true);
            Console.WriteLine("processing history...");
            Changeset c;
            foreach (Microsoft.TeamFoundation.VersionControl.Client.Changeset changeset in history)
            {
                c = new Changeset
                {
                    ChangesetId = changeset.ChangesetId,
                    Status = ChangesetStatus.New,
                    CreationDate = changeset.CreationDate,
                    CommitterDisplayName = changeset.CommitterDisplayName,
                    Comment = changeset.Comment
                };
                triangles.Changesets.Add(c);
            }
            Console.WriteLine("{0} changesets processed", triangles.Changesets.Count);
            db.SaveChanges();
            Console.WriteLine("saved changesets");

            foreach (var changeset in triangles.Changesets.OrderBy(cs=>cs.CreationDate))
            {
                HackChangeset(changeset.ChangesetId);
            }
            return;

            //changesets.Reverse();



            Dictionary<string, Microsoft.TeamFoundation.VersionControl.Client.Item> items = new Dictionary<string, Microsoft.TeamFoundation.VersionControl.Client.Item>();
            Dictionary<string, Stream> streams = new Dictionary<string, Stream>();
            Dictionary<string, Dictionary<int, DiffSummary>> diff = new Dictionary<string, Dictionary<int, DiffSummary>>();

            DiffOptions diffOptions = new DiffOptions();
            diffOptions.UseThirdPartyTool = false;
            diffOptions.OutputType = DiffOutputType.Binary;

            //foreach (Microsoft.TeamFoundation.VersionControl.Client.Changeset changeset in changesets)
            //{

            //    Console.WriteLine("{0}\t{1}\t{2}\t{3}",
            //        changeset.ChangesetId,
            //        changeset.CreationDate,
            //        changeset.CommitterDisplayName.Substring(10, changeset.CommitterDisplayName.Length - 10),
            //        changeset.Comment
            //        );

            //    foreach (Microsoft.TeamFoundation.VersionControl.Client.Change change in changeset.Changes)
            //    {
            //        Item item = pios.Items.SingleOrDefault(i => i.ServerItem == change.Item.ServerItem);
            //        if (item == null)
            //        {
            //            item = new Item();
            //            item.ServerItem = change.Item.ServerItem;
            //            pios.Items.Add(item);
            //        }
            //        db.SaveChanges();

            //        ItemVersion version = new ItemVersion();
            //        version.Version = 42;
            //        version.Stream = "";
            //        item.Versions.Add(version);

            //        Change ch = new Change();
            //        ch.ChangeType = change.ChangeType;
            //        ch.ItemVersion = version;
            //        //ch.ItemType = ItemType.File;
            //        c.Changes.Add(ch);

            //        DiffStat diff2 = new DiffStat();
            //        diff2.InitialLineCount = 42;
            //        diff2.Added = 20;
            //        diff2.Modified = 10;
            //        diff2.Deleted = 10;
            //        diff2.FinalLineCount = 52;

            //        ch.DiffStats = diff2;

            //        db.SaveChanges();

            //        //if (change.Item.ItemType != Microsoft.TeamFoundation.VersionControl.Client.ItemType.File)
            //        //    continue;
            //        //string key = change.Item.ServerItem;
            //        //Console.Write("{0}\t{1}\t", change.ChangeType, key);
            //        //if (items.ContainsKey(key))
            //        //{
            //        //    Microsoft.TeamFoundation.VersionControl.Client.Item previousVersion = items[key];
            //        //    Microsoft.TeamFoundation.VersionControl.Client.Item currentVersion = change.Item;
            //        //    Stream previousStream = streams[key];
            //        //    Stream currentStream = currentVersion.DownloadFile();

            //        //    DiffSummary summary = DiffUtil.Diff(previousStream, Encoding.ASCII, currentStream, Encoding.ASCII, diffOptions, true);
            //        //    Console.Write("{0} +{1} ~{2} -{3} {4}",
            //        //        summary.OriginalLineCount,
            //        //        summary.TotalLinesAdded,
            //        //        summary.TotalLinesModified,
            //        //        summary.TotalLinesDeleted,
            //        //        summary.ModifiedLineCount);

            //        //    if (!diff.ContainsKey(key))
            //        //    {
            //        //        diff[key] = new Dictionary<int, DiffSummary>();
            //        //    }
            //        //    diff[key][currentVersion.ChangesetId] = summary;
            //        //}
            //        //items[key] = change.Item;
            //        //streams[key] = items[key].DownloadFile();
            //        //Console.WriteLine();

            //        //else
            //        //{
            //        //    items.Add(change.Item.ServerItem, change.Item);
            //        //}

            //        /*
            //        IDiffItem source = new DiffItemVersionedFile(change.Item, VersionSpec);
            //        Difference.VisualDiffItems(versionControl, source, target);
            //        */
            //    }
            //}
        }

        private static void OpenTfs()
        {
            Console.Write("username: ");
            string username = Console.ReadLine();
            Console.Write("password: ");
            string password = Console.ReadLine();

            NetworkCredential netCred = new NetworkCredential(username, password);
            BasicAuthCredential basicCred = new BasicAuthCredential(netCred);
            TfsClientCredentials tfsCred = new TfsClientCredentials(basicCred);
            tfsCred.AllowInteractive = false;

            TfsTeamProjectCollection tpc =
                new TfsTeamProjectCollection(new Uri("https://ctrucza.visualstudio.com/DefaultCollection"), tfsCred);
            tpc.Authenticate();
            versionControl = tpc.GetService<VersionControlServer>();
        }
    }
}
