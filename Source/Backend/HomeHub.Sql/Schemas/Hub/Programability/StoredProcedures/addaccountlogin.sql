CREATE PROCEDURE [hub].[addaccountlogin]
    @user UNIQUEIDENTIFIER
   ,@home UNIQUEIDENTIFIER
   ,@manufacturer NVARCHAR(128)
   ,@meta NVARCHAR(MAX)
AS
    BEGIN TRY

        BEGIN TRANSACTION

        DECLARE @manId INT

        SELECT @manId = [id]
        FROM [hub].[devicemanufacturer]
        WHERE [name] = @manufacturer

        IF @@ROWCOUNT = 0
        BEGIN
           ;THROW 50001, N'Manufacturer not found', 0
        END

        MERGE INTO [hub].[accountcredentials] AS S
        USING (SELECT @user AS [user], @manId AS [manufacturer], @home AS [home], @meta AS [meta]) AS T
           ON (S.[user] = T.[user] AND S.[manufacturer] = T.[manufacturer] AND S.[home] = T.[home])
        WHEN NOT MATCHED BY TARGET THEN
        INSERT 
        (
            [user]
           ,[home]
           ,[manufacturer]
           ,[meta]
        )
        VALUES
        (
            T.[user]
           ,T.[home]
           ,T.[manufacturer]
           ,T.[meta]
        )
        WHEN MATCHED THEN
        UPDATE
        SET S.[meta] = @meta;


        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
       ;THROW
    END CATCH
RETURN 0
