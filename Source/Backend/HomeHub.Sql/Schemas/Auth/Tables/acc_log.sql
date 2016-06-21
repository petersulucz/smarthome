﻿/*
* Access log table
*/
CREATE TABLE [auth].[acc_log]
(
    [id] UNIQUEIDENTIFIER NOT NULL
   ,[time] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
   ,[ip] BINARY(16)
)
