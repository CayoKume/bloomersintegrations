using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public interface ILinxProdutosTabelasService<T1> : ILinxMicrovixServiceBase<T1> where T1 : class, new()
    {
        public Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador, string cnpj_emp);
        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador, string cnpj_emp);
    }
}
