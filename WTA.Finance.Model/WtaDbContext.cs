using System;
using Microsoft.EntityFrameworkCore;
namespace WTA.Finance.Model
{
    public partial class WtaDbContext: DbContext
    {
        public WtaDbContext(DbContextOptions<WtaDbContext> options) : base(options)
        {
            Console.WriteLine("This is JDDbContext DbContextOptions");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // var builder = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json");
            // var configuration = builder.Build();
            // var conn = configuration.GetConnectionString("JDDbConnection");
            // optionsBuilder.UseSqlServer(conn);

            //optionsBuilder.UseSqlServer(this._IConfiguration.GetConnectionString("JDDbConnection"));
            //optionsBuilder.UseLoggerFactory(new CustomEFLoggerFactory());

            //optionsBuilder.UseLoggerFactory(this._iLoggerFactory);

            //optionsBuilder.UseSqlServer(StaticConstraint.JDDbConnection);

            //optionsBuilder.UseSqlServer("Server=.;Database=advanced11;User id=sa;password=Passw0rd");

        }

        //public virtual DbSet<Category> Categories { get; set; }
        //public virtual DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Category>()
            //    .Property(e => e.Code)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Category>()
            //    .Property(e => e.ParentCode)
            //    .IsUnicode(false);
        }
    }
}
