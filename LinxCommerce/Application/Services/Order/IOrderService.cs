using BloomersCommerceIntegrations.LinxCommerce.Application.Base;

namespace BloomersCommerceIntegrations.LinxCommerce.Application.Services
{
    public interface IOrderService<TEntity> : ILinxCommerceServiceBase<TEntity> where TEntity : class, new()
    {
        public Task IntegraRegistrosIndividual(string database, string identificador);
    }
}
