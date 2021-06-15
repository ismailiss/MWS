using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWSAPP.Models
{
    public class FileRecordInventory
    {
        public string SKU { get; set; }

        public decimal? ProductId { get; set; } 

        public int? ProductIdType { get; set; }

        public decimal? Price { get; set; }


        public string MinSellerAllowedPrice { get; set; } = "EUR";

        public string MaxSellerAllowedPrice { get; set; } = "Down";

        public int ItemCondition { get; set; } = 11;

        public int Quantity { get; set; } = 0;

        public string AddDelete{ get;set; } = "a";

        public string ItemNote { get; set; } = "a";

        public string ExpeditedShipping { get; set; } = "";

        public string Product_tax_code { get; set; } = "";

        public int HandlingTime { get; set; } = 2;

        // public string willShipInternationally { get; set; } = "";

        public string ToFormatFlatFile()
        {
            string priceString = Price.HasValue ? Price.Value.ToString("0,##") : "";
        
            string format = $"{SKU }\t{ProductId}{ProductIdType}\t {priceString}" +
                            $"\t{MinSellerAllowedPrice }\t{MaxSellerAllowedPrice}\t{ItemCondition}\t" +
                            $"{Quantity}\t{AddDelete }\t{ItemNote }\t{ExpeditedShipping }\t{Product_tax_code }" +
                            $"\t{ HandlingTime }";
            return format;


        }
    }
}
