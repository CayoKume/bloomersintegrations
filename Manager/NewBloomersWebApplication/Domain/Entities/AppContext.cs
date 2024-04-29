using Microsoft.AspNetCore.Components.Web;

namespace NewBloomersWebApplication.Domain.Entities
{
    public class AppContext
    {
        public class DateInterval
        {
            public string? shippingCompany { get; set; }
            public DateTime initialDate { get; set; }
            public DateTime finalDate { get; set; }
        }

        public class Enter
        {
            public KeyboardEventArgs e { get; set; }
            public string? orderNumber { get; set; }
        }

        public static class User
        {
            public static string? user { get; set; }
            public static string? reason_company { get; set; }
            public static string? doc_company { get; set; }
            public static int cod_company { get; set; }
            public static string serie_order { get; set; }
        }

        public static class Company
        {
            public static string? reason_company { get; set; }
            public static string? doc_company { get; set; }
            public static int cod_company { get; set; }
            public static string serie_order { get; set; }
        }
    }
}
