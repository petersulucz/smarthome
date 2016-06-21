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
        USING [hub].[accountcredentials] AS T
        ON (S.[user] = @user AND S.[manufacturer] = @manId AND S.[home] = @home)
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
            @user
           ,@home
           ,@manId
           ,@meta
        )
        WHEN MATCHED THEN
        UPDATE
        SET [meta] = @meta;


        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
       ;THROW
    END CATCH
RETURN 0
