IF OBJECT_ID(N'Retrospectives.Sessions', N'U') IS NULL
BEGIN
  CREATE TABLE [Retrospectives].[Sessions] (
    [Id] NVARCHAR(32) PRIMARY KEY NOT NULL,
    [Name] NVARCHAR(255) NOT NULL,
    [SprintId] INT NOT NULL,
    [ProjectId] INT NOT NULL
  );
END;

IF OBJECT_ID(N'Retrospectives.Cards', N'U') IS NULL
BEGIN
  CREATE TABLE [Retrospectives].[Cards] (
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [Content] NVARCHAR(255) NOT NULL,
    [Type] TINYINT NOT NULL,
    [SessionId] NVARCHAR(32) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL,
    FOREIGN KEY ([SessionId]) REFERENCES [Retrospectives].[Sessions](Id)
    ON DELETE CASCADE
  );
END;