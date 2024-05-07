using BloomersIntegrationsCore.Domain.Entities;

namespace NewBloomersWebApplication.Application.Services
{
    public interface ICompanyService
    {
        public Task<List<Company>?> GetCompanies();
    }
}
