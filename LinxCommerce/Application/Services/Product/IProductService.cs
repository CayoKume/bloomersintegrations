using BloomersCommerceIntegrations.LinxCommerce.Application.Base;

namespace BloomersCommerceIntegrations.LinxCommerce.Application.Services
{
    public interface IProductService<TEntity> : ILinxCommerceServiceBase<TEntity> where TEntity : class, new()
    {
    }
}
