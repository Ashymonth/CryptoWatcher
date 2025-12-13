using CryptoWatcher.Modules.Uniswap.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWatcher.Infrastructure.Configuration.Uniswap;

public class UniswapLiquidityPositionCashFlowConfiguration : IEntityTypeConfiguration<UniswapLiquidityPositionCashFlow>
{
    public void Configure(EntityTypeBuilder<UniswapLiquidityPositionCashFlow> builder)
    {
        builder.HasKey(flow => new { flow.PositionId, flow.NetworkName, flow.TransactionHash }); 
    }
}