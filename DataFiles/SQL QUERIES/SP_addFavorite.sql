DROP PROCEDURE IF EXISTS SP_addFavorite;
GO

CREATE PROCEDURE SP_addFavorite
    @userId INT,
    @songId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Favorites WHERE userId = @userId AND songId = @songId)
    BEGIN
        THROW 50004, 'Song already added to this user''s favorites!', 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM MusicUsers WHERE id = @userId)
    BEGIN
        THROW 50004, 'User does not exist!', 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM Songs WHERE songId = @songId)
    BEGIN
        THROW 50004, 'Song does not exist!', 1;
    END;

    INSERT INTO Favorites (userId, songId)
    VALUES (@userId, @songId);
END;
GO
