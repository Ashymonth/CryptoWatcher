using CryptoWatcher.AaveModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWatcher.Infrastructure.Configuration.Aave;

public class AavePositionConfiguration : IEntityTypeConfiguration<AavePosition>
{
    public void Configure(EntityTypeBuilder<AavePosition> builder)
    {
        builder.Property(aavePosition => aavePosition.Network).HasMaxLength(32);
        builder.Property(aavePosition => aavePosition.WalletAddress).HasMaxLength(64);
        builder.Property(aavePosition => aavePosition.TokenAddress).HasMaxLength(64);

        builder.HasMany(aavePosition => aavePosition.PositionSnapshots)
            .WithOne()
            .HasForeignKey(snapshot => snapshot.PositionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}