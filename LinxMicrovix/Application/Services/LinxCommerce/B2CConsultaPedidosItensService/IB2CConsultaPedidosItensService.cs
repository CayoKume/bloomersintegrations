using BloomersMicrovixIntegrations.Saida.Core.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Ecommerce.Services.Interfaces
{
    public interface IB2CConsultaPedidosItensService<T1> : IMicrovixSaidaCoreService<T1> where T1 : class, new()
    {
        public Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador);
        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador);
    }
}
