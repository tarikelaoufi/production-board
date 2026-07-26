using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ProductionBoard.Data
{
    public sealed class ConnectionFactory
    {
        private readonly string _connectionString;

        public ConnectionFactory(
            string connectionStringName = "ProductionBoardConnection")
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[
                    connectionStringName
                ];

            if (settings == null ||
                string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{connectionStringName}' " +
                    "was not found in Web.config.");
            }

            _connectionString = settings.ConnectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}