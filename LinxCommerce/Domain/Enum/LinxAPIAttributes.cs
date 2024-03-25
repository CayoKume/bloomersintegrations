using System.ComponentModel;

namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Enums
{
    public class LinxAPIAttributes
    {
        public enum TypeEnum
        {
            #region Database
            [Description("BLOOMERS_LINX")]
            Producao,

            [Description("HOMOLOG_BLOOMERS_LINX")]
            Homologacao,
            #endregion

            [Description(@"layer.misha")]
            authentication,

            [Description(@"y5RuoAA")]
            chave,
        }
    }
}
