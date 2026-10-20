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
    public class PropertyGridRichText : UITypeEditor

    {

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)

        {

            return UITypeEditorEditStyle.DropDown;

        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)

        {

            try

            {

                IWindowsFormsEditorService edSvc =

(IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (edSvc != null)

                {

                    if (value is string)

                    {

                        RichTextBox box = new RichTextBox();

                        box.Text = value as string;

                        edSvc.DropDownControl(box);

                        return box.Text;

                    }

                }

            }

            catch (Exception ex)

            {

                System.Console.WriteLine("PropertyGridRichText Error : " + ex.Message);

                return value;

            }

            return value;

        }
         
}
}

