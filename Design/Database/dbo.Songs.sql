CREATE TABLE [dbo].[Songs]
(
	[Id] INT NOT NULL PRIMARY KEY,
    [Title] VARCHAR(50) NOT NULL, 
    [ArtistName] VARCHAR(50) NOT NULL, 
    [AlbumId] INT NULL, 
	[SequenceID] INT NULL,
    [Description] TEXT NULL, 
    [PathToFile] VARCHAR(100) NOT NULL, 
    [Year] DATE NULL, 
    [Genre] VARCHAR(50) NULL, 
    [Rating] INT NULL,
    
)
