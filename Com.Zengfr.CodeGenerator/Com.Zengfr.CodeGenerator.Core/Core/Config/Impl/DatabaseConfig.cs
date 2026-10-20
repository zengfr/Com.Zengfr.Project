namespace Com.Zengfr.CodeGenerator.Config.Impl
{
    using System.Collections.Generic;
    using System.ComponentModel;
    //using Com.Zengfr.VsAddin.CodeGenerator.View;
    using SchemaExplorer;

    /// <summary>
    /// 配置类
    /// </summary>
    public class DataAbstractConfig : AbstractConfig
    {
        public DataAbstractConfig()
        {
            this.Tables = new List<string>();
            this.Views = new List<string>();
        }
        [Category("0.数据源")]
        [Editor(typeof(Editor.DatabaseSchemaEditor),
typeof(System.Drawing.Design.UITypeEditor))]
        public virtual DatabaseSchema DatabaseSchema { set; get; }

        [Category("1.数据设置")]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
        "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
       typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(CsvConverter))]
        public virtual IList<string> Tables { set; get; }
        [Category("1.数据设置")]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
        "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
       typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(CsvConverter))]
        public virtual IList<string> Views { set; get; }
    }


}
