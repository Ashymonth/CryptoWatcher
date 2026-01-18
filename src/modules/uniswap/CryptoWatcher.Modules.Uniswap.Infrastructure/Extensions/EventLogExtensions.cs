using System.Numerics;
using CryptoWatcher.ValueObjects;
using Nethereum.Contracts;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Extensions;

public static class EventLogExtensions
{
    public static Token MapEventToToken(this EventLog<TransferEventDTO> log)
    {
        return new Token
        {
            Address = EvmAddress.Create(log.Log.Address),
            Balance = log.Event.Value
        };
    }

    public static Token MapEventToToken(this EventLog<TransferEventDTO> log, BigInteger amount)
    {
        return new Token
        {
            Address = EvmAddress.Create(log.Log.Address),
            Balance = log.Event.Value
        };
    }
}