using ExamWithDesktop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamWithDesktop.Data.Context
{
    public class ExamWithDesktopDbCOntext : DbContext
    {
        protected virtual DbSet<User> Users { get; set; }
        protected virtual DbSet<Attachments> Attachments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = "Host=localhost;" +
                "Port=5432;" +
                "Database=ExamDb;" +
                "Username=postgres;" +
                "Password=muham1812";
            optionsBuilder.UseNpgsql(path);
        }
    }
}
