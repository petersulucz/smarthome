/*
* Stores credentials for connected services
*/
CREATE TABLE [hub].[accountcredentials]
(
    [user] UNIQUEIDENTIFIER NOT NULL
   ,[home] UNIQUEIDENTIFIER NOT NULL
   ,[manufacturer] INT NOT NULL
   ,[meta] XML NOT NULL
   ,PRIMARY KEY ([home], [manufacturer])
)
