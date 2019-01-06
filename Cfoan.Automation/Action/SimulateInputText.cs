using System;
using System.Threading;
using System.Windows.Forms;

namespace Cfoan.Automation
{
    public partial class Actions
    {
        public class SimulateInputText : AbstractSimulateAction
        {
            string msg;
            

            public override SimulateActionType Type
            {
                get
                {
                    return SimulateActionType.InputSimulateSetText;
                }
            }

            public SimulateInputText(SilmulateContext ctx, string msg)
                :base(ctx)
            {
                this.msg = msg;
            }

            public override void Perform()
            {
                AutomationUtils.ResetInputLanguage();

                var inputLanguageName = InputLanguage.CurrentInputLanguage.LayoutName;
                if (inputLanguageName.Contains("搜狗") || inputLanguageName.Contains("谷歌")
                    || inputLanguageName.Contains("ABC") || inputLanguageName.Contains("微软"))
                {
                    var handle = Context.CurrentElement != null ? new IntPtr(Context.CurrentElement.Current.NativeWindowHandle) : IntPtr.Zero;
                    if (handle != IntPtr.Zero)
                    {
                        IntPtr prt = WinApi.ImmGetContext(handle);
                        int iMode = 1025;
                        int iSentence = 0;
                        WinApi.ImmSetConversionStatus(prt, iMode, iSentence);
                    }
                }

                if (Context.CurrentElement != null)
                {
                    var rect = Context.CurrentElement.Current.BoundingRectangle;
                    var CenterPoint = new System.Drawing.Point();
                    CenterPoint.X = Convert.ToInt32(rect.Left + rect.Width / 2);
                    CenterPoint.Y = Convert.ToInt32(rect.Top + rect.Height / 2);
                    WinApi.SetCursorPos(CenterPoint.X, CenterPoint.Y);

                    WinApi.mouse_event(WinApi.MOUSEEVENTF_LEFTDOWN, CenterPoint.X, CenterPoint.Y, 0, 0);
                    WinApi.mouse_event(WinApi.MOUSEEVENTF_LEFTUP, CenterPoint.X, CenterPoint.Y, 0, 0);
                    WinApi.mouse_event(WinApi.MOUSEEVENTF_LEFTDOWN, CenterPoint.X, CenterPoint.Y, 0, 0);
                    WinApi.mouse_event(WinApi.MOUSEEVENTF_LEFTUP, CenterPoint.X, CenterPoint.Y, 0, 0);
                }
                Thread.Sleep(200);
                SendKeys.SendWait(msg);//其他方式都不好使 win10 测试
            }
        }
    }
}
