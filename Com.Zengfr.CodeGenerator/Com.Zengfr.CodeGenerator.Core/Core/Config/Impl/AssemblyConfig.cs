namespace Com.Zengfr.CodeGenerator.Config.Impl
{
    using System.ComponentModel;

    /// <summary>
    /// 配置类
    /// </summary>
    public class AssemblyConfig : AbstractConfig
    {
        public AssemblyConfig()
        {

        }
        [Category("0.程序集")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor),
typeof(System.Drawing.Design.UITypeEditor))]
        public virtual string Assembly { set; get; }


    }


}
