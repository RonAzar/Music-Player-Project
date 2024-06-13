SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
alter PROCEDURE SP_Get3RandomsArtists 
	-- Add the parameters for the stored procedure here
	@artistName VARCHAR(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 3 *
	FROM Artists
	WHERE artistName <> @artistName -- Exclude the specified artist
	ORDER BY NEWID(); -- This will return 3 random artists excluding the specified one
END
GO
