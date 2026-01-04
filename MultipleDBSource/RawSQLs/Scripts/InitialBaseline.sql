IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260103141133_InitialBaseline'
)
BEGIN
    CREATE TABLE [Persons] (
        [Id] uniqueidentifier NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [CreatedTimestamp] datetimeoffset NOT NULL DEFAULT (CURRENT_TIMESTAMP),
        [UpdatedTimestamp] datetimeoffset NOT NULL DEFAULT (CURRENT_TIMESTAMP),
        [Details] json NULL,
        [History] json NULL,
        CONSTRAINT [PK_Persons] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260103141133_InitialBaseline'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260103141133_InitialBaseline', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260103142653_RefreshSqlLogic'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260103142653_RefreshSqlLogic', N'10.0.1');
END;

COMMIT;
GO

