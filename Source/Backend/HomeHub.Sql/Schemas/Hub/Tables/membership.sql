/*
* Definies membership into a home
*/
CREATE TABLE [hub].[membership]
(
    [home] UNIQUEIDENTIFIER NOT NULL
   ,[user] UNIQUEIDENTIFIER NOT NULL
   ,[role] TINYINT NOT NULL
   ,PRIMARY KEY ([user], [home])
)
