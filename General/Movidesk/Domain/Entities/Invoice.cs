namespace BloomersGeneralIntegrations.Movidesk.Domain.Entities
{
    public class Invoice
    {
        public string nome_empresa { get; set; }
        public string cnpj_empresa { get; set; }

        public int id_fornecedor { get; set; }
        public string nome_fantasia_fornecedor { get; set; }
        public string cnpj_fornecedor { get; set; }

        public string endereco_fornecedor { get; set; }
        public string numero_endereco_fornecedor { get; set; }
        public string uf_fornecedor { get; set; }
        public string cidade_fornecedor { get; set; }
        public string cep_fornecedor { get; set; }

        public string agencia_fornecedor { get; set; }
        public string banco_fornecedor { get; set; }
        public string conta_corrente_fornecedor { get; set; }

        public string IdFaturaCadastrada { get; set; }
        public string forma_pagamento_fatura { get; set; }
        public string codigo_barras_fatura { get; set; }
        public string pix_fatura { get; set; }
        public long numero_documento_fatura { get; set; }
        public long serie_documento_fatura { get; set; }
        public DateTime data_emissao_fatura { get; set; }
        public DateTime data_vencimento_fatura { get; set; }
        public DateTime data_lancamento_fatura { get; set; }
        public int numero_parcela_fatura { get; set; }
        public int total_parcelas_fatura { get; set; }
        public decimal valor_fatura { get; set; }
        public string centro_custo_fatura { get; set; }
        public string historico_contabil_fatura { get; set; }
        public string obs { get; set; }

        public string serializable { get; set; }
        public int id_ticket { get; set; }
    }

    public class SerializedInvoice
    {
        public int IdContaFluxo { get; set; }
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdFornecedor { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdCentroCusto { get; set; }
        public int IdHistoricoContabil { get; set; }
        public int IdCategoriaFinanceira { get; set; }
        public int IdPortador { get; set; }
        public string ValorFatura { get; set; }
        public int NumeroParcela { get; set; }
        public int TotalParcelas { get; set; }
        public long NumeroDocumento { get; set; }
        public long SerieDocumento { get; set; }
        public string DataEmissao { get; set; }
        public string DataVencimento { get; set; }
        public string DataLancamento { get; set; }
        public string Observacao { get; set; }
    }
}
