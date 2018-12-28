using AssertLibrary;
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
        readonly AppSilmulateInfo appSilmulateInfo;
        readonly List<AutomationEntry> entries = new List<AutomationEntry>();
        readonly volatile int index = 0;

        public AutomationElement Element => GetFromCache(index);
        public AutomationElement Root => GetFromCache(0);
        public AutomationInfo AutomationInfo => entries[index]?.Info;
        
        public SilmulateContext(AppSilmulateInfo info)
        {
            Assert.UseDebug();
            Assert.IsNotNull(info);
            Assert.IsNotNull(info.AutomationData);
            Assert.HasElements(info.AutomationData);

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
            Assert.IsLess(index, entries.Count);
            return entries[index]?.Element;
        }

        public int Found(AutomationElement ae)
        {
            Assert.IsLess(index, entries.Count);
            entries[index].Element = ae;
            return index;
        }

        public Process Process { get; set; }

        /// <summary>
        /// 这个有点不好使啊 考虑换掉
        /// </summary>
        public InputSimulator InputSimulator { get; } = new InputSimulator();
    }

    public class AutomationEntry
    {
        public int Index { get; set; }
        public AutomationElement Element { get; set; }
        public AutomationInfo Info { get; set; }
    }
}
