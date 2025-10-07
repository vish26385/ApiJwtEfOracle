using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = ApiJwtEfOracle.Models.Task;

namespace ApiJwtEfOracle.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable("TASKS");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.Title).HasColumnName("TITLE").HasMaxLength(200);
            builder.Property(x => x.Description).HasColumnName("DESCRIPTION");
            builder.Property(x => x.DueDate).HasColumnName("DUEDATE");
            builder.Property(x => x.Completed).HasColumnName("COMPLETED");
            builder.Property(x => x.Priority).HasColumnName("PRIORITY");
            builder.Property(x => x.UserId).HasColumnName("USERID");
        }
    }
}
