using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MWSAPP.Models;

namespace MWSAPP.Services
{
    public static class DataTableToDataExport
    {
        private static int getInputIndex(DataColumnCollection Columns,string header)
        {
            InputIndex inputIndex = new();
            var indiceAlias = Columns.Cast<DataColumn>().
              Select((c,i) => new { c.ColumnName,i }).Where(c => c.ColumnName == header).FirstOrDefault();
            return indiceAlias.i;
        }
        public static IList<InputRecordInventory> DataTableToInputRecordInventory(DataSet ds)
        {
            IList<InputRecordInventory> inputs = new List<InputRecordInventory>();
            InputIndex inputIndex = new();
            inputIndex.AliasIndex = getInputIndex(ds.Tables[0].Columns,"Alias");
            inputIndex.PoidsIndex = getInputIndex(ds.Tables[0].Columns, "Poids");
            inputIndex.PriceIndex = getInputIndex(ds.Tables[0].Columns, "Prezzo");

            foreach (DataRow r in ds.Tables[0].Rows.Cast<DataRow>())
            {
                inputs.Add(new InputRecordInventory() {
                    Codice = 0, 
                    Alias = (decimal)r.ItemArray[inputIndex.AliasIndex],
                    Poids= (decimal)r.ItemArray[inputIndex.PoidsIndex],
                    Price = (decimal)r.ItemArray[inputIndex.PriceIndex]

                });
            }
            return inputs;
        }
        public static IList<InputRecordPricing> DataTableToInputRecordPricing(DataSet ds)
        {
            IList<InputRecordPricing> inputs = new List<InputRecordPricing>();
            InputIndex inputIndex = new();
            inputIndex.AliasIndex = getInputIndex(ds.Tables[0].Columns, "Alias");

            foreach (DataRow r in ds.Tables[0].Rows.Cast<DataRow>())
            {
                inputs.Add(new InputRecordPricing()
                {
                    Codice = 0,
                    Alias = (decimal)r.ItemArray[inputIndex.AliasIndex],
                });
            }
            return inputs;
        }







    }
}
