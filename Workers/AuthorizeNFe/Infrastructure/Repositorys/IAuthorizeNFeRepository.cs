using BloomersWorkers.AuthorizeNFe.Domain.Entities;
using BloomersWorkersCore.Domain.Entities;

namespace BloomersWorkers.AuthorizeNFe.Infrastructure.Repositorys
{
    public interface IAuthorizeNFeRepository
    {
        public Task<List<Order>> GetPendingNFesFromB2CConsultaNFe();
        public Task<MicrovixUser> GetMicrovixUser(string gabot);
    }
}
