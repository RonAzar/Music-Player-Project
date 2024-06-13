drop table if exists MusicUsers
go

create table MusicUsers(
	id int identity (1,1) primary key,
	userName varchar(30),
	firstName varchar(30),
	lastname varchar(30),
	email varchar(50),
	userPassword varchar(50),
	phone varchar(30),
	dateOfRegistration datetime
)