Reading from TFS
================

Connect to TFS
---

In order to use the TFS services you need the Microsoft assemblies which give you a client object model.

**Note**: there seems to be a problem with the nuget package: if your project's Target Framework is set to 4.5 (or 4.5.1) not all assemblies are added to the project.
You can set your target framework to 4 before installing or you can add manually the missing assemblies from the packages\Microsoft.TeamFoundation.11.0.0.0\lib\net20 folder.

To get the necessary libs in your project:

            PM> install-package Microsoft.TeamFoundation

[TeamFoundation on nuget](https://www.nuget.org/packages/Microsoft.TeamFoundation)

To connect:
```
            using Microsoft.TeamFoundation.Client;

```
```
            Uri uri = new Uri("https://ctrucza.visualstudio.com/DefaultCollection");
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(uri);
            tfs.Authenticate();
```
Get the Version Control Server
---
```
            using Microsoft.TeamFoundation.VersionControl.Client;
```
```
            VersionControlServer vcs = tfs.GetService<VersionControlServer>();
```
Read the Project History
---
```
            QueryHistoryParameters parameters = new QueryHistoryParameters("$/book", RecursionType.Full);
            parameters.IncludeChanges = true;
            IEnumerable<Changeset> history = vcs.QueryHistory(parameters);
```
[Read here more about QueryHistoryParameters](http://msdn.microsoft.com/en-us/library/microsoft.teamfoundation.versioncontrol.client.queryhistoryparameters.aspx)

`Changeset`s and `Change`s
---
```
            foreach (Changeset changeset in history)
            {
                Console.WriteLine(changeset.ChangesetId + "\t" + changeset.Committer + "\t" + changeset.Comment);
                foreach (Change change in changeset.Changes)
                {
                    Console.WriteLine("\t" + change.Item.ItemId + "\t" + change.Item.ItemType + "\t" + change.Item.ServerItem);
                }
            }
```


Reading
---

- [MSDN: Connect to Team Foundation Server from a Console Application](http://msdn.microsoft.com/en-us/library/bb286958.aspx)
- [Introducing the TfsConnection, TfsConfigurationServer and TfsTeamProjectCollection Classes](http://blogs.msdn.com/b/taylaf/archive/2010/02/23/introducing-the-tfsconnection-tfsconfigurationserver-and-tfsteamprojectcollection-classes.aspx)
- [Using TFS Impersonation with the Version Control Client APIs](http://blogs.msdn.com/b/taylaf/archive/2010/03/29/using-tfs-impersonation-with-the-version-control-client-apis.aspx)
