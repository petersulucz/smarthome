CREATE PROCEDURE [auth].[logaccess]
    @id UNIQUEIDENTIFIER
   ,@ip BINARY(16)
AS
    INSERT INTO auth.acc_log
        (
            id
           ,ip
        )
    VALUES
        (
            @id
           ,@ip
        )
RETURN 0
