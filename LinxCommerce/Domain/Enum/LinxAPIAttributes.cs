using System.ComponentModel;

namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Enums
{
    public class LinxAPIAttributes
    {
        public enum TypeEnum
        {
            [Description("LINX_COMMERCE")]
            Producao,

            [Description("HOMOLOG_LINX_COMMERCE")]
            Homologacao,

            [Description(@"layer.misha")]
            authentication,

            [Description(@"y5RuoAA")]
            chave,
        }
    }
}
