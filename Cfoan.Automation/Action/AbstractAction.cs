using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;

namespace Cfoan.Automation
{
    public enum SimulateActionType
    {
        None = 0,
        SingleVK = 1,
        SendMessageClick = 2,
        SendMessageSetText = 3,
        InputSimulateClick = 4,
        InputSimulateSetText = 5,
        Sleep = 6,
        ComboxBoxSetIndex = 7,
        InvokePattern = 8,
        ValuePattern = 9,
        TogglePattern=10
    }

    public abstract class AbstractSimulateAction
    {
        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public abstract SimulateActionType Type { get; }

        public SilmulateContext Context { get; }

        public AbstractSimulateAction(SilmulateContext ctx)
        {
            Context = ctx;
        }

        public abstract void Perform();
    }
}
