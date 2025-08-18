using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using CryptoWatcher.Abstractions;
using CryptoWatcher.Entities;
using CryptoWatcher.Entities.Hyperliquid;
using Z.BulkOperations;

namespace CryptoWatcher.Data;

/// <summary>
/// <see cref="IRepository{TEntity}"/>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class EfRepository<TEntity> : RepositoryBase<TEntity>, IRepository<TEntity> where TEntity : class
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly Dictionary<Type, List<string>> Type2PrimaryKeyFields = new()
    {
        [typeof(PoolPosition)] =
        [
            nameof(PoolPosition.PositionId), nameof(PoolPosition.NetworkName)
        ],
        [typeof(PoolPositionSnapshot)] =
        [
            nameof(PoolPositionSnapshot.Day), nameof(PoolPositionSnapshot.PoolPositionId),
            nameof(PoolPositionSnapshot.NetworkName)
        ],
        [typeof(HyperliquidVaultPosition)] =
        [
            nameof(HyperliquidVaultPosition.VaultAddress), nameof(HyperliquidVaultPosition.WalletAddress)
        ],
        [typeof(HyperliquidVaultEvent)] =
        [
            nameof(HyperliquidVaultPosition.VaultAddress), nameof(HyperliquidVaultPosition.WalletAddress),
            nameof(HyperliquidVaultPositionSnapshot.Day)
        ],
        [typeof(HyperliquidVaultPositionSnapshot)] =
        [
            nameof(HyperliquidVaultPositionSnapshot.WalletAddress),
            nameof(HyperliquidVaultPositionSnapshot.VaultAddress),
            nameof(HyperliquidVaultPositionSnapshot.Day)
        ],
    };

    private readonly CryptoWatcherDbContext _dbContext;

    public EfRepository(CryptoWatcherDbContext dbContext, IUnitOfWork unitOfWork) :
        base(dbContext)
    {
        _dbContext = dbContext;
        UnitOfWork = unitOfWork;
    }

    public EfRepository(CryptoWatcherDbContext dbContext, ISpecificationEvaluator specificationEvaluator,
        IUnitOfWork unitOfWork) : base(dbContext, specificationEvaluator)
    {
        _dbContext = dbContext;
        UnitOfWork = unitOfWork;
    }

    public async Task BulkMergeWithGraphAsync(IList<TEntity> entities, CancellationToken ct)
    {
        if (entities.Count == 0)
        {
            return;
        }

        await _dbContext.BulkMergeAsync(entities, operation =>
        {
            operation.IncludeGraph = true;
            operation.IncludeGraphOperationBuilder = bulkOperation =>
            {
                if (Type2PrimaryKeyFields.TryGetValue(bulkOperation.EntityType.ClrType, out var primaryKeys))
                {
                    bulkOperation.ColumnPrimaryKeyNames = primaryKeys;
                }
            };
        }, ct);
    }

    public async Task BulkMergeAsync(IList<TEntity> entities, CancellationToken ct)
    {
        if (entities.Count == 0)
        {
            return;
        }

        await _dbContext.BulkMergeAsync(entities, operation => operation.ColumnPrimaryKeyExpression =
                entity => Type2PrimaryKeyFields.GetValueOrDefault(typeof(TEntity)), ct);
    }

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken ct)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, ct);

        return entity;
    }

    public void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public IUnitOfWork UnitOfWork { get; }
}