CREATE TABLE [dbo].[Asset] (
    [Id]			INT            IDENTITY (1, 1) NOT NULL,
    [AssetTypeId]	INT            NOT NULL,
    [Title]			NVARCHAR (100) NOT NULL,
    [Description]	NVARCHAR (500) NULL,
    [Url]			NVARCHAR(2083) NOT NULL, 
    [IsActive]		BIT            NOT NULL,
    CONSTRAINT [PK_Asset] PRIMARY KEY CLUSTERED ([Id] ASC)
);

