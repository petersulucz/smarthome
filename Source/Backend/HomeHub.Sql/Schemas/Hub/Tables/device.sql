CREATE TABLE [hub].[device]
(
    [id] UNIQUEIDENTIFIER NOT NULL
   ,[home] UNIQUEIDENTIFIER NOT NULL
   ,[name] NVARCHAR(256) NOT NULL
   ,[description] NVARCHAR(1024) NOT NULL
   ,[devicedefinition] UNIQUEIDENTIFIER NOT NULL
   ,[metadata] NVARCHAR(MAX) NOT NULL
   ,PRIMARY KEY ([home], [id])
)
