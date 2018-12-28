using AssertLibrary;
using Cfoan.Automation.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cfoan.Automation
{
    public class FileDataLoader
    {
        public static ProcessData LoadProcessStartInfo(string appName)
        {
            var file = ProcessDataFile(appName);
            var content = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<ProcessData>(content);
        }

        public static void SaveProcessData(string appName, ProcessData processData)
        {
            var file = ProcessDataFile(appName);
            File.WriteAllText(file, JsonConvert.SerializeObject(processData), Encoding.UTF8);
        }

        public static List<AutomationInfo> GetCloginFile(string appName)
        {
            var loginFile = CloginFile(appName);
            var content = File.ReadAllText(loginFile);
            var infos = JsonConvert.DeserializeObject<List<AutomationInfo>>(content);
            return infos;
        }

        public static void SaveCloginFile(string appName, List<AutomationInfo> content)
        {
            var file = CloginFile(appName);
            File.WriteAllText(file, JsonConvert.SerializeObject(content), Encoding.UTF8);
        }

        public static AppSilmulateInfo GetCombined(string appName)
        {
            var file = ConbinedFile(appName);
            var content=File.ReadAllText(file);
            var info = JsonConvert.DeserializeObject<AppSilmulateInfo>(content);
            return info;
        }

        public static void SaveCombined(string appName, AppSilmulateInfo content)
        {
            var file = ConbinedFile(appName);
            File.WriteAllText(file, JsonConvert.SerializeObject(content), Encoding.UTF8);
        }

        private static string CloginFile(string appName)
        {
            Assert.IsNotNull(appName);
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"apps//{appName}//");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return Path.Combine(dir, $"{appName}.cfoanlogin");
        }

        private static string ProcessDataFile(string appName)
        {
            Assert.IsNotNull(appName);
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"apps//{appName}//");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return Path.Combine(dir, $"{appName}.process");
        }

        private static string ConbinedFile(string appName)
        {
            Assert.IsNotNull(appName);
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"apps//{appName}//");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return Path.Combine(dir, $"{appName}.combined");
        }
    }
}
