using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PrintRemittance.Core.Entities;

namespace PrintRemittance.Core;

public class PrintRemittanceDbContext : DbContext
{
    public PrintRemittanceDbContext()
    {
        
    }
    public PrintRemittanceDbContext(DbContextOptions<PrintRemittanceDbContext> options)
        :base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json")
              .Build();

        string connectionString = configuration.GetConnectionString("SqlConnectionString") ?? string.Empty;

        optionsBuilder.UseSqlServer(connectionString);
    }

    public DbSet<Document> Documents { get; set; }
}
