USE [productionboard_db];
GO

/*
    Product-rate and product-change support — compatibility migration.

    This migration deliberately uses dbo.ProductionProducts instead of
    dbo.Products because an older dbo.Products table may already exist
    with a different structure.

    Test rates:
      Product 1 =  60 units/hour = 1 unit/minute
      Product 2 = 120 units/hour = 2 units/minute
      Product 3 = 180 units/hour = 3 units/minute

    Planned stops:
      H1 =  5 minutes (team meeting)
      H5 = 20 minutes (planned pause)
      H8 = 10 minutes (workstation organization)
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

IF OBJECT_ID(N'dbo.ProductionProducts', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProductionProducts
    (
        Id INT IDENTITY(1, 1) NOT NULL,

        Code NVARCHAR(50) NOT NULL,
        Name NVARCHAR(150) NOT NULL,

        StandardRatePerHour INT NOT NULL,

        IsActive BIT NOT NULL
            CONSTRAINT DF_ProductionProducts_IsActive
            DEFAULT 1,

        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_ProductionProducts_CreatedAt
            DEFAULT SYSUTCDATETIME(),

        UpdatedAt DATETIME2(0) NULL,

        CONSTRAINT PK_ProductionProducts
            PRIMARY KEY (Id),

        CONSTRAINT UQ_ProductionProducts_Code
            UNIQUE (Code),

        CONSTRAINT UQ_ProductionProducts_Name
            UNIQUE (Name),

        CONSTRAINT CK_ProductionProducts_StandardRatePerHour
            CHECK (StandardRatePerHour > 0)
    );
END;
GO

MERGE dbo.ProductionProducts AS target
USING
(
    VALUES
        (N'P001', N'Product 1',  60, CAST(1 AS BIT)),
        (N'P002', N'Product 2', 120, CAST(1 AS BIT)),
        (N'P003', N'Product 3', 180, CAST(1 AS BIT))
) AS source
(
    Code,
    Name,
    StandardRatePerHour,
    IsActive
)
ON target.Code = source.Code

WHEN MATCHED THEN
    UPDATE SET
        target.Name = source.Name,
        target.StandardRatePerHour =
            source.StandardRatePerHour,
        target.IsActive = source.IsActive,
        target.UpdatedAt = SYSUTCDATETIME()

WHEN NOT MATCHED BY TARGET THEN
    INSERT
    (
        Code,
        Name,
        StandardRatePerHour,
        IsActive
    )
    VALUES
    (
        source.Code,
        source.Name,
        source.StandardRatePerHour,
        source.IsActive
    );
GO

IF COL_LENGTH(
       N'dbo.ProductionBoards',
       N'InitialProductId') IS NULL
BEGIN
    ALTER TABLE dbo.ProductionBoards
        ADD InitialProductId INT NULL;
END;
GO

IF COL_LENGTH(
       N'dbo.ProductionBoards',
       N'InitialProductRatePerHourSnapshot') IS NULL
BEGIN
    ALTER TABLE dbo.ProductionBoards
        ADD InitialProductRatePerHourSnapshot INT NULL;
END;
GO

IF COL_LENGTH(
       N'dbo.ProductionBoardHours',
       N'ProductId') IS NULL
BEGIN
    ALTER TABLE dbo.ProductionBoardHours
        ADD ProductId INT NULL;
END;
GO

IF COL_LENGTH(
       N'dbo.ProductionBoardHours',
       N'ProductNameSnapshot') IS NULL
BEGIN
    ALTER TABLE dbo.ProductionBoardHours
        ADD ProductNameSnapshot NVARCHAR(150) NULL;
END;
GO

IF COL_LENGTH(
       N'dbo.ProductionBoardHours',
       N'RatePerHourSnapshot') IS NULL
BEGIN
    ALTER TABLE dbo.ProductionBoardHours
        ADD RatePerHourSnapshot INT NULL;
END;
GO

IF COL_LENGTH(
       N'dbo.ProductionBoardHours',
       N'PlannedStopMinutes') IS NULL
BEGIN
    ALTER TABLE dbo.ProductionBoardHours
        ADD PlannedStopMinutes INT NOT NULL
            CONSTRAINT DF_ProductionBoardHours_PlannedStopMinutes
            DEFAULT 0;
END;
GO

IF COL_LENGTH(
       N'dbo.ProductionBoardHours',
       N'ChangeoverMinutes') IS NULL
BEGIN
    ALTER TABLE dbo.ProductionBoardHours
        ADD ChangeoverMinutes INT NOT NULL
            CONSTRAINT DF_ProductionBoardHours_ChangeoverMinutes
            DEFAULT 0;
END;
GO

/*
    Remove foreign keys created by the previous failed migration.
    They may point to an unrelated legacy dbo.Products table.
*/
IF OBJECT_ID(
       N'dbo.FK_ProductionBoards_InitialProduct',
       N'F') IS NOT NULL
