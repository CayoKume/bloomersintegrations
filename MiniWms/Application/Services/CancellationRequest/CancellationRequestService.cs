using BloomersMiniWmsIntegrations.Domain.Entities.CancellationRequest;
using BloomersMiniWmsIntegrations.Infrastructure.Repositorys;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public class CancellationRequestService : ICancellationRequestService
    {
        private readonly ICancellationRequestRepository _cancellationRequestRepository;

        public CancellationRequestService(ICancellationRequestRepository cancellationRequestRepository) =>
            (_cancellationRequestRepository) = (cancellationRequestRepository);

        public async Task CreateCancellationRequest(string serializedOrder)
        {
            try
            {
                var order = JsonConvert.DeserializeObject<Order>(serializedOrder);
                await _cancellationRequestRepository.CreateCancellationRequest(order);
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
                var reasons = await _cancellationRequestRepository.GetReasons();
                return JsonConvert.SerializeObject(reasons);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
