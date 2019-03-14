using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SC.BL.Domain;

namespace SC.DAL.EF
{
    internal class SupportCenterDbContext : DbContext
    {
        
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<HardwareTicket> HardwareTickets { get; set; }
        public DbSet<TicketResponse> TicketResponses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
          
            builder.DataSource = "dotnetdeploymentserver.database.windows.net";
            builder.UserID = "Pieter";
            builder.Password = "testing123!";
            builder.InitialCatalog = "testingDB";
           
            optionsBuilder.UseSqlServer(new SqlConnection(builder.ConnectionString));
            
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
