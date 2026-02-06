using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.BuildingBlocks.Modules.Consistency.Inbox.Repository;

public class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public const string SCHEMA_NAME = "inbox_messages";

    public virtual void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable(SCHEMA_NAME);
        builder.HasKey(c => new { c.Id });

        builder.Property(c => c.Id).HasColumnName("id").ValueGeneratedNever();
        builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
        builder.Property(c => c.UpdatedOn).HasColumnName("updated_on").IsRequired();
        builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
        builder.Property(c => c.UpdatedBy).HasColumnName("updated_by").HasMaxLength(80).IsRequired();

        builder.Property(c => c.Data).HasColumnName("data").IsRequired();
        builder.Property(c => c.Type).HasColumnName("type").HasMaxLength(80);
        builder.Property(c => c.Status).HasColumnName("status").IsRequired();
        builder.Property(c => c.OccurredOn).HasColumnName("occurred_on").IsRequired();
        builder.Property(c => c.ProcessedDate).HasColumnName("processed_date");
        builder.Property(c => c.ErrorMessage).HasColumnName("error_message");

    }
}