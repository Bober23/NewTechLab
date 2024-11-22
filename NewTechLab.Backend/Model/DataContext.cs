using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NewTechLab.DTOs;

namespace NewTechLab.Backend.Model
{
    public class DataContext : DbContext
    {
        public DbSet<Student> Student { get; set; }
        public DataContext()
        {
            
        }
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string ip = "147.45.183.210";
            optionsBuilder.UseNpgsql("Host=147.45.183.210;Port=5432;Database=labdb;Username=user;Password=12345");
            Console.WriteLine(ip);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
