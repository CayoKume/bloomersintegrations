using BloomersMicrovixIntegrations.Saida.Core.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces
{
    public interface ILinxXMLDocumentosService<T1> : IMicrovixSaidaCoreService<T1> where T1 : class, new()
    {
        public Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador1, string identificador2, string cnpj_emp);
        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador1, string identificador2, string cnpj_emp);
    }
}
