CREATE PROCEDURE [hub].[createhome]
    @name NVARCHAR(256)
   ,@user UNIQUEIDENTIFIER
AS
    DECLARE @id UNIQUEIDENTIFIER = NEWID()
    DECLARE @created DATETIME2 = GETUTCDATE()

    BEGIN TRANSACTION
    BEGIN TRY

        INSERT INTO hub.home
        (
            [id]
           ,[name]
           ,[created]
        )
        VALUES
        (
            @id
           ,@name
           ,@created
        )

        INSERT INTO hub.membership
        (
            [home]
           ,[user]
           ,[role]
        )
        VALUES
        (
            @id
           ,@user
           ,0xFF
        )

        COMMIT TRANSACTION

        SELECT
            @id AS id
           ,@name AS name
           ,@created AS created
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW
    END CATCH
RETURN 0
