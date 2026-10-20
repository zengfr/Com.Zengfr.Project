using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockSample;
using log4net;
using UniTask_UI.common;
using UniTask_UI.p1.c;

namespace UniTask_UI.p1
{
    public partial class MySettingForm : ToolWindow
    {
         
        public MySettingForm()
        {
            InitializeComponent();
            
        }
        private void init2()
        {
            MyCtrl.Instance.settingForm = this;
            for (int i = 0; i < 15; i++)
            {
                var d = DateTime.Today.AddDays(i);
                this.comboBox出发.Items.Add(d);
                this.comboBox截止.Items.Add(d);
            }
            for (int i = 1; i <= 5; i += 1)
            {
                this.comboBox线程.Items.Add(i);
            }
            for (int i = 6; i < 30; i+=2)
            {
                this.comboBox线程.Items.Add(i);
            }
            load();
            load2();
            toolTip1.SetToolTip(textBox间隔, "单个账号,两次请求间隔时间");
            toolTip2.SetToolTip(textBox间隔2, "错误或异常,间隔时间");
        }
        private void load()
        {
            LoggerUtil.Info("load");
            MyData d = MyData.Instance;
            var gs = d.goalSetting;
            this.comboBox线程.SelectedItem = gs.initThread;
            textBox间隔.Text= "" + gs.wait4q;
            textBox间隔2.Text = "" + gs.wait4e;
            this.comboBox行程.SelectedItem = d.setting.line;
            this.comboBox截止.SelectedIndex =3;
            this.comboBox出发.SelectedIndex = 1;
             
            checkBox1.Checked = gs.isStart4p;
            checkBox2.Checked = gs.isStart4q;
            checkBox3.Checked = gs.isStart4s;
            checkBox4.Checked = gs.isStart4o;

            comboBox1.SelectedIndex= (int)gs.priority;

            d.goalSetting.init = true;
            

        }
        private void load2()
        {
            MyData d = MyData.Instance;
            int s1 = d.goalSetting.totalOrders;
            int s2 = d.orders.Count;
            var t1= String.Format("累计订单{0}:{1:D5}", "", s1 < s2 ? s2 : s1);
            t1 += String.Format("{0}{1:D5}", "|", s2);
            t1 += String.Format("{0}{1:D5}", "|", d.orders.Where(t=>t.LastUpdate>=DateTime.Today).Count());
            t1 += String.Format("{0}:{1}|{2}|Q:362505707", "|时间"
                , DateTime.Now.ToString("yy-MM-dd HH:mm:ss.ffff")
                , d.goalSetting.regDate.ToString("yy-MM-dd HH:mm:ss.ffff"));
            LoggerUtil.Info("{0}",t1);
            foreach (Order order in d.orders)
                LoggerUtil.Info ("订单:{0} 客户:{1} 账户:{2} 备注:{3}", order.OrderNum, order.CustomerName, order.AccountName, order.Comment);
        }
        private void save()
        {
            UpdaterUI.Update(this, new UpdaterUI.UpdateInvoke(save_));
        }
        private void save_()
        {
            MyData d = MyData.Instance;
            var gs = d.goalSetting;
            var s = d.setting;
            if (gs.init)
            {
                
                s.line = "" + comboBox行程.SelectedItem;
                s.start = "" + comboBox出发.SelectedItem;
                s.end = "" + comboBox截止.SelectedItem;

                gs.isStart4p = checkBox1.Checked;
                gs.isStart4q = checkBox2.Checked;
                gs.isStart4s = checkBox3.Checked;
                gs.isStart4o = checkBox4.Checked;
                //button3.Enabled =! d.setting.autoRefresh;
                gs.isStart = button1.Text.Contains("停止");
                int.TryParse(textBox间隔.Text, out gs.wait4q);
                int.TryParse(textBox间隔2.Text, out gs.wait4e);
               
                gs.initThread = int.Parse(comboBox线程.SelectedItem == null ? "2" : "" + comboBox线程.SelectedItem);
                //gs.initThread = 3;
                if (gs.wait4q < 200)
                {
                    gs.wait4q = 200;
                    //textBox间隔.Text = "" + gs.wait4q;
                }
                if (gs.wait4e < 200)
                {
                    gs.wait4e = 200; 
                    //textBox间隔2.Text = "" + gs.wait4e;
                }
                 gs.wait4o = gs.wait4p = gs.wait4s= gs.wait4q ;
                if (gs.wait <= 50)
                {
                    gs.wait = 50;
                }
                switch (d.setting.line)
                {
                    case "珠海->香港":
                        d.setting.lineCode = "ZHOHKG";
                        break;
                    case "香港->珠海":
                        d.setting.lineCode = "HKGZHO";
                        break;
                    case "香港->澳门":
                        d.setting.lineCode = "HKGMAC";
                        break;
                    case "澳门->香港":
                        d.setting.lineCode = "MACHKG";
                        break;
                    default:
                        d.setting.lineCode = "HKGZHO";
                        break;
                }
                d.goalSetting.priority = (Amib.Threading.WorkItemPriority)comboBox1.SelectedIndex;
                LoggerUtil.Info("save");
            }
        }
        private void chackDate()
        {
            int i = this.comboBox出发.SelectedIndex;
            int j = this.comboBox截止.SelectedIndex;
            if (i > j)
            {
                var msg = "日期出发 必须<= 截止日期";
                MessageBox.Show(msg);
                LoggerUtil.Warn(msg);
            }
            else
            {
                save();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            UpdaterUI.Update(this, new UpdaterUI.UpdateInvoke(runyq));
        }
        private void runyq()
        {
           
            button1.Enabled = !button1.Enabled;
            MyData d = MyData.Instance;
            var  isStart =! d.goalSetting.isStart;
            if (isStart)
            {
                //MyCtrl.Instance.webBrowser1 = webBrowser1;
                button1.Text = "停止引擎";
                save();
                this.comboBox出发.Enabled = false;
                this.comboBox截止.Enabled = false;
                this.comboBox行程.Enabled = false;
                foreach(var a in d.accounts)
                {
                    a.Use = 0;
                    a.Status = "未执行";
                }
                MyTaskUtil.Instance.start();
            }
            else
            {
                button1.Text = "启动引擎";
                save();
                this.comboBox出发.Enabled = true;
                this.comboBox截止.Enabled = true;
                this.comboBox行程.Enabled = true;
                MyTaskUtil.Instance.stop();
            }
            button1.Enabled = !button1.Enabled;

        }

        private void MySettingForm_Load(object sender, EventArgs e)
        {
            init2();
        }
       
        private void comboBox出发_SelectedIndexChanged(object sender, EventArgs e)
        {
            chackDate();

        }

        private void comboBox截止_SelectedIndexChanged(object sender, EventArgs e)
        {
            chackDate();
        }

        private void comboBox线程_SelectedValueChanged(object sender, EventArgs e)
        {
            save();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            save();
        }
        private void textBox间隔_TextChanged(object sender, EventArgs e)
        {
            save();
        }
        private void textBox间隔2_TextChanged(object sender, EventArgs e)
        {
            save();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            save();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            save();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = checkBox3.Checked;
            save();
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = checkBox4.Checked;
            save();
        }
        private void comboBox行程_SelectedValueChanged(object sender, EventArgs e)
        {
            save();
        }

        private void comboBox出发_SelectedValueChanged(object sender, EventArgs e)
        {
            save();
        }

        private void comboBox截止_SelectedValueChanged(object sender, EventArgs e)
        {
            save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = !button3.Enabled;
            bbrefresh(null);
            button3.Enabled = !button3.Enabled;
        }
 
        private void bbrefresh(object state)
        {
            
            load2(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = !button2.Enabled;
            var s = String.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory, "logs");
            System.Diagnostics.Process.Start(s);
            button2.Enabled = !button2.Enabled;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdaterUI.Update(button4, new  UpdaterUI.UpdateInvoke(runtask1) );
        }
        private void runtask1() { 
            button4.Enabled = !button4.Enabled;
            try
            {
                MyData d = MyData.Instance;
                if (false)
                {
                    d.prices.Clear();
                    d.products.Clear();
                }

                var ps = d.prices.Values.Where(t => t.buyDate<DateTime.Today.AddDays(-2)).ToList();
                foreach(var p in ps)
                {
                    var k = String.Format("{0},{1},{2}", p.lineCode, p.buyDate.ToString("yyyy-MM-dd"), "" + p.feeType);
                    d.prices.Remove(k);
                }

                MyTaskThread4padd t4q = new MyTaskThread4padd();
                t4q.Name = "行程";
                t4q.init();
                t4q.runTask();
                //Thread.Sleep(3000);
                t4q.cancel();
            }catch(Exception ex)
            {
                LoggerUtil.Error("行程", ex);
            }
            button4.Enabled = !button4.Enabled;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            save();
        }
    }
}
