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

        public decimal MinSellerAllowedPrice { get; set; } = 0;

        public decimal MaxSellerAllowedPrice { get; set; } = 0;

        public string CountryCode { get; set; } = "IT";

        public string CurrencyCode { get; set; } = "EUR";

        public string RuleName { get; set; } = "Down";

        public string RuleAction { get; set; } = "START";

        public string BusinessRuleName { get; set; } = "Down-Business";

        public string BusinessRuleAction { get; set; } = "START";


        public string ToFormatFlatFile()
        {
            return SKU + "\t"+ MinSellerAllowedPrice
                 + "\t" + MaxSellerAllowedPrice + "\t" + CountryCode + "\t" + CurrencyCode
                  + "\t" + RuleName + "\t" + RuleAction + "\t" + BusinessRuleName + 
                  "\t" + BusinessRuleAction;
        }
    }
}
