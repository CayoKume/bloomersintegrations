using BloomersGeneralIntegrations.Mobsim.Domain.Entities;

namespace BloomersGeneralIntegrations.Mobsim.Infrastructure.Repositorys
{
    public interface IMobsimRepository
    {
        public Task<Client> GetClient(string cod_client);
        public Task<List<IT4_WMS_Documento>> GetInvoicedOrders();
        public Task<List<IT4_WMS_Documento>> GetShippedOrders();
        public Task<List<IT4_WMS_Documento>> GetDeliveredOrders();
        public Task InsertMobsimHistoric(Guid idMobsim, string order, string client);
        public Task UpdateStatusMobsimHistoric(string order, bool sended, bool delivered);
        public Task<MobsimHistoricoModel> HasLogInMobsimHistoric(string order);
    }
}
