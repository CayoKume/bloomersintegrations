using BloomersMicrovixIntegrations.Saida.Core.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Ecommerce.Repositorys.Interfaces
{
    public interface IB2CConsultaClientesRepository<T1> : IMicrovixSaidaCoreRepository<T1> where T1 : class
    {
        public Task<string> GetLastTimestampClientesERP();
        public string GetLastTimestampClientesERPSync();
        public Task InsereRegistroIndividual(T1 registro, string? tableName, string? db);
        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db);
    }
}
