
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/12/2013 12:23:48
-- Generated from EDMX file: C:\Users\csaba.trucza\Documents\Hacks\HackingTFS\HackingTFS\Churn\Churn.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [churn];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ProjectChangeset]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Changesets] DROP CONSTRAINT [FK_ProjectChangeset];
GO
IF OBJECT_ID(N'[dbo].[FK_ChangesetChange]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Changes] DROP CONSTRAINT [FK_ChangesetChange];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Items] DROP CONSTRAINT [FK_ProjectItem];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemItemVersion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ItemVersions] DROP CONSTRAINT [FK_ItemItemVersion];
GO
IF OBJECT_ID(N'[dbo].[FK_ChangeItemVersion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Changes] DROP CONSTRAINT [FK_ChangeItemVersion];
GO
IF OBJECT_ID(N'[dbo].[FK_ChangeDiffStat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Changes] DROP CONSTRAINT [FK_ChangeDiffStat];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Projects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Projects];
GO
IF OBJECT_ID(N'[dbo].[Changesets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Changesets];
GO
IF OBJECT_ID(N'[dbo].[Changes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Changes];
GO
IF OBJECT_ID(N'[dbo].[Items]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Items];
GO
IF OBJECT_ID(N'[dbo].[ItemVersions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemVersions];
GO
IF OBJECT_ID(N'[dbo].[DiffStats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DiffStats];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Path] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Changesets'
CREATE TABLE [dbo].[Changesets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [CommitterDisplayName] nvarchar(max)  NOT NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [Status] int  NOT NULL,
    [ChangesetId] bigint  NOT NULL,
    [Project_Id] int  NOT NULL
);
GO

-- Creating table 'Changes'
CREATE TABLE [dbo].[Changes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ChangeType] int  NOT NULL,
    [Changeset_Id] int  NOT NULL,
    [ItemVersion_Id] int  NOT NULL,
    [DiffStats_Id] int  NOT NULL
);
GO

-- Creating table 'Items'
CREATE TABLE [dbo].[Items] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ServerItem] nvarchar(max)  NOT NULL,
    [ItemType] int  NOT NULL,
    [Project_Id] int  NOT NULL
);
GO

-- Creating table 'ItemVersions'
CREATE TABLE [dbo].[ItemVersions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Version] bigint  NOT NULL,
    [Stream] nvarchar(max)  NOT NULL,
    [Encoding] bigint  NOT NULL,
    [Item_Id] int  NOT NULL
);
GO

-- Creating table 'DiffStats'
CREATE TABLE [dbo].[DiffStats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InitialLineCount] bigint  NOT NULL,
    [Added] bigint  NOT NULL,
    [Modified] bigint  NOT NULL,
    [Deleted] bigint  NOT NULL,
    [FinalLineCount] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Changesets'
ALTER TABLE [dbo].[Changesets]
ADD CONSTRAINT [PK_Changesets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Changes'
ALTER TABLE [dbo].[Changes]
ADD CONSTRAINT [PK_Changes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [PK_Items]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ItemVersions'
ALTER TABLE [dbo].[ItemVersions]
ADD CONSTRAINT [PK_ItemVersions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DiffStats'
ALTER TABLE [dbo].[DiffStats]
ADD CONSTRAINT [PK_DiffStats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Project_Id] in table 'Changesets'
ALTER TABLE [dbo].[Changesets]
ADD CONSTRAINT [FK_ProjectChangeset]
    FOREIGN KEY ([Project_Id])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectChangeset'
CREATE INDEX [IX_FK_ProjectChangeset]
ON [dbo].[Changesets]
    ([Project_Id]);
GO

-- Creating foreign key on [Changeset_Id] in table 'Changes'
ALTER TABLE [dbo].[Changes]
ADD CONSTRAINT [FK_ChangesetChange]
    FOREIGN KEY ([Changeset_Id])
    REFERENCES [dbo].[Changesets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ChangesetChange'
CREATE INDEX [IX_FK_ChangesetChange]
ON [dbo].[Changes]
    ([Changeset_Id]);
GO

-- Creating foreign key on [Project_Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_ProjectItem]
    FOREIGN KEY ([Project_Id])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectItem'
CREATE INDEX [IX_FK_ProjectItem]
ON [dbo].[Items]
    ([Project_Id]);
GO

-- Creating foreign key on [Item_Id] in table 'ItemVersions'
ALTER TABLE [dbo].[ItemVersions]
ADD CONSTRAINT [FK_ItemItemVersion]
    FOREIGN KEY ([Item_Id])
    REFERENCES [dbo].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemItemVersion'
CREATE INDEX [IX_FK_ItemItemVersion]
ON [dbo].[ItemVersions]
    ([Item_Id]);
GO

-- Creating foreign key on [ItemVersion_Id] in table 'Changes'
ALTER TABLE [dbo].[Changes]
ADD CONSTRAINT [FK_ChangeItemVersion]
    FOREIGN KEY ([ItemVersion_Id])
    REFERENCES [dbo].[ItemVersions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ChangeItemVersion'
CREATE INDEX [IX_FK_ChangeItemVersion]
ON [dbo].[Changes]
    ([ItemVersion_Id]);
GO

-- Creating foreign key on [DiffStats_Id] in table 'Changes'
ALTER TABLE [dbo].[Changes]
ADD CONSTRAINT [FK_ChangeDiffStat]
    FOREIGN KEY ([DiffStats_Id])
    REFERENCES [dbo].[DiffStats]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ChangeDiffStat'
CREATE INDEX [IX_FK_ChangeDiffStat]
ON [dbo].[Changes]
    ([DiffStats_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------