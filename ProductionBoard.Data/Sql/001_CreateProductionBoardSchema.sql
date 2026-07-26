IF DB_ID(N'productionboard_db') IS NULL
BEGIN
    CREATE DATABASE [productionboard_db];
END;
GO

USE [productionboard_db];
GO

IF OBJECT_ID(N'dbo.ProductionBoards', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProductionBoards
    (
        Id INT IDENTITY(1, 1) NOT NULL,

        BoardDate DATE NOT NULL,
        TeamName NVARCHAR(100) NOT NULL,
        ShiftName NVARCHAR(50) NOT NULL,
        LineName NVARCHAR(100) NOT NULL,
        ProductName NVARCHAR(150) NOT NULL,

        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_ProductionBoards_CreatedAt
            DEFAULT SYSUTCDATETIME(),

        UpdatedAt DATETIME2(0) NULL,

        CONSTRAINT PK_ProductionBoards
            PRIMARY KEY (Id),

        CONSTRAINT UQ_ProductionBoards_Identity
            UNIQUE
            (
                BoardDate,
                TeamName,
                ShiftName,
                LineName,
                ProductName
            )
    );
END;
GO

IF OBJECT_ID(N'dbo.ProductionBoardHours', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProductionBoardHours
    (
        Id INT IDENTITY(1, 1) NOT NULL,

        ProductionBoardId INT NOT NULL,
        HourNumber TINYINT NOT NULL,
        HourLabel NVARCHAR(5) NOT NULL,

        TargetQuantity INT NOT NULL
            CONSTRAINT DF_ProductionBoardHours_TargetQuantity
            DEFAULT 0,

        ActualQuantity INT NOT NULL
            CONSTRAINT DF_ProductionBoardHours_ActualQuantity
            DEFAULT 0,

        ScrapQuantity INT NOT NULL
            CONSTRAINT DF_ProductionBoardHours_ScrapQuantity
            DEFAULT 0,

        Comment NVARCHAR(1000) NULL,
        StopType NVARCHAR(100) NULL,

        StopDurationMinutes INT NOT NULL
            CONSTRAINT DF_ProductionBoardHours_StopDuration
            DEFAULT 0,

        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_ProductionBoardHours_CreatedAt
            DEFAULT SYSUTCDATETIME(),

        UpdatedAt DATETIME2(0) NULL,

        CONSTRAINT PK_ProductionBoardHours
            PRIMARY KEY (Id),

        CONSTRAINT FK_ProductionBoardHours_ProductionBoards
            FOREIGN KEY (ProductionBoardId)
            REFERENCES dbo.ProductionBoards(Id)
            ON DELETE CASCADE,

        CONSTRAINT UQ_ProductionBoardHours_BoardHour
            UNIQUE (ProductionBoardId, HourNumber),

        CONSTRAINT CK_ProductionBoardHours_HourNumber
            CHECK (HourNumber BETWEEN 1 AND 8),

        CONSTRAINT CK_ProductionBoardHours_TargetQuantity
            CHECK (TargetQuantity >= 0),

        CONSTRAINT CK_ProductionBoardHours_ActualQuantity
            CHECK (ActualQuantity >= 0),

        CONSTRAINT CK_ProductionBoardHours_ScrapQuantity
            CHECK (ScrapQuantity >= 0),

        CONSTRAINT CK_ProductionBoardHours_StopDuration
            CHECK (StopDurationMinutes >= 0)
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_ProductionBoards_BoardDate'
      AND object_id = OBJECT_ID(N'dbo.ProductionBoards')
)
BEGIN
    CREATE INDEX IX_ProductionBoards_BoardDate
        ON dbo.ProductionBoards(BoardDate);
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_ProductionBoardHours_ProductionBoardId'
      AND object_id = OBJECT_ID(N'dbo.ProductionBoardHours')
)
BEGIN
    CREATE INDEX IX_ProductionBoardHours_ProductionBoardId
        ON dbo.ProductionBoardHours(ProductionBoardId);
END;
GO