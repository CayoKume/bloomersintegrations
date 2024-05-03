using System.Net;

namespace BloomersMiniWmsIntegrations.Infrastructure.Apis.Labels
{
    public interface IAPICall
    {
        public bool CallAPI(byte[] zpl, string path, string number, bool typeLabel);
    }
}
