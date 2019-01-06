using System;

namespace Cfoan.Automation
{
    public partial class Actions
    {
        public class SendMessageClick : AbstractSimulateAction
        {
            public SendMessageClick(SilmulateContext ctx)
                :base(ctx)
            {
                
            }

            public override SimulateActionType Type { get { return SimulateActionType.SendMessageClick; } }

            public override void Perform()
            {
                if (Context.CurrentElement != null)
                {
                    WinApi.SendMessage((IntPtr)Context.CurrentElement.Current.NativeWindowHandle, WinApi.WM_CLICK, IntPtr.Zero, null);
                }
            }
        }
        
    }
}
