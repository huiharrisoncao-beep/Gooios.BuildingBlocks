using Dapper;
using Gooios.BuildingBlocks.Extensions;
using Gooios.BuildingBlocks.Infrastructure.Repository;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Domain;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Gooios.BuildingBlocks.Modules.Consistency.Outbox.Repository;

public class OutboxMessageRepository<TDbContext> : Repository<OutboxMessage, Guid, TDbContext>, IOutboxMessageRepository
where TDbContext : DbContext
{
    readonly ILogger _logger;
    public OutboxMessageRepository(IDbContextProvider<TDbContext> provider, ILogger logger) : base(provider)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<OutboxMessage>> ObtainOutboxMessagesToProcessAsync(string outboxMessageSchemaName)
    {
        var messages = new List<OutboxMessage>();

        using (DbConnection connection = ServiceDbContext!.Database.GetDbConnection())
        {
            var sqlQuery = $"select * from {outboxMessageSchemaName} where status=1";
            messages = (await connection.QueryAsync<OutboxMessage>(sqlQuery)).ToList();

            var ids = messages.Select(item => $"'{item.Id}'").ToList();
            var idsStr = string.Join(',', ids);
            if (!string.IsNullOrEmpty(idsStr))
            {
                var updateSql = $"Update {outboxMessageSchemaName} set status=1 where status=3 and id in ({idsStr})";
                await connection.ExecuteAsync(updateSql);
            }
        }

        return messages;
    }

    public async Task<int> SetOutboxMessageStatusAsync(string outboxMessageSchemaName, Guid id, OutboxMessageStatus status, string? errorMessage = null)
    {
        var formattedSql = FormattableStringFactory.Create($"Update {outboxMessageSchemaName} set Status=@status, error_message=@errorMessage where id=@id");

        var statusParam = ServiceDbContext.CreateParameter("@status", status);
        var errorMessageParam = ServiceDbContext.CreateParameter("@errorMessage", errorMessage);
        var idParam = ServiceDbContext.CreateParameter("@id", id);

        return await ServiceDbContext!.Database.ExecuteSqlRawAsync(formattedSql.Format
                                              , statusParam
                                              , errorMessageParam
                                              , idParam);
    }

}