/*
* Stores definitions of devices. These cannot be modified by a user. These define physical devices
*/
CREATE TABLE [hub].[devicedefinition]
(
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY
   ,[manufacturer] INT NOT NULL
   ,[type] INT NOT NULL
   ,[product] NVARCHAR(256) NOT NULL
)
