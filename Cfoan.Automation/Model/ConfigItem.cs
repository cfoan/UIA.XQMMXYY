using Cfoan.Automation;
using WindowsInput.Native;

namespace Cfoan.Automation.Model
{
    public class ConfigItem
    {
        public int Type { get; set; }
        public bool IncludeDescendants{get;set;}
        public int? Index{ get; set; }
        public int ParentId { get; set; }
        public string ParameterName { get; set; }
        public int SleepMillis { get; set; }
        public VirtualKeyCode VKCode { get; set; }
    }
}
