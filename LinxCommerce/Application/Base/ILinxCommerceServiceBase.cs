namespace BloomersCommerceIntegrations.LinxCommerce.Application.Base
{
    public interface ILinxCommerceServiceBase<TEntity> where TEntity : class, new()
    {
        public Task IntegraRegistros(string database);
        public void IntegraRegistrosSync(string database);
        public Task<bool> IntegraRegistrosIndividual(string database, string identificador);
        public bool IntegraRegistrosIndividualSync(string database, string identificador);
    }
}
