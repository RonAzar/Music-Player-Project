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
alter PROCEDURE SP_InsertScore
    @score INT,
    @userId INT,
	@userName varchar(30)
AS
BEGIN
    --SET NOCOUNT ON;

    -- Insert the new score into the Scoreboard table
		
    IF NOT EXISTS (SELECT 1 FROM MusicUsers WHERE (userName = @userName AND id = @userId))
    BEGIN
        THROW 50004, 'User Id Or User name do not exists!', 1;
    END;
	IF EXISTS (SELECT 1 FROM Scoreboard where score = @score and userId = @userId and userName = @userName)
	BEGIN
		THROW 50004, 'That score for that user already exists!', 1;
	END

    INSERT INTO Scoreboard (score, userId, userName)
    VALUES (@score, @userId, @userName);
END;
