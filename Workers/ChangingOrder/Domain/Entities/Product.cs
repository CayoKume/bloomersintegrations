namespace BloomersWorkers.ChangingOrder.Domain.Entities
{
    public class Product
    {
        public int cod_product_microvix { get; set; }
        public int cod_product_volo { get; set; }
        public int qtde_microvix { get; set; }
        public int qtde_volo { get; set; }

        public int CompareTo(Product other)
        {
            if (this.cod_product_microvix > other.cod_product_microvix)
                return 1;
            else if (this.cod_product_microvix < other.cod_product_microvix)
                return -1;
            else if (this.cod_product_volo > other.cod_product_volo)
                return 1;
            else if (this.cod_product_volo < other.cod_product_volo)
                return -1;
            else
                return 0;
        }
    }
}
