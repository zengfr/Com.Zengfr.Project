using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniTask_UI.common
{
    public class UpdaterUI
    {
        public delegate void UpdateInvoke();
        public static void Update(Control c, UpdateInvoke ik)
        {
            if (!c.InvokeRequired)
                ik.Invoke();
            else
                c.BeginInvoke(ik);
        }
    }
}
