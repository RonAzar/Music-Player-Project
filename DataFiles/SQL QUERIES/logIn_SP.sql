drop procedure if exists logIn_SP
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
CREATE PROCEDURE logIn_SP
	-- Add the parameters for the stored procedure here
	@email varchar(50),
	@userPassword varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	if exists (SELECT * from MusicUsers where (email = @email or userName = @email) and userPassword = @userPassword)
		begin 
			SELECT * from MusicUsers where (email = @email or userName = @email) and userPassword = @userPassword
		end
	else
		begin
			THROW 50003, 'wrong password or user does not exist', 1
		end
END
GO
