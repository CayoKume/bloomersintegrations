using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public interface ILinxProdutosInventarioService<TEntity> : ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
        public Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador, string identificador2, string cnpj_emp);
        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador, string identificador2, string cnpj_emp);
    }
}
