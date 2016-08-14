CREATE TABLE [security].[asymmetric]
(
    [id] INT NOT NULL PRIMARY KEY IDENTITY (1, 1)
   ,[fingerprint] VARBINARY(32) NOT NULL UNIQUE
   ,[revoked] BIT NOT NULL DEFAULT 0
)
