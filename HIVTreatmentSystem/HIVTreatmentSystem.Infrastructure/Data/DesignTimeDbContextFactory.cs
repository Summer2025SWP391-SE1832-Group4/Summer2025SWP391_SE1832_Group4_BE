using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HIVTreatmentSystem.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HIVDbContext>
    {
        public HIVDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Server=(localdb)\\mssqllocaldb;Database=HIVTreatmentSystem;Trusted_Connection=True;";

            var optionsBuilder = new DbContextOptionsBuilder<HIVDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new HIVDbContext(optionsBuilder.Options);
        }
    }
} 