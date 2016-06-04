CREATE PROCEDURE [hub].[adddevice]
    @name NVARCHAR(128)
   ,@home UNIQUEIDENTIFIER
   ,@description NVARCHAR(1024)
   ,@definition UNIQUEIDENTIFIER
AS
    DECLARE @id UNIQUEIDENTIFIER

    BEGIN TRY
        BEGIN TRANSACTION

        IF NOT EXISTS (SELECT TOP 1 1 FROM hub.devicedefinition WHERE id = @definition)
        BEGIN
           ;THROW 50001, N'Definition not found', 0
        END

        SET @id = NEWID()

        INSERT INTO hub.device
        (
            [id]
           ,[home]
           ,[name]
           ,[description]
           ,[devicedefinition]
        )
        VALUES
        (
            @id
           ,@home
           ,@name
           ,@description
           ,@definition
        )

        -- Get the id
        SELECT @id AS id

        -- Get the functions for the device
        SELECT
            func.device
           ,func.name
        FROM hub.devicefunction func
        WHERE func.device = @definition

        -- Get the device definition
        SELECT
            man.name AS manufacturer
           ,def.type
        FROM hub.devicedefinition def
        JOIN hub.devicemanufacturer man
          ON def.manufacturer = man.id
        WHERE def.id = @definition

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW
    END CATCH
RETURN 0
