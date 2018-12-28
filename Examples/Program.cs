using Cfoan.Automation;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config",Watch =true)]
namespace Examples
{
    class Program
    {
        static ILog logger = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            
            //QQLite lite = new QQLite();
            //lite.Start("123","3456");

            QQTim tim = new QQTim();
            tim.Start("1029554868","456");
            Console.ReadKey();
        }
    }
}
