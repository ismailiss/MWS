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
        public static InputIndex getInputIndexd(DataColumnCollection Columns,
            IList<string> aliasLabel, IList<string> codiceLabels, IList<string> priceLabels, IList<string> poidsLabels
            , IList<string> COPRELabels, IList<string> SITEALabels, IList<string> MarchioLabels, IList<string> skuLabels)
        {
            InputIndex inputIndex = new();
            var indexAlias = Columns.Cast<DataColumn>().Select((c, i) => new { c.ColumnName, i }).Where(c => aliasLabel.Contains(c.ColumnName)).FirstOrDefault();
            if (indexAlias!=null)
            {
                inputIndex.AliasIndex = indexAlias.i;
            } 

            var indexCodice = Columns.Cast<DataColumn>().Select((c, i) => new { c.ColumnName, i }).Where(c => codiceLabels.Contains(c.ColumnName)).FirstOrDefault();
            if (indexCodice != null)
            {
                inputIndex.CodiceIndex = indexCodice.i;
            }
            var indexprice = Columns.Cast<DataColumn>().Select((c, i) => new { c.ColumnName, i }).Where(c => priceLabels.Contains(c.ColumnName)).FirstOrDefault();
            if (indexprice!=null)
            {
                inputIndex.PriceIndex = indexprice.i;

            }

            var indexpoids = Columns.Cast<DataColumn>().Select((c, i) => new { c.ColumnName, i }).Where(c => poidsLabels.Contains(c.ColumnName)).FirstOrDefault();
            if(indexpoids != null) inputIndex.PoidsIndex = indexpoids.i;

            var indexCOPRE = Columns.Cast<DataColumn>().Select((c, i) => new { c.ColumnName, i }).Where(c => COPRELabels.Contains(c.ColumnName)).FirstOrDefault();
            if (indexCOPRE != null)
            {
                inputIndex.COPREIndex = indexCOPRE.i;

            }

            var indexSTEA = Columns.Cast<DataColumn>().Select((c, i) => new { c.ColumnName, i }).Where(c => SITEALabels.Contains(c.ColumnName)).FirstOrDefault();
            if (indexSTEA != null)
            {
                inputIndex.SITEAIndex = indexSTEA.i;

            }
            var indexMarchio = Columns.Cast<DataColumn>().Select((c, i) => new { c.ColumnName, i }).Where(c => MarchioLabels.Contains(c.ColumnName)).FirstOrDefault();
            if (indexMarchio != null)
            {
                inputIndex.MarchioIndex = indexMarchio.i;

            }
            var indexSKU = Columns.Cast<DataColumn>().Select((c, i) => new { c.ColumnName, i }).Where(c => skuLabels.Contains(c.ColumnName)).FirstOrDefault();
            if (indexSKU != null)
            {
                inputIndex.SkuIndex = indexSKU.i;

            }
            return inputIndex;
        }
        private static int getInputIndex(DataColumnCollection Columns,IList<string> headerLabels)
        {
            
            var indiceAlias = Columns.Cast<DataColumn>().
              Select((c,i) => new { c.ColumnName,i }).Where(c => headerLabels.Contains(c.ColumnName)).FirstOrDefault();
            return indiceAlias.i;
        }
        public static IList<InputRecordInventory> DataTableToInputRecordInventory(DataSet ds, InputIndex inputIndex, DataSet dsPoids, InputIndex inputIndexPoids)
        {
            IList<InputRecordInventory> inputs = new List<InputRecordInventory>();
            foreach (DataRow r in ds.Tables[0].Rows.Cast<DataRow>())
            {
                decimal poids=15M;
                foreach (DataRow rPoids in dsPoids.Tables[0].Rows.Cast<DataRow>())
                {
                    if (r["SKU"].ToString() == rPoids["SKU"].ToString())   
                    {
                        if(inputIndex.PoidsIndex.HasValue)
                        poids = (decimal)r.ItemArray[inputIndexPoids.PoidsIndex.Value];
                        goto AfterLoop;

                    }
                }
            AfterLoop:
                inputs.Add(new InputRecordInventory() {
                    SKU = r["SKU"].ToString(),
                    Alias = (decimal)r.ItemArray[inputIndex.AliasIndex.Value],
                    Poids= poids,
                    Price = (decimal)r.ItemArray[inputIndex.PriceIndex.Value]

                });
            }
            return inputs;
        }
        public static IList<InputRecordPricing> DataTableToInputRecordPricing(DataSet ds, InputIndex inputIndex)
        {
            IList<InputRecordPricing> inputs = new List<InputRecordPricing>();

            foreach (DataRow r in ds.Tables[0].Rows.Cast<DataRow>())
            {
                inputs.Add(new InputRecordPricing()
                {
                    SKU = r["SKU"].ToString(),
                    Alias = (decimal)r.ItemArray[inputIndex.AliasIndex.Value],
                });
            }
            return inputs;
        }
    }
}
