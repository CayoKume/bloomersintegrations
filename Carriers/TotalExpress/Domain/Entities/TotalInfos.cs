namespace BloomersCarriersIntegrations.TotalExpress.Domain.Entities
{
    public class TotalInfos
    {
        public class Destinatario
        {

            public string nome { get; set; }
            public string cpfCnpj { get; set; }
            public string ie { get; set; }
            public Endereco endereco { get; set; }
            public string email { get; set; }
            public string telefone1 { get; set; }
            public string telefone2 { get; set; }
            public string telefone3 { get; set; }
        }

        public class Endereco
        {
            public string logradouro { get; set; }
            public string numero { get; set; }
            public string complemento { get; set; }
            public string pontoReferencia { get; set; }
            public string bairro { get; set; }
            public string cidade { get; set; }
            public string estado { get; set; }
            public string pais { get; set; }
            public string cep { get; set; }
        }

        public class Cod
        {
            public string formaPagamento { get; set; }
            public int parcelas { get; set; }
            public decimal valor { get; set; }
        }

        public class Agendamento
        {
            public string data { get; set; }
            public string periodo1 { get; set; }
            public string periodo2 { get; set; }
        }

        public class Nfe
        {
            public int nfeNumero { get; set; }
            public string nfeSerie { get; set; }
            public string nfeData { get; set; }
            public decimal nfeValTotal { get; set; }
            public decimal nfeValProd { get; set; }
            public string nfeCfop { get; set; }
            public string nfeChave { get; set; }
        }

        public class DocFiscal
        {
            public List<Nfe> nfe { get; set; }
        }

        public class Encomendas
        {
            public int servicoTipo { get; set; }
            public string servicoTipoInfo { get; set; }
            public int entregaTipo { get; set; }
            public int peso { get; set; }
            public int volumes { get; set; }
            public string condFrete { get; set; }
            public string pedido { get; set; }
            public int clienteCodigo { get; set; }
            public string natureza { get; set; }
            public string volumesTipo { get; set; }

            public int icmsIsencao { get; set; }
            public string coletaInfo { get; set; }

            //public string campanha { get; set; }
            //public Cod cod { get; set; }
            public Destinatario destinatario { get; set; }

            public DocFiscal docFiscal { get; set; }
        }

        public class Registro
        {
            public string remetenteId { get; set; }
            public string cnpj { get; set; }
            public string remessaCodigo { get; set; }
            public List<Encomendas> encomendas { get; set; }
        }
    }
}
