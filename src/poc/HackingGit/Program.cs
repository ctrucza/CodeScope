using System;
using System.Linq;
using LibGit2Sharp;

namespace HackingGitHub
{
    class Program
    {
        static Repository repo;
        static void Main(string[] args)
        {
            repo = new Repository(@"C:\Users\csaba.trucza\Documents\Projects\programming school\projects\Yahtzee");
            foreach (Commit commit in repo.Commits)
            {
                DateTime commitDate = commit.Committer.When.UtcDateTime;
                Console.WriteLine(
                    "{0}\t{1}\t{2}\t{3}",
                    commit.Id,
                    commit.Author,
                    commit.Tree.Count,
                    commit.Message.TrimEnd());

                if (commit.Parents.Count() == 1)
                {
                    Commit parent = commit.Parents.Single();

                    TreeChanges changes = repo.Diff.Compare(parent.Tree, commit.Tree);
                    foreach (TreeEntryChanges treeEntryChanges in changes)
                    {
                        TreeEntry entry = commit.Tree.SingleOrDefault(t => t.Target.Id == treeEntryChanges.Oid);
                        if (entry != null)
                        {
                            Console.WriteLine(
                                "{0}\t{1}\t{2}\t{3}",
                                entry.TargetType,
                                entry.Target,
                                entry.Name,
                                entry.Path);
                        }
                        //Console.WriteLine(
                        //    "{0}\t{1}\t{2}",
                        //    treeEntryChanges.Oid,
                        //    treeEntryChanges.Status,
                        //    treeEntryChanges.Path);
                    }
                    Console.WriteLine();
                }
                else
                {
                    DumpTree(commit.Tree, "");
                }
            }
        }

        private static void DumpTree(Tree tree, string path)
        {
            foreach (TreeEntry entry in tree)
            {
                Console.WriteLine(
                    "{0}\t{1}\t{2}\t{3}",
                    entry.TargetType,
                    entry.Target,
                    entry.Name,
                    path + @"\" + entry.Path);

                Console.WriteLine(entry.Target);
                if (entry.TargetType == TreeEntryTargetType.Blob)
                {
                    Blob blob = repo.Lookup<Blob>(entry.Target.Id);
                    string content = blob.ContentAsText();
                    //Console.WriteLine(content);
                }
                else if (entry.TargetType == TreeEntryTargetType.Tree)
                {
                    Tree thisTree = repo.Lookup<Tree>(entry.Target.Id);
                    DumpTree(thisTree, path + @"\" + entry.Path);
                }
            }
        }
    }
}
