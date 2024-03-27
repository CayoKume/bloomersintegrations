using BloomersCommerceIntegrations.LinxCommerce.Application.Base;

namespace BloomersCommerceIntegrations.LinxCommerce.Application.Services
{
    public interface ISKUService<TEntity> : ILinxCommerceServiceBase<TEntity> where TEntity : class, new()
    {
    }
}
