using BloomersGeneralIntegrations.Dootax.Domain.Entities;

namespace BloomersGeneralIntegrations.Dootax.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<XML> PostXmlAsync(XML xml);
    }
}
