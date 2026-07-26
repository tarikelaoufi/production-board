using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using global::ProductionBoard.Core.Interfaces;

using FilterModel =
    global::ProductionBoard.Core.Models.ProductionBoardSearchFilter;

using SummaryModel =
    global::ProductionBoard.Core.Models.ProductionBoardSummary;

namespace ProductionBoard.Data.Repositories
{
    public sealed class ProductionBoardQueryRepository
        : IProductionBoardQueryRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public ProductionBoardQueryRepository(
            ConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(
                    nameof(connectionFactory));
            }

            _connectionFactory = connectionFactory;
        }

        public IList<SummaryModel> Search(
            FilterModel filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            const string sql = @"
SELECT TOP (@MaxResults)
    Id,
    BoardDate,
    TeamName,
    ShiftName,
    LineName,
    ProductName,
    CreatedAt,
    UpdatedAt
FROM dbo.ProductionBoards
WHERE
    (@BoardDate IS NULL OR BoardDate = @BoardDate)
    AND (@TeamName IS NULL OR TeamName = @TeamName)
    AND (@ShiftName IS NULL OR ShiftName = @ShiftName)
    AND (@LineName IS NULL OR LineName = @LineName)
    AND (@ProductName IS NULL OR ProductName = @ProductName)
ORDER BY
    BoardDate DESC,
    CASE ShiftName
        WHEN N'Night' THEN 3
        WHEN N'Afternoon' THEN 2
        WHEN N'Morning' THEN 1
        ELSE 0
    END DESC,
    TeamName ASC,
    LineName ASC,
    ProductName ASC,
    Id DESC;";

            IList<SummaryModel> results =
                new List<SummaryModel>();

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@MaxResults",
                    SqlDbType.Int).Value =
                    filter.MaxResults;

                command.Parameters.Add(
                    "@BoardDate",
                    SqlDbType.Date).Value =
                    filter.BoardDate.HasValue
                        ? (object)filter.BoardDate.Value.Date
                        : DBNull.Value;

                command.Parameters.Add(
                    "@TeamName",
                    SqlDbType.NVarChar,
                    100).Value =
                    ToDatabaseValue(filter.TeamName);

                command.Parameters.Add(
                    "@ShiftName",
                    SqlDbType.NVarChar,
                    50).Value =
                    ToDatabaseValue(filter.ShiftName);

                command.Parameters.Add(
                    "@LineName",
                    SqlDbType.NVarChar,
                    100).Value =
                    ToDatabaseValue(filter.LineName);

                command.Parameters.Add(
                    "@ProductName",
                    SqlDbType.NVarChar,
                    150).Value =
                    ToDatabaseValue(filter.ProductName);

                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(
                            MapSummary(reader));
                    }
                }
            }

            return results;
        }

        public int? FindAdjacentBoardId(
            int currentBoardId,
            bool moveNext,
            string teamNameRestriction)
        {
            if (currentBoardId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(currentBoardId));
            }

            const string sql = @"
;WITH CurrentBoard AS
(
    SELECT
        Id,
        LineName,
        ProductName
    FROM dbo.ProductionBoards
    WHERE Id = @CurrentBoardId
),
OrderedBoards AS
(
    SELECT
        boards.Id,
        ROW_NUMBER() OVER
        (
            ORDER BY
                boards.BoardDate ASC,
                CASE boards.ShiftName
                    WHEN N'Morning' THEN 1
                    WHEN N'Afternoon' THEN 2
                    WHEN N'Night' THEN 3
                    ELSE 4
                END ASC,
                boards.TeamName ASC,
                boards.Id ASC
        ) AS RowNumber
    FROM dbo.ProductionBoards AS boards
    CROSS JOIN CurrentBoard AS currentBoard
    WHERE
        boards.LineName = currentBoard.LineName
        AND boards.ProductName = currentBoard.ProductName
        AND
        (
            @TeamNameRestriction IS NULL
            OR boards.TeamName = @TeamNameRestriction
        )
),
CurrentPosition AS
(
    SELECT RowNumber
    FROM OrderedBoards
    WHERE Id = @CurrentBoardId
)
SELECT TOP (1)
    orderedBoards.Id
FROM OrderedBoards AS orderedBoards
CROSS JOIN CurrentPosition AS currentPosition
WHERE
    orderedBoards.RowNumber =
        currentPosition.RowNumber + @Direction;";

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@CurrentBoardId",
                    SqlDbType.Int).Value =
                    currentBoardId;

                command.Parameters.Add(
                    "@Direction",
                    SqlDbType.Int).Value =
                    moveNext ? 1 : -1;

                command.Parameters.Add(
                    "@TeamNameRestriction",
                    SqlDbType.NVarChar,
                    100).Value =
                    ToDatabaseValue(
                        teamNameRestriction);

                connection.Open();

                object result =
                    command.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt32(result);
            }
        }

        private static SummaryModel MapSummary(
            SqlDataReader reader)
        {
            int updatedAtOrdinal =
                reader.GetOrdinal("UpdatedAt");

            return new SummaryModel
            {
                Id = reader.GetInt32(
                    reader.GetOrdinal("Id")),

                BoardDate = reader.GetDateTime(
                    reader.GetOrdinal("BoardDate")),

                TeamName = reader.GetString(
                    reader.GetOrdinal("TeamName")),

                ShiftName = reader.GetString(
                    reader.GetOrdinal("ShiftName")),

                LineName = reader.GetString(
                    reader.GetOrdinal("LineName")),

                ProductName = reader.GetString(
                    reader.GetOrdinal("ProductName")),

                CreatedAt = reader.GetDateTime(
                    reader.GetOrdinal("CreatedAt")),

                UpdatedAt =
                    reader.IsDBNull(updatedAtOrdinal)
                        ? (DateTime?)null
                        : reader.GetDateTime(
                            updatedAtOrdinal)
            };
        }

        private static object ToDatabaseValue(
            string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? (object)DBNull.Value
                : value.Trim();
        }
    }
}
