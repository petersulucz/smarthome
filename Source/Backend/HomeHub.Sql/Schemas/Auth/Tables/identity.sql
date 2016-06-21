/*
* List of user identities
*/
CREATE TABLE [auth].[identity]
(
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY
   ,[email] NVARCHAR(128) NOT NULL
   ,[salt] BINARY(64) NOT NULL
   ,[password] BINARY(64) NOT NULL
)
