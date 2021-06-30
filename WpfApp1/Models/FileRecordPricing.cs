using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWSAPP.Models
{
    public class FileRecordPricing
    {
        public string SKU { get; set; }

        public decimal? MinSellerAllowedPrice { get; set; } 

        public decimal? MaxSellerAllowedPrice { get; set; } 

        public string CountryCode { get; set; } = "IT";

        public string CurrencyCode { get; set; } = "EUR";

        public string RuleName { get; set; } = "MyRule";

        public string RuleAction { get; set; } = "START";

        public string BusinessRuleName { get; set; } = "Down-Business";

        public string BusinessRuleAction { get; set; } = "START";


        public string ToFormatFlatFile()
        {
            string MinSellerAllowedPriceString = MinSellerAllowedPrice.HasValue ?
           MinSellerAllowedPrice.Value.ToString("0.00").Replace('.', ',') : "";
            string MaxSellerAllowedPriceString = MaxSellerAllowedPrice.HasValue ? MaxSellerAllowedPrice.Value.ToString("0.00").Replace('.', ',') : "";

            return SKU + "\t"+ MinSellerAllowedPriceString
                 + "\t" + MaxSellerAllowedPriceString + "\t" + CountryCode + "\t" + CurrencyCode
                  + "\t" + RuleName + "\t" + RuleAction + "\t" + BusinessRuleName + 
                  "\t" + BusinessRuleAction;
        }
    }
}
