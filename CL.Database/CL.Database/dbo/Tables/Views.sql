CREATE TABLE [dbo].[Views]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[UserId] NVARCHAR(128) NOT NULL,
	[AssetId] INT NOT NULL,
	[ShareId] INT NULL,
	[Duration] INT NOT NULL,
	[CreateDt] smalldatetime NOT NULL,
	[IsLiked] BIT NOT NULL,
	CONSTRAINT [PK_Views] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Views_UserId_AspNetUsers_Id] FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id),
    CONSTRAINT [FK_Views_AssetId_Asset_Id] FOREIGN KEY (AssetId) REFERENCES Asset(Id),
    CONSTRAINT [FK_Views_ShareId_Share_Id] FOREIGN KEY (ShareId) REFERENCES Share(Id)
)
