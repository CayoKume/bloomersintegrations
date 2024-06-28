namespace BloomersMiniWmsIntegrations.Application.Services
{
    public interface ICompanyService
    {
        public Task<string> GetCompanys();
        public Task<string> GetUsers();
    }
}
