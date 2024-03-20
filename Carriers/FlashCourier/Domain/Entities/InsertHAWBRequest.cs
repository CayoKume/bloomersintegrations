namespace BloomersCarriersIntegrations.FlashCourier.Domain.Entities
{
    public class InsertHAWBRequest
    {
        public InsertHAWBRequest()
        {

        }
        public class EndHawbs
        {
            public string nome_des { get; set; }
            public string logr_dest { get; set; }
            public string bairro_des { get; set; }
            public string num_des { get; set; }
            public string compl_end_dest { get; set; }
            public string cid_dest { get; set; }
            public string uf_dest { get; set; }
            public int cep_dest { get; set; }
            public string fone1_des { get; set; }
            public string fone2_des { get; set; }
            public string fone3_des { get; set; }
        }

        public class EndHawbs2
        {
            public string nome_des { get; set; }
            public string logr_dest { get; set; }
            public string bairro_des { get; set; }
            public string num_des { get; set; }
            public string compl_end_dest { get; set; }
            public string cid_dest { get; set; }
            public string uf_dest { get; set; }
            public object cep_dest { get; set; }
            public string fone1_des { get; set; }
            public string fone2_des { get; set; }
            public string fone3_des { get; set; }
        }

        public class Root
        {
            public int cliente_id { get; set; }
            public int ctt_id { get; set; }
            public int dna_hawb { get; set; }
            public int ccusto_id { get; set; }
            public int tipo_enc_id { get; set; }
            public int prod_flash_id { get; set; }
            public string frq_rec_id { get; set; }
            public object ctt_ter_id { get; set; }
            public object tipo_merc_id { get; set; }
            public string num_enc_cli { get; set; }
            public string num_enc_ter { get; set; }
            public string num_cliente { get; set; }
            public string nome_rem { get; set; }
            public EndHawbs endHawbs { get; set; }
            public string depto_des { get; set; }
            public string cod_lote { get; set; }
            public object peso_aferido { get; set; }
            public double peso_declarado { get; set; }
            public string qtde_itens { get; set; }
            public double valor { get; set; }
            public EndHawbs2 endHawbs2 { get; set; }
            public string depto_des_2 { get; set; }
            public int id_local_rem { get; set; }
            public string cpf_des { get; set; }
            public string cnpj_des { get; set; }
            public string ie_des { get; set; }
            public string email_des { get; set; }
            public string agrupamento { get; set; }
            public string obs1 { get; set; }
            public string obs2 { get; set; }
            public string obs3 { get; set; }
            public string obs4 { get; set; }
            public string ge1 { get; set; }
            public string ge2 { get; set; }
            public string ge3 { get; set; }
            public string ge4 { get; set; }
            public string chave_nf { get; set; }
        }
    }
}
