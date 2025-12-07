CREATE TABLE [dbo].[UserProfiles] (
    [Id]                        UNIQUEIDENTIFIER  NOT NULL DEFAULT NEWID(),
    [UserId]                    NVARCHAR(450)     NOT NULL,
    [FirstName]                 NVARCHAR(100)     NULL,
    [LastName]                  NVARCHAR(100)     NULL,
    [BirthDate]                 DATE              NULL,
    [ProfilePhotoUrl]           NVARCHAR(500)     NULL,
    [ProfilePhotoPublicId]      NVARCHAR(200)     NULL,
    [CreatedAt]                 DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
    [LastActive]                DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]                 DATETIME2         NULL,

    CONSTRAINT [PK_UserProfiles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_UserProfiles_UserId] UNIQUE ([UserId]),
    CONSTRAINT [FK_UserProfiles_AspNetUsers_UserId]
        FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers]([Id]) ON DELETE CASCADE
);