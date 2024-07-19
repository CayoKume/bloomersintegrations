using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomersCarriersIntegrations.Jadlog.Domain.Entities
{
    public class Product : BloomersIntegrationsCore.Domain.Entities.Product
    {
        public double weight_product { get; set; }
    }
}
