using NewBloomersWebApplication.Domain.Entities.Companys;

namespace NewBloomersWebApplication.Application.Services
{
    public interface ICompanyService
    {
        public Task<List<Company>?> GetCompanies();
    }
}
