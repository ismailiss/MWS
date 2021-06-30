using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWSAPP.Models
{
    public class FileRecordInventory
    {
        public string SKU { get; set; }

        public string ProductId { get; set; } 

        public int ProductIdType { get; set; }

        public decimal? Price { get; set; }


        public decimal? MinSellerAllowedPrice { get; set; } 

        public decimal? MaxSellerAllowedPrice { get; set; } 

        public int ItemCondition { get; set; } = 11;

        public int Quantity { get; set; }

        public string AddDelete{ get;set; } = "a";

        public string ItemNote { get; set; } = "";

        public string ExpeditedShipping { get; set; } = "";

        public string Product_tax_code { get; set; } = "";

        public int HandlingTime { get; set; } = 2;

        // public string willShipInternationally { get; set; } = "";

        public string ToFormatFlatFile()
        {

            string priceString = Price.HasValue ? Price.Value.ToString("0.00").Replace('.', ',') : "";
            string MinSellerAllowedPriceString = MinSellerAllowedPrice.HasValue ?
                MinSellerAllowedPrice.Value.ToString("0.00").Replace('.', ',') : "";
            string MaxSellerAllowedPriceString = MaxSellerAllowedPrice.HasValue ? MaxSellerAllowedPrice.Value.ToString("0.00").Replace('.', ',') : "";

            string format = $"{SKU}\t{ProductId}\t{ProductIdType}\t {priceString}" +
                            $"\t{MinSellerAllowedPriceString }\t{MaxSellerAllowedPriceString}\t{ItemCondition}\t" +
                            $"{Quantity}\t{AddDelete }\t{ItemNote }\t{ExpeditedShipping }\t{Product_tax_code}" +
                            $"\t{ HandlingTime }";
            return format;


        }
    }
}
