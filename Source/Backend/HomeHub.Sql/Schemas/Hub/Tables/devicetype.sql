/*
* Definition of devices types... Light, Microwave
*/
CREATE TABLE [hub].[devicetype]
(
    [type] INT NOT NULL PRIMARY KEY IDENTITY
   ,[description] NVARCHAR(128) NOT NULL
)
