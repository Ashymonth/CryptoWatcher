using CryptoWatcher.Modules.Uniswap.Application.Abstractions.OperationReaders;
using CryptoWatcher.Modules.Uniswap.Application.Services.PoisitionEventsSync.UniswapV3.Models.Operations;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockchain.Api;
using CryptoWatcher.Modules.Uniswap.Infrastructure.UniswapV3.Abstractions;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.UniswapV3.Services;

public class UniswapV3PositionOperationsSource : IPositionOperationsSource
{
    private readonly IUniswapTransactionLogsDecoderFactory _decoderFactory;
    private readonly IWeb3BlockchainApi _blockchainApi;

    public UniswapV3PositionOperationsSource(IUniswapTransactionLogsDecoderFactory decoderFactory,
        IWeb3BlockchainApi blockchainApi)
    {
        _decoderFactory = decoderFactory;
        _blockchainApi = blockchainApi;
    }

    public async Task<PositionOperation?> GetOperationFromTransactionAsync(UniswapChainConfiguration chainConfiguration,
        TransactionHash hash,
        CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var transactionReceipt =
            await _blockchainApi.GetTransactionReceiptAsync(chainConfiguration, hash);

        ct.ThrowIfCancellationRequested();

        return _decoderFactory.GetOperationFromTransaction(transactionReceipt);
    }
}