USE [igroup116_test2]
GO
/****** Object:  StoredProcedure [dbo].[SPAddSong]    Script Date: 17/07/2023 11:42:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SPAddSong]
	-- Add the parameters for the stored procedure here
	@songName NVARCHAR(100),
    @lyrics NVARCHAR(MAX),
    @link NVARCHAR(200),
    @artistName VARCHAR(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	-- Check if the artist exists in the Artists table
	IF NOT EXISTS (SELECT 1 FROM Artists WHERE artistName = @artistName)
	BEGIN
		-- Throw a custom exception indicating that the artist doesn't exist.
		THROW 50002, 'The artist does not exist.', 1;
	END

	-- Check if the song already exists
	IF NOT EXISTS (SELECT 1 FROM Songs WHERE songName = @songName AND artistName = @artistName)
	BEGIN
		-- Insert the song if it does not exist
		INSERT INTO Songs (songName, lyrics, link, artistName)
		VALUES (@songName, @lyrics, @link, @artistName);
	END
	ELSE
	BEGIN
		-- Throw a custom exception indicating that the song already exists.
		THROW 50001, 'The song already exists.', 1;
	END
END