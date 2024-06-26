SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE SP_GetRandomSongByArtist 
	-- Add the parameters for the stored procedure here
	@artistName VARCHAR(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 *
	FROM Songs
	where @artistName =artistName
	ORDER BY NEWID(); -- This will return a random row from the Songs table
END
GO
