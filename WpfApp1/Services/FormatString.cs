using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MWSAPP.Services
{
    public static class FormatString
    {
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
  
        public static string FormatSKU(DataRow dtr, string prefix)
        {
            string sku = prefix + "_";
            sku = (decimal)dtr["Alias"] == 0M? sku += "NO" : sku+=dtr["Alias"].ToString();
            sku += "_";
            sku = (decimal)dtr["Codice"] == 0M ? sku += "NO" : sku += dtr["Codice"].ToString();
            sku += "_";
            sku = (string)dtr["Marchio"] == "" ? sku += "NO" : sku += dtr["Marchio"];
            sku = FormatString.RemoveSpecialCharacters(sku);
            sku = sku.Length < 40 ? sku : sku.Substring(0, 39);
            return sku;
        }

    }
}
