using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public static class CreateFlatFile
    {
        public static void add()
        {
            List<FileRecord> records = new List<FileRecord>();
            records.Add(new FileRecord { name = "12345" });
        }


    }
}
