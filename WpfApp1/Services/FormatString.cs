using MWSAPP.Models;
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
            return Regex.Replace(str, "[^a-zA-Z0-9-.]+", "", RegexOptions.Compiled);
        }
  
        public static string FormatSKU(DataRow dtr, string prefix, InputIndex inputIndex)
        {
            string sku = prefix ;
            sku = inputIndex.AliasIndex.HasValue == true && (string)dtr[inputIndex.CodeAricleIndex.Value].ToString() != ""?
                sku += "-" + dtr[inputIndex.AliasIndex.Value].ToString()  : sku += "-NO";
            
            sku = inputIndex.CodiceIndex.HasValue == true && dtr[inputIndex.CodiceIndex.Value].ToString() != ""?
                    sku += "-" + dtr[inputIndex.CodiceIndex.Value].ToString() : sku += "-NO";
            
            sku = inputIndex.MarchioIndex.HasValue == true && (string)dtr[inputIndex.MarchioIndex.Value] != "" ?
                sku += "-" + dtr[inputIndex.MarchioIndex.Value]  : sku += "-NO";

            sku = inputIndex.CodeAricleIndex.HasValue ==true && dtr[inputIndex.CodeAricleIndex.Value].ToString() != ""
                ? sku += "-" + dtr[inputIndex.CodeAricleIndex.Value]: sku += "";
            sku = FormatString.RemoveSpecialCharacters(sku);
            sku = sku.Length < 40 ? sku : sku.Substring(0, 39);
            return sku;
        }

    }
}
