IF OBJECT_ID(N'EstimationPoker.Sessions', N'U') IS NULL
BEGIN
    CREATE TABLE [EstimationPoker].[Sessions] (
        [Id] NVARCHAR(32) PRIMARY KEY NOT NULL,
        [Name] NVARCHAR(255) NOT NULL,
        [UserId] NVARCHAR(255) NOT NULL,
        [ProjectId] INT NOT NULL
    );
END;

IF OBJECT_ID(N'EstimationPoker.SessionTasks', N'U') IS NULL
BEGIN
    CREATE TABLE [EstimationPoker].[SessionTasks] (
        Id INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
        Title NVARCHAR(255) NOT NULL,
        Description NVARCHAR(255) NOT NULL,
        SessionId NVARCHAR(32) NOT NULL,
        CreatedAt DATETIME2 NOT NULL,
        FOREIGN KEY (SessionId) REFERENCES [EstimationPoker].[Sessions](Id)
        ON DELETE CASCADE
    );
END;

IF OBJECT_ID(N'EstimationPoker.SessionUsers', N'U') IS NULL
BEGIN
    CREATE TABLE [EstimationPoker].[SessionUsers] (
        Id INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
        UserName NVARCHAR(255) NOT NULL,
        SessionId NVARCHAR(32) NOT NULL,
        FOREIGN KEY (SessionId) REFERENCES [EstimationPoker].[Sessions](Id)
        ON DELETE CASCADE
    );
END;

IF OBJECT_ID(N'EstimationPoker.TaskEstimations', N'U') IS NULL
BEGIN
    CREATE TABLE [EstimationPoker].[TaskEstimations] (
        TaskId INT NOT NULL,
        UserId INT NOT NULL,
        Value DECIMAL(18, 2) NOT NULL,
        PRIMARY KEY (TaskId, UserId),
        FOREIGN KEY (TaskId) REFERENCES [EstimationPoker].[SessionTasks](Id),
        FOREIGN KEY (UserId) REFERENCES [EstimationPoker].[SessionUsers](Id)
        ON DELETE CASCADE
    );
END;