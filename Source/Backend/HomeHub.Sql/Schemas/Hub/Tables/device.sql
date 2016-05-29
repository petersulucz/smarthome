CREATE TABLE [hub].[device]
(
    [id] INT NOT NULL
   ,[home] UNIQUEIDENTIFIER NOT NULL
   ,[name] NVARCHAR(256) NOT NULL
   ,PRIMARY KEY ([home], [id])
)
