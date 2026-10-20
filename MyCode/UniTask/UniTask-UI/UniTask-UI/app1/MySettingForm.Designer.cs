using UniTask_UI.common;

namespace UniTask_UI.p1
{
    partial class MySettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox行程 = new System.Windows.Forms.ComboBox();
            this.comboBox出发 = new System.Windows.Forms.ComboBox();
            this.comboBox截止 = new System.Windows.Forms.ComboBox();
            this.行程 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox间隔2 = new System.Windows.Forms.TextBox();
            this.textBox间隔 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox线程 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox行程
            // 
            this.comboBox行程.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox行程.FormattingEnabled = true;
            this.comboBox行程.Items.AddRange(new object[] {
            "珠海->香港",
            "香港->珠海",
            "澳门->香港",
            "香港->澳门"});
            this.comboBox行程.Location = new System.Drawing.Point(76, 3);
            this.comboBox行程.Name = "comboBox行程";
            this.comboBox行程.Size = new System.Drawing.Size(131, 20);
            this.comboBox行程.TabIndex = 0;
            this.comboBox行程.SelectedValueChanged += new System.EventHandler(this.comboBox行程_SelectedValueChanged);
            // 
            // comboBox出发
            // 
            this.comboBox出发.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox出发.FormattingEnabled = true;
            this.comboBox出发.Location = new System.Drawing.Point(76, 31);
            this.comboBox出发.Name = "comboBox出发";
            this.comboBox出发.Size = new System.Drawing.Size(131, 20);
            this.comboBox出发.TabIndex = 1;
            this.comboBox出发.SelectedIndexChanged += new System.EventHandler(this.comboBox出发_SelectedIndexChanged);
            this.comboBox出发.SelectedValueChanged += new System.EventHandler(this.comboBox出发_SelectedValueChanged);
            // 
            // comboBox截止
            // 
            this.comboBox截止.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox截止.FormattingEnabled = true;
            this.comboBox截止.Location = new System.Drawing.Point(76, 57);
            this.comboBox截止.Name = "comboBox截止";
            this.comboBox截止.Size = new System.Drawing.Size(131, 20);
            this.comboBox截止.TabIndex = 2;
            this.comboBox截止.SelectedIndexChanged += new System.EventHandler(this.comboBox截止_SelectedIndexChanged);
            this.comboBox截止.SelectedValueChanged += new System.EventHandler(this.comboBox截止_SelectedValueChanged);
            // 
            // 行程
            // 
            this.行程.AutoSize = true;
            this.行程.Location = new System.Drawing.Point(3, 6);
            this.行程.Name = "行程";
            this.行程.Size = new System.Drawing.Size(35, 12);
            this.行程.TabIndex = 3;
            this.行程.Text = "路线:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "出发:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "截止:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.行程);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.comboBox行程);
            this.panel1.Controls.Add(this.comboBox截止);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.comboBox出发);
            this.panel1.Location = new System.Drawing.Point(12, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(218, 108);
            this.panel1.TabIndex = 6;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(143, 82);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(64, 23);
            this.button4.TabIndex = 16;
            this.button4.Text = "添加价格";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.textBox间隔2);
            this.panel2.Controls.Add(this.textBox间隔);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.comboBox线程);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(12, 112);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(218, 82);
            this.panel2.TabIndex = 7;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "极低",
            "低",
            "普通",
            "高",
            "最高"});
            this.comboBox1.Location = new System.Drawing.Point(76, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(131, 20);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "优先级别:";
            // 
            // textBox间隔2
            // 
            this.textBox间隔2.Location = new System.Drawing.Point(127, 53);
            this.textBox间隔2.Name = "textBox间隔2";
            this.textBox间隔2.Size = new System.Drawing.Size(34, 21);
            this.textBox间隔2.TabIndex = 5;
            this.textBox间隔2.Text = "3023";
            this.textBox间隔2.TextChanged += new System.EventHandler(this.textBox间隔2_TextChanged);
            // 
            // textBox间隔
            // 
            this.textBox间隔.Location = new System.Drawing.Point(76, 53);
            this.textBox间隔.Name = "textBox间隔";
            this.textBox间隔.Size = new System.Drawing.Size(34, 21);
            this.textBox间隔.TabIndex = 3;
            this.textBox间隔.Text = "3012";
            this.textBox间隔.TextChanged += new System.EventHandler(this.textBox间隔_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "基准(ms):";
            // 
            // comboBox线程
            // 
            this.comboBox线程.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox线程.FormattingEnabled = true;
            this.comboBox线程.Location = new System.Drawing.Point(76, 27);
            this.comboBox线程.Name = "comboBox线程";
            this.comboBox线程.Size = new System.Drawing.Size(131, 20);
            this.comboBox线程.TabIndex = 1;
            this.comboBox线程.SelectedValueChanged += new System.EventHandler(this.comboBox线程_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "并发线程:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(147, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "启动引擎";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button2);
            this.panel3.Controls.Add(this.checkBox4);
            this.panel3.Controls.Add(this.checkBox3);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.checkBox2);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.checkBox1);
            this.panel3.Location = new System.Drawing.Point(12, 200);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(218, 60);
            this.panel3.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(5, 25);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "日志目录";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(167, 3);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(48, 16);
            this.checkBox4.TabIndex = 13;
            this.checkBox4.Text = "抢单";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Enabled = false;
            this.checkBox3.Location = new System.Drawing.Point(113, 3);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(48, 16);
            this.checkBox3.TabIndex = 12;
            this.checkBox3.Text = "刷态";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(59, 3);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(48, 16);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "刷量";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(76, 25);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(64, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "订单历史";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(5, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "刷价";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "提示";
            // 
            // toolTip2
            // 
            this.toolTip2.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip2.ToolTipTitle = "提示";
            // 
            // MySettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 472);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "MySettingForm";
            this.TabText = "详细设置";
            this.Text = "详细设置";
            this.Load += new System.EventHandler(this.MySettingForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        //private void WebBrowser1_Navigated(object sender, System.Windows.Forms.WebBrowserNavigatedEventArgs e)
        //{
        //    webBrowser1.Document.InvokeScript("eval", new string[] { "window.alert = null;" });
        //    LoggerUtil.Info("nav:{0}", e.Url);

        //    string oldDoc = this.webBrowser1.DocumentText;

        //    string xjs = oldDoc;
        //    xjs = xjs.Replace("setInterval(function(){_0x4db1c();},0xfa0);", "");
        //    xjs = xjs.Replace("document.location.reload();", "");
        //    xjs = xjs.Replace(@"x73']){}", @"x73']){break;}");
        //    webBrowser1.Document.Write(xjs);

        //    webBrowser1.Document.InvokeScript("eval", new string[] { "window.alert = null;" });
        //    string cookieStr = webBrowser1.Document.Cookie;
        //}

        #endregion

        private System.Windows.Forms.ComboBox comboBox行程;
        private System.Windows.Forms.ComboBox comboBox出发;
        private System.Windows.Forms.ComboBox comboBox截止;
        private System.Windows.Forms.Label 行程;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBox间隔;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox线程;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox间隔2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
    }
}