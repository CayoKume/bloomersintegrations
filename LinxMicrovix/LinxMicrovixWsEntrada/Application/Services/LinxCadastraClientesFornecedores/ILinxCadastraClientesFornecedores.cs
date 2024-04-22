namespace BloomersMicrovixIntegrations.LinxMicrovixWsEntrada.Application.Services
{
    public interface ILinxCadastraClientesFornecedores
    {
        public Task<bool> CreateClienteFornec(string doc_company, string doc_client, string reason_client, string address_client, string street_number_client, string zip_code_client, string city_client, string uf_client);
    }
}
