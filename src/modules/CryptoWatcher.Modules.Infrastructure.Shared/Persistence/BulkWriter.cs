using Microsoft.EntityFrameworkCore;
using Z.BulkOperations;

namespace CryptoWatcher.Modules.Infrastructure.Shared.Persistence;

public abstract class BulkWriter<TContext>
    where TContext : DbContext
{
    protected readonly TContext Db;

    protected BulkWriter(TContext db)
    {
        Db = db;
    }

    protected async ValueTask MergeAsync<T>(IReadOnlyList<T> entities, Action<BulkOperation<T>> configure,
        CancellationToken ct)
        where T : class
    {
        if (entities.Count == 0)
        {
            return;
        }

        await Db.BulkMergeAsync(entities, op =>
        {
            op.BatchSize = 5000;
            configure(op);
        }, ct);
    }
}