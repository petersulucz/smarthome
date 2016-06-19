CREATE PROCEDURE [hub].[getdefinitions]
AS
    SELECT
        func.device
       ,func.name
    FROM hub.devicefunction func

    SELECT
        def.id
       ,def.type
       ,def.product
       ,man.name
    FROM hub.devicedefinition def
    JOIN hub.devicemanufacturer man
      ON man.id = def.manufacturer

RETURN 0
