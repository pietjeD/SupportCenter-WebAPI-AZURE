using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Proxies; // NuGet-package!
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug; // NuGet-package!
using SC.BL.Domain;

namespace SC.DAL.EF
{
    internal class SupportCenterDbContext : DbContext
    {
        public SupportCenterDbContext()
        {
            SupportCenterDbInitializer.Initialize(this, dropCreateDatabase: false);
        }
        
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<HardwareTicket> HardwareTickets { get; set; }
        public DbSet<TicketResponse> TicketResponses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=supportcenter1/supportcenterDB");
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "supportcenter1.database.windows.net"; 
            builder.UserID = "PieterEnStijn";            
            builder.Password = "testing123!";     
            builder.InitialCatalog = "supportcenterDB";

            optionsBuilder.UseSqlServer(new SqliteConnection(builder.ConnectionString));
            //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["supportcdnterDB"].ConnectionString);
            // configure logging-information
            optionsBuilder.UseLoggerFactory(new LoggerFactory(
                new[] { new DebugLoggerProvider(
                    (category, level) => category == DbLoggerCategory.Database.Command.Name
                                         && level == LogLevel.Information
                )}
            ));

            // configure lazy-loading: requires ALL navigation-properties to be 'virtual'!!
            //optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().HasIndex(t => t.State);

            modelBuilder.Entity<TicketResponse>().Property<int>("TicketFK");
            modelBuilder.Entity<TicketResponse>().HasOne(tr => tr.Ticket).WithMany(t => t.Responses)
                                                 .HasForeignKey("TicketFK");
        }
    }
}