select top 10
    len(Stream)-len(replace(Stream, char(13), '')),
    *
from 
    ItemVersions


