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
            //  optionsBuilder.UseNpgsql("User ID=postgres;Password=123;Host=localhost;Port=5432;Database=TransactionMgnt;Pooling=true;");
        }

        public IConfigurationRoot GetConnection()

        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();

            return builder;

        }



        //public IActionResult Index()

        //{

        //    SqlConnection conSql = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

        //    return View();

        //}
    }
}
