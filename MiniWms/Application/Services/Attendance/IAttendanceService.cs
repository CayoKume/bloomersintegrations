namespace BloomersMiniWmsIntegrations.Application.Services
{
    public interface IAttendanceService
    {
        public Task<string> GetOrdersToContact(string serie, string doc_company);
        public Task<bool> UpdateDateContacted(string number, string atendente, string obs);
    }
}
