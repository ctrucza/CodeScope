select 
    Changesets.CreationDate,
    --Changesets.Id,
    --count(*)
    ----Changesets.Id,
    sum(DiffStats.InitialLineCount) ILOC,
    sum(DiffStats.Added) Added,
    sum(DiffStats.Modified) Modified,
    sum(DiffStats.Deleted) Deleted,
    sum(DiffStats.FinalLineCount) FLOC
    --*
from 
    Changesets
    inner join Changes on Changes.Changeset_Id = Changesets.Id
    inner join ItemVersions on ItemVersions.Id = Changes.ItemVersion_Id
    inner join Items on Itemversions.Item_Id = Items.Id
    inner join DiffStats on DiffStats.Id = Changes.DiffStats_Id
where
    ServerItem like '%.cs' 
    and ServerItem not like '%tools%'
    and ServerItem like '%Roche-CSSP/src/Roche.PIOS/%'
    and ServerItem not like '%Test%'
    and ServerItem not like '%ODataService%'
    and ServerItem not like '%Reference%'
    and ServerItem not like '%Assembly%'


group by
    Changesets.CreationDate
--    Changesets.Id
order by
    Changesets.CreationDate
