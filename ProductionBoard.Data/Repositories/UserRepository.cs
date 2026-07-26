using System;
using System.Data;
using System.Data.SqlClient;

using global::ProductionBoard.Core.Interfaces;

using UserModel =
    global::ProductionBoard.Core.Models.ApplicationUser;

using AssignmentModel =
    global::ProductionBoard.Core.Models.UserShiftAssignment;

namespace ProductionBoard.Data.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public UserRepository(
            ConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(
                    nameof(connectionFactory));
            }

            _connectionFactory = connectionFactory;
        }

        public UserModel FindByUsername(
            string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            const string sql = @"
SELECT TOP (1)
    Id,
    Username,
    FullName,
    PasswordHash,
    Role,
    IsActive,
    MustChangePassword,
    FailedLoginCount,
    LockedUntilUtc,
    LastLoginAtUtc
FROM dbo.Users
WHERE Username = @Username;";

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@Username",
                    SqlDbType.NVarChar,
                    80).Value = username.Trim();

                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return MapUser(reader);
                }
            }
        }

        public AssignmentModel GetCurrentAssignment(
            int userId,
            DateTime effectiveDate)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(userId),
                    "The user ID must be greater than zero.");
            }

            const string sql = @"
SELECT TOP (1)
    Id,
    UserId,
    TeamName,
    ShiftName,
    EffectiveFrom,
    EffectiveTo,
    IsActive
FROM dbo.UserShiftAssignments
WHERE UserId = @UserId
  AND IsActive = 1
  AND EffectiveFrom <= @EffectiveDate
  AND
  (
      EffectiveTo IS NULL
      OR EffectiveTo >= @EffectiveDate
  )
ORDER BY EffectiveFrom DESC, Id DESC;";

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@UserId",
                    SqlDbType.Int).Value =
                    userId;

                command.Parameters.Add(
                    "@EffectiveDate",
                    SqlDbType.Date).Value =
                    effectiveDate.Date;

                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return MapAssignment(reader);
                }
            }
        }

        public void RecordSuccessfulLogin(
            int userId,
            DateTime loginTimeUtc)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(userId));
            }

            const string sql = @"
UPDATE dbo.Users
SET
    FailedLoginCount = 0,
    LockedUntilUtc = NULL,
    LastLoginAtUtc = @LastLoginAtUtc,
    UpdatedAtUtc = SYSUTCDATETIME()
WHERE Id = @UserId;";

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@UserId",
                    SqlDbType.Int).Value =
                    userId;

                command.Parameters.Add(
                    "@LastLoginAtUtc",
                    SqlDbType.DateTime2).Value =
                    loginTimeUtc;

                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void RecordFailedLogin(
            int userId,
            int failedLoginCount,
            DateTime? lockedUntilUtc)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(userId));
            }

            if (failedLoginCount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(failedLoginCount));
            }

            const string sql = @"
UPDATE dbo.Users
SET
    FailedLoginCount = @FailedLoginCount,
    LockedUntilUtc = @LockedUntilUtc,
    UpdatedAtUtc = SYSUTCDATETIME()
WHERE Id = @UserId;";

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@UserId",
                    SqlDbType.Int).Value =
                    userId;

                command.Parameters.Add(
                    "@FailedLoginCount",
                    SqlDbType.Int).Value =
                    failedLoginCount;

                command.Parameters.Add(
                    "@LockedUntilUtc",
                    SqlDbType.DateTime2).Value =
                    lockedUntilUtc.HasValue
                        ? (object)lockedUntilUtc.Value
                        : DBNull.Value;

                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        private static UserModel MapUser(
            SqlDataReader reader)
        {
            int lockedUntilOrdinal =
                reader.GetOrdinal(
                    "LockedUntilUtc");

            int lastLoginOrdinal =
                reader.GetOrdinal(
                    "LastLoginAtUtc");

            return new UserModel
            {
                Id = reader.GetInt32(
                    reader.GetOrdinal("Id")),

                Username = reader.GetString(
                    reader.GetOrdinal("Username")),

                FullName = reader.GetString(
                    reader.GetOrdinal("FullName")),

                PasswordHash = reader.GetString(
                    reader.GetOrdinal("PasswordHash")),

                Role = reader.GetString(
                    reader.GetOrdinal("Role")),

                IsActive = reader.GetBoolean(
                    reader.GetOrdinal("IsActive")),

                MustChangePassword = reader.GetBoolean(
                    reader.GetOrdinal(
                        "MustChangePassword")),

                FailedLoginCount = reader.GetInt32(
                    reader.GetOrdinal(
                        "FailedLoginCount")),

                LockedUntilUtc =
                    reader.IsDBNull(lockedUntilOrdinal)
                        ? (DateTime?)null
                        : reader.GetDateTime(
                            lockedUntilOrdinal),

                LastLoginAtUtc =
                    reader.IsDBNull(lastLoginOrdinal)
                        ? (DateTime?)null
                        : reader.GetDateTime(
                            lastLoginOrdinal)
            };
        }

        private static AssignmentModel MapAssignment(
            SqlDataReader reader)
        {
            int effectiveToOrdinal =
                reader.GetOrdinal(
                    "EffectiveTo");

            return new AssignmentModel
            {
                Id = reader.GetInt32(
                    reader.GetOrdinal("Id")),

                UserId = reader.GetInt32(
                    reader.GetOrdinal("UserId")),

                TeamName = reader.GetString(
                    reader.GetOrdinal("TeamName")),

                ShiftName = reader.GetString(
                    reader.GetOrdinal("ShiftName")),

                EffectiveFrom = reader.GetDateTime(
                    reader.GetOrdinal(
                        "EffectiveFrom")),

                EffectiveTo =
                    reader.IsDBNull(effectiveToOrdinal)
                        ? (DateTime?)null
                        : reader.GetDateTime(
                            effectiveToOrdinal),

                IsActive = reader.GetBoolean(
                    reader.GetOrdinal("IsActive"))
            };
        }
    }
}