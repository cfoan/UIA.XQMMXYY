using AssertLibrary;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace Cfoan.Automation
{
    public class AutomationUtils
	{
        static ILog logger = LogManager.GetLogger(typeof(AutomationUtils));
		private static object locker = new object();

		private AutomationUtils()
		{
            AssertLibrary.Assert.DoesNotReachHere();
        }

        
        public static AutomationProperties GetAumationData(AutomationElement automationElement)
        {
            AutomationProperties controlInfo = new AutomationProperties();
            controlInfo.AutomationId = automationElement.Current.AutomationId;
            controlInfo.ClassName = automationElement.Current.ClassName;
            controlInfo.FrameworkId = automationElement.Current.FrameworkId;
            controlInfo.Name = automationElement.Current.Name;
            controlInfo.ControlTypeId = automationElement.Current.ControlType.Id;
            return controlInfo;
        }

        public static Process[] GetAllProcessesByPath(string path)
        {
            try
            {
               
                var file = Path.GetFileNameWithoutExtension(path);
                return Process.GetProcessesByName(file);
            }
            catch (Exception ex)
            {
                logger.Error($"GetAllProcessesByPath,ErrorMessage:{ex.Message}", ex);
            }
            return new Process[0];
        }

        /**
         * TreeScope枚举值
         *  Ancestors	
         *指定搜索包括该元素的上级，包括父。 不支持。
         *  Children	
         *指定搜索包括元素的直接子级。
         *  Descendants	
         *指定搜索包括该元素的后代，其中包括子级。
         *  Element	
         *指定搜索包括元素本身。
         *  Parent	
         *指定搜索包括该元素的父级。 不支持。
         *  Subtree	
         *指定该搜索包含搜索和所有后代的根。
         * 
         **/
        public static AutomationElement FindElements(AutomationProperties controlInfo,TreeScope scope, AutomationElement parent=null,int timeoutMillis=5000,int? index=null)
        {
            Assert.IsNotNull(controlInfo);

            parent = parent ?? AutomationElement.RootElement;
            parent.GetUpdatedCache(CacheRequest.Current);
            var startTime = DateTime.Now;
            for (;;)
            {
                if ((DateTime.Now - startTime).TotalMilliseconds > timeoutMillis)
                {
                    break;
                }

                AutomationElement element = null;

                try
                {
                    if(index==null)
                    {
                        element = parent.FindFirst(scope, GetLookupConditions(controlInfo));
                    }
                    else
                    {
                        var elements = parent.FindAll(scope, GetLookupConditions(controlInfo));
                        if(index.Value<=elements.Count-1)
                        {
                            var mills2 = DateTime.Now;
                            return elements[index.Value];
                        }
                    }
                    
                    if (element != null)
                    {
                        var mills2 = DateTime.Now;
                        return element;
                    }
                }
                catch (Exception ex)
                {
                    logger.Info(ex.Message, ex);
                }
                Thread.Sleep(500);
            }

            return null;
        }

        /// <summary>
        /// 获取查询条件，尽可能用AutomationId
        /// </summary>
        /// <param name="controlInfo"></param>
        /// <returns></returns>
        public static Condition GetLookupConditions(AutomationProperties controlInfo)
        {
            List<PropertyCondition> automationPropertyConditions = new List<PropertyCondition>();
            if (!string.IsNullOrEmpty(controlInfo.FrameworkId))
            {
                automationPropertyConditions.Add(new PropertyCondition(AutomationElement.FrameworkIdProperty, controlInfo.FrameworkId));
            }
            if (!string.IsNullOrEmpty(controlInfo.AutomationId))
            {
                automationPropertyConditions.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, controlInfo.AutomationId));
            }
            if (!string.IsNullOrEmpty(controlInfo.ClassName))
            {
                automationPropertyConditions.Add(new PropertyCondition(AutomationElement.ClassNameProperty, controlInfo.ClassName));
            }
            if (string.IsNullOrEmpty(controlInfo.FrameworkId) || controlInfo.FrameworkId != "WinForm")
            {
                if (controlInfo.ControlTypeId != 0)
                {
                    var type = ControlType.LookupById(controlInfo.ControlTypeId);
                    automationPropertyConditions.Add(new PropertyCondition(AutomationElement.ControlTypeProperty, type));
                }
                if (!string.IsNullOrEmpty(controlInfo.Name))
                {
                    automationPropertyConditions.Add(new PropertyCondition(AutomationElement.NameProperty, controlInfo.Name));
                }
            }

            if (automationPropertyConditions.Count == 0)
            {
                return new AndCondition(PropertyCondition.TrueCondition);
            }

            return new AndCondition(automationPropertyConditions.ToArray());
        }

        public static void ResetInputLanguage()
        {
            var cultrue = CultureInfo.CreateSpecificCulture("en-us");
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(cultrue);
        }
    }
}
