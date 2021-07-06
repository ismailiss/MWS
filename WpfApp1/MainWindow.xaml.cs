using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using MWSAPP.Models;
using MWSAPP.Services;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;

namespace MWSAPP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //Upload file step 1
        private DataSet dsStep1 = new();
        private DataSet dsStep2 = new();
        private DataSet dsStep2Poids = new();

        private string fileNameInventoryLoader = "";
        private string fileNameAutomatePricing = "";

        #region step 1
        private void btnUploadInput_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //display message 
                TextProgressBar.Text = "start upload file step1";
                TextErrorProgressBar.Text = "";
                // Create OpenFileDialog 

                OpenFileDialog openFileDialog = new OpenFileDialog();
                // Set filter for file extension and default file extension 
                openFileDialog.DefaultExt = ".xml";
                openFileDialog.Filter = "xml Files|*.xml";
                // Display OpenFileDialog by calling ShowDialog method 
                bool? result = openFileDialog.ShowDialog();


                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    TextProgressBar.Text = "Open Document";
               

                    // Open document 
                    string filename = openFileDialog.FileName;
                    //Create dataSet
                    TextProgressBar.Text = "start upload file step1";
                    dsStep1 = XMLtoDataTable.ImportExcelXML(filename, true, true);
                    TextProgressBar.Text = "file step1 successfully uploaded";

                }
                else
                {
                    TextProgressBar.Text = "";         
                }
                TextErrorProgressBar.Text = "";
                MyProgressBar.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                TextErrorProgressBar.Visibility = Visibility.Visible;
                TextProgressBar.Text = "Upload file step1 Failed";
                TextErrorProgressBar.Text = ex.Message;

            }

        }
        private void GenerateStep1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextProgressBar.Text = "Generate file step 1";
                TextErrorProgressBar.Visibility = Visibility.Hidden;
                DataTable dt = dsStep1.Tables[0];
                if (! dt.Columns.Contains("SKU") && !dt.Columns.Contains("Quantity"))
                {
                    dt.Columns.Add("SKU", typeof(string));
                    dt.Columns.Add("Quantity", typeof(int));

                    InputIndex inputIndex = new();
                    //create Input object
                    IList<string> aliasLabel = AliasInput.Text.Split(',');
                    IList<string> codiceLabels = CodiceInput.Text.Split(',');
                    IList<string> priceLabels = PriceInput.Text.Split(',');
                    IList<string> poidsLabels = PoidsInput.Text.Split(',');
                    IList<string> copreLabels = COPREInput.Text.Split(',');
                    IList<string> siteaLabels = SITEAInput.Text.Split(',');
                    IList<string> marchioLabels = MarchioInput.Text.Split(',');
                    IList<string> codeArticleLabels = CodeArticleInput.Text.Split(',');

                    inputIndex = DataTableToDataExport.getInputIndexd(dt.Columns, aliasLabel, codiceLabels, priceLabels, poidsLabels
                                                     , copreLabels, siteaLabels, marchioLabels, new List<string>(), codeArticleLabels);
                    int rowsCount = dt.Rows.Count;
                    for (int i = 0; i < rowsCount; i++)
                    {
                        dt.Rows[i]["SKU"] = FormatString.FormatSKU(dt.Rows[i], "FOR", inputIndex); ;
                        dt.Rows[i]["Quantity"] = dt.Rows[i][inputIndex.COPREIndex.Value];
                        if (int.Parse(dt.Rows[i]["SITEA"].ToString()) > 0)
                        {
                            string skuSITEA = FormatString.FormatSKU(dt.Rows[i], "SITEA", inputIndex); 
                            DataRow dr = dt.NewRow();
                            for (int j = 0; j < dt.Columns.Count - 2; j++)
                            {
                                dr[j] = dt.Rows[i][j];
                            }
                            dr["SKU"] = skuSITEA;
                            dr["Quantity"] = dt.Rows[i][inputIndex.SITEAIndex.Value];
                            dsStep1.Tables[0].Rows.Add(dr);
                        }
                    }
                }

                string directory = System.IO.Path.Combine("C:\\", "MWSFiles\\step1");
                Directory.CreateDirectory(directory);

                string filename = System.IO.Path.Combine(directory, "MyExcelFile" + DateTime.Now.ToString("-ddMMyyyy-HHmmss") + ".XLSX");
                string filenameXml = System.IO.Path.Combine(directory, "MyExcelFile" + DateTime.Now.ToString("-ddMMyyyy-HHmmss") + ".xml");
                SpreadsheetWorkbook.CreateSpreadsheetWorkbook(filename, dsStep1);
                SpreadsheetWorkbook.CreateSpreadsheetWorkbook(filename, dsStep1);

                //   ExcelLibrary.DataSetHelper.CreateWorkbook(filename, dsStep1);
                TextProgressBar.Text = $"File Generated {filename}" ;

            }
            catch (Exception ex)
            {
                TextErrorProgressBar.Visibility = Visibility.Hidden;
                TextProgressBar.Visibility = Visibility.Visible;
                TextErrorProgressBar.Visibility = Visibility.Visible;
                TextProgressBar.Text = "Failed";
                TextErrorProgressBar.Text = ex.Message;
            }

        }

        #endregion setp 1
        #region step 2
        private void UploadInventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //display message 
                TextProgressBar.Text = "start upload File step 2";
                TextErrorProgressBar.Text = "";
                MyProgressBar.Visibility = Visibility.Visible;
                // Create OpenFileDialog 
                OpenFileDialog openFileDialog = new OpenFileDialog();
                // Set filter for file extension and default file extension 
                openFileDialog.DefaultExt = ".xml";
                openFileDialog.Filter = "xml Files|*.xml";
                // Display OpenFileDialog by calling ShowDialog method 
                bool? result = openFileDialog.ShowDialog();

                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    TextProgressBar.Text = "Open File Step 2";

                    // Open document 
                    string filename = openFileDialog.FileName;
                    //Create dataSet
                    TextProgressBar.Text = "Read DATA";
                    dsStep2 = XMLtoDataTable.ImportExcelXML(filename, true, true);
                    TextProgressBar.Text = "file step 2 successfully uploaded";
                }
                else
                {
                    TextProgressBar.Text = "";
                }
                TextErrorProgressBar.Text = "";
                MyProgressBar.Visibility = Visibility.Hidden;

            }
            catch (Exception ex)
            {
                TextProgressBar.Text = "Failed";
                MyProgressBar.Visibility = Visibility.Hidden;
                TextErrorProgressBar.Visibility = Visibility.Hidden;

                TextErrorProgressBar.Text = ex.Message;
            }

        }

        private void UploadPoidsFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //display message 
                TextProgressBar.Text = "start upload File Poids";
                TextErrorProgressBar.Text = "";
                MyProgressBar.Visibility = Visibility.Visible;
                // Create OpenFileDialog 
                OpenFileDialog openFileDialog = new OpenFileDialog();
                // Set filter for file extension and default file extension 
                openFileDialog.DefaultExt = ".xml";
                openFileDialog.Filter = "xml Files|*.xml";
                // Display OpenFileDialog by calling ShowDialog method 
                bool? result = openFileDialog.ShowDialog();

                // Get the selected file name and display in a TextBox 
                // Get the selected  name and display in a TextBox 
                if (result == true)
                {
                    TextProgressBar.Text = "Open File Step 2";

                    // Open document 
                    string filename = openFileDialog.FileName;
                    //Create dataSet
                    TextProgressBar.Text = "Read DATA";
                    dsStep2Poids = XMLtoDataTable.ImportExcelXML(filename, true, true);
                    TextProgressBar.Text = "file poids successfully uploaded";
                }
                else
                {
                    TextProgressBar.Text = "";
                }
                TextErrorProgressBar.Text = "";
                MyProgressBar.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                TextErrorProgressBar.Text = "";
                TextProgressBar.Text = "Failed";
                MyProgressBar.Visibility = Visibility.Hidden;
                TextErrorProgressBar.Text = ex.Message;
            }


        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextProgressBar.Text = "Generate files step 2";
                MyProgressBar.Visibility = Visibility.Visible;

                //create Input object
                InputIndex inputIndex = new();
                InputIndex inputIndexpoids = new();

                IList<string> aliasLabel = AliasInput.Text.Split(',');
                IList<string> codiceLabels = CodiceInput.Text.Split(',');
                IList<string> priceLabels = PriceInput.Text.Split(',');
                IList<string> poidsLabels = PoidsInput.Text.Split(',');
                IList<string> copreLabels = COPREInput.Text.Split(',');
                IList<string> siteaLabels = SITEAInput.Text.Split(',');
                IList<string> marchioLabels = MarchioInput.Text.Split(',');
                IList<string> codeArticleLabels = CodeArticleInput.Text.Split(',');

                IList<string> skuLabels = "SKU".Split(',');


                inputIndex = DataTableToDataExport.getInputIndexd(dsStep2.Tables[0].Columns, aliasLabel, codiceLabels, priceLabels, poidsLabels
                                                 , copreLabels, siteaLabels, marchioLabels, skuLabels, codeArticleLabels);

                inputIndexpoids = DataTableToDataExport.getInputIndexd(dsStep2Poids.Tables[0].Columns, aliasLabel, codiceLabels, priceLabels, poidsLabels
                                                 , copreLabels, siteaLabels, marchioLabels, skuLabels, codeArticleLabels) ;
                if (inputIndexpoids==null || inputIndexpoids.PoidsIndex == null)
                {
                    TextProgressBar.Text = "Poids not found";
                }
                else
                {

                IList<InputRecordInventory> RecordInventory = 
                    DataTableToDataExport.DataTableToInputRecordInventory(dsStep2, inputIndex, dsStep2Poids, inputIndexpoids);
                IList<InputRecordPricing> RecordRecordPricing =
                    DataTableToDataExport.DataTableToInputRecordPricing(dsStep2, inputIndex);

                IList<FileRecordInventory> fileRecordsInventory = new List<FileRecordInventory>();
                IList<FileRecordPricing> fileRecordsPricing = new List<FileRecordPricing>();
                TextProgressBar.Text = "Create Records";
                foreach (InputRecordInventory input in RecordInventory)
                {
                        FileRecordPricing frp = new();
                        FileRecordInventory fri = new();
                        decimal tarif = Tarif.CountIntervals(input.Poids, 6);
                        fri.SKU = input.SKU;
                        frp.SKU= input.SKU;
                        fri.ProductId = input.Alias;
                        fri.Quantity = input.Quantity;
                        if (fri.Quantity == 0) fri.AddDelete = "d";
                        if (input.Alias.Length < 13) fri.ProductIdType = 3;   
                        else fri.ProductIdType = 3;
                        string inputprice = input.Price.Replace(",",".") ;
                        decimal temp;
                        decimal? price = decimal.TryParse(inputprice, out temp) ? temp : (decimal?)null;
                        price +=  tarif;
                        price *= (1+ decimal.Parse(AmazonComissionValue.Text) / 100) ;
                        price *= (1 + decimal.Parse(TVAValue.Text) / 100);
                        fri.MinSellerAllowedPrice =price;
                        fri.MaxSellerAllowedPrice = price*1.4M;
                        fri.Price = price * 1.08M;

                        frp.MinSellerAllowedPrice = price;
                        frp.MaxSellerAllowedPrice = price * 1.4M;

                    fileRecordsPricing.Add(frp);
                    fileRecordsInventory.Add(fri);
                }
                TextProgressBar.Text = "Create Flat Files";
                    string directory = System.IO.Path.Combine("C:\\", "MWSFiles\\step2");
                    Directory.CreateDirectory(directory);

                fileNameInventoryLoader = System.IO.Path.Combine(directory, "Flat.File." + DateTime.Now.ToString("-ddMMyyyy-HHmmss") + "InventoryLoader.txt"); 
                fileNameAutomatePricing = System.IO.Path.Combine(directory, "Flat.File." + DateTime.Now.ToString("-ddMMyyyy-HHmmss") + "AutomatePricing.txt"); 
                TextProgressBar.Text = "Create Inventory Loader Flat File";
               
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter swInventoryLoader = new(fileNameInventoryLoader);
                //Write a line of text
                swInventoryLoader.WriteLine("sku\tproduct-id\tproduct-id-type\tprice\tminimum-seller-allowed-price\tmaximum-seller-allowed-price\titem-condition\tquantity\tadd-delete\titem-note\texpedited-shipping\tproduct_tax_code\thandling-time");
                //Close the file
                foreach (FileRecordInventory record in fileRecordsInventory)
                {
                    swInventoryLoader.WriteLine(record.ToFormatFlatFile());
                }

                swInventoryLoader.Close();
                TextProgressBar.Text = "Create Automate Pricing Flat File";
                if (File.Exists(fileNameAutomatePricing))
                {
                    File.Delete(fileNameAutomatePricing);
                }
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter swPricing = new StreamWriter(fileNameAutomatePricing);
                //Write a line of text
                swPricing.WriteLine($"sku\tminimum-seller-allowed-price\tmaximum-seller-allowed-price\tcountry-code\tcurrency-code\trule-name\trule-action\tbusiness-rule-name\tbusiness-rule-action");
                //Close the file
                foreach (var record in fileRecordsPricing)
                    swPricing.WriteLine(record.ToFormatFlatFile());
                swPricing.Close();
                TextProgressBar.Text = "Files generated : " + fileNameInventoryLoader + ", " + fileNameAutomatePricing;
                MyProgressBar.Visibility = Visibility.Hidden;

                }
            }
            catch (Exception ex)
            {
                TextProgressBar.Text = "Failed";
                MyProgressBar.Visibility = Visibility.Hidden;
                TextErrorProgressBar.Text = ex.Message;
            }
        }
        #endregion setp 2
        #region send Email
        private void AddEmail_Click(object sender, RoutedEventArgs e)
        {
            if (EmailInput.Text != "")
            {
                StringInput data = new() { Email = EmailInput.Text };
                ListViewItem lvi = new ListViewItem();
                ListViewEmails.Items.Add(data);
                EmailInput.Text = "";
            }
        }

        private void DeleteEmail_Click(object sender, RoutedEventArgs e)
        {
            StringInput selected = (StringInput)ListViewEmails.SelectedItem;
            if (selected != null)
            {
                ListViewEmails.Items.Remove(ListViewEmails.SelectedItem);
            }


        }
        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SendEmail.IsEnabled = false;
                List<string> emails = new();
                foreach (var li in ListViewEmails.Items)
                {
                    StringInput emailInput = (StringInput)li;
                    emails.Add(emailInput.Email);
                }
               /* if (fileNameInventoryLoader != "" && fileNameInventoryLoader != "")
                {*/
                    EmailSender.SendEmail(emails, "Flat Files", fileNameAutomatePricing, fileNameInventoryLoader);
               // }
                SendEmail.IsEnabled = true;

            }
            catch (Exception ex)
            {
                TextErrorProgressBar.Visibility = Visibility.Hidden;
                TextProgressBar.Text = "Send Email Failed";
                TextErrorProgressBar.Text = ex.Message;
                SendEmail.IsEnabled = true;

            }

        }

        #endregion Send Email

        #region global
          private void Next_Click(object sender, RoutedEventArgs e)
        {
            TextProgressBar.Text = "";
            TextErrorProgressBar.Text = "";
                if (TabControl1.SelectedIndex + 1 < TabControl1.Items.Count)
                  {
                TabControl1.SelectedIndex++;
                if (TabControl1.SelectedIndex == TabControl1.Items.Count - 1)
                {
                    Next.Visibility = Visibility.Hidden;
                }

                if (Precedent.Visibility == Visibility.Hidden)
                {
                    Precedent.Visibility = Visibility.Visible;
                }
            }
        }

        private void Precedent_Click(object sender, RoutedEventArgs e)
        {
         
            MyProgressBar.Visibility = Visibility.Hidden;
            if (TabControl1.SelectedIndex > 0)
            {
                TabControl1.SelectedIndex = TabControl1.SelectedIndex - 1;
                if (TabControl1.SelectedIndex == 0)
                {
                    Precedent.Visibility = Visibility.Hidden;
                }

                if (Next.Visibility == Visibility.Hidden)
                {
                    Next.Visibility = Visibility.Visible;
                }
            }
        }
        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void TVAValue_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]{1,2}.[0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Precedent.Visibility = Visibility.Hidden;
            Next.Visibility = Visibility.Visible;
            MyProgressBar.Visibility= Visibility.Hidden;
            TabControl1.SelectedIndex = 0;
            dsStep1 = new();
            dsStep2 = new();
            fileNameInventoryLoader = "";
            fileNameAutomatePricing = "";
            TextProgressBar.Text = "";
            TextErrorProgressBar.Text = "";           

        }

        #endregion global

  
    }
}
