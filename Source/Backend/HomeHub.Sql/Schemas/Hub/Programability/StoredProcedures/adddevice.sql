CREATE PROCEDURE [hub].[adddevice]
    @name NVARCHAR(128)
   ,@home UNIQUEIDENTIFIER
   ,@description NVARCHAR(1024)
   ,@definition UNIQUEIDENTIFIER
   ,@meta NVARCHAR(MAX)
AS
    DECLARE @id UNIQUEIDENTIFIER

    BEGIN TRY
        BEGIN TRANSACTION

        IF NOT EXISTS (SELECT TOP 1 1 FROM hub.devicedefinition WHERE id = @definition)
        BEGIN
           ;THROW 50001, N'Definition not found', 1;
        END

        --  Make sure the name isnt already there in a home
        IF EXISTS (SELECT TOP 1 1 FROM hub.device WHERE home = @home AND name = @name)
        BEGIN
            ;THROW 50003, N'Device with name already exists', 1;
        END

        SET @id = NEWID()

        INSERT INTO hub.device
        (
            [id]
           ,[home]
           ,[name]
           ,[description]
           ,[devicedefinition]
           ,[metadata]
        )
        VALUES
        (
            @id
           ,@home
           ,@name
           ,@description
           ,@definition
           ,@meta
        )

        -- Get the id
        SELECT @id AS id

        -- Get the functions for the device
        SELECT
            map.device
           ,func.name
           ,func.argumenttype
        FROM hub.devicefunctionmapping map
        JOIN hub.functiondefinition func
          ON func.id = map.[function]
        WHERE map.device = @definition

        -- Get the device definition
        SELECT
            man.name AS manufacturer
           ,def.type
           ,def.product
        FROM hub.devicedefinition def
        JOIN hub.devicemanufacturer man
          ON def.manufacturer = man.id
        WHERE def.id = @definition

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
       ;THROW
    END CATCH
RETURN 0
