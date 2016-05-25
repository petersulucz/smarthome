ALTER TABLE [auth].[identity]
    ADD CONSTRAINT [unique_email]
    UNIQUE (email)
