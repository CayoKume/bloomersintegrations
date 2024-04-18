using BloomersGeneralIntegrations.Movidesk.Domain.Entities;

namespace BloomersGeneralIntegrations.Movidesk.Application.Services
{
    public interface IMovideskService
    {
        public Task<bool> InsertTicket(string request);
        public Task<bool> UpdateTicket(string request);
        public Task BuildInvoicesFromTickets();
    }
}
