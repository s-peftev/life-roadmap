CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR(450) NOT NULL,
    [RoleId] NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId], [RoleId])
);

GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId]
ON [dbo].[AspNetUserRoles] ([RoleId]);

GO

ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers]([Id])
ON DELETE CASCADE;

GO

ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles]([Id])
ON DELETE CASCADE;