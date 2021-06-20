using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWSAPP.Models
{
    public class InputRecordInventory
    {
        public decimal Alias { get; set; }

        public uint Codice { get; set; }

        public decimal Poids { get; set; }

        public decimal Price { get; set; }


    }
    public class InputRecordPricing
    {
        public decimal Alias { get; set; }

        public uint Codice { get; set; }

    }
}
