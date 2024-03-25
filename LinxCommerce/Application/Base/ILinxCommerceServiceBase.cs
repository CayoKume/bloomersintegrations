namespace BloomersCommerceIntegrations.LinxCommerce.Application.Base
{
    public interface ILinxCommerceServiceBase<TEntity> where TEntity : class, new()
    {
        public Task IntegraRegistros(string tableName, string procName, string database);
        public void IntegraRegistrosSync(string tableName, string procName, string database);
        public Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador);
        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador);
    }
}
