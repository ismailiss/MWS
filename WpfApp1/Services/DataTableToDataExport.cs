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
        private static InputIndex getInputIndex(DataColumnCollection Columns)
        {
            InputIndex inputIndex = new();
            var indiceAlias = Columns.Cast<DataColumn>().
              Select((c,i) => new { c.ColumnName,i }).Where(c => c.ColumnName == "Alias").FirstOrDefault();
           inputIndex.AliasIndex = indiceAlias.i;
            return inputIndex;
        }
        public static IList<Input> DataTableToInput(DataSet ds)
        {
            IList<Input> inputs = new List<Input>();

            InputIndex inputIndex = getInputIndex(ds.Tables[0].Columns);
            foreach (var r in ds.Tables[0].Rows.Cast<DataRow>())
            {
               inputs.Add(new Input() {Codice=0,Alias= (decimal)r.ItemArray[inputIndex.AliasIndex] });
            }
            return inputs;
        }
        






    }
}
