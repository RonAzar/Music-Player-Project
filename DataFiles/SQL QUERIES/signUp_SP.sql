drop procedure if exists signUp_SP
GO
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
CREATE PROCEDURE signUp_SP
	-- Add the parameters for the stored procedure here
	@userName varchar(30),
	@firstName varchar(30),
	@lastname varchar(30),
	@email varchar(50),
	@userPassword varchar(50),
	@phone varchar(30)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;
	if exists (select * from MusicUsers where email = @email or userName = @userName)
	begin
		throw 50000, 'User Name/Email already exists.', 1
	end
    -- Insert statements for procedure here
	INSERT INTO MusicUsers (userName, firstName, lastName, email, userPassword, phone, dateOfRegistration)
	VALUES (@userName, @firstName, @lastname, @email, @userPassword, @phone, GETDATE())
	
	
END
GO
