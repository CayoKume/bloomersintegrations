using BloomersGeneralIntegrations.Pagarme.Domain.Entities;

namespace BloomersGeneralIntegrations.Pagarme.Infrastructure.Repositorys
{
    public interface IPagarmeRepository
    {
        public Task InsereReceivableInDatabase(Root root);
    }
}
