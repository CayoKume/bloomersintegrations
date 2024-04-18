using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public interface ILinxMovimentoPlanosService<TEntity> : ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
        public Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador, string identificador2);
        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador, string identificador2);
    }
}
