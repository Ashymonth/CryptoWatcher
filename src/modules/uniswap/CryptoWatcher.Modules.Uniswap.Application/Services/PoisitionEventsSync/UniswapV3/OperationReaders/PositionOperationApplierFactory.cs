using CryptoWatcher.Modules.Uniswap.Application.Abstractions.OperationReaders;
using CryptoWatcher.Modules.Uniswap.Application.Services.PoisitionEventsSync.UniswapV3.Models.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoWatcher.Modules.Uniswap.Application.Services.PoisitionEventsSync.UniswapV3.OperationReaders;

public class PositionOperationApplierFactory : IPositionOperationApplierFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PositionOperationApplierFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPositionMutationOperation GetOperationApplier(PositionOperation operation)
    {
        return _serviceProvider.GetRequiredKeyedService<IPositionMutationOperation>(operation.GetType());
    }
}