namespace Cfoan.Automation
{
    public class AutomationProperties
    {
        /// <summary>
        /// title 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 例如textBox1
        /// </summary>
        public string AutomationId { get; set; }

        /// <summary>
        /// ClassName
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 例如WinForm,Win32,Null
        /// </summary>
        public string FrameworkId { get; set; }

        /// <summary>
        /// 控件类型
        /// </summary>
        public int ControlTypeId { get; set; }
    }
}
