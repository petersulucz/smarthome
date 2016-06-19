CREATE TABLE [hub].[devicedefinition]
(
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY
   ,[manufacturer] INT NOT NULL
   ,[type] INT NOT NULL
   ,[product] NVARCHAR(256) NOT NULL
)
