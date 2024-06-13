drop table if exists Favorites
go
create table Favorites(
	userId int,
	songId int,
	FOREIGN KEY (userId) REFERENCES MusicUsers(id),
	FOREIGN KEY (songId) REFERENCES Songs(songId),
	primary key(userId, songId)
)