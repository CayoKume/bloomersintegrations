using BloomersGeneralIntegrations.Movidesk.Domain.Entities;

namespace BloomersGeneralIntegrations.Movidesk.Infrastructure.Repositorys
{
    public interface IMovideskRepository
    {
        public Task<List<Ticket>> GetBlankTicketsFromDatabase();
        public Task<List<Ticket>> GetTicketsFromDatabase();
        public Task UpdateTicketFromDatabase(List<Ticket> tickets);
        public Task UpdateTicketStatusFromDatabase(Invoice invoice);
        public Task<bool> InsertInvoice(Invoice invoice);
        public Task<bool> InsertTicket(Ticket? ticket);
    }
}
