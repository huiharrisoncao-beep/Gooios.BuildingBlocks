using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Npgsql;
using System.Data.Common;
using System.Data.SqlClient;

namespace Gooios.BuildingBlocks.Extensions;

public static class DbContextExtension
{
    public static DbParameter CreateParameter(this DbContext dbContext, string name, object? value)
    {
        var connection = dbContext.Database.GetDbConnection();

        return connection switch
        {
            MySqlConnection => new MySqlParameter(name, value ?? DBNull.Value),
            NpgsqlConnection => new NpgsqlParameter(name, value ?? DBNull.Value),
            // Add SqlConnection if needed:
            SqlConnection => new SqlParameter(name, value ?? DBNull.Value),
            _ => throw new NotSupportedException($"Database provider '{connection.GetType().Name}' not supported.")
        };
    }
}
