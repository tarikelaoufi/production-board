using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using global::ProductionBoard.Core.Interfaces;

using FilterModel =
    global::ProductionBoard.Core.Models.WeeklySynthesisFilter;

using ResultModel =
    global::ProductionBoard.Core.Models.WeeklySynthesisResult;

using RowModel =
    global::ProductionBoard.Core.Models.WeeklyTeamDayResult;

namespace ProductionBoard.Data.Repositories
{
    public sealed class WeeklySynthesisRepository
        : IWeeklySynthesisRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public WeeklySynthesisRepository(
            ConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(
                    nameof(connectionFactory));
            }

            _connectionFactory = connectionFactory;
        }

        public ResultModel GetWeeklySynthesis(
            FilterModel filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            {
                connection.Open();

                string boardForeignKey =
                    FindFirstExistingColumn(
                        connection,
                        "dbo.ProductionBoardHours",
                        "ProductionBoardId",
                        "BoardId");

                string targetColumn =
                    FindFirstExistingColumn(
                        connection,
                        "dbo.ProductionBoardHours",
                        "TargetQuantity",
                        "ObjectiveQuantity",
                        "ObjectQuantity");

                string actualColumn =
                    FindFirstExistingColumn(
                        connection,
                        "dbo.ProductionBoardHours",
                        "ActualQuantity",
                        "RealQuantity",
                        "ReelQuantity");

                string scrapColumn =
                    FindFirstExistingColumn(
                        connection,
                        "dbo.ProductionBoardHours",
                        "ScrapQuantity",
                        "RejectQuantity",
                        "RebutQuantity");

                string joinClause =
                    string.IsNullOrWhiteSpace(boardForeignKey)
                        ? string.Empty
                        : " LEFT JOIN dbo.ProductionBoardHours AS hours" +
                          " ON hours.[" + boardForeignKey + "] = boards.Id ";

                if (string.IsNullOrWhiteSpace(boardForeignKey))
                {
                    targetColumn = null;
                    actualColumn = null;
                    scrapColumn = null;
                }

                string targetExpression =
                    BuildSumExpression(
                        targetColumn);

                string actualExpression =
                    BuildSumExpression(
                        actualColumn);

                string scrapExpression =
                    BuildSumExpression(
                        scrapColumn);

                string sql = @"
SELECT
    boards.BoardDate,
    boards.TeamName,
    " + targetExpression + @" AS TargetQuantity,
    " + actualExpression + @" AS ActualQuantity,
    " + scrapExpression + @" AS ScrapQuantity
FROM dbo.ProductionBoards AS boards
" + joinClause + @"
WHERE
    boards.BoardDate >= @WeekStart
    AND boards.BoardDate < @WeekEndExclusive
    AND (@TeamName IS NULL OR boards.TeamName = @TeamName)
    AND (@LineName IS NULL OR boards.LineName = @LineName)
    AND (@ProductName IS NULL OR boards.ProductName = @ProductName)
GROUP BY
    boards.BoardDate,
    boards.TeamName
ORDER BY
    boards.BoardDate ASC,
    boards.TeamName ASC;";

                ResultModel result =
                    new ResultModel
                    {
                        WeekStart = filter.WeekStart.Date,
                        WeekEnd = filter.WeekStart.Date.AddDays(6)
                    };

                using (SqlCommand command =
                       new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(
                        "@WeekStart",
                        SqlDbType.Date).Value =
                        filter.WeekStart.Date;

                    command.Parameters.Add(
                        "@WeekEndExclusive",
                        SqlDbType.Date).Value =
                        filter.WeekStart.Date.AddDays(7);

                    command.Parameters.Add(
                        "@TeamName",
                        SqlDbType.NVarChar,
                        100).Value =
                        ToDatabaseValue(filter.TeamName);

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

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Rows.Add(
                                new RowModel
                                {
                                    BoardDate = reader.GetDateTime(0),
                                    TeamName = reader.GetString(1),
                                    TargetQuantity = reader.GetInt32(2),
                                    ActualQuantity = reader.GetInt32(3),
                                    ScrapQuantity = reader.GetInt32(4)
                                });
                        }
                    }
                }

                return result;
            }
        }

        public IList<string> GetTeams()
        {
            return GetDistinctValues("TeamName");
        }

        public IList<string> GetProductionLines()
        {
            return GetDistinctValues("LineName");
        }

        public IList<string> GetProducts()
        {
            return GetDistinctValues("ProductName");
        }

        private IList<string> GetDistinctValues(
            string columnName)
        {
            string[] allowedColumns =
            {
                "TeamName",
                "LineName",
                "ProductName"
            };

            bool allowed = false;

            foreach (string allowedColumn in allowedColumns)
            {
                if (string.Equals(
                        allowedColumn,
                        columnName,
                        StringComparison.Ordinal))
                {
                    allowed = true;
                    break;
                }
            }

            if (!allowed)
            {
                throw new ArgumentException(
                    "Unsupported column name.",
                    nameof(columnName));
            }

            string sql =
                "SELECT DISTINCT [" + columnName + "]" +
                " FROM dbo.ProductionBoards" +
                " WHERE [" + columnName + "] IS NOT NULL" +
                " AND LTRIM(RTRIM([" + columnName + "])) <> N''" +
                " ORDER BY [" + columnName + "] ASC;";

            IList<string> values =
                new List<string>();

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        values.Add(reader.GetString(0));
                    }
                }
            }

            return values;
        }

        private static string FindFirstExistingColumn(
            SqlConnection connection,
            string tableName,
            params string[] candidates)
        {
            foreach (string candidate in candidates)
            {
                using (SqlCommand command =
                       new SqlCommand(
                           "SELECT COL_LENGTH(@TableName, @ColumnName);",
                           connection))
                {
                    command.Parameters.Add(
                        "@TableName",
                        SqlDbType.NVarChar,
                        256).Value = tableName;

                    command.Parameters.Add(
                        "@ColumnName",
                        SqlDbType.NVarChar,
                        128).Value = candidate;

                    object result =
                        command.ExecuteScalar();

                    if (result != null &&
                        result != DBNull.Value)
                    {
                        return candidate;
                    }
                }
            }

            return null;
        }

        private static string BuildSumExpression(
            string columnName)
        {
            return string.IsNullOrWhiteSpace(columnName)
                ? "CAST(0 AS INT)"
                : "CAST(ISNULL(SUM(ISNULL(hours.[" +
                  columnName +
                  "], 0)), 0) AS INT)";
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
