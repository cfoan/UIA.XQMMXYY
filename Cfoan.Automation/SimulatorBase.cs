﻿using AssertLibrary;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;

namespace Cfoan.Automation
{
    
    public enum WaitState
    {
        WaitProcess,
        WaitMainFormFirst,
        WaitMainFormSecond,
        WaitAction
    }
    
    public class SimulatorBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SimulatorBase));
        private readonly List<int> m_existedProcessIdsThisApp;
        private AppSilmulateInfo m_silmulateInfo;
        private NameValueCollection m_appSettings;
        private SilmulateContext m_silmulateContext;

        public SimulatorBase(AppSilmulateInfo appSilmulateInfo)
        {
            Assert.IsNotNull(appSilmulateInfo);
            Assert.IsNotNull(appSilmulateInfo.ProcessData);
            Assert.IsNotNull(appSilmulateInfo.ProcessData.FileName);
            Assert.IsNotNull(appSilmulateInfo.ProcessData.MainWindowFileName);
            Assert.IsNotNull(appSilmulateInfo.AutomationData);

            this.m_appSettings = ConfigurationManager.AppSettings;
            this.m_existedProcessIdsThisApp = new List<int>();
            this.m_silmulateInfo = appSilmulateInfo;
            this.m_silmulateContext = new SilmulateContext(m_silmulateInfo);

            var processes = AutomationUtils.GetAllProcessesByPath(MainWindowFile);
            processes.ToList().ForEach((p) =>
            {
                m_existedProcessIdsThisApp.Add(p.Id);
            });
        }

        WaitState State { get; set; }

        string StartFile => m_silmulateInfo?.ProcessData?.FileName;
        string MainWindowFile => m_silmulateInfo?.ProcessData?.MainWindowFileName??StartFile;
        public void Start(List<KeyValuePair<string, string>> parameters)
        {
            var @params = parameters.ToDictionary((pair) => pair.Key, (pair) => pair.Value);
            StartProcess(m_silmulateInfo.ProcessData);
            WaitForMainWindow();
            WaitForActions(@params);
        }

        private void StartProcess(ProcessData processData)
        {
            var path = Environment.GetEnvironmentVariable("Path");
            var process = new Process();//新进程
            process.StartInfo.FileName = processData.FileName;//打开cmd程序
            process.StartInfo.UseShellExecute = false;//不使用shell启动程序
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(processData.FileName);
            process.StartInfo.Arguments = processData.StartArgument;
            process.StartInfo.EnvironmentVariables["path"] = path;
            process.Start();
            WinApi.ShowWindow(process.MainWindowHandle, 4);
            WinApi.SetActiveWindow(process.MainWindowHandle);
            WinApi.SetForegroundWindow(process.MainWindowHandle);
            logger.Debug($"启动参数:{process.StartInfo.Arguments}");
        }

        private void WaitForMainWindow()
        {
            var automationInfo = m_silmulateContext.AutomationInfo;
            Assert.IsNotNull(automationInfo);
            int.TryParse(m_appSettings["timeout"], out int timeout);
            if (timeout <= 0) { timeout = 15000; }
            DateTime startTime = DateTime.UtcNow;
            Process process = null;
            IntPtr mainWindowHandle = IntPtr.Zero;
            AutomationElement element = null;
            int loop = 1;
            for (; ; )
            {
                if ((DateTime.UtcNow - startTime).TotalMilliseconds > timeout || element != null)
                {
                    break;
                }

                try
                {
                    if (State.Equals(WaitState.WaitProcess))
                    {
                        loop++;
                        logger.Debug($"[Loop:{loop}],主进程:{MainWindowFile}");
                        process = AutomationUtils.GetAllProcessesByPath(MainWindowFile).FirstOrDefault((p) => IsProcessJustStarted(p));
                        if (process != null)
                        {
                            process.WaitForInputIdle();
                            m_silmulateContext.Process = process;
                            State = WaitState.WaitMainFormFirst;
                            logger.Debug($"[Loop:{loop}]process started,主进程:{MainWindowFile},processId:{process.Id}");
                        }
                    }
                    else if (State.Equals(WaitState.WaitMainFormFirst))
                    {
                        process = Process.GetProcessById(process.Id);
                        if (process == null || process.HasExited)
                        {
                            State = WaitState.WaitProcess;
                            continue;
                        }
                        logger.Debug($"[Loop:{loop}]wait for mainform");
                        if (process.MainWindowHandle != IntPtr.Zero)
                        {
                            mainWindowHandle = process.MainWindowHandle;
                            logger.Debug($"[Loop:{loop}]get mainform {process.MainWindowTitle},{process.MainWindowHandle}");
                            State = WaitState.WaitMainFormSecond;
                        }
                        else
                        {

                            var root = WinApi.GetDesktopWindow();
                            var wantWindowTitle = m_silmulateInfo.ProcessData.MainWindowTitle;
                            //todo 会有直接退出的情况
                            WinApi.EnumChildWindows(root, (hwnd, lParam) =>
                            {
                                StringBuilder sb = new StringBuilder();
                                WinApi.GetWindowText(hwnd, sb, 512);
                                if (sb.ToString().Equals(wantWindowTitle))
                                {
                                    mainWindowHandle = hwnd;

                                    return false;
                                }
                                return true;
                            }, 0);
                            logger.Debug($"[Loop:{loop}]get mainform,by win32Api:{wantWindowTitle},{mainWindowHandle}");
                            if (mainWindowHandle != IntPtr.Zero)
                            {
                                State = WaitState.WaitMainFormSecond;
                            }
                        }
                    }
                    else if (State.Equals(WaitState.WaitMainFormSecond))
                    {
                        var handle = mainWindowHandle;
                        if (handle == IntPtr.Zero || process.HasExited)
                        {
                            State = WaitState.WaitProcess;
                            continue;
                        }
                        logger.Debug($"[Loop:{loop}]validate mainform:{process.ProcessName},pid:{process.Id},MainWindowHandle:{handle}");
                        AutomationElement target = AutomationElement.FromHandle(handle);
                        if (target != null)
                        {
                            if (automationInfo != null && automationInfo.AutomationProperties != null
                                && ((string.IsNullOrEmpty(automationInfo.AutomationProperties.ClassName) || automationInfo.AutomationProperties.ClassName.Equals(target.Current.ClassName))
                                && (string.IsNullOrEmpty(automationInfo.AutomationProperties.AutomationId) || automationInfo.AutomationProperties.AutomationId.Equals(target.Current.AutomationId))
                                && (string.IsNullOrEmpty(automationInfo.AutomationProperties.Name) || automationInfo.AutomationProperties.Name.Equals(target.Current.Name))))
                            {
                                logger.Debug($"[Loop:{loop}]method1,validate sucess");
                                element = target;
                                break;
                            }
                        }

                        if (process.HasExited)
                        {
                            State = WaitState.WaitProcess;
                            continue;
                        }
                        //方法2
                        target = AutomationElement.RootElement.FindFirst(TreeScope.Element | TreeScope.Children, AutomationUtils.GetLookupConditions(automationInfo.AutomationProperties));

                        if (target != null)
                        {
                            logger.Debug($"[Loop:{loop}]method2,validate sucess");
                            element = target;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    State = WaitState.WaitProcess;
                    logger.Debug($"Error:{ex.Message}");
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }

            logger.Info($"找到启动窗体，耗时{(DateTime.UtcNow - startTime).TotalMilliseconds}ms");
            if (element != null)
            {
                State = WaitState.WaitAction;
                m_silmulateContext.Found(element);
            }
        }

        private void WaitForActions(Dictionary<String, String> parameters)
        {
            bool running = true;
            while (m_silmulateContext.MoveNext()&& running)
            {
                logger.Debug($"------开始找第{m_silmulateContext.Index}个控件------");
                var childInfo = m_silmulateContext.AutomationInfo;
                var parentId = childInfo.Config?.ParentId;
                if (parentId != null)
                {
                    var parent = m_silmulateContext.GetFromCache(parentId.Value);
                    var treeScope = childInfo.Config.IncludeDescendants ? TreeScope.Descendants : TreeScope.Children;
                    var me = AutomationUtils.FindElements(childInfo.AutomationProperties, treeScope, parent, index: childInfo.Config.Index);
                    if (me == null) { return; }
                    var index = m_silmulateContext.Found(me);
                    logger.Debug($"put into cache,{index},{JsonConvert.SerializeObject(AutomationUtils.GetAumationData(me))}");
                    running = SimulateAction.WaitForActionToComplete(m_silmulateContext, childInfo.Config, parameters);
                }
                Thread.Sleep(200);
            }
        }

        private bool IsProcessJustStarted(Process process)
        {
            return !m_existedProcessIdsThisApp.Contains(process.Id);
        }
    }
}