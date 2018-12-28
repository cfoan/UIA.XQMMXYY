using System;
using WindowsInput.Native;

namespace Cfoan.Automation
{
    public partial class Actions
    {
        public class SingleVirtualKey : AbstractSimulateAction
        {
            private readonly VirtualKeyCode keyCode;
            
            public SingleVirtualKey(SilmulateContext ctx,VirtualKeyCode keyCode)
                :base(ctx)
            {
                this.keyCode = keyCode;
            }

            public override SimulateActionType Type
            {
                get
                {
                    return SimulateActionType.SingleVK;
                }
            }

            public override void Perform()
            {
                Context.InputSimulator.Keyboard.KeyPress(keyCode);
            }
        }
    }
}
