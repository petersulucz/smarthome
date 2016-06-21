/*
* Stores credentials for connected services
*/
CREATE TABLE [hub].[accountcredentials]
(
    [user] UNIQUEIDENTIFIER NOT NULL
   ,[home] UNIQUEIDENTIFIER NOT NULL
   ,[manufacturer] INT NOT NULL
   ,[meta] NVARCHAR(MAX) NOT NULL
   ,PRIMARY KEY ([user], [manufacturer])
)
