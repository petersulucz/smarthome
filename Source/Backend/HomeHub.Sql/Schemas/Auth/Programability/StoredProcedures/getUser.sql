CREATE PROCEDURE [auth].[getUser]
    @token      BINARY(64)
   ,@ip         BINARY(16)
   ,@expiration INT
AS
    DECLARE @Unauthorized INT = 50002;

    BEGIN TRANSACTION

    TRY
    BEGIN
        DECLARE @id UNIQUEIDENTIFIER = NULL

        SELECT @id = [id]
        FROM auth.[token]
        WHERE token = @token
          AND DATEADD(MINUTE, @expiration, [assigned]) > GETUTCDATE()

        IF (@id IS NULL)
        BEGIN
           ;THROW @Unauthorized, N'Token not found', 1
        END

        SELECT 
            usr.[id]
           ,usr.[first]
           ,usr.[last]
           ,usr.[created]
        FROM auth.[user] usr
        WHERE usr.[id] = @id

        SELECT
            r.claim
        FROM auth.[roles] r
        WHERE r.id = @id

        EXECUTE auth.logaccess   @id = @id
                                ,@ip = @ip

        COMMIT TRANSACTION
    CATCH
        ROLLBACK TRANSACTION
        THROW
    END
RETURN 0