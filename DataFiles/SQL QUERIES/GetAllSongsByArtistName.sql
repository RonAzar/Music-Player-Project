drop PROCEDURE if exists GetAllSongsByArtistName
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
CREATE PROCEDURE GetAllSongsByArtistName
    @artistName VARCHAR(30)
AS
BEGIN
    -- Check if the artist exists
    DECLARE @artistId INT;

    SELECT @artistId = artistId
    FROM Artists
    WHERE artistName = @artistName;

    IF (@artistId IS NOT NULL)
    BEGIN
        -- Retrieve all songs by the artist
        SELECT *
        FROM Songs s
        WHERE s.artistId = @artistId;
    END
    ELSE
    BEGIN
        -- If the artist does not exist, raise an exception
        THROW 50001, 'Artist not found!', 1;
    END
END;
