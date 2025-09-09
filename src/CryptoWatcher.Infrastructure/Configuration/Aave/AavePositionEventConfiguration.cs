using CryptoWatcher.AaveModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWatcher.Infrastructure.Configuration.Aave;

public class AavePositionEventConfiguration : IEntityTypeConfiguration<AavePositionEvent >
{
    public void Configure(EntityTypeBuilder<AavePositionEvent> builder)
    {
        
    }
}