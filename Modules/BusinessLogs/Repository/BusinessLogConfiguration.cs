using Gooios.BuildingBlocks.Modules.BusinessLogs.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.BuildingBlocks.Modules.BusinessLogs.Repository;

public class BusinessLogConfiguration : IEntityTypeConfiguration<BusinessLog>
{
    public const string SCHEMA_NAME = "business_logs";

    public void Configure(EntityTypeBuilder<BusinessLog> builder)
    {
        builder.ToTable(SCHEMA_NAME);
        builder.HasKey(c => new { c.Id });

        builder.Property(c => c.Id).HasColumnName("id").HasMaxLength(36);
        builder.Property(c => c.OperateName).HasColumnName("operate_name").HasMaxLength(120).IsRequired();
        builder.Property(c => c.SchemaName).HasColumnName("schema_name").HasMaxLength(120).IsRequired();
        builder.Property(c => c.SchemaKeyValue).HasColumnName("schema_key_value").HasMaxLength(120).IsRequired();
        builder.Property(c => c.OldValue).HasColumnName("old_value");
        builder.Property(c => c.NewValue).HasColumnName("new_value");
        builder.Property(c => c.Status).HasColumnName("status");
        builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
        builder.Property(c => c.UpdatedOn).HasColumnName("updated_on").IsRequired();
        builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
        builder.Property(c => c.UpdatedBy).HasColumnName("updated_by").HasMaxLength(80).IsRequired();

    }
}
