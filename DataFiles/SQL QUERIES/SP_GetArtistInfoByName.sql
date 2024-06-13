DROP PROCEDURE IF EXISTS SP_GetArtistInfoByName
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE SP_GetArtistInfoByName
    -- Add the parameters for the stored procedure here
    @artistName varchar(30)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    --SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT *
    FROM Artists
    WHERE artistName = @artistName;
END
GO
