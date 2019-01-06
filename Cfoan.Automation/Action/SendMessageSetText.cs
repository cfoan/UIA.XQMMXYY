using System;

namespace Cfoan.Automation
{
    public partial class Actions
    {
        public class SendMessageSetText : AbstractSimulateAction
        {
            private string msg;
            public SendMessageSetText(SilmulateContext ctx,string message)
                :base(ctx)
            {
                this.msg = message;
            }

            public override SimulateActionType Type { get { return SimulateActionType.SendMessageSetText; } }

            public override void Perform()
            {
                //LogUtil.Debug($"SendMessageSetText:{msg}");
                if (Context.CurrentElement != null)
                {
                    WinApi.SendMessage((IntPtr)Context.CurrentElement.Current.NativeWindowHandle, WinApi.WM_SETTEXT, IntPtr.Zero, msg);
                }
            }
        }

    }
}
