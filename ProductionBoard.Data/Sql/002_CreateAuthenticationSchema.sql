USE [productionboard_db];
GO

/* =========================================================
   Application users
   ========================================================= */

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        Id INT IDENTITY(1, 1) NOT NULL,

        Username NVARCHAR(80) NOT NULL,
        FullName NVARCHAR(150) NOT NULL,
        PasswordHash NVARCHAR(500) NOT NULL,

        Role NVARCHAR(30) NOT NULL,

        IsActive BIT NOT NULL
            CONSTRAINT DF_Users_IsActive
            DEFAULT 1,

        MustChangePassword BIT NOT NULL
            CONSTRAINT DF_Users_MustChangePassword
            DEFAULT 1,

        FailedLoginCount INT NOT NULL
            CONSTRAINT DF_Users_FailedLoginCount
            DEFAULT 0,

        LockedUntilUtc DATETIME2(0) NULL,
        LastLoginAtUtc DATETIME2(0) NULL,

        CreatedAtUtc DATETIME2(0) NOT NULL
            CONSTRAINT DF_Users_CreatedAtUtc
            DEFAULT SYSUTCDATETIME(),

        UpdatedAtUtc DATETIME2(0) NULL,

        CONSTRAINT PK_Users
            PRIMARY KEY (Id),

        CONSTRAINT UQ_Users_Username
            UNIQUE (Username),

        CONSTRAINT CK_Users_Role
            CHECK
            (
                Role IN
                (
                    N'Admin',
                    N'TeamLeader',
                    N'Viewer'
                )
            ),

        CONSTRAINT CK_Users_FailedLoginCount
            CHECK (FailedLoginCount >= 0)
    );
END;
GO

/* =========================================================
   Team and shift assignments

   This separate table supports future shift rotations.
   A leader can receive a new assignment without changing
   the account itself.
   ========================================================= */

IF OBJECT_ID(N'dbo.UserShiftAssignments', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserShiftAssignments
    (
        Id INT IDENTITY(1, 1) NOT NULL,

        UserId INT NOT NULL,

        TeamName NVARCHAR(100) NOT NULL,
        ShiftName NVARCHAR(50) NOT NULL,

        EffectiveFrom DATE NOT NULL,
        EffectiveTo DATE NULL,

        IsActive BIT NOT NULL
            CONSTRAINT DF_UserShiftAssignments_IsActive
            DEFAULT 1,

        CreatedAtUtc DATETIME2(0) NOT NULL
            CONSTRAINT DF_UserShiftAssignments_CreatedAtUtc
            DEFAULT SYSUTCDATETIME(),

        UpdatedAtUtc DATETIME2(0) NULL,

        CONSTRAINT PK_UserShiftAssignments
            PRIMARY KEY (Id),

        CONSTRAINT FK_UserShiftAssignments_Users
            FOREIGN KEY (UserId)
            REFERENCES dbo.Users(Id),

        CONSTRAINT CK_UserShiftAssignments_Shift
            CHECK
            (
                ShiftName IN
                (
                    N'Morning',
                    N'Afternoon',
                    N'Night'
                )
            ),

        CONSTRAINT CK_UserShiftAssignments_Dates
            CHECK
            (
                EffectiveTo IS NULL
                OR EffectiveTo >= EffectiveFrom
            )
    );
END;
GO

/* =========================================================
   Audit log

   Records who modified production and when.
   ========================================================= */

IF OBJECT_ID(N'dbo.ProductionBoardAuditLogs', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProductionBoardAuditLogs
    (
        Id BIGINT IDENTITY(1, 1) NOT NULL,

        UserId INT NOT NULL,
        ProductionBoardId INT NULL,
        ProductionBoardHourId INT NULL,

        ActionName NVARCHAR(50) NOT NULL,

        OldValues NVARCHAR(MAX) NULL,
        NewValues NVARCHAR(MAX) NULL,

        IpAddress NVARCHAR(45) NULL,

        CreatedAtUtc DATETIME2(0) NOT NULL
            CONSTRAINT DF_ProductionBoardAuditLogs_CreatedAtUtc
            DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_ProductionBoardAuditLogs
            PRIMARY KEY (Id),

        CONSTRAINT FK_ProductionBoardAuditLogs_Users
            FOREIGN KEY (UserId)
            REFERENCES dbo.Users(Id),

        CONSTRAINT FK_ProductionBoardAuditLogs_Boards
            FOREIGN KEY (ProductionBoardId)
            REFERENCES dbo.ProductionBoards(Id),

        CONSTRAINT FK_ProductionBoardAuditLogs_Hours
            FOREIGN KEY (ProductionBoardHourId)
            REFERENCES dbo.ProductionBoardHours(Id)
    );
END;
GO

/* =========================================================
   Indexes
   ========================================================= */

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Users_Username_IsActive'
      AND object_id = OBJECT_ID(N'dbo.Users')
)
BEGIN
    CREATE INDEX IX_Users_Username_IsActive
        ON dbo.Users(Username, IsActive);
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_UserShiftAssignments_Current'
      AND object_id =
          OBJECT_ID(N'dbo.UserShiftAssignments')
)
BEGIN
    CREATE INDEX IX_UserShiftAssignments_Current
        ON dbo.UserShiftAssignments
        (
            UserId,
            IsActive,
            EffectiveFrom,
            EffectiveTo
        );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_ProductionBoardAuditLogs_Board'
      AND object_id =
          OBJECT_ID(N'dbo.ProductionBoardAuditLogs')
)
BEGIN
    CREATE INDEX IX_ProductionBoardAuditLogs_Board
        ON dbo.ProductionBoardAuditLogs
        (
            ProductionBoardId,
            CreatedAtUtc
        );
END;
GO