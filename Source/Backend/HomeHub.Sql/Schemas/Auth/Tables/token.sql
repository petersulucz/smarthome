﻿/*
* The access tokens for each user
*/
CREATE TABLE [auth].[token]
(
    [id] UNIQUEIDENTIFIER NOT NULL
   ,[token] BINARY(64) NOT NULL PRIMARY KEY CLUSTERED
   ,[expiration] DATETIME2 NOT NULL
   ,[assigned] DATETIME2 NOT NULL
)
