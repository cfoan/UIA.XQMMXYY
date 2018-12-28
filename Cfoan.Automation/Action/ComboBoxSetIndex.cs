using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace Cfoan.Automation
{
    public partial class Actions
    {
        public class ComboBoxSetIndex : AbstractSimulateAction
        {
            string msg;
            int? index;
            public ComboBoxSetIndex(SilmulateContext ctx, string msg,int? index=null)
                :base(ctx)
            {
                this.msg = msg;
                this.index = index;
            }

            public override SimulateActionType Type
            {
                get
                {
                    return SimulateActionType.ComboxBoxSetIndex;
                }
            }

            public override void Perform()
            {
                var comboBox= Context.Element;
                var allChildControls=comboBox.FindAll(TreeScope.Children,Condition.TrueCondition);
                AutomationElement button = null;
                AutomationElement edit = null;
                foreach (AutomationElement control in allChildControls)
                {
                    if (control.Current.ControlType == ControlType.Button)
                    {
                        button = control;
                    }
                    if (control.Current.ControlType == ControlType.Edit)
                    {
                        edit = control;
                    }
                }

                object objPattern2;
                InvokePattern invokePattern;
                if (button.TryGetCurrentPattern(InvokePattern.Pattern, out objPattern2))
                {
                    invokePattern = (InvokePattern)objPattern2;
                    invokePattern.Invoke();
                }

                var allComboxBoxItems =comboBox.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty,ControlType.ListItem));
                var theOne = (from AutomationElement el in allComboxBoxItems
                             where el.Current.Name == msg
                             select el).FirstOrDefault();

                object objPattern3;
                SelectionItemPattern selectionPattern;
                if (theOne != null&&theOne.TryGetCurrentPattern(SelectionItemPattern.Pattern,out objPattern3))
                {
                    selectionPattern = objPattern3 as SelectionItemPattern;
                    selectionPattern.Select();
                }

                Context.InputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);

                object objPattern;
                ValuePattern valuePattern;
                if (edit.TryGetCurrentPattern(ValuePattern.Pattern, out objPattern))
                {
                    valuePattern = (ValuePattern)objPattern;
                    valuePattern.SetValue(msg);
                }
            }
        }
    }
}
