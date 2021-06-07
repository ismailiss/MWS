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
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1
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
            // Create OpenFileDialog 

            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Set filter for file extension and default file extension 
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "xml Files|*.xml"
;


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = openFileDialog.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                int aliasIndex = 0;
                int odiceIndex = 0;

                // Open document 
                string filename = openFileDialog.FileName;
                //Create dataSet
                DataSet ds= XMLtoDataTable.ImportExcelXML(filename, true, true);

                //create Input object
                 IList<Input> inputs = DataTableToDataExport.DataTableToInput(ds);

            }

        }
    }
}
