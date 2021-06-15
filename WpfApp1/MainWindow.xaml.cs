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

                //create Input object
                 IList<Input> inputs = DataTableToDataExport.DataTableToInput(ds);
                IList<FileRecordInventory> fileRecordsInventory = new List<FileRecordInventory>();
                IList<FileRecordPricing> fileRecordsPricing = new List<FileRecordPricing>();
                TextProgressBar.Text = "Create Records";

                foreach (var input in inputs)
                {
                    FileRecordPricing frp = new FileRecordPricing();
                    FileRecordInventory fri = new FileRecordInventory();
                    fri.ProductId = input.Alias;
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
                foreach (var record in fileRecordsInventory)
                    swInventoryLoader.WriteLine(record.ToFormatFlatFile());
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
    }
}
