CREATE TABLE [dbo].[UserProfiles] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [UserId] NVARCHAR(450) NOT NULL UNIQUE, -- FK to AspNetUsers(Id)
    [FirstName] NVARCHAR(100) NULL,
    [LastName] NVARCHAR(100) NULL,
    [BirthDate] DATE NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,

    CONSTRAINT FK_UserProfiles_AspNetUsers_UserId FOREIGN KEY (UserId)
        REFERENCES [dbo].[AspNetUsers]([Id]) ON DELETE CASCADE
);