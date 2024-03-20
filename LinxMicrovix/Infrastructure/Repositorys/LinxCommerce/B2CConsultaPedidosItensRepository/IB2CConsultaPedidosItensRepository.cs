using BloomersMicrovixIntegrations.Saida.Core.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Ecommerce.Repositorys.Interfaces
{
    public interface IB2CConsultaPedidosItensRepository<T1> : IMicrovixSaidaCoreRepository<T1> where T1 : class, new()
    {
        public Task<string> GetLastTimestampPedidosItens();
        public string GetLastTimestampPedidosItensSync();
        public Task InsereRegistroIndividual(T1 registro, string? tableName, string? db);
        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db);
    }
}
