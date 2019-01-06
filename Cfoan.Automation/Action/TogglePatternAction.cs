using System.Windows.Automation;

namespace Cfoan.Automation
{
    public partial class Actions
    {
        public class TogglePatternAction: AbstractSimulateAction
        {
            public override SimulateActionType Type
            {
                get
                {
                    return SimulateActionType.TogglePattern;
                }
            }
            string state;
            public TogglePatternAction(SilmulateContext ctx, string state)
                :base(ctx)
            {
                this.state = state;
            }

            public override void Perform()
            {
                if (Context.CurrentElement != null)
                {
                    if (Context.CurrentElement.TryGetCurrentPattern(TogglePattern.Pattern, out object objPattern2))
                    {
                        var togglePattern = (TogglePattern)objPattern2;
                        var toState = ParseState(state);
                        if (!toState.Equals(togglePattern.Current.ToggleState))
                        {
                            //LogUtil.DebugFormat("toggle-->{0}", toState);
                            togglePattern.Toggle();
                        }
                    }
                }
            }

            ToggleState ParseState(string state)
            {
                if ("1".Equals(state))
                { 
                    return ToggleState.On;
                }
                return ToggleState.Off;
                
            }
        }
    }
}
