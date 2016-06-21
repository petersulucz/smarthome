CREATE PROCEDURE [hub].[getaccountlogin]
    @user UNIQUEIDENTIFIER
   ,@home UNIQUEIDENTIFIER
   ,@manufacturer NVARCHAR(128)
AS
    DECLARE @manId INT

    SELECT @manId = [id]
    FROM [hub].[devicemanufacturer]
    WHERE [name] = @manufacturer

    SELECT [meta]
    FROM [hub].[accountcredentials]
    WHERE [user] = @user AND [manufacturer] = @manId AND [home] = @home

RETURN 0
