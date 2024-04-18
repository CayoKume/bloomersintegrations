using BloomersGeneralIntegrations.Mobsim.Domain.Entities;

namespace BloomersGeneralIntegrations.Mobsim.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task PostAsync(MobsimObject body);
    }
}
