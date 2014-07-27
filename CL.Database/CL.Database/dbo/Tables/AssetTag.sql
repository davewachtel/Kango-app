CREATE TABLE [dbo].[AssetTag] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [AssetId] INT NOT NULL,
    [TagId]   INT NOT NULL,
    CONSTRAINT [PK_AssetTags] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Asset_Tag]
    ON [dbo].[AssetTag]([AssetId] ASC, [TagId] ASC);

