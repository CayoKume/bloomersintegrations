using NewBloomersWebApplication.Domain.Entities.Home;

namespace NewBloomersWebApplication.Application.Services
{
    public interface IHomeService
    {
        public Task<List<Order>?> GetPickupOrders(string cnpj_emp);
    }
}
