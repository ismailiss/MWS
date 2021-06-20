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
        private void btnUploadInput_Click(object sender, RoutedEventArgs e)
        {
            try
            {

     
            //display message 
            TextProgressBar.Text = "start";
            // Create OpenFileDialog 

            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Set filter for file extension and default file extension 
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "xml Files|*.xml";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = openFileDialog.ShowDialog();


                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    TextProgressBar.Text = "Open Document ";
                    myProgressBar.Visibility = Visibility.Visible;
                    TextProgressBar.Visibility = Visibility.Visible;

                    // Open document 
                    string filename = openFileDialog.FileName;
                    //Create dataSet
                    TextProgressBar.Text = "Read DATA";

                    DataSet ds = XMLtoDataTable.ImportExcelXML(filename, true, true);
                    //   DataSet dsClonce = ds;
                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("SKU", typeof(string));
                    dt.Columns.Add("Quantity", typeof(int));
                    int rowsCount = dt.Rows.Count;
                    for (int i = 0; i < rowsCount; i++)
                    {

                        dt.Rows[i]["SKU"] = FormatString.FormatSKU(dt.Rows[i], "FOR"); ;
                        dt.Rows[i]["Quantity"] = dt.Rows[i]["COPRE"];
                        if (int.Parse(dt.Rows[i]["SITEA"].ToString()) > 0)
                        {
                            string skuSITEA = FormatString.FormatSKU(dt.Rows[i], "SITEA"); ;

                            DataRow dr = dt.NewRow();
                            for (int j = 0; j < dt.Columns.Count - 2; j++)
                            {
                                dr[j] = dt.Rows[i][j];
                            }
                            dr["SKU"] = skuSITEA;
                            dr["Quantity"] = dt.Rows[i]["SITEA"];

                            ds.Tables[0].Rows.Add(dr);
                        }

                    }
                    ExcelLibrary.DataSetHelper.CreateWorkbook(@"MyExcelFile" + DateTime.Now.ToString("MM-dd-yyyy-HH-mm") + ".xls", ds);
                }
            }
            catch (Exception ex)
            {
                TextErrorProgressBar.Visibility = Visibility.Visible;
                TextProgressBar.Text = "Failed";
                TextErrorProgressBar.Text = ex.Message;

            }

        }

    
        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void AddEmail_Click(object sender, RoutedEventArgs e)
        {
            if (EmailInput.Text != "")
            {
                MyData data = new() { Email = EmailInput.Text };
                ListViewItem lvi = new ListViewItem();
                ListViewEmails.Items.Add(data);
                EmailInput.Text = "";
            }
        }

        public class MyData
        {
            public string Email { get; set; }
        }

        private void DeleteEmail_Click(object sender, RoutedEventArgs e)
        {
            var selected = (MyData)ListViewEmails.SelectedItem;
            if (selected!=null)
            {
              ListViewEmails.Items.Remove(ListViewEmails.SelectedItem);
            }
          

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl1.SelectedIndex + 1 < TabControl1.Items.Count)
            {
                TabControl1.SelectedIndex = TabControl1.SelectedIndex + 1;
                if (TabControl1.SelectedIndex == TabControl1.Items.Count-1) Next.Visibility = Visibility.Hidden;
                if (Precedent.Visibility==Visibility.Hidden) Precedent.Visibility = Visibility.Visible;

            }
        
        }

        private void Precedent_Click(object sender, RoutedEventArgs e)
        {
            if(TabControl1.SelectedIndex> 0)
            {
                TabControl1.SelectedIndex = TabControl1.SelectedIndex - 1;
                if(TabControl1.SelectedIndex==0) Precedent.Visibility = Visibility.Hidden;
                if (Next.Visibility == Visibility.Hidden) Next.Visibility = Visibility.Visible;
            }
         

        }

        private void UploadInventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //display message 
                TextProgressBar.Text = "start";
                // Create OpenFileDialog 
                OpenFileDialog openFileDialog = new OpenFileDialog();
                // Set filter for file extension and default file extension 
                openFileDialog.DefaultExt = ".xml";
                openFileDialog.Filter = "xml Files|*.xml";
                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = openFileDialog.ShowDialog();

                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    TextProgressBar.Text = "Open Document ";
                    myProgressBar.Visibility = Visibility.Visible;
                    TextProgressBar.Visibility = Visibility.Visible;

                    // Open document 
                    string filename = openFileDialog.FileName;
                    //Create dataSet
                    TextProgressBar.Text = "Read DATA";

                    DataSet ds = XMLtoDataTable.ImportExcelXML(filename, true, true);

                    int[,] arrTarifs = new int[,] { { 0, 2 }, { 2, 5 }, { 5, 10 }, { 10, 25 }, { 25, 50 }, { 50, 100 } };
                    decimal tva = 0.22M;
                    decimal amazonComission = 0.1552M;


                    //create Input object
                    IList<InputRecordInventory> RecordInventory = DataTableToDataExport.DataTableToInputRecordInventory(ds);
                    IList<InputRecordPricing> RecordRecordPricing = DataTableToDataExport.DataTableToInputRecordPricing(ds);

                    IList<FileRecordInventory> fileRecordsInventory = new List<FileRecordInventory>();
                    IList<FileRecordPricing> fileRecordsPricing = new List<FileRecordPricing>();
                    TextProgressBar.Text = "Create Records";
                    foreach (InputRecordInventory input in RecordInventory)
                    {
                        FileRecordPricing frp = new();
                        FileRecordInventory fri = new();
                        decimal tarif = Tarif.CountIntervals(arrTarifs, input.Poids, 6);
                        fri.ProductId = input.Alias;
                        fri.Price = tarif + (1 + amazonComission) * input.Price + (1 + TVAValue.Text) * input.Price;
                        fileRecordsPricing.Add(frp);
                        fileRecordsInventory.Add(fri);
                    }
                    string path = Directory.GetCurrentDirectory();
                    TextProgressBar.Text = "Create Flat Files";

                    string fileNameInventoryLoader = @"Flat.File.InventoryLoader.txt";
                    string fileNameAutomatePricing = @"Flat.File.AutomatePricing.txt";
                    if (File.Exists(fileNameInventoryLoader))
                    {
                        File.Delete(fileNameInventoryLoader);
                    }
                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter swInventoryLoader = new StreamWriter(fileNameInventoryLoader);
                    //Write a line of text
                    swInventoryLoader.WriteLine("sku	product-id	product-id-type	price	minimum-seller-allowed-price	maximum-seller-allowed-price	item-condition	quantity	add-delete	item-note	expedited-shipping  product_tax_code    handling-time");
                    //Close the file
                    foreach (FileRecordInventory record in fileRecordsInventory)
                    {
                        swInventoryLoader.WriteLine(record.ToFormatFlatFile());
                    }

                    swInventoryLoader.Close();

                    if (File.Exists(fileNameAutomatePricing))
                    {
                        File.Delete(fileNameAutomatePricing);
                    }
                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter swPricing = new StreamWriter(fileNameAutomatePricing);
                    //Write a line of text
                    swPricing.WriteLine("sku	minimum-seller-allowed-price	maximum-seller-allowed-price	country-code	currency-code	rule-name	rule-action	business-rule-name	business-rule-action");
                    //Close the file
                    foreach (var record in fileRecordsPricing)
                        swPricing.WriteLine(record.ToFormatFlatFile());
                    swPricing.Close();

                    TextProgressBar.Text = "Successed";
                 //   SendEmail.Email("Bonjour");
                }
            }
            catch (Exception ex)
            {
                TextErrorProgressBar.Visibility = Visibility.Visible;
                TextProgressBar.Text = "Failed";
                TextErrorProgressBar.Text = ex.Message;

            }
                   

        }

        private void Generate_Click(object sender, RoutedEventArgs e)
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
    }
}
