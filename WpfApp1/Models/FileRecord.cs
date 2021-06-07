using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class FileRecord
    {
        public string name{ get;set; }

        public string ToFormatFlatFile()
        {
            return name+' ';
        }
    }
}
