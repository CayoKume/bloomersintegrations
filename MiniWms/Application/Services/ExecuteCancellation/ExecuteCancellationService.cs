using BloomersMiniWmsIntegrations.Infrastructure.Repositorys;
using Newtonsoft.Json;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public class ExecuteCancellationService : IExecuteCancellationService
    {
        private readonly IExecuteCancellationRepository _executeCancellationRepository;

        public ExecuteCancellationService(IExecuteCancellationRepository executeCancellationRepository) =>
            (_executeCancellationRepository) = (executeCancellationRepository);

        public async Task<string> GetOrdersToCancel(string serie, string doc_company)
        {
            try
            {
                var orders = await _executeCancellationRepository.GetOrdersToCancel(serie, doc_company);
                return JsonConvert.SerializeObject(orders);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetReasons()
        {
            try
            {
                var reasons = await _executeCancellationRepository.GetReasons();
                return JsonConvert.SerializeObject(reasons);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateDateCanceled(string number, string suporte, string inputObs)
        {
            try
            {
                return await _executeCancellationRepository.UpdateDateCanceled(number, suporte, inputObs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
