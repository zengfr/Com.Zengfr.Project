namespace Com.Zengfr.CodeGenerator.Config
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Linq;
    using System.Text;
    /// <summary>
    /// 配置类
    /// </summary>
    public abstract class AbstractConfig
    {
        public AbstractConfig()
        {
            this.TablePrefix = "ut_";
            this.NamespacePrefix = "Com.Zengfr.Proj";
            this.NamespaceSuffix = string.Empty;
            this.FileExtention = ".cs";
            this.FileNamePrefix = string.Empty;
            this.FileName = string.Empty;
            this.FileNameSuffix = "_gen";
            this.OutputFolder = string.Empty;
            this.OutputEncoding = Encoding.UTF8;
            this.OutputZIP = true;
            this.Templates = new List<string>();
        }

        [Category("2.模板")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(UITypeEditor))]
        public virtual string TemplatesDir { set; get; }
        [Category("2.模板")]
        [ReadOnlyAttribute(true)]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
       "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
      typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(CsvConverter))]
        public virtual IList<string> Templates { set; get; }
        [Category("3.设置")]
        public virtual string TablePrefix { set; get; }
        [Category("3.设置")]
        public virtual string NamespacePrefix { set; get; }
        [Category("3.设置")]
        public virtual string NamespaceSuffix { set; get; }
        [Category("3.设置")]
        public virtual string ProjectName { set; get; }

        [Category("4.输出")]
        public virtual bool OutputSingleFile { get; set; }
        [Category("4.输出")]
        public virtual string OutputFolder { set; get; }
        [Category("4.输出")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(UITypeEditor))]
        public virtual string OutputRootDir { set; get; }
        [Category("4.输出")]
        public virtual bool OutputZIP { set; get; }
        [Category("4.输出")]
        [ReadOnlyAttribute(true)]
        public virtual Encoding OutputEncoding { set; get; }

        [Category("4.输出")]
        public virtual string FileName { set; get; }
        [Category("4.输出")]
        public virtual string FileNamePrefix { set; get; }
        [Category("4.输出")]
        public virtual string FileNameSuffix { set; get; }
        [Category("4.输出")]
        public virtual string FileExtention { set; get; }

        [Category("4.输出")]
        public virtual bool GenImplementEmptyFile { set; get; }
        [Category("4.输出")]
        public virtual bool GenProjFile { set; get; }

        [ReadOnlyAttribute(true)]
        [Category("9.其他")]
        public virtual string AddinDir { set; get; }

        [ReadOnlyAttribute(true)]
        [Category("9.其他")]
        public virtual string AddinName { get { return "代码生成插件V2.0"; } }
        [ReadOnlyAttribute(true)]
        [Category("9.其他")]
        public virtual string AddinEditor { get { return "曾繁荣 2013.12.27"; } }
    }

    public class CsvConverter : TypeConverter
    {

        public override object ConvertTo(ITypeDescriptorContext context,
           System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            IList<string> v = value as IList<string>;
            if (v != null && destinationType == typeof(string))
            {
                return string.Join(",", v.ToArray());
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
