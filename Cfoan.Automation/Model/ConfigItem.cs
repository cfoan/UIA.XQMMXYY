using Cfoan.Automation;
using WindowsInput.Native;

namespace Cfoan.Automation.Model
{
    public partial class ConfigItem
    {
        public int ActionType { get; set; }
        public string ParameterName { get; set; }
        public int SleepMillis { get; set; }
        public VirtualKeyCode VKCode { get; set; }
        public AutomationFindOptions FindOptions { get; set; }
    }

    //todo 放到AutomationProperties类里面？
    public class AutomationFindOptions
    {
        public bool IncludeDescendants { get; set; }
        /// <summary>
        /// 通过AutomationProperties查询得到多个元素的下标
        /// </summary>
        public int? CandidateIndex { get; set; }
        public int ParentIndex { get; set; }
    }
}
