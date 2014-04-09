select top 10 * from Items

select * from Changesets order by ChangesetId 


select
    Item_Id,
    max(Version) as Version
from 
    ItemVersions 
where 
    ItemVersions.Version <= 145230
group by
    Item_Id



create view ChangesetSnapshotsCSharp as
select 
    Ch.ChangeSetID,
    IV.Item_ID,
    max(IV.Version) AS Version
--INTO #T
from Changesets Ch
    inner join ItemVersions IV ON IV.Version <= Ch.ChangeSetID 
    inner join Items on Items.Id = IV.Item_Id and Items.ServerItem like '%.cs'
group by Ch.ChangeSetID, IV.Item_ID

--order by Ch.ChangeSetID, items.ServerItem, IV.Item_ID

--SELECT *
--FROM #T
--ORDER BY ChangeSetID, ServerItem

select
    Changesets.ChangesetId,
    count(*)
from
    Changesets 
    inner join ChangesetSnapshotsCSharp on ChangesetSnapshotsCSHarp.ChangesetId = Changesets.ChangesetId
group by
    Changesets.ChangesetId
order by Changesets.ChangesetId


select * from ChangesetSnapshots where ChangesetID = 14523

14523
14525
14526
14529
14530
14536
14538
14539
14542