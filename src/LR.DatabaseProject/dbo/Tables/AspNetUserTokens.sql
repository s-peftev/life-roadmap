CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId] NVARCHAR(450) NOT NULL,
    [LoginProvider] NVARCHAR(128) NOT NULL,
    [Name] NVARCHAR(128) NOT NULL,
    [Value] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId], [LoginProvider], [Name])
);

GO

ALTER TABLE [dbo].[AspNetUserTokens]
ADD CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers]([Id])
ON DELETE CASCADE;