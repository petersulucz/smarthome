/*
* Defines standard functions for physical devices
*/
CREATE TABLE [hub].[functiondefinition]
(
    [id] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY
   ,[name] NVARCHAR(64) NOT NULL
   ,[argumenttype] INT NOT NULL
)
