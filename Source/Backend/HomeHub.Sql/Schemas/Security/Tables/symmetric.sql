CREATE TABLE [security].[symmetric]
(
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1,1)
   ,[asymmetric] INT NOT NULL
   ,[key] VARBINARY(1024) NOT NULL
   ,[iv] VARBINARY(64) NOT NULL

)
