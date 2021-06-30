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

        public string Alias { get; set; }

        public string Codice { get; set; }

        public decimal Poids { get; set; }

        public string Price { get; set; }


    }
    public class InputRecordPricing
    {
        public string SKU  { get; set; }

        public string Alias { get; set; }

        public uint Codice { get; set; }

    }
}
