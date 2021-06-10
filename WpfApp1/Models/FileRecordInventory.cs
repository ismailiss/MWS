using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class FileRecordInventory
    {
        public string SKU { get; set; }

        public ulong productId { get; set; } = 0;

        public int productIdType { get; set; } = 0;

        public decimal price { get; set; } = 0;

        public string minSellerAllowedPrice { get; set; } = "EUR";

        public string maxSellerAllowedPrice { get; set; } = "Down";

        public int itemCondition { get; set; } = 11;

        public int quantity { get; set; } = 0;

        public string addDelete{ get;set; } = "a";

        public string ItemNote { get; set; } = "a";


        public string ToFormatFlatFile()
        {
            return SKU + "\t"+ minSellerAllowedPrice
                 + "\t" + maxSellerAllowedPrice + "\t" + countryCode
                  + "\t" + ruleName + "\t" + ruleAction + "\t" + businessRuleName + "\n";
        }
    }
}
