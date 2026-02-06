namespace Gooios.BuildingBlocks.Infrastructure.Repository;

public interface IDbConfiguration
{
    DatabaseType DatabaseType { get; }
    string ConnectionString { get; }
}

public enum DatabaseType
{
    MySql,
    PostgreSql
}