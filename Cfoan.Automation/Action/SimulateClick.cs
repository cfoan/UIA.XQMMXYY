using System;

namespace Cfoan.Automation
{
    public partial class Actions
    {
        public class SimulateClick : AbstractSimulateAction
        {
            public override SimulateActionType Type
            {
                get
                {
                    return SimulateActionType.InputSimulateClick;
                }
            }

            public SimulateClick(SilmulateContext ctx)
                :base(ctx)
            {
                
            }

            public override void Perform()
            {
                if (Context.CurrentElement != null)
                {
                    var rect = Context.CurrentElement.Current.BoundingRectangle;
                    var CenterPoint = new System.Drawing.Point();
                    CenterPoint.X = Convert.ToInt32(rect.Left + rect.Width / 2);
                    CenterPoint.Y = Convert.ToInt32(rect.Top + rect.Height / 2);
                    WinApi.SetCursorPos(CenterPoint);
                    WinApi.mouse_event(WinApi.MOUSEEVENTF_LEFTDOWN, CenterPoint.X, CenterPoint.Y, 0, 0);
                    WinApi.mouse_event(WinApi.MOUSEEVENTF_LEFTUP, CenterPoint.X, CenterPoint.Y, 0, 0);
                }
            }
        }
    }
}
