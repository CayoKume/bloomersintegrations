using BloomersGeneralIntegrations.Dootax.Domain.Entities;

namespace BloomersGeneralIntegrations.Dootax.Infrastructure.Repositorys
{
    public interface IDootaxRepository
    {
        public Task<IEnumerable<XML>> GetXMLs();
        public Task InsertSendXMLOk_Log(string cnpjcpf, string documento, string serie, string chavenfe);
    }
}
