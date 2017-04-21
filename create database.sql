CREATE TABLE [dbo].[Songs]
(
	[Id] INT NOT NULL PRIMARY KEY,
    [Title] VARCHAR(50) NOT NULL, 
    [ArtistName] VARCHAR(50) NOT NULL, 
    [AlbumId] INT NULL, 
	[SequenceId] INT NULL,
    [PathToFile] VARCHAR(100) NOT NULL, 
    [Year] DATE NOT NULL, 
    [Genre] VARCHAR(50) NULL, 
    [Rating] INT NULL,
    [Description] TEXT NULL 
)

CREATE TABLE [dbo].[Albums]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
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
	[PlayListName] VARCHAR(50) NOT NULL,	
    [SongId] INT NOT NULL,  
    [Description] TEXT NULL
)

drop table [dbo].[Albums];
drop table Playlists;

alter table Songs
add constraint Songs_Albums_FK FOREIGN KEY ( AlbumId ) references Albums(Id);

alter table PlayLists
add constraint PlayLists_Songs_FK FOREIGN KEY ( SongId ) references Songs(Id);


select *
from INFORMATION_SCHEMA.COLUMNS
where TABLE_NAME='songs';

[Id] INT NOT NULL PRIMARY KEY,
    [Title] VARCHAR(50) NOT NULL, 
    [ArtistName] VARCHAR(50) NOT NULL, 
    [AlbumId] INT NULL, 
	[SequenceId] INT NULL,
    [PathToFile] VARCHAR(100) NOT NULL, 
    [Year] DATE NOT NULL, 
    [Genre] VARCHAR(50) NULL, 
    [Rating] INT NULL,
    [Description] TEXT NULL 

INSERT INTO Songs (Title, ArtistName, AlbumId,SequenceId,PathToFile,Year,Genre,Rating,Description)
VALUES ('Loving songs', 'Bill Gates',1,1,'','1999-01-01','Classic','2','This is a good songs');

INSERT INTO Songs VALUES (1,'Loving songs', 'Bill Gates',1,1,'','1999-01-01','Classic','2','This is a good songs');

[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AlbumTitle] VARCHAR(50) NULL, 
    [Year] DATE NULL, 
    [Artist] VARCHAR(50) NULL, 
    [Tracks] INT NULL, 
    [Cover] IMAGE NULL, 
    [CoverPatch] VARCHAR(100) NULL,
	[Description] TEXT NULL
	
INSERT INTO Albums VALUES ('Good','','','','','','');


CREATE TABLE [dbo].[PlayLists]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[PlayListName] VARCHAR(50) NOT NULL,	
    [SongId] INT NOT NULL,  
    [Description] TEXT NULL
)

INSERT INTO PlayLists VALUES ('first playlist',1,'good songs');
INSERT INTO Songs [(column1, column2, column3,...columnN)]   
VALUES (value1, value2, value3,...valueN); 