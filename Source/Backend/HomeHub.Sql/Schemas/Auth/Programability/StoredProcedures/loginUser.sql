CREATE PROCEDURE [auth].[loginUser]
    @email NVARCHAR(128)
   ,@password BINARY(64)
   ,@token BINARY(64)
   ,@ip BINARY(16)
AS
    BEGIN TRANSACTION

    DECLARE @id UNIQUEIDENTIFIER    = NULL
    DECLARE @result INT             = 0
    DECLARE @error NVARCHAR(2048)   = NULL
    DECLARE @UtcNow     DATETIME2   = GETUTCDATE()

    -- find the user id
    SELECT
        @id = ident.id
    FROM auth.[identity] ident
    WHERE ident.[password] = @password
      AND ident.[email] = @email

    IF (@id IS NULL)
    BEGIN
        SET @result = 50004
        SET @error = N'Could not find user with email/password provided'
        GOTO ErrorHandler
    END

    INSERT INTO auth.token
    VALUES
        (
             @id
            ,@token
            ,@UtcNow
        )

    EXECUTE auth.logaccess   @id = @id
                            ,@ip = @ip

    COMMIT TRANSACTION

    SELECT
        @id AS id
       ,@token AS token
       ,@UtcNow AS assigned

    SELECT
        id
       ,claim
    FROM auth.[roles]
    WHERE id = @id


RETURN @result

ErrorHandler:
    ROLLBACK TRANSACTION
   ;THROW @result, @error, 1
