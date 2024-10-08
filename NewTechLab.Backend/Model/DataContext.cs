using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NewTechLab.DTOs;

namespace NewTechLab.Backend.Model
{
    public class DataContext : DbContext
    {
        public DbSet<Student> Student { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=NewTechDB;Username=postgres;Password=boberman");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
