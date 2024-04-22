using BloomersMicrovixIntegrations.LinxMicrovixWsEntrada.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsEntrada.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsEntrada.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsEntrada.Application.Services
{
    public class LinxCadastraClientesFornecedores : ILinxCadastraClientesFornecedores
    {
        private readonly IAPICall _apiCall;
        private string KEY = LinxAPIAttributes.TypeEnum.key.ToName();
        private string AUTHENTICATION = LinxAPIAttributes.TypeEnum.authentication.ToName();

        public LinxCadastraClientesFornecedores(IAPICall apiCall) =>
            (_apiCall) = (apiCall);

        public async Task<bool> CreateClienteFornec(string doc_company, string doc_client, string reason_client, string address_client, string street_number_client, string zip_code_client, string city_client, string uf_client)
        {
            string type;
            if (doc_client.Length > 11)
                type = "J";
            else
                type = "F";

            var parameters = @$"<linx1:CommandParameter>
                                    <linx1:Name>codigo</linx1:Name>
                                    <linx1:Value>{doc_company}</linx1:Value>
                                </linx1:CommandParameter>
                                <linx1:CommandParameter>
                                    <linx1:Name>nome_razao_social</linx1:Name>
                                    <linx1:Value>{reason_client}</linx1:Value>
                                </linx1:CommandParameter>
                                <linx1:CommandParameter>
                                    <linx1:Name>doc_cliente</linx1:Name>
                                    <linx1:Value>{doc_client}</linx1:Value>
                                </linx1:CommandParameter>
                                <linx1:CommandParameter>
                                    <linx1:Name>pf_pj</linx1:Name>
                                    <linx1:Value>J</linx1:Value>
                                </linx1:CommandParameter>
                                <linx1:CommandParameter>
                                    <linx1:Name>endereco</linx1:Name>
                                    <linx1:Value>{address_client}</linx1:Value>
                                </linx1:CommandParameter>
                                <linx1:CommandParameter>
                                    <linx1:Name>numero_endereco</linx1:Name>
                                    <linx1:Value>{street_number_client}</linx1:Value>
                                </linx1:CommandParameter>
                                <linx1:CommandParameter>
                                    <linx1:Name>cep</linx1:Name>
                                    <linx1:Value>{zip_code_client}</linx1:Value>
                                </linx1:CommandParameter>
                                <linx1:CommandParameter>
                                    <linx1:Name>cidade</linx1:Name>
                                    <linx1:Value>{city_client}</linx1:Value>
                                </linx1:CommandParameter>
                                <linx1:CommandParameter>
                                    <linx1:Name>uf</linx1:Name>
                                    <linx1:Value>{uf_client}</linx1:Value>
                                </linx1:CommandParameter>
                                <linx1:CommandParameter>
                                    <linx1:Name>estado_civil</linx1:Name>
                                    <linx1:Value>1</linx1:Value>
                                </linx1:CommandParameter>
                                <linx1:CommandParameter>
                                    <linx1:Name>tipo</linx1:Name>
                                    <linx1:Value>{type}</linx1:Value>
                                </linx1:CommandParameter>";
            try
            {
                var body = _apiCall.BuildBodyRequest(parameters, "LinxCadastraClientesFornecedores", AUTHENTICATION, KEY, doc_company);
                var response = await _apiCall.CallAPI(body);

                if (response is not null)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
