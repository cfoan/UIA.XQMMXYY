using Cfoan.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    public class QQLite
    {
        AppSilmulateInfo appSilmulateInfo;

        public QQLite()
        {
            appSilmulateInfo = FileDataLoader.GetCombined("qqlite");
            if (appSilmulateInfo == null)
            {
                appSilmulateInfo = DefualtConfig();
            }
        }

        private AppSilmulateInfo DefualtConfig()
        {
            AppSilmulateInfo appSilmulateInfo = new AppSilmulateInfo();
            appSilmulateInfo.ProcessData = new ProcessData()
            {
                FileName = "C:\\Program Files (x86)\\Tencent\\QQLite\\Bin\\QQScLauncher.exe",
                MainWindowFileName = "C:\\Program Files (x86)\\Tencent\\QQLite\\Bin\\QQ.exe",
                MainWindowTitle = "QQ"
            };

            appSilmulateInfo.AutomationData = new List<Cfoan.Automation.Model.AutomationInfo>();
            appSilmulateInfo.AutomationData.Add(new Cfoan.Automation.Model.AutomationInfo()
            {
                AutomationProperties = new AutomationProperties()
                {
                    ClassName = "TXGuiFoundation"
                },
                Config = new Cfoan.Automation.Model.ConfigItem()
                {
                    ActionType = 0
                }
            });
            appSilmulateInfo.AutomationData.Add(new Cfoan.Automation.Model.AutomationInfo()
            {
                AutomationProperties = new AutomationProperties()
                {
                    ControlTypeId = 50004
                },
                Config = new Cfoan.Automation.Model.ConfigItem()
                {
                    ActionType = (int)SimulateActionType.InputSimulateSetText,
                    ParameterName = "username",
                    FindOptions = new Cfoan.Automation.Model.AutomationFindOptions()
                    {
                        IncludeDescendants = true,
                    }
                }
            });

            appSilmulateInfo.AutomationData.Add(new Cfoan.Automation.Model.AutomationInfo()
            {
                AutomationProperties = new AutomationProperties()
                {
                    ControlTypeId = 50033
                },
                Config = new Cfoan.Automation.Model.ConfigItem()
                {
                    ActionType = (int)SimulateActionType.InputSimulateSetText,
                    ParameterName = "password",
                    FindOptions = new Cfoan.Automation.Model.AutomationFindOptions()
                    {
                        IncludeDescendants = true,
                        CandidateIndex = 19,
                    }
                }
            });

            appSilmulateInfo.AutomationData.Add(new Cfoan.Automation.Model.AutomationInfo()
            {
                AutomationProperties = new AutomationProperties()
                {
                    ControlTypeId = 50000,
                    Name = "登   录"
                },
                Config = new Cfoan.Automation.Model.ConfigItem()
                {
                    ActionType = (int)SimulateActionType.InputSimulateClick,
                    FindOptions = new Cfoan.Automation.Model.AutomationFindOptions()
                    {
                        IncludeDescendants = true,
                    }
                }
            });

            //FileDataLoader.SaveCloginFile("qqlite", appSilmulateInfo.AutomationData);
            //FileDataLoader.SaveProcessData("qqlite", appSilmulateInfo.ProcessData);
            //FileDataLoader.SaveCombined("qqlite", appSilmulateInfo);
            return appSilmulateInfo;
        }

        /// <summary>
        ///最好约定username,password作为默认的key
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Start(string username,string password)
        {
            SimulatorBase simulator = new SimulatorBase(appSilmulateInfo);
            simulator.Start(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username",username),
                new KeyValuePair<string, string>("password",password),
            });
        }

        public void Start(List<KeyValuePair<string, string>> param)
        {
            SimulatorBase simulator = new SimulatorBase(appSilmulateInfo);
            simulator.Start(param);
        }
    }
}
