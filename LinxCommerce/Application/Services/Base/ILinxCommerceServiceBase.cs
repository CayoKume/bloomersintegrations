namespace BloomersCommerceIntegrations.LinxCommerce.Application.Base
{
    public interface ILinxCommerceServiceBase<TEntity>
    {
        public Task IntegraRegistros(string database);
    }
}
