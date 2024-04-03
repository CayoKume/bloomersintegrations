using BloomersWorkersCore.Domain.Entities;

namespace BloomersWorkers.ChangingPassword.Infrastructure.Repositorys
{
    public interface IChangingPasswordRepository
    {
        public Task<List<MicrovixUser>> GetMicrovixUser();
        public Task UpdateLastupdateonFromMicrovixUsers(MicrovixUser usuario);
    }
}
