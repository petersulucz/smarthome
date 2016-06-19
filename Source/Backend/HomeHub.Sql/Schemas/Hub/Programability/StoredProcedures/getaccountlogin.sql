CREATE PROCEDURE [hub].[getaccountlogin]
    @user UNIQUEIDENTIFIER
   ,@manufacturer NVARCHAR(128)
AS
    DECLARE @manId INT

    SELECT @manId = [id]
    FROM [hub].[devicemanufacturer]
    WHERE [name] = @manufacturer

    SELECT [meta]
    FROM [hub].[accountcredentials]
    WHERE [user] = @user AND [manufacturer] = @manId

RETURN 0
