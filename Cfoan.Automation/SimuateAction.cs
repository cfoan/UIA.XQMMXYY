using Cfoan.Automation.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Windows.Automation;
using static Cfoan.Automation.Actions;

namespace Cfoan.Automation
{
    public class SimulateAction
    {
        static Dictionary<int, Action<SilmulateContext, ConfigItem, Dictionary<string, string>>> handlers = new Dictionary<int, Action<SilmulateContext, ConfigItem, Dictionary<string, string>>>();
        static ILog logger = LogManager.GetLogger(typeof(SimulateAction));
        static SimulateAction()
        {
            handlers[(int)SimulateActionType.None] = ((ctx, item, parameters) =>
            {

            });

            handlers[(int)SimulateActionType.SingleVK] = ((ctx, item, parameters) =>
              {
                  new SingleVirtualKey(ctx,item.VKCode).Perform();

              });

            handlers[(int)SimulateActionType.SendMessageSetText] = ((ctx, item, parameters) =>
            {
                if (parameters.TryGetValue(item.ParameterName, out string parameter))
                {
                    new SendMessageSetText(ctx, parameter).Perform();
                }
            });

            handlers[(int)SimulateActionType.SendMessageClick] = ((ctx, item, parameters) =>
            {
                new SendMessageClick(ctx).Perform();
            });

            handlers[(int)SimulateActionType.SendMessageSetText] = ((ctx, item, parameters) =>
            {
                if (parameters.TryGetValue(item.ParameterName, out string parameter))
                {
                    new SendMessageSetText(ctx, parameter).Perform();
                }

            });

            handlers[(int)SimulateActionType.InputSimulateClick] = ((ctx, item, parameters) =>
            {
                new SimulateClick(ctx).Perform();
            });

            handlers[(int)SimulateActionType.InputSimulateSetText] = ((ctx, item, parameters) =>
            {
                if (parameters.TryGetValue(item.ParameterName, out string parameter))
                {
                    new SimulateInputText(ctx, parameter).Perform();
                    
                }
            });

            handlers[(int)SimulateActionType.Sleep] = ((ctx, item, parameters) =>
            {
                new PostponeNext(item.SleepMillis).Perform();
            });

            handlers[(int)SimulateActionType.ComboxBoxSetIndex] = ((ctx, item, parameters) =>
            {
                parameters.TryGetValue(item.ParameterName, out string parameter);
                new ComboBoxSetIndex(ctx, parameter, null).Perform();
            });

            handlers[(int)SimulateActionType.ValuePattern] = ((ctx, item, parameters) =>
            {
                if (parameters.TryGetValue(item.ParameterName, out string parameter))
                {
                    new SetValuePattern(ctx, parameter).Perform();
                }
            });

            handlers[(int)SimulateActionType.InvokePattern] = ((ctx, item, parameters) =>
            {
                new InvokePatternAction(ctx).Perform();
            });

            handlers[(int)SimulateActionType.TogglePattern] = ((ctx, item, parameters) =>
            {
                if (parameters.TryGetValue(item.ParameterName, out string parameter))
                {
                    new TogglePatternAction(ctx, parameter).Perform(); 
                }
            });
        }

        public static bool WaitForActionToComplete(SilmulateContext ctx, ConfigItem childConfig, Dictionary<string, string> parameters)
        {
            try
            {
                handlers[childConfig.ActionType]?.Invoke(ctx, childConfig, parameters);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                return false;
            }
        }
    }
}
