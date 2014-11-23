﻿CREATE TABLE [dbo].[Share]
(
	[Id] INT IDENTITY NOT NULL,
	[ViewId] INT NOT NULL,
	[RecipientId] INT NOT NULL,
	[CreateDt] SMALLDATETIME NOT NULL,
	
    CONSTRAINT [PK_Share] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Share_View_Id] FOREIGN KEY (ViewId) REFERENCES Views(Id),
    CONSTRAINT [FK_Share_User_Id] FOREIGN KEY (RecipientId) REFERENCES AspNetUsers(Id)
)