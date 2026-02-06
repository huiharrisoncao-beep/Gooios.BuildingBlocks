using Dapper;
using Gooios.BuildingBlocks.Extensions;
using Gooios.BuildingBlocks.Infrastructure.Repository;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Domain;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Gooios.BuildingBlocks.Modules.Consistency.Inbox.Repository;

public class InboxMessageRepository<TDbContext> : Repository<InboxMessage, Guid, TDbContext>, IInboxMessageRepository
    where TDbContext : DbContext
{
    readonly ILogger _logger;
    public InboxMessageRepository(IDbContextProvider<TDbContext> provider, ILogger logger) : base(provider)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<InboxMessage>> ObtainInboxMessagesToProcessAsync(string inboxMessageSchemaName)
    {
        var messages = new List<InboxMessage>();

        using (DbConnection connection = ServiceDbContext!.Database.GetDbConnection())
        {
            var sqlQuery = $"select * from {inboxMessageSchemaName} where status=1";
            messages = (await connection.QueryAsync<InboxMessage>(sqlQuery)).ToList();

            var ids = messages.Select(item => $"'{item.Id}'").ToList();
            var idsStr = string.Join(',', ids);
            if (!string.IsNullOrEmpty(idsStr))
            {
                var updateSql = $"Update {inboxMessageSchemaName} set status=1 where status=3 and id in ({idsStr})";
                connection.Execute(updateSql);
            }

        }

        return messages;
    }

    public async Task<int> SetInboxMessageStatusAsync(string inboxMessageSchemaName, Guid id, InboxMessageStatus status, string? errorMessage = null)
    {
        var formattedSql = FormattableStringFactory.Create($"Update {inboxMessageSchemaName} set status=@status, error_message=@errorMessage where id=@id");

        var statusParam = ServiceDbContext.CreateParameter("@status", status);
        var errorMessageParam = ServiceDbContext.CreateParameter("@errorMessage", errorMessage);
        var idParam = ServiceDbContext.CreateParameter("@id", id);

        return await ServiceDbContext.Database.ExecuteSqlRawAsync(formattedSql.Format
                                              , statusParam
                                              , errorMessageParam
                                              , idParam);
    }
}