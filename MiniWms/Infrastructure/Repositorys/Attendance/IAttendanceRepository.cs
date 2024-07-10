using BloomersMiniWmsIntegrations.Domain.Entities.Attendence;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface IAttendanceRepository
    {
        public Task<IEnumerable<Order>> GetOrdersToContact(string serie, string doc_company);
        public Task<bool> UpdateDateContacted(string number, string atendente, string obs);
    }
}
