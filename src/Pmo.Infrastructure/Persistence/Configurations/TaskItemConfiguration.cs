using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pmo.Domain.Entities;

namespace Pmo.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> b)
    {
        b.Property(t => t.Title).HasMaxLength(200).IsRequired();
        b.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(t => t.Priority).HasConversion<string>().HasMaxLength(20);
        b.Property(t => t.Version).IsRowVersion();

        b.HasOne(t => t.Project).WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(t => t.Assignee).WithMany()
            .HasForeignKey(t => t.AssigneeId).OnDelete(DeleteBehavior.SetNull);

        b.HasIndex(t => new { t.ProjectId, t.Status, t.DueDate });

        b.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Task_Progress", "\"ProgressPct\" BETWEEN 0 AND 100");
            t.HasCheckConstraint("CK_Task_Weight", "\"Weight\" > 0");
        });
    }
}
