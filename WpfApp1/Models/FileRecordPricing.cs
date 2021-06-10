using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class FileRecordPricing
    {
        public string SKU { get; set; }

        public int minSellerAllowedPrice { get; set; } = 0;

        public int maxSellerAllowedPrice { get; set; } = 0;

        public string countryCode { get; set; } = "IT";

        public string currencyCode { get; set; } = "EUR";

        public string ruleName { get; set; } = "Down";

        public string ruleAction { get; set; } = "Down-Business";

        public string businessRuleName { get; set; } = "START";

        public string name{ get;set; }

        public string ToFormatFlatFile()
        {
            return SKU + "\t"+ minSellerAllowedPrice
                 + "\t" + maxSellerAllowedPrice + "\t" + countryCode
                  + "\t" + ruleName + "\t" + ruleAction + "\t" + businessRuleName + "\n";
        }
    }
}
