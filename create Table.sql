CREATE TABLE [dbo].[Songs]
(
	[Id] INT NOT NULL PRIMARY KEY,
    [Title] VARCHAR(50) NOT NULL, 
    [ArtistName] VARCHAR(50) NOT NULL, 
    [AlbumId] INT NULL, 
	[SequenceID] INT NULL,
    [Description] TEXT NULL, 
    [PathToFile] VARCHAR(100) NOT NULL, 
    [Year] DATE NOT NULL, 
    [Genre] VARCHAR(50) NULL, 
    [Rating] INT NULL
)

CREATE TABLE [dbo].[Albums]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AlbumID] INT NULL, 
    [AlbumTitle] VARCHAR(50) NULL, 
    [Year] DATE NULL, 
    [Artist] VARCHAR(50) NULL, 
    [Tracks] INT NULL, 
    [Cover] IMAGE NULL, 
    [CoverPatch] VARCHAR(100) NULL,
	[Description] TEXT NULL
)

CREATE TABLE [dbo].[PlayLists]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PlayListId] INT NOT NULL, 
    [PlayListName] VARCHAR(50) NOT NULL, 
    [Comment] TEXT NULL
)



select *
from INFORMATION_SCHEMA.COLUMNS
where TABLE_NAME='songs';