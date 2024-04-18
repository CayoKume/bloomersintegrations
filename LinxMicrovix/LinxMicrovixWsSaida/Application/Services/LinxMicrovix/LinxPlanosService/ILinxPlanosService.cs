using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public interface ILinxPlanosService<TEntity> : ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
        public Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador);
        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador);
    }
}
