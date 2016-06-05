CREATE PROCEDURE [hub].[addhomeuser]
    @home UNIQUEIDENTIFIER
   ,@caller UNIQUEIDENTIFIER
   ,@user UNIQUEIDENTIFIER
   ,@role TINYINT
AS
    BEGIN TRY
        DECLARE @callerRole TINYINT = NULL
        BEGIN TRANSACTION

            -- Make sure this dude is a member of the home
            SELECT @callerRole = m.[role]
            FROM hub.membership m
            WHERE m.home = @home

            -- Just shut this crap down right here
            IF (@callerRole IS NULL)
            BEGIN
               ;THROW 50002, N'Caller is not a member of this group.', 0
            END

            -- Make sure they arent trying to pull something fishy
            IF (@callerRole < @role)
            BEGIN
               ;THROW 50002, N'You cannot give someone more permissions than you have', 0
            END

            INSERT INTO [hub].[membership]
            (
                [user]
               ,[home]
               ,[role]
            )
            VALUES
            (
                @user
               ,@home
               ,@role
            )

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
       ;THROW
    END CATCH
RETURN 0
