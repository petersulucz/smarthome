CREATE PROCEDURE [hub].[gethomeusers]
    @home UNIQUEIDENTIFIER
   ,@user UNIQUEIDENTIFIER
AS
    IF NOT EXISTS (SELECT TOP 1 1 FROM hub.membership WHERE [home] = @home AND [user] = @user)
    BEGIN
       ;THROW 50002, N'Access denied, not member of home', 0
    END

    SELECT 
        m.[user]
       ,m.[role]
    FROM [hub].[membership] m
    WHERE m.home = @home

RETURN 0
