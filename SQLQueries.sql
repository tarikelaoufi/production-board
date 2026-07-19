IF DB_ID(N'productionboard_db') IS NULL
BEGIN
    CREATE DATABASE [productionboard_db];
END;
GO

USE [productionboard_db];
GO