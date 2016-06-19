CREATE PROCEDURE [hub].[getdevice]
    @user UNIQUEIDENTIFIER
   ,@device UNIQUEIDENTIFIER
AS
    DECLARE @devices TABLE
    (
        [id] UNIQUEIDENTIFIER
       ,[name] NVARCHAR(128)
       ,[home] UNIQUEIDENTIFIER
       ,[description] NVARCHAR(1024)
       ,[devicedefinition] UNIQUEIDENTIFIER
       ,[manufacturer] NVARCHAR(64)
       ,[product] NVARCHAR(256)
       ,[type] INT
       ,[meta] NVARCHAR(MAX)
    )

    INSERT INTO @devices
    SELECT
         device.id
        ,device.name
        ,device.home
        ,device.description
        ,device.devicedefinition
        ,man.name
        ,def.product
        ,def.type
        ,device.metadata
    FROM hub.device device
    JOIN hub.devicedefinition def
      ON def.id = device.devicedefinition
    JOIN hub.devicemanufacturer man
      ON man.id = def.manufacturer
    WHERE device.id = @device

    -- Get the device types
    SELECT
        func.device
       ,func.name
    FROM hub.devicefunction func
    WHERE func.device IN (SELECT devicedefinition FROM @devices)

    -- Get the devices
    SELECT * FROM @devices
RETURN 0
