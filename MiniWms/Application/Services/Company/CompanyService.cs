using BloomersMiniWmsIntegrations.Infrastructure.Repositorys;
using Newtonsoft.Json;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository) =>
            (_companyRepository) = (companyRepository);

        public async Task<string> GetCompanys()
        {
            var list = await _companyRepository.GetCompanys();
            return JsonConvert.SerializeObject(list);
        }//
    }
}
