using Microsoft.EntityFrameworkCore;

namespace WebShop.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserInfo> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasData(
        //            new User(1, "Tom", "123"),
        //            new User(2, "Bob", "123"),
        //            new User(3, "Sam", "123")
        //    );
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }
    }
}
