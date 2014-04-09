select 
    items.ServerItem,
    sum(Diffstats.Added) as A,
    sum(DiffStats.Modified) as M,
    sum(DiffStats.Deleted) as D,
    count(*) as ChangeCount
into U#
from 
    Items
--    join LatestVersions on LatestVersions.Item_Id = Items.ID
    join ItemVersions on ItemVersions.Item_Id = Items.id --and ItemVersions.Version = latestversions.Version
    join Changes on Changes.ItemVersion_Id = ItemVersions.Id
    join DiffStats on Changes.DiffStats_Id = DiffStats.Id
group by items.ServerItem



select 
--    sum(FinalLineCount)
    * 
from 
    T# 
where 
    ServerItem like '%.cs' 
    and ServerItem not like '%tools%'
    and ServerItem like '%Roche-CSSP/src/Roche.PIOS/%'
    and ServerItem not like '%Test%'
    and ServerItem not like '%ODataService%'
    and ServerItem not like '%Reference%'
    and ServerItem not like '%Assembly%'
--order by Finallinecount desc
--order by serveritem


drop table T#
select 
    ServerItem,
    len(Stream)-len(replace(Stream, char(13), '')) as linecount--,
--    * 
into T#
from LatestVersions
inner join Items on Items.Id = LatestVersions.Item_Id
inner join ItemVersions on ItemVersions.Item_Id = latestversions.Item_id and ItemVersions.Version = LatestVersions.Version
where Encoding = 65001
    and ServerItem like '%.cs' 
    and ServerItem not like '%tools%'
    and ServerItem like '%Roche-CSSP/src/Roche.PIOS/%'
    and ServerItem not like '%Test%'
    and ServerItem not like '%ODataService%'
    and ServerItem not like '%Reference%'
    and ServerItem not like '%Assembly%'


select * from ItemVersions



select * from T# order by linecount desc




select T#.ServerItem, T#.LineCount, U#.A, U#.D, U#.M, U#.ChangeCount from U# inner join T# on U#.ServerItem = T#.ServerItem
select * from T#