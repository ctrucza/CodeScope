select 
    Changesets.ChangesetId
    ----Changesets.Id,
    --sum(DiffStats.InitialLineCount) ILOC,
    --sum(DiffStats.Added) Added,
    --sum(DiffStats.Modified) Modified,
    --sum(DiffStats.Deleted) Deleted,
    --sum(DiffStats.FinalLineCount) FLOC
from 
    Changesets
    inner join Changes on Changes.Changeset_Id = Changesets.Id
    inner join ItemVersions on ItemVersions.Item_Id = Changes.ItemVersion_Id