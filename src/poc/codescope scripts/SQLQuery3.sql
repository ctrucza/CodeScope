select * from Changesets

--select * from ItemVersions

select 
    * 
from 
    Items
    join LatestVersions on LatestVersions.Item_Id = Items.Id


select top 10
    *
from
    ItemVersions


select * from LatestVersions



select top 100
    ChangesetId,
    Comment,
    ServerItem,
    ItemType,
    Stream,
    * 
from 
    Changesets
    join ItemVersions on ItemVersions.Version = Changesets.ChangesetId
    join Items on Items.Id = ItemVersions.Item_Id





select Encoding, count(*) from ItemVersions group by Encoding

select top 10 * from Items join ItemVersions on ItemVersions.Item_Id = Items.Id where Encoding = -1
select top 10 * from Items join ItemVersions on ItemVersions.Item_Id = Items.Id where Encoding = -3
select top 10 * from Items join ItemVersions on ItemVersions.Item_Id = Items.Id where Encoding = 1200
select top 10 * from Items join ItemVersions on ItemVersions.Item_Id = Items.Id where Encoding = 1252
