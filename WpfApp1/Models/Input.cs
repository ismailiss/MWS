using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWSAPP.Models
{
    public class InputRecordInventory
    {
        public string SKU { get; set; }

        public decimal Alias { get; set; }

        public decimal Codice { get; set; }

        public decimal Poids { get; set; }

        public decimal Price { get; set; }


    }
    public class InputRecordPricing
    {
        public string SKU  { get; set; }

        public decimal Alias { get; set; }

        public uint Codice { get; set; }

    }
}
