using BloomersIntegrationsCore.Domain.Entities;
using BloomersMiniWmsIntegrations.Domain.Entities.Company;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<Company>?> GetCompanys();
        public Task<IEnumerable<User>?> GetUsers();
    }
}
