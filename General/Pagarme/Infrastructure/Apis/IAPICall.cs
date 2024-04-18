using BloomersGeneralIntegrations.Pagarme.Domain.Entities;

namespace BloomersGeneralIntegrations.Pagarme.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<Root?> GetAsync(string dataInicio, string dataFinal);
    }
}
