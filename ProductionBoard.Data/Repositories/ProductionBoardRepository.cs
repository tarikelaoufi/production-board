using System;
using System.Data;
using System.Data.SqlClient;
using global::ProductionBoard.Core.Interfaces;

using BoardModel =
    global::ProductionBoard.Core.Models.ProductionBoard;

using HourModel =
    global::ProductionBoard.Core.Models.ProductionHour;

namespace ProductionBoard.Data.Repositories
{
    public sealed class ProductionBoardRepository
        : IProductionBoardRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public ProductionBoardRepository(
            ConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(
                    nameof(connectionFactory));
            }

            _connectionFactory = connectionFactory;
        }

        public BoardModel GetById(int boardId)
        {
            if (boardId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(boardId),
                    "The board ID must be greater than zero.");
            }

            const string sql = @"
SELECT
    Id,
    BoardDate,
    TeamName,
    ShiftName,
    LineName,
    ProductName,
    CreatedAt,
    UpdatedAt
FROM dbo.ProductionBoards
WHERE Id = @Id;";

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@Id",
                    SqlDbType.Int).Value = boardId;

                connection.Open();

                BoardModel board;

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    board = MapProductionBoard(reader);
                }

                LoadHours(connection, board);

                return board;
            }
        }

        public BoardModel Find(
            DateTime boardDate,
            string teamName,
            string shiftName,
            string lineName,
            string productName)
        {
            ValidateRequiredText(
                teamName,
                nameof(teamName));

            ValidateRequiredText(
                shiftName,
                nameof(shiftName));

            ValidateRequiredText(
                lineName,
                nameof(lineName));

            ValidateRequiredText(
                productName,
                nameof(productName));

            const string sql = @"
SELECT
    Id,
    BoardDate,
    TeamName,
    ShiftName,
    LineName,
    ProductName,
    CreatedAt,
    UpdatedAt
FROM dbo.ProductionBoards
WHERE BoardDate = @BoardDate
  AND TeamName = @TeamName
  AND ShiftName = @ShiftName
  AND LineName = @LineName
  AND ProductName = @ProductName;";

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@BoardDate",
                    SqlDbType.Date).Value =
                    boardDate.Date;

                command.Parameters.Add(
                    "@TeamName",
                    SqlDbType.NVarChar,
                    100).Value =
                    teamName.Trim();

                command.Parameters.Add(
                    "@ShiftName",
                    SqlDbType.NVarChar,
                    50).Value =
                    shiftName.Trim();

                command.Parameters.Add(
                    "@LineName",
                    SqlDbType.NVarChar,
                    100).Value =
                    lineName.Trim();

                command.Parameters.Add(
                    "@ProductName",
                    SqlDbType.NVarChar,
                    150).Value =
                    productName.Trim();

                connection.Open();

                BoardModel board;

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    board = MapProductionBoard(reader);
                }

                LoadHours(connection, board);

                return board;
            }
        }

        public int Create(BoardModel board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            ValidateRequiredText(
                board.TeamName,
                nameof(board.TeamName));

            ValidateRequiredText(
                board.ShiftName,
                nameof(board.ShiftName));

            ValidateRequiredText(
                board.LineName,
                nameof(board.LineName));

            ValidateRequiredText(
                board.ProductName,
                nameof(board.ProductName));

            const string insertBoardSql = @"
INSERT INTO dbo.ProductionBoards
(
    BoardDate,
    TeamName,
    ShiftName,
    LineName,
    ProductName
)
VALUES
(
    @BoardDate,
    @TeamName,
    @ShiftName,
    @LineName,
    @ProductName
);

SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            {
                connection.Open();

                using (SqlTransaction transaction =
                       connection.BeginTransaction())
                {
                    try
                    {
                        int boardId;

                        using (SqlCommand command =
                               new SqlCommand(
                                   insertBoardSql,
                                   connection,
                                   transaction))
                        {
                            command.Parameters.Add(
                                "@BoardDate",
                                SqlDbType.Date).Value =
                                board.BoardDate.Date;

                            command.Parameters.Add(
                                "@TeamName",
                                SqlDbType.NVarChar,
                                100).Value =
                                board.TeamName.Trim();

                            command.Parameters.Add(
                                "@ShiftName",
                                SqlDbType.NVarChar,
                                50).Value =
                                board.ShiftName.Trim();

                            command.Parameters.Add(
                                "@LineName",
                                SqlDbType.NVarChar,
                                100).Value =
                                board.LineName.Trim();

                            command.Parameters.Add(
                                "@ProductName",
                                SqlDbType.NVarChar,
                                150).Value =
                                board.ProductName.Trim();

                            object result =
                                command.ExecuteScalar();

                            boardId =
                                Convert.ToInt32(result);
                        }

                        if (board.Hours != null)
                        {
                            foreach (HourModel hour in board.Hours)
                            {
                                if (hour == null)
                                {
                                    continue;
                                }

                                hour.ProductionBoardId = boardId;

                                InsertHour(
                                    connection,
                                    transaction,
                                    hour);
                            }
                        }

                        transaction.Commit();

                        board.Id = boardId;

                        return boardId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void SaveHour(HourModel hour)
        {
            ValidateHour(hour);

            const string sql = @"
UPDATE dbo.ProductionBoardHours
SET
    HourLabel = @HourLabel,
    TargetQuantity = @TargetQuantity,
    ActualQuantity = @ActualQuantity,
    ScrapQuantity = @ScrapQuantity,
    Comment = @Comment,
    StopType = @StopType,
    StopDurationMinutes = @StopDurationMinutes,
    UpdatedAt = SYSUTCDATETIME()
WHERE ProductionBoardId = @ProductionBoardId
  AND HourNumber = @HourNumber;

IF @@ROWCOUNT = 0
BEGIN
    INSERT INTO dbo.ProductionBoardHours
    (
        ProductionBoardId,
        HourNumber,
        HourLabel,
        TargetQuantity,
        ActualQuantity,
        ScrapQuantity,
        Comment,
        StopType,
        StopDurationMinutes
    )
    VALUES
    (
        @ProductionBoardId,
        @HourNumber,
        @HourLabel,
        @TargetQuantity,
        @ActualQuantity,
        @ScrapQuantity,
        @Comment,
        @StopType,
        @StopDurationMinutes
    );
END;";

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                AddHourParameters(command, hour);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static void LoadHours(
            SqlConnection connection,
            BoardModel board)
        {
            const string sql = @"
SELECT
    Id,
    ProductionBoardId,
    HourNumber,
    HourLabel,
    TargetQuantity,
    ActualQuantity,
    ScrapQuantity,
    Comment,
    StopType,
    StopDurationMinutes
FROM dbo.ProductionBoardHours
WHERE ProductionBoardId = @ProductionBoardId
ORDER BY HourNumber;";

            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@ProductionBoardId",
                    SqlDbType.Int).Value =
                    board.Id;

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        board.Hours.Add(
                            MapProductionHour(reader));
                    }
                }
            }
        }

        private static void InsertHour(
            SqlConnection connection,
            SqlTransaction transaction,
            HourModel hour)
        {
            ValidateHour(hour);

            const string sql = @"
INSERT INTO dbo.ProductionBoardHours
(
    ProductionBoardId,
    HourNumber,
    HourLabel,
    TargetQuantity,
    ActualQuantity,
    ScrapQuantity,
    Comment,
    StopType,
    StopDurationMinutes
)
VALUES
(
    @ProductionBoardId,
    @HourNumber,
    @HourLabel,
    @TargetQuantity,
    @ActualQuantity,
    @ScrapQuantity,
    @Comment,
    @StopType,
    @StopDurationMinutes
);";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                AddHourParameters(command, hour);
                command.ExecuteNonQuery();
            }
        }

        private static void AddHourParameters(
            SqlCommand command,
            HourModel hour)
        {
            command.Parameters.Add(
                "@ProductionBoardId",
                SqlDbType.Int).Value =
                hour.ProductionBoardId;

            command.Parameters.Add(
                "@HourNumber",
                SqlDbType.TinyInt).Value =
                hour.HourNumber;

            command.Parameters.Add(
                "@HourLabel",
                SqlDbType.NVarChar,
                5).Value =
                string.IsNullOrWhiteSpace(hour.HourLabel)
                    ? hour.HourNumber.ToString()
                    : hour.HourLabel.Trim();

            command.Parameters.Add(
                "@TargetQuantity",
                SqlDbType.Int).Value =
                hour.TargetQuantity;

            command.Parameters.Add(
                "@ActualQuantity",
                SqlDbType.Int).Value =
                hour.ActualQuantity;

            command.Parameters.Add(
                "@ScrapQuantity",
                SqlDbType.Int).Value =
                hour.ScrapQuantity;

            command.Parameters.Add(
                "@Comment",
                SqlDbType.NVarChar,
                1000).Value =
                ToDatabaseValue(hour.Comment);

            command.Parameters.Add(
                "@StopType",
                SqlDbType.NVarChar,
                100).Value =
                ToDatabaseValue(hour.StopType);

            command.Parameters.Add(
                "@StopDurationMinutes",
                SqlDbType.Int).Value =
                hour.StopDurationMinutes;
        }

        private static BoardModel MapProductionBoard(
            SqlDataReader reader)
        {
            return new BoardModel
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

                UpdatedAt = reader.IsDBNull(
                    reader.GetOrdinal("UpdatedAt"))
                    ? (DateTime?)null
                    : reader.GetDateTime(
                        reader.GetOrdinal("UpdatedAt"))
            };
        }

        private static HourModel MapProductionHour(
            SqlDataReader reader)
        {
            return new HourModel
            {
                Id = reader.GetInt32(
                    reader.GetOrdinal("Id")),

                ProductionBoardId = reader.GetInt32(
                    reader.GetOrdinal(
                        "ProductionBoardId")),

                HourNumber = Convert.ToInt32(
                    reader["HourNumber"]),

                HourLabel = reader.GetString(
                    reader.GetOrdinal("HourLabel")),

                TargetQuantity = reader.GetInt32(
                    reader.GetOrdinal(
                        "TargetQuantity")),

                ActualQuantity = reader.GetInt32(
                    reader.GetOrdinal(
                        "ActualQuantity")),

                ScrapQuantity = reader.GetInt32(
                    reader.GetOrdinal(
                        "ScrapQuantity")),

                Comment = reader.IsDBNull(
                    reader.GetOrdinal("Comment"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Comment")),

                StopType = reader.IsDBNull(
                    reader.GetOrdinal("StopType"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("StopType")),

                StopDurationMinutes = reader.GetInt32(
                    reader.GetOrdinal(
                        "StopDurationMinutes"))
            };
        }

        private static void ValidateHour(HourModel hour)
        {
            if (hour == null)
            {
                throw new ArgumentNullException(nameof(hour));
            }

            if (hour.ProductionBoardId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.ProductionBoardId),
                    "A valid production board ID is required.");
            }

            if (hour.HourNumber < 1 ||
                hour.HourNumber > 8)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.HourNumber),
                    "The hour number must be between 1 and 8.");
            }

            if (hour.TargetQuantity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.TargetQuantity),
                    "Target quantity cannot be negative.");
            }

            if (hour.ActualQuantity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.ActualQuantity),
                    "Actual quantity cannot be negative.");
            }

            if (hour.ScrapQuantity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.ScrapQuantity),
                    "Scrap quantity cannot be negative.");
            }

            if (hour.StopDurationMinutes < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.StopDurationMinutes),
                    "Stop duration cannot be negative.");
            }
        }

        private static object ToDatabaseValue(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? (object)DBNull.Value
                : value.Trim();
        }

        private static void ValidateRequiredText(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "A value is required.",
                    parameterName);
            }
        }
    }
}