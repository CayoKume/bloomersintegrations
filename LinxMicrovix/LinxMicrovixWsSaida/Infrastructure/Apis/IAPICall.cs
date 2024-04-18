using System.Net;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<string> CallAPIAsync(string endPointName, string body);
        public string CallAPINotAsync(string endPointName, string body);
        public string BuildBodyRequest(string parametersList, string? endPointName, string authentication, string key, string? cnpj);
        public string BuildBodyRequest(string? endPointName, string authentication, string key);
        public List<Dictionary<string, string>> DeserializeXML(string response);
        public HttpWebRequest CreateClient(string route, byte[] bytes);
    }
}
