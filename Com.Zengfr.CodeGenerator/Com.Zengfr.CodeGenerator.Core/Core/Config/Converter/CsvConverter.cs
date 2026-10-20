namespace Com.Zengfr.CodeGenerator.Config.Converter
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Linq;
    using System.Text;
    
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
