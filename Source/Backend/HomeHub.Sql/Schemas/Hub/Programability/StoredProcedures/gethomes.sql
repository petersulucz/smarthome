CREATE PROCEDURE [hub].[gethomes]
    @user UNIQUEIDENTIFIER
AS
    SELECT
        h.id
       ,h.name
       ,h.created
    FROM [hub].[membership] m
    JOIN [hub].[home] h
      ON h.[id]= m.[home]
    WHERE m.[user] = @user
RETURN 0
