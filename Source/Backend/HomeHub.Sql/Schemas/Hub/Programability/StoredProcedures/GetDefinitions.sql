CREATE PROCEDURE [hub].[getdefinitions]
AS
    SELECT
        map.device
       ,func.name
       ,func.argumenttype
    FROM hub.functiondefinition func
    JOIN hub.devicefunctionmapping map
      ON func.id = map.[function]

    SELECT
        def.id
       ,def.type
       ,def.product
       ,man.name
    FROM hub.devicedefinition def
    JOIN hub.devicemanufacturer man
      ON man.id = def.manufacturer

RETURN 0
