CREATE PROCEDURE [hub].[getdevices]
    @home UNIQUEIDENTIFIER
   ,@user UNIQUEIDENTIFIER
AS

    IF NOT EXISTS (SELECT TOP 1 1 FROM hub.membership WHERE [home] = @home AND [user] = @user)
    BEGIN
        ;THROW 50002, N'NO ACCESS', 0
    END

    DECLARE @devices TABLE
    (
        [id] UNIQUEIDENTIFIER
       ,[name] NVARCHAR(128)
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
    WHERE device.home = @home

    -- Get the device types
    SELECT
        map.device
       ,func.name
       ,func.argumenttype
    FROM hub.devicefunctionmapping map
    JOIN hub.functiondefinition func
      ON func.id = map.[function]
    WHERE map.device IN (SELECT devicedefinition FROM @devices)

    -- Get the devices
    SELECT * FROM @devices

RETURN 0
