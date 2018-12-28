using Cfoan.Automation.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using WindowsInput;

namespace Cfoan.Automation
{
    public class SilmulateContext
    {
        AppSilmulateInfo appSilmulateInfo;
        private List<AutomationEntry> entries = new List<AutomationEntry>();
        private volatile int index = 0;

        public AutomationElement Element => GetFromCache(index);
        public AutomationElement Root => GetFromCache(0);
        public AutomationInfo AutomationInfo => entries[index]?.Info;
        
        public SilmulateContext(AppSilmulateInfo info)
        {
            appSilmulateInfo = info;
            var temp = 0;
            var entry=info.AutomationData.Select((i) =>
            {
                return new AutomationEntry()
                {
                    Info = i,
                    Index = temp++
                };
            });
            entries.AddRange(entry);
        }

        public bool MoveNext()
        {
            Interlocked.Increment(ref index);
            return index < entries.Count;
        }

        public AutomationElement GetFromCache(int index)
        {
            return (index > this.entries.Count - 1) ? null :
                entries[index]?.Element;
        }

        public int Found(AutomationElement ae)
        {
            entries[index].Element = ae;
            return index;
        }

        public Process Process { get; set; }

        /// <summary>
        /// 这个有点不好使啊 考虑换掉
        /// </summary>
        public InputSimulator InputSimulator { get; } = new InputSimulator();

        public int Index => index;
    }

    public class AutomationEntry
    {
        public int Index { get; set; }
        public AutomationElement Element { get; set; }
        public AutomationInfo Info { get; set; }
    }
}
