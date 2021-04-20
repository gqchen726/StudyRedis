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
GO

CREATE TABLE [AccessHistoryLogs] (
    [Id] int NOT NULL IDENTITY,
    [IpAddress] nvarchar(max) NULL,
    [DateTime] datetime2 NOT NULL,
    [AccessPath] nvarchar(max) NULL,
    CONSTRAINT [PK_AccessHistoryLogs] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210330092916_StudyRedisInit', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [AccessHistoryLogs] ADD [SessionId] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210402053151_InsertColumns1', N'5.0.0');
GO

COMMIT;
GO