BEGIN
    ALTER TABLE dbo.ProductionBoards
        DROP CONSTRAINT FK_ProductionBoards_InitialProduct;
END;
GO

IF OBJECT_ID(
       N'dbo.FK_ProductionBoardHours_Product',
       N'F') IS NOT NULL
BEGIN
    ALTER TABLE dbo.ProductionBoardHours
        DROP CONSTRAINT FK_ProductionBoardHours_Product;
END;
GO

IF OBJECT_ID(
       N'dbo.ProductionBoardProductChanges',
       N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProductionBoardProductChanges
    (
        Id INT IDENTITY(1, 1) NOT NULL,

        ProductionBoardId INT NOT NULL,
        EffectiveHourNumber TINYINT NOT NULL,

        PreviousProductId INT NOT NULL,
        PreviousProductCodeSnapshot NVARCHAR(50) NOT NULL,
        PreviousProductNameSnapshot NVARCHAR(150) NOT NULL,
        PreviousRatePerHourSnapshot INT NOT NULL,

        NewProductId INT NOT NULL,
        NewProductCodeSnapshot NVARCHAR(50) NOT NULL,
        NewProductNameSnapshot NVARCHAR(150) NOT NULL,
        NewRatePerHourSnapshot INT NOT NULL,

        ChangeoverMinutes INT NOT NULL
            CONSTRAINT DF_ProductChanges_ChangeoverMinutes
            DEFAULT 0,

        Reason NVARCHAR(500) NULL,
        ChangedBy NVARCHAR(256) NULL,

        ChangedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_ProductChanges_ChangedAt
            DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_ProductionBoardProductChanges
            PRIMARY KEY (Id),

        CONSTRAINT FK_ProductChanges_Board
            FOREIGN KEY (ProductionBoardId)
            REFERENCES dbo.ProductionBoards(Id)
            ON DELETE CASCADE,

        CONSTRAINT UQ_ProductChanges_BoardHour
            UNIQUE
            (
                ProductionBoardId,
                EffectiveHourNumber
            ),

        CONSTRAINT CK_ProductChanges_EffectiveHour
            CHECK
            (
                EffectiveHourNumber BETWEEN 1 AND 8
            ),

        CONSTRAINT CK_ProductChanges_PreviousRate
            CHECK
            (
                PreviousRatePerHourSnapshot > 0
            ),

        CONSTRAINT CK_ProductChanges_NewRate
            CHECK
            (
                NewRatePerHourSnapshot > 0
            ),

        CONSTRAINT CK_ProductChanges_ChangeoverMinutes
            CHECK
            (
                ChangeoverMinutes BETWEEN 0 AND 60
            )
    );
END;
GO

IF OBJECT_ID(
       N'dbo.FK_ProductChanges_PreviousProduct',
       N'F') IS NOT NULL
BEGIN
    ALTER TABLE dbo.ProductionBoardProductChanges
        DROP CONSTRAINT FK_ProductChanges_PreviousProduct;
END;
GO

IF OBJECT_ID(
       N'dbo.FK_ProductChanges_NewProduct',
       N'F') IS NOT NULL
BEGIN
    ALTER TABLE dbo.ProductionBoardProductChanges
        DROP CONSTRAINT FK_ProductChanges_NewProduct;
END;
GO

/*
    Remap any records left by the previous migration to the dedicated
    product table using their immutable code/name snapshots.
*/
UPDATE productChange
SET
    PreviousProductId =
        previousProduct.Id
FROM dbo.ProductionBoardProductChanges AS productChange
INNER JOIN dbo.ProductionProducts AS previousProduct
    ON previousProduct.Code =
       productChange.PreviousProductCodeSnapshot
    OR previousProduct.Name =
       productChange.PreviousProductNameSnapshot;
GO

UPDATE productChange
SET
    NewProductId =
        newProduct.Id
FROM dbo.ProductionBoardProductChanges AS productChange
INNER JOIN dbo.ProductionProducts AS newProduct
    ON newProduct.Code =
       productChange.NewProductCodeSnapshot
    OR newProduct.Name =
       productChange.NewProductNameSnapshot;
GO

ALTER TABLE dbo.ProductionBoards
    ADD CONSTRAINT FK_ProductionBoards_InitialProduct
        FOREIGN KEY (InitialProductId)
        REFERENCES dbo.ProductionProducts(Id);
GO

ALTER TABLE dbo.ProductionBoardHours
    ADD CONSTRAINT FK_ProductionBoardHours_Product
        FOREIGN KEY (ProductId)
        REFERENCES dbo.ProductionProducts(Id);
GO

ALTER TABLE dbo.ProductionBoardProductChanges
    ADD CONSTRAINT FK_ProductChanges_PreviousProduct
        FOREIGN KEY (PreviousProductId)
        REFERENCES dbo.ProductionProducts(Id);
GO

ALTER TABLE dbo.ProductionBoardProductChanges
    ADD CONSTRAINT FK_ProductChanges_NewProduct
        FOREIGN KEY (NewProductId)
        REFERENCES dbo.ProductionProducts(Id);
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.check_constraints
    WHERE name =
        N'CK_ProductionBoards_InitialProductRate'
)
BEGIN
    ALTER TABLE dbo.ProductionBoards
        ADD CONSTRAINT CK_ProductionBoards_InitialProductRate
            CHECK
            (
                InitialProductRatePerHourSnapshot IS NULL
                OR InitialProductRatePerHourSnapshot > 0
            );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.check_constraints
    WHERE name =
        N'CK_ProductionBoardHours_RatePerHour'
)
BEGIN
    ALTER TABLE dbo.ProductionBoardHours
        ADD CONSTRAINT CK_ProductionBoardHours_RatePerHour
            CHECK
            (
                RatePerHourSnapshot IS NULL
                OR RatePerHourSnapshot > 0
            );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.check_constraints
    WHERE name =
        N'CK_ProductionBoardHours_PlannedStopMinutes'
)
BEGIN
    ALTER TABLE dbo.ProductionBoardHours
        ADD CONSTRAINT CK_ProductionBoardHours_PlannedStopMinutes
            CHECK
            (
                PlannedStopMinutes BETWEEN 0 AND 60
            );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.check_constraints
    WHERE name =
        N'CK_ProductionBoardHours_ChangeoverMinutes'
)
BEGIN
    ALTER TABLE dbo.ProductionBoardHours
        ADD CONSTRAINT CK_ProductionBoardHours_ChangeoverMinutes
            CHECK
            (
                ChangeoverMinutes BETWEEN 0 AND 60
            );
