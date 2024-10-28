using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using WebAPI.Models.STR.Localidades;
using WebAPI.Models.STR.Usuario;

namespace WebAPI.BD
{
    public class AplicacionDBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<UserModel> Usuario { get; set; }
        public AplicacionDBContext()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Localidades>()
                .HasNoKey();
        }

        public DbSet<Localidades> Localidades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ConnectionSQL"));
        }


    }
}
