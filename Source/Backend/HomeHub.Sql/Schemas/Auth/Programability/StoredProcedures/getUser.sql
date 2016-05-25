CREATE PROCEDURE [auth].[getUser]
    @token BINARY(64)
AS
    DECLARE @id UNIQUEIDENTIFIER = NULL

    SELECT @id = id
    FROM auth.token
    WHERE token = @token

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

RETURN 0
