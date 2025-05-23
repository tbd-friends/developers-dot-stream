using Developers.Stream.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Developers.Stream.Infrastructure.Contexts.Configurations;

public class LabelEntityTypeConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        builder.Property(p => p.Text)
            .HasMaxLength(50);
    }
}