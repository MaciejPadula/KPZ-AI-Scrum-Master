IF OBJECT_ID(N'ScrumMaster.UserSettings', N'U') IS NULL
BEGIN
    CREATE TABLE [ScrumMaster].[UserSettings] (
        [UserId] varchar(255),
        [TaigaAccess] nvarchar(max),
        PRIMARY KEY ([UserId])
    )
END