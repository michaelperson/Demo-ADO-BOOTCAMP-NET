CREATE TABLE [dbo].[Game]
(
	[Id] INT NOT NULL IDENTITY, 
    [Title] NVARCHAR(100) NOT NULL, 
    [ReleaseDate] DATETIME NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Price] MONEY NULL, 
    [IsNew] BIT NOT NULL, 
    [PEGI] SMALLINT NOT NULL, 
    [StudioId] int NOT NULL,
    CONSTRAINT [CK_Game_PEGI] CHECK ([PEGI] IN (3,7,12,16,18)),
    CONSTRAINT [CK_Game_Price] CHECK ([Price] >= 0),
    CONSTRAINT [CK_Game_ReleaseDate] CHECK ([ReleaseDate] >= '1958-10-18'),
    CONSTRAINT [UK_Game_Title_ReleaseDate] UNIQUE ([Title],[ReleaseDate]),
    CONSTRAINT [PK_Game_Id] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Game_Studio] FOREIGN KEY ([StudioId]) REFERENCES [Studio]([Id]),
)
