CREATE PROCEDURE [auth].[revokealltoken]
    @id UNIQUEIDENTIFIER
AS
        BEGIN TRY
        BEGIN TRANSACTION

        DELETE FROM [auth].[token]
        WHERE [id] = @id

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW
    END CATCH
RETURN 0
