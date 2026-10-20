using Com.Zengfr.CodeGenerator.Config;
using Com.Zengfr.CodeGenerator.Config.Impl;
using WeifenLuo.WinFormsUI.Docking;
namespace Com.Zengfr.CodeGenerator.UI.Common
{
    public class VarUtils
    {
        static VarUtils()
        {
            DataAbstractConfig = new DataAbstractConfig();
            EntityConfig = new AssemblyConfig();
            ConfigType = 1;
        }
        public static string SolutionDirectory { get; set; }
        public static EnvDTE80.DTE2 DTEObject { get; set; }
        public static DockPanel DockPanel { get; set; }

        private static DataAbstractConfig DataAbstractConfig { get; set; }
        private static AssemblyConfig EntityConfig { get; set; }

        private static int ConfigType = 1;
        public static int ChangeConfigType()
        {
            return ConfigType = (ConfigType == 1 ? 2 : 1);
        }
        public static void ShowInfo(string message)
        {
            UIUtils.ShowInfo(DockPanel, message);
        }
        public static void ShowError(System.Exception ex)
        {
            UIUtils.ShowError(DockPanel, ex);
        }
        public static AbstractConfig Config
        {
            get
            {
                switch (ConfigType)
                {
                    case 1: return DataAbstractConfig;
                    case 2: return EntityConfig;
                    default: return null;
                }
            }
        }
    }

}
