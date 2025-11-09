using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Map UserRole enum to string column in AspNetUsers
            var userRoleConverter = new EnumToStringConverter<UserRole>();
            builder.Entity<ApplicationUser>()
                .Property(u => u.Role)
                .HasConversion(userRoleConverter)
                .HasMaxLength(64)
                .IsRequired()
                .HasDefaultValue(UserRole.Performer);

            // Map DocumentStatus and TaskStatus to strings for readability
            builder.Entity<Document>()
                .Property(d => d.Status)
                .HasConversion(new EnumToStringConverter<DocumentStatus>())
                .HasMaxLength(64)
                .IsRequired()
                .HasDefaultValue(DocumentStatus.Draft);

            builder.Entity<TaskItem>()
                .Property(t => t.Status)
                .HasConversion(new EnumToStringConverter<Models.TaskStatus>())
                .HasMaxLength(64)
                .IsRequired()
                .HasDefaultValue(Models.TaskStatus.Open);

            // Document -> Tasks (cascade on delete)
            builder.Entity<TaskItem>()
                .HasOne(t => t.Document)
                .WithMany(d => d.Tasks)
                .HasForeignKey(t => t.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Task.Assignee -> ApplicationUser (restrict delete)
            builder.Entity<TaskItem>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Task.CreatedByUser -> ApplicationUser (restrict delete)
            builder.Entity<TaskItem>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            builder.Entity<Document>()
                .HasIndex(d => d.Status)
                .HasDatabaseName("IX_Documents_Status");

            builder.Entity<TaskItem>()
                .HasIndex(t => t.AssigneeId)
                .HasDatabaseName("IX_Tasks_AssigneeId");

            builder.Entity<TaskItem>()
                .HasIndex(t => t.DocumentId)
                .HasDatabaseName("IX_Tasks_DocumentId");

            // If you prefer user index on Role:
            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Role)
                .HasDatabaseName("IX_Users_Role");
        }
    }
}