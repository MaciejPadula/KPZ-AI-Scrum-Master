IF OBJECT_ID(N'ScrumMaster.UserSettings', N'U') IS NULL
BEGIN
    CREATE TABLE [ScrumMaster].[UserSettings] (
        [UserId] nvarchar(255),
        [TaigaAccessToken] nvarchar(1000),
        [TaigaRefreshToken] nvarchar(1000),
        PRIMARY KEY ([UserId])
    )
END