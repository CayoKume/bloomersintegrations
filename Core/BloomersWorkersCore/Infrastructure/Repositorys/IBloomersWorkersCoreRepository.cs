using BloomersWorkersCore.Domain.Entities;

namespace BloomersWorkersCore.Infrastructure.Repositorys
{
    public interface IBloomersWorkersCoreRepository
    {
        public Task<MicrovixUser> GetMicrovixUser(string gabot);
    }
}
