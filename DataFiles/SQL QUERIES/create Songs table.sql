DROP TABLE IF EXISTS Songs;
go

CREATE TABLE Songs (
    songId INT IDENTITY (1,1) PRIMARY KEY,
    songName VARCHAR(255),
    lyrics VARCHAR(MAX),
    link VARCHAR(255),
    artistName VARCHAR(30),
    FOREIGN KEY (artistName) REFERENCES Artists(artistName)
);