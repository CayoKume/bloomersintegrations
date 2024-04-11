using BloomersMicrovixIntegrations.LinxMicrovix.Application.Services.Base;

namespace BloomersMicrovixIntegrations.Application.Services.LinxCommerce
{
    public interface IB2CConsultaNFeService<TEntity> : ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
        public Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador);
        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador);
    }
}
