CREATE PROCEDURE [hub].[getaccountlogins]
    @user UNIQUEIDENTIFIER
   ,@home UNIQUEIDENTIFIER
AS

    SELECT cred.[meta]
          ,man.[name] AS manufacturer
          ,cred.[user]
    FROM [hub].[accountcredentials] cred
    JOIN [hub].[devicemanufacturer] man
      ON cred.[manufacturer] = man.[id]
    WHERE cred.[home] = @home

RETURN 0
