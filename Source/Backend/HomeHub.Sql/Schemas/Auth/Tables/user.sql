/*
* User information
*/
CREATE TABLE [auth].[user]
(
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY
   ,[first] NVARCHAR(64) NOT NULL
   ,[last] NVARCHAR(64) NOT NULL
   ,[created] DATETIME2 NOT NULL
)
