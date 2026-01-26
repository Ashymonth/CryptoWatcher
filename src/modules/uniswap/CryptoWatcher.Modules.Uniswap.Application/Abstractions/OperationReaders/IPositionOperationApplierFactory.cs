using CryptoWatcher.Modules.Uniswap.Application.Services.PoisitionEventsSync.UniswapV3.Models.Operations;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions.OperationReaders;

public interface IPositionOperationApplierFactory
{
    IPositionMutationOperation GetOperationApplier(PositionOperation operation);
}