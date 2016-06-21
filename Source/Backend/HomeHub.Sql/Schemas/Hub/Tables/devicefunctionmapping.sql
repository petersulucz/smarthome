/*
* Maps a device definition to a function
*/
CREATE TABLE [hub].[devicefunctionmapping]
(
    [device] UNIQUEIDENTIFIER NOT NULL
   ,[function] INT NOT NULL
   ,PRIMARY KEY ([device], [function])
)
