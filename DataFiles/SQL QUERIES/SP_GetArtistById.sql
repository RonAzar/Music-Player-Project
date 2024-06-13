drop PROCEDURE if exists SP_GetArtistById
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
CREATE PROCEDURE SP_GetArtistById
	-- Add the parameters for the stored procedure here
	@artistId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	-- Check if the artist exists
    IF NOT EXISTS (SELECT 1 FROM Artists WHERE artistId = @artistId)
    BEGIN
        -- Throw an exception (using RAISERROR for example)
        Throw 50001, 'The artist does not exist.',1
    END

    -- Insert statements for procedure here
	SELECT * from Artists where (artistId = @artistId)
END
GO
