CREATE PROCEDURE [auth].[createUser]
    @firstname NVARCHAR(64)
   ,@lastname NVARCHAR(64)
   ,@email NVARCHAR(128)
   ,@salt BINARY(64)
   ,@password BINARY(64)
   ,@token BINARY(64)
   ,@ip BINARY(16)
   ,@roles auth.rolesList READONLY

AS
    DECLARE @DuplicateKey INT     = 50002
    DECLARE @result       INT     = 0
    DECLARE @error        NVARCHAR(2048) = NULL

    DECLARE @UtcNow     DATETIME2 = GETUTCDATE()
    DECLARE @id         UNIQUEIDENTIFIER = NEWID()

    BEGIN TRANSACTION

        IF EXISTS (SELECT TOP 1 1 FROM auth.[identity]
            WHERE email = @email)
        BEGIN
            SET @error = N'Duplicate email insertion'
            SET @result = @DuplicateKey
            GOTO ErrorHandler
        END

        INSERT INTO auth.[user]
        VALUES
            (
                @id
               ,@firstname
               ,@lastname
               ,@UtcNow
            )

        INSERT INTO auth.[identity]
        VALUES
            (
                @id
               ,@email
               ,@salt
               ,@password
            )

        INSERT INTO auth.token
        VALUES  
            (
                @id
               ,@token
               ,@UtcNow
            )

        INSERT INTO auth.[roles]
        SELECT
            @id,
            rl.claim
        FROM @roles rl

        EXECUTE auth.logaccess   @id = @id
                                ,@ip = @ip

    COMMIT TRANSACTION

    SELECT
        @id AS id
       ,@token AS token
       ,@UtcNow AS assigned

RETURN @result

ErrorHandler:
    ROLLBACK TRANSACTION
   ;THROW @result, @error, 1
