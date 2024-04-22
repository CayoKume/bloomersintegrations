using System.Net;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsEntrada.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<string> CallAPI(string body);
        public string BuildBodyRequest(string parametersList, string? endPointName, string authentication, string key, string? doc_company);
        public List<Dictionary<string, string>> DeserializeXML(string response);
    }
}
