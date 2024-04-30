using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<Company>?> GetCompanys();
    }
}
