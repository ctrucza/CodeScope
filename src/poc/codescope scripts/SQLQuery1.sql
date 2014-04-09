--select * from Changesets order by CreationDate 
select 
    ServerItem,
    Version,
    Stream 
from 
    Items 
    join ItemVersions on ItemVersions.Item_Id = Items.Id
where 
    ItemType = 2 and 
    ServerItem like '%Domain/Document.cs'
order by
    ServerItem, Version


Select * from Changes

select count(*) from Items
select count(*) from ItemVersions


select
    ServerItem,
    count(*)
from 
    Items 
    join ItemVersions on ItemVersions.Item_Id = Items.Id
where 
    ItemType = 2 
    and ServerItem like '%.cs'
    and ServerItem not like '%tools%'
    and ServerItem not like '%Test%'
group by
    ServerItem
order by
    count(*) desc
