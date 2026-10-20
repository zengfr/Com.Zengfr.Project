using DockSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTask_UI.Task
{
    public partial class TaskSettingForm : ToolWindow
    {
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblWaitingCallbacks;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblWorkItemsGenerated;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblThreadInUse;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblThreadsInPool;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown spinConsumingTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown spinInterval;
        private System.Windows.Forms.NumericUpDown spinIdleTimeout;
        private System.Windows.Forms.NumericUpDown spinMaxThreads;
        private System.Windows.Forms.NumericUpDown spinMinThreads;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblWorkItemsCompleted;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblWaitingCallbacks = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblWorkItemsGenerated = new System.Windows.Forms.Label();
            this.lblWorkItemsCompleted = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblThreadInUse = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblThreadsInPool = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.spinConsumingTime = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.spinInterval = new System.Windows.Forms.NumericUpDown();
            this.spinIdleTimeout = new System.Windows.Forms.NumericUpDown();
            this.spinMaxThreads = new System.Windows.Forms.NumericUpDown();
            this.spinMinThreads = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.timerPoll = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinConsumingTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinIdleTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinMaxThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinMinThreads)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblWaitingCallbacks);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.lblWorkItemsGenerated);
            this.groupBox2.Controls.Add(this.lblWorkItemsCompleted);
            this.groupBox2.Location = new System.Drawing.Point(12, 31);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(168, 96);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Work items";
            // 
            // lblWaitingCallbacks
            // 
            this.lblWaitingCallbacks.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaitingCallbacks.Location = new System.Drawing.Point(64, 16);
            this.lblWaitingCallbacks.Name = "lblWaitingCallbacks";
            this.lblWaitingCallbacks.Size = new System.Drawing.Size(80, 24);
            this.lblWaitingCallbacks.TabIndex = 22;
            this.lblWaitingCallbacks.Text = "XXXXXXXXX";
            this.lblWaitingCallbacks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 24);
            this.label9.TabIndex = 21;
            this.label9.Text = "Queued";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(8, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 24);
            this.label8.TabIndex = 25;
            this.label8.Text = "Generated";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(8, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 24);
            this.label10.TabIndex = 26;
            this.label10.Text = "Completed";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWorkItemsGenerated
            // 
            this.lblWorkItemsGenerated.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkItemsGenerated.Location = new System.Drawing.Point(64, 40);
            this.lblWorkItemsGenerated.Name = "lblWorkItemsGenerated";
            this.lblWorkItemsGenerated.Size = new System.Drawing.Size(80, 24);
            this.lblWorkItemsGenerated.TabIndex = 27;
            this.lblWorkItemsGenerated.Text = "XXXXXXXXX";
            this.lblWorkItemsGenerated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWorkItemsCompleted
            // 
            this.lblWorkItemsCompleted.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkItemsCompleted.Location = new System.Drawing.Point(64, 64);
            this.lblWorkItemsCompleted.Name = "lblWorkItemsCompleted";
            this.lblWorkItemsCompleted.Size = new System.Drawing.Size(80, 24);
            this.lblWorkItemsCompleted.TabIndex = 28;
            this.lblWorkItemsCompleted.Text = "XXXXXXXXX";
            this.lblWorkItemsCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblThreadInUse);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.lblThreadsInPool);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(12, 133);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(168, 72);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Threads";
            // 
            // lblThreadInUse
            // 
            this.lblThreadInUse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThreadInUse.Location = new System.Drawing.Point(80, 40);
            this.lblThreadInUse.Name = "lblThreadInUse";
            this.lblThreadInUse.Size = new System.Drawing.Size(80, 24);
            this.lblThreadInUse.TabIndex = 18;
            this.lblThreadInUse.Text = "XXXXXXXXX";
            this.lblThreadInUse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 24);
            this.label7.TabIndex = 17;
            this.label7.Text = "Used (Green)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblThreadsInPool
            // 
            this.lblThreadsInPool.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThreadsInPool.Location = new System.Drawing.Point(80, 16);
            this.lblThreadsInPool.Name = "lblThreadsInPool";
            this.lblThreadsInPool.Size = new System.Drawing.Size(80, 24);
            this.lblThreadsInPool.TabIndex = 11;
            this.lblThreadsInPool.Text = "XXXXXXXXX";
            this.lblThreadsInPool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "In pool (Red)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spinConsumingTime
            // 
            this.spinConsumingTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.spinConsumingTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spinConsumingTime.Increment = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.spinConsumingTime.Location = new System.Drawing.Point(6, 175);
            this.spinConsumingTime.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.spinConsumingTime.Name = "spinConsumingTime";
            this.spinConsumingTime.Size = new System.Drawing.Size(68, 29);
            this.spinConsumingTime.TabIndex = 45;
            this.spinConsumingTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.spinConsumingTime.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(80, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(142, 24);
            this.label6.TabIndex = 44;
            this.label6.Text = "item consuming time";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spinInterval
            // 
            this.spinInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.spinInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spinInterval.Increment = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.spinInterval.Location = new System.Drawing.Point(6, 140);
            this.spinInterval.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.spinInterval.Name = "spinInterval";
            this.spinInterval.Size = new System.Drawing.Size(68, 29);
            this.spinInterval.TabIndex = 43;
            this.spinInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.spinInterval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // spinIdleTimeout
            // 
            this.spinIdleTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.spinIdleTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spinIdleTimeout.Location = new System.Drawing.Point(6, 88);
            this.spinIdleTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinIdleTimeout.Name = "spinIdleTimeout";
            this.spinIdleTimeout.Size = new System.Drawing.Size(68, 29);
            this.spinIdleTimeout.TabIndex = 42;
            this.spinIdleTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.spinIdleTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // spinMaxThreads
            // 
            this.spinMaxThreads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.spinMaxThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spinMaxThreads.Location = new System.Drawing.Point(6, 56);
            this.spinMaxThreads.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.spinMaxThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinMaxThreads.Name = "spinMaxThreads";
            this.spinMaxThreads.Size = new System.Drawing.Size(68, 29);
            this.spinMaxThreads.TabIndex = 41;
            this.spinMaxThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.spinMaxThreads.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spinMaxThreads.ValueChanged += new System.EventHandler(this.spinMaxThreads_ValueChanged);
            // 
            // spinMinThreads
            // 
            this.spinMinThreads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.spinMinThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spinMinThreads.Location = new System.Drawing.Point(6, 20);
            this.spinMinThreads.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.spinMinThreads.Name = "spinMinThreads";
            this.spinMinThreads.Size = new System.Drawing.Size(68, 29);
            this.spinMinThreads.TabIndex = 40;
            this.spinMinThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.spinMinThreads.ValueChanged += new System.EventHandler(this.spinMinThreads_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(80, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 24);
            this.label5.TabIndex = 39;
            this.label5.Text = "item production interval";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(80, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 24);
            this.label4.TabIndex = 38;
            this.label4.Text = "Idle timeout";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(80, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 24);
            this.label3.TabIndex = 37;
            this.label3.Text = "Maximum Threads";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(80, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 24);
            this.label1.TabIndex = 36;
            this.label1.Text = "Minimum Threads";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.spinMinThreads);
            this.groupBox1.Controls.Add(this.spinConsumingTime);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.spinInterval);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.spinIdleTimeout);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.spinMaxThreads);
            this.groupBox1.Location = new System.Drawing.Point(12, 220);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 238);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Setting";
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(186, 77);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(72, 24);
            this.btnStop.TabIndex = 48;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(186, 47);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(72, 24);
            this.btnStart.TabIndex = 47;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // timerPoll
            // 
            this.timerPoll.Interval = 2000;
            this.timerPoll.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(188, 107);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 24);
            this.button1.TabIndex = 49;
            this.button1.Text = "Screen";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TaskSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(270, 498);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "TaskSettingForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinConsumingTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinIdleTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinMaxThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinMinThreads)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer timerPoll;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button button1;
    }
}
