using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SC.BL.Domain;

namespace SC.DAL.EF.Production
{
    public class ProductionRepository : ITicketRepository
    {
        private ProductionContext Pctx = null;

        public ProductionRepository()
        {
            Pctx = new ProductionContext();
        }
        
        public IEnumerable<Ticket> ReadTickets()
        {
            IEnumerable<Ticket> tickets = Pctx.Tickets
                                             .Include(t => t.Responses) // eager-loading 'Responses'
                                             .AsEnumerable();
            return tickets;
        }

        public Ticket CreateTicket(Ticket ticket)
        {
            Pctx.Tickets.Add(ticket);
            Pctx.SaveChanges();
                        
          
            return ticket;
        }

        public Ticket ReadTicket(int ticketNumber)
        {
            //Ticket ticket = ctx.Tickets.Single(t => t.TicketNumber == ticketNumber);
            Ticket ticket = Pctx.Tickets.Find(ticketNumber);
            return ticket;
        }

        public void UpdateTicket(Ticket ticket)
        {
            Pctx.Tickets.Update(ticket);
            Pctx.SaveChanges();
        }

        public void DeleteTicket(int ticketNumber)
        {
            Ticket ticketToDelete = this.ReadTicket(ticketNumber);
            Pctx.Tickets.Remove(ticketToDelete);
            Pctx.SaveChanges();
        }

        public IEnumerable<TicketResponse> ReadTicketResponsesOfTicket(int ticketNumber)
        {
            IEnumerable<TicketResponse> responses = Pctx.TicketResponses
                                                       .Where(response => response.Ticket.TicketNumber == ticketNumber)
                                                       .AsEnumerable();

            return responses;
            
            /* Explicit-loading */
            //Ticket ticket = ctx.Tickets.Find(ticketNumber);
            //ctx.Entry(ticket).Collection(t => t.Responses).Load();
            //return ticket.Responses;
        }

        public TicketResponse CreateTicketResponse(TicketResponse response)
        {
            Pctx.TicketResponses.Add(response);
            Pctx.SaveChanges();

            return response;
        }
    }
}