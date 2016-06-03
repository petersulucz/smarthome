CREATE PROCEDURE [hub].[getdevices]
    @home UNIQUEIDENTIFIER
AS

    DECLARE @devices TABLE
    (
        [id] UNIQUEIDENTIFIER
       ,[name] NVARCHAR(128)
       ,[description] NVARCHAR(1024)
       ,[devicedefinition] UNIQUEIDENTIFIER
       ,[manufacturur] NVARCHAR(64)
       ,[type] INT
    )

    INSERT INTO @devices
    SELECT
         device.id
        ,device.name
        ,device.description
        ,device.devicedefinition
        ,def.manufacturer
        ,def.type
    FROM hub.device device
    JOIN hub.devicedefinition def
        ON def.id = device.devicedefinition
    WHERE device.home = @home

    SELECT * FROM @devices

    SELECT
        func.device
       ,func.name
    FROM hub.devicefunction func
    WHERE func.device IN (SELECT devicedefinition FROM @devices)

RETURN 0
