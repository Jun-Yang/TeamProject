CREATE TABLE [dbo].[Songs]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
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
	[Id] INT NOT NULL IDENTITY,
	[SongId] INT NOT NULL,
	[PlayListName] VARCHAR(50) NOT NULL,	  
    [Description] TEXT NULL,
	CONSTRAINT pk_playlists PRIMARY KEY (Id,SongId)
)

alter table Songs
add constraint Songs_Albums_FK FOREIGN KEY ( AlbumId ) references Albums(Id);

alter table PlayLists
add constraint PlayLists_Songs_FK1 FOREIGN KEY ( SongId ) references Songs(Id);


=======

Albums -> Songs-> Playlists

INSERT INTO Albums VALUES ('Good','1999','Chinese super star','','','','');

INSERT INTO Songs 
VALUES ('How Lucky', 'HanAnXU',1,1,'C:\MusicLibrary\how lucky.mp3','2003-01-01','Classic','2','This is not a good songs');

INSERT INTO Songs 
VALUES ('Singing for your loneliness', 'HanAnXU',1,1,'C:\MusicLibrary\Singing for your loneliness.mp3','1999-01-01','Classic','2','This is a good songs');



ALTER TABLE PlayLists ADD PRIMARY KEY(Id,SongId);

drop table [dbo].[Albums];
drop table Playlists;

alter table Songs
add constraint Songs_Albums_FK FOREIGN KEY ( AlbumId ) references Albums(Id);

alter table PlayLists
add constraint PlayLists_Songs_FK1 FOREIGN KEY ( SongId ) references Songs(Id);


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

INSERT INTO Songs (Title, ArtistName, AlbumId,SequenceId,PathToFile,Year,Genre,Rating,Description)
VALUES ('Singing for your loneliness', 'ASong',1,1,'C:/MusicLibrary/','1999-01-01','Classic','2','This is a good songs');

INSERT INTO Songs 
VALUES ('Singing for your loneliness', 'ASong',1,1,'C:/MusicLibrary/','1999-01-01','Classic','2','This is a good songs');

select *
from INFORMATION_SCHEMA.COLUMNS
where TABLE_NAME='songs'

[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AlbumTitle] VARCHAR(50) NULL, 
    [Year] DATE NULL, 
    [Artist] VARCHAR(50) NULL, 
    [Tracks] INT NULL, 
    [Cover] IMAGE NULL, 
    [CoverPatch] VARCHAR(100) NULL,
	[Description] TEXT NULL
	



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


Alter Table Names Add Id_new Int Identity(1,1)

alter table songs add Id Int Identity(1,1);
alter table songs drop Id ;

ALTER TABLE songs
DROP PRIMARY KEY;

get primary key name

SELECT Id
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1
AND TABLE_NAME = 'songs' AND TABLE_SCHEMA = 'Schema'

ALTER TABLE <TABLE_NAME> DROP CONSTRAINT <FOREIGN_KEY_NAME>

alter table songs drop constraint Songs_Albums_FK;
alter table Playlists drop constraint PlayLists_Songs_FK;

drop table songs;

SELECT
    * 
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
    WHERE CONSTRAINT_NAME ='PlayLists_Songs_FK'
	
	0424
	
	INSERT INTO Songs 
VALUES ('Singing for your loneliness', 'ASong',1,1,'C:\MusicLibrary\','1999-01-01','Classic','2','This is a good songs');

INSERT INTO Songs 
VALUES ('How Lucky', 'HanAnXU',1,1,'C:\MusicLibrary\how lucky.mp3','2003-01-01','Classic','2','This is not a good songs');

INSERT INTO Songs 
VALUES ('Singing for your loneliness', 'HanAnXU',1,1,'C:\MusicLibrary\Singing for your loneliness.mp3','1999-01-01','Classic','2','This is a good songs');


SELECT Col.Column_Name from 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
WHERE 
    Col.Constraint_Name = Tab.Constraint_Name
    AND Col.Table_Name = Tab.Table_Name
    AND Constraint_Type = 'PRIMARY KEY'
    AND Col.Table_Name = 'songs'
	
	
CREATE TABLE [dbo].[PlayLists]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[PlayListName] VARCHAR(50) NOT NULL,	
    [SongId] INT NOT NULL,  
    [Description] TEXT NULL
)

INSERT INTO PlayLists 
VALUES (1,'Chinese','This is chinese playlist');

INSERT INTO PlayLists 
VALUES (2,'Classic','This is Classic playlist');

INSERT INTO Albums 
VALUES (3,'English POP','This is English pop playlist');

INSERT INTO Albums 
VALUES (4,'R&R','This is R&R');