using BloomersGeneralIntegrations.AfterSale.Domain.Entities;
using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersGeneralIntegrations.AfterSale.Infrastructure.Repositorys;

public interface IAfterSaleRepository
{
    public Task<IEnumerable<Company>> GetCompanys();
    public Task<BearerToken> GetBearerToken(string doc_company);
    public Task InsertIntoTable(List<Reverses> registros);
    public Task CallDbProcMerge();
}
