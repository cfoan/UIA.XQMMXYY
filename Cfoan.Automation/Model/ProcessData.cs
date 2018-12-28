using Cfoan.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfoan.Automation
{
    public class ProcessData
    {
        public string FileMD5 { get; set; }

        public string FileName { get; set; }

        public string MainWindowFileName { get; set; }

        public string MainWindowTitle { get; set; }

        public string StartArgument { get; set; }
    }
}
