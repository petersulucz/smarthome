﻿CREATE PROCEDURE [auth].[getPassword]
    @email NVARCHAR(128)
AS
    SELECT
        [id]
       ,[salt]
       ,[password]
    FROM auth.[identity]
    WHERE [email] = @email

RETURN 0
