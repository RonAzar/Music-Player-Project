drop table if exists Artists
go
create table Artists(
	artistId int identity(1,1),
	artistName varchar(30) primary key,
	popularity int
)