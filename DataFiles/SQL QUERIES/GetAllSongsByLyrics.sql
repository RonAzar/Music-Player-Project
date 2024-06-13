drop PROCEDURE if exists GetAllSongsByLyrics
go
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- Create the stored procedure
CREATE PROCEDURE GetAllSongsByLyrics
    @searchLyrics VARCHAR(MAX)
AS
BEGIN 
    -- Retrieve all songs that contain the specified lyrics
	if Not exists (SELECT * FROM Songs
		WHERE lyrics LIKE '%' + @searchLyrics + '%')
		BEGIN
			 -- Handle the exception
			THROW 50001, 'Song not found!', 1;
		END 
	SELECT * FROM Songs
		WHERE lyrics LIKE '%' + @searchLyrics + '%'
END;
GO
