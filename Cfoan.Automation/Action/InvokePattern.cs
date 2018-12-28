using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace Cfoan.Automation
{
    public partial class Actions
    {
        public class InvokePatternAction : AbstractSimulateAction
        {
            public override SimulateActionType Type
            {
                get
                {
                    return SimulateActionType.InvokePattern;
                }
            }

            public InvokePatternAction(SilmulateContext ctx)
                :base(ctx)
            {

            }

            public override void Perform()
            {
                if (Context.Element != null)
                {
                    InvokePattern invokePattern;
                    if (Context.Element.TryGetCurrentPattern(InvokePattern.Pattern, out object patternObject))
                    {
                        invokePattern = (InvokePattern)patternObject;
                        invokePattern.Invoke();
                    }
                }
            }
        }
    }
}

