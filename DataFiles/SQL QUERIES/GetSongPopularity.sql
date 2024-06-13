drop procedure if exists GetSongPopularity
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
CREATE PROCEDURE GetSongPopularity
	-- Add the parameters for the stored procedure here
	@songId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	if not exists (select * from Songs where songId = @songId)
	begin
		throw 50000, 'This song does not exist!', 1
	end
	SELECT COUNT(*) AS CountOfValue
	FROM Favorites
	WHERE songId = @songId;
END
GO
