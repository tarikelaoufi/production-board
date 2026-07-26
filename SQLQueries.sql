USE master;
GO

IF DB_ID(N'productionboard_db') IS NOT NULL
BEGIN
    ALTER DATABASE productionboard_db
    SET SINGLE_USER
    WITH ROLLBACK IMMEDIATE;

    DROP DATABASE productionboard_db;
END;
GO

CREATE DATABASE productionboard_db;
GO

USE productionboard_db;
GO

USE productionboard_db;
GO

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

    CONSTRAINT UQ_ProductionBoards_BoardIdentity
        UNIQUE
        (
            BoardDate,
            TeamName,
            ShiftName,
            LineName,
            ProductName
        )
);
GO

CREATE TABLE dbo.ProductionBoardHours
(
    Id INT IDENTITY(1, 1) NOT NULL,
    ProductionBoardId INT NOT NULL,
    HourNumber TINYINT NOT NULL,
    HourLabel NVARCHAR(5) NOT NULL,

    TargetQuantity INT NOT NULL DEFAULT 0,
    ActualQuantity INT NOT NULL DEFAULT 0,
    ScrapQuantity INT NOT NULL DEFAULT 0,

    Comment NVARCHAR(1000) NULL,
    StopType NVARCHAR(100) NULL,
    StopDurationMinutes INT NOT NULL DEFAULT 0,

    CreatedAt DATETIME2(0) NOT NULL
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
GO