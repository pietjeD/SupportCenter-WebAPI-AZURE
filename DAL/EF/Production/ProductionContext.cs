using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using SC.BL.Domain;

namespace SC.DAL.EF.Production
{
    public class ProductionContext :DbContext
    {
        
        
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<HardwareTicket> HardwareTickets { get; set; }
        public DbSet<TicketResponse> TicketResponses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "ProductionDB.database.windows.net"; 
            builder.UserID = "PieterEnStijn";            
            builder.Password = "testing123!";     
            builder.InitialCatalog = "ProductionDB";

            optionsBuilder.UseSqlServer(new SqlConnection(builder.ConnectionString));
            
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