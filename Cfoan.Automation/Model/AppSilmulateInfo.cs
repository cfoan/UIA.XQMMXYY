using Cfoan.Automation.Model;
using System.Collections.Generic;


namespace Cfoan.Automation
{
    /// <summary>
    /// 自动登陆配置信息
    /// </summary>
    public class AppSilmulateInfo
    {
        public ProcessData ProcessData { get; set; }
        public List<AutomationInfo> AutomationData { get; set; }
    }
}
