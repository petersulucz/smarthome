/*
* Griddles definition of a home
*/
CREATE TABLE [hub].[home]
(
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY
   ,[name] NVARCHAR(256) NOT NULL
   ,[created] DATETIME2 NOT NULL

)
