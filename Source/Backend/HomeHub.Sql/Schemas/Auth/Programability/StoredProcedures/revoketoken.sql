CREATE PROCEDURE [auth].[revoketoken]
    @token      BINARY(64)
   ,@id         UNIQUEIDENTIFIER
AS
    BEGIN TRY
        BEGIN TRANSACTION

        DELETE FROM [auth].[token]
        WHERE [id] = @id
          AND [token] = @token

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW
    END CATCH
RETURN 0
