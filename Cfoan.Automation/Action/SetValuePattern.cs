using System.Windows.Automation;

namespace Cfoan.Automation
{
    public partial class Actions
    {
        public class SetValuePattern : AbstractSimulateAction
        {
            public override SimulateActionType Type
            {
                get
                {
                    return SimulateActionType.ValuePattern;
                }
            }

            string msg;

            public SetValuePattern(SilmulateContext ctx, string msg)
                :base(ctx)
            {
                this.msg = msg;
            }

            public override void Perform()
            {
                if (Context.Element != null)
                {
                    if (Context.Element.TryGetCurrentPattern(ValuePattern.Pattern, out object objPattern2))
                    {
                        ValuePattern invokePattern = (ValuePattern)objPattern2;
                        invokePattern.SetValue(msg);
                    }
                }
            }
        }
    }
}
