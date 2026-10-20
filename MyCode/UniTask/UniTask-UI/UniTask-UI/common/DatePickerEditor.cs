using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace UniTask_UI.common
{
   public  class DatePickerEditor: UITypeEditor {
    IWindowsFormsEditorService editorService;
        DateTimePicker picker = new DateTimePicker();
        string time;
        string customFormat = "yyyy-MM-dd";
        public DatePickerEditor()
        {
            picker.MinDate = DateTime.Today.AddYears(-1);
            picker.MaxDate = DateTime.Today.AddYears(1);
            picker.Format = DateTimePickerFormat.Custom;
            picker.CustomFormat = customFormat;
            picker.ShowUpDown = false;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                this.editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }
            if (this.editorService != null)
            {
                if (value == null)
                {
                    time = DateTime.Today.ToString(customFormat);
                }
                this.editorService.DropDownControl(picker);
                value = DateTime.Parse(picker.Value.ToString(customFormat));
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
}