END;
GO

/*
    Link all existing boards whose ProductName matches a configured
    test product. Unknown legacy products remain nullable.
*/
UPDATE board
SET
    InitialProductId =
        product.Id,

    InitialProductRatePerHourSnapshot =
        product.StandardRatePerHour
FROM dbo.ProductionBoards AS board
INNER JOIN dbo.ProductionProducts AS product
    ON product.Name = board.ProductName
    OR product.Code = board.ProductName
WHERE board.InitialProductId IS NULL
   OR board.InitialProductRatePerHourSnapshot IS NULL
   OR board.InitialProductId <> product.Id
   OR board.InitialProductRatePerHourSnapshot <>
      product.StandardRatePerHour;
GO

/*
    Backfill hour snapshots and recalculate targets.
    Actual production, scrap and comments are preserved.
*/
UPDATE productionHour
SET
    ProductId =
        board.InitialProductId,

    ProductNameSnapshot =
        COALESCE(
            product.Name,
            board.ProductName),

    RatePerHourSnapshot =
        board.InitialProductRatePerHourSnapshot,

    PlannedStopMinutes =
        CASE productionHour.HourNumber
            WHEN 1 THEN 5
            WHEN 5 THEN 20
            WHEN 8 THEN 10
            ELSE 0
        END,

    ChangeoverMinutes =
        0,

    TargetQuantity =
        CONVERT
        (
            INT,
            ROUND
            (
                board.InitialProductRatePerHourSnapshot
                *
                (
                    60
                    -
                    CASE productionHour.HourNumber
                        WHEN 1 THEN 5
                        WHEN 5 THEN 20
                        WHEN 8 THEN 10
                        ELSE 0
                    END
                )
                / 60.0,
                0
            )
        ),

    UpdatedAt =
        SYSUTCDATETIME()
FROM dbo.ProductionBoardHours AS productionHour
INNER JOIN dbo.ProductionBoards AS board
    ON board.Id =
       productionHour.ProductionBoardId
LEFT JOIN dbo.ProductionProducts AS product
    ON product.Id =
       board.InitialProductId
WHERE board.InitialProductId IS NOT NULL
  AND board.InitialProductRatePerHourSnapshot IS NOT NULL;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name =
        N'IX_ProductChanges_BoardHour'
      AND object_id =
        OBJECT_ID(
            N'dbo.ProductionBoardProductChanges')
)
BEGIN
    CREATE INDEX IX_ProductChanges_BoardHour
        ON dbo.ProductionBoardProductChanges
        (
            ProductionBoardId,
            EffectiveHourNumber
        );
END;
GO

SELECT
    Code,
    Name,
    StandardRatePerHour,
    CAST(
        StandardRatePerHour / 60.0
        AS DECIMAL(10, 2)
    ) AS UnitsPerMinute,
    IsActive
FROM dbo.ProductionProducts
ORDER BY Id;
GO
