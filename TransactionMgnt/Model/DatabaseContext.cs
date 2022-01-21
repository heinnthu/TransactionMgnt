using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TransactionMgnt
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {        
                 optionsBuilder.UseNpgsql(GetConnection().GetSection("ConnectionStrings").GetSection("Default").Value);
          
        }

        public IConfigurationRoot GetConnection()

        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            return builder;

        }
    }
}
