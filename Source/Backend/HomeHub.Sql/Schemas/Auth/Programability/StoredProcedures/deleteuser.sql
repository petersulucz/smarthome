CREATE PROCEDURE [auth].[deleteuser]
    @id UNIQUEIDENTIFIER
AS
    DECLARE @NotFound INT = 50001

    BEGIN TRY
        BEGIN TRANSACTION

            -- Delete the user record
            DELETE FROM [auth].[user]
            WHERE [id] = @id

            IF (@@ROWCOUNT <> 1)
            BEGIN
               ;THROW @NotFound, N'Could not find user', 1
            END

            -- Destroy all of the login tokens
            DELETE FROM [auth].[token]
            WHERE [id] = @id

            -- Delete the identity info
            DELETE FROM [auth].[identity]
            WHERE [id] = @id

            -- Delete the auth roles
            DELETE FROM [auth].[roles]
            WHERE [id] = @id

            -- Delete the stored creds
            DELETE FROM [hub].[accountcredentials]
            WHERE [user] = @id

            -- Get the homes which the user is a part of
            DECLARE @userhomes TABLE
            (
                home UNIQUEIDENTIFIER
            )

            INSERT INTO @userhomes
            SELECT [home] FROM [hub].[membership]
            WHERE [user] = @id

            DECLARE @nonemptyhomes TABLE
            (
                home UNIQUEIDENTIFIER
            )

            -- Get the homes which still have members
            INSERT INTO @nonemptyhomes
            SELECT DISTINCT m.[home] FROM [hub].[membership] m
            JOIN @userhomes u
              ON u.[home] = m.[home]
            WHERE m.[user] <> @id
            -- TODO, we only want homes which have admins here

            DECLARE @homestodelete TABLE
            (
                home UNIQUEIDENTIFIER
            )

            INSERT INTO @homestodelete
            SELECT [home] FROM @userhomes
            WHERE [home] NOT IN (SELECT [home] FROM @nonemptyhomes)

            -- Delete all attached homes
            DELETE FROM [hub].[home]
            WHERE [id] IN (SELECT [home] FROM @homestodelete)

            -- Delete all memberships
            DELETE FROM [hub].[membership]
            WHERE [user] = @id

            -- Delete all attached devices
            DELETE FROM [hub].[device]
            WHERE [home] IN (SELECT [home] FROM @homestodelete)
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
       ;THROW 50099, N'UNKNOWN FAILURE', 1
    END CATCH
RETURN 0

