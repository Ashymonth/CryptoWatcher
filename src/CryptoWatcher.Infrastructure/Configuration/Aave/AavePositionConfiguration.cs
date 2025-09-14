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

        builder.Navigation(position => position.PositionSnapshots).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(position => position.PositionEvents).UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.HasMany(aavePosition => aavePosition.PositionSnapshots)
            .WithOne()
            .HasForeignKey(snapshot => snapshot.PositionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(aavePosition => aavePosition.PositionEvents)
            .WithOne()
            .HasForeignKey(snapshot => snapshot.PositionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}