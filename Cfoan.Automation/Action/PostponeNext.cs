using System.Threading;

namespace Cfoan.Automation
{
    public partial class Actions
    {
        public class PostponeNext : AbstractSimulateAction
        {
            public int SleepMillis { get; private set; }
            public PostponeNext(int millis)
                :base(null)
            {
                SleepMillis = millis;
            }

            public override SimulateActionType Type
            {
                get
                {
                    return SimulateActionType.Sleep;
                }
            }

            public override void Perform()
            {
                Thread.Sleep(SleepMillis);
            }
        }
    }
}
