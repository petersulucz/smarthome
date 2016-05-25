CREATE TABLE [auth].[roles]
(
    [id] UNIQUEIDENTIFIER NOT NULL
   ,[claim] NVARCHAR(48) NOT NULL
   ,PRIMARY KEY ([id], [claim])
)
