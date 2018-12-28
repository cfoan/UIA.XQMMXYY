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
                if (Context.Element != null)
                {
                    WinApi.SendMessage((IntPtr)Context.Element.Current.NativeWindowHandle, WinApi.WM_CLICK, IntPtr.Zero, null);
                }
            }
        }
        
    }
}
