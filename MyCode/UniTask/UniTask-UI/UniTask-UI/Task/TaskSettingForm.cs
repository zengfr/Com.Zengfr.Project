using Amib.Threading;
using Amib.Threading.Internal;
using DockSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniTask_UI.p1;

namespace UniTask_UI.Task
{
    public partial class TaskSettingForm : ToolWindow
    {
        public TaskSettingForm()
        {
            InitializeComponent();
            InitializeLocalPerformanceCounters();

        }
        private SmartThreadPool _smartThreadPool;
        private IWorkItemsGroup _workItemsGroup;
        private Thread workItemsProducerThread;
        private bool running;
        private long workItemsGenerated;
        private long workItemsCompleted;

        private Func<long> _getActiveThreads;
        private Func<long> _getInUseThreads;
        private Func<long> _getQueuedWorkItems;
        private Func<long> _getCompletedWorkItems;
        private void InitializeLocalPerformanceCounters()
        {
            _getActiveThreads = () => _smartThreadPool.PerformanceCountersReader.ActiveThreads;
            _getInUseThreads = () => _smartThreadPool.PerformanceCountersReader.InUseThreads;
            _getQueuedWorkItems = () => _smartThreadPool.PerformanceCountersReader.WorkItemsQueued;
            _getCompletedWorkItems = () => _smartThreadPool.PerformanceCountersReader.WorkItemsProcessed;
        }
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != _smartThreadPool)
            {
                _smartThreadPool.Shutdown(true, 1000);
                _smartThreadPool = null;
                _workItemsGroup = null;
            }
        }
        private void spinMinThreads_ValueChanged(object sender, System.EventArgs e)
        {
            if (spinMinThreads.Value > spinMaxThreads.Value)
            {
                spinMaxThreads.Value = spinMinThreads.Value;
            }

            if (null != _smartThreadPool)
            {
                _smartThreadPool.MinThreads = (int)spinMinThreads.Value;
            }
        }

        private void spinMaxThreads_ValueChanged(object sender, System.EventArgs e)
        {
            if (spinMaxThreads.Value < spinMinThreads.Value)
            {
                spinMinThreads.Value = spinMaxThreads.Value;
            }
            //usageThreadsInPool.Maximum = Convert.ToInt32(spinMaxThreads.Value);
            if (null != _smartThreadPool)
            {
                _smartThreadPool.MaxThreads = (int)spinMaxThreads.Value;
            }
        }
        private void Form1_Load(object sender, System.EventArgs e)
        {
            UpdateControls(false);
        }
        private void btnStart_Click(object sender, System.EventArgs e)
        {
            UpdateControls(true);
            workItemsCompleted = 0;
            workItemsGenerated = 0;

            STPStartInfo stpStartInfo = new STPStartInfo();
            stpStartInfo.IdleTimeout = Convert.ToInt32(spinIdleTimeout.Value) * 1000;
            stpStartInfo.MaxWorkerThreads = Convert.ToInt32(spinMaxThreads.Value);
            stpStartInfo.MinWorkerThreads = Convert.ToInt32(spinMinThreads.Value);
          
                stpStartInfo.EnableLocalPerformanceCounters = true;
           

            _smartThreadPool = new SmartThreadPool(stpStartInfo);

            //_workItemsGroup = _smartThreadPool.CreateWorkItemsGroup(1);
            _workItemsGroup = _smartThreadPool;

            workItemsProducerThread = new Thread(new ThreadStart(this.WorkItemsProducer));
            workItemsProducerThread.IsBackground = true;
            workItemsProducerThread.Start();
        }

        private void btnStop_Click(object sender, System.EventArgs e)
        {
            running = false;
            workItemsProducerThread.Join();

            _smartThreadPool.Shutdown(true, 1000);
            _smartThreadPool.Dispose();
            _smartThreadPool = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            UpdateControls(false);

        }
        private void UpdateControls(bool start)
        {
            running = start;
            //spinMinThreads.Enabled = !start;
            //spinMaxThreads.Enabled = !start;
            spinIdleTimeout.Enabled = !start;
            btnStart.Enabled = !start;

            btnStop.Enabled = start;
            timerPoll.Enabled = start;

            lblThreadInUse.Text = "0";
            lblThreadsInPool.Text = "0";
            lblWaitingCallbacks.Text = "0";
            //usageThreadsInPool.Maximum = Convert.ToInt32(spinMaxThreads.Value);
            //usageThreadsInPool.Value1 = 0;
           // usageThreadsInPool.Value2 = 0;
            lblWorkItemsCompleted.Text = "0";
            lblWorkItemsGenerated.Text = "0";
            //usageHistorySTP.Reset();
            //usageHistorySTP.Maximum = usageThreadsInPool.Maximum;
        }
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            SmartThreadPool stp = _smartThreadPool;
            if (null == stp)
            {
                return;
            }

            //int threadsInUse = (int)_pcInUseThreads.NextValue();
            //int threadsInPool = (int)_pcActiveThreads.NextValue();
            int threadsInUse = (int)_getInUseThreads();
            int threadsInPool = (int)_getActiveThreads();

            lblThreadInUse.Text = threadsInUse.ToString();
            lblThreadsInPool.Text = threadsInPool.ToString();
            lblWaitingCallbacks.Text = _getQueuedWorkItems().ToString();  //stp.WaitingCallbacks.ToString();
            //usageThreadsInPool.Value1 = threadsInUse;
            //usageThreadsInPool.Value2 = threadsInPool;
            lblWorkItemsCompleted.Text = _getCompletedWorkItems().ToString();
            lblWorkItemsGenerated.Text = workItemsGenerated.ToString();
            //usageHistorySTP.AddValues(threadsInUse, threadsInPool);
        }
        private void WorkItemsProducer()
        {
            WorkItemCallback workItemCallback = new WorkItemCallback(this.DoWork);
            while (running)
            {
                IWorkItemsGroup workItemsGroup = _workItemsGroup;
                if (null == workItemsGroup)
                {
                    return;
                }

                try
                {
                    workItemCallback = new WorkItemCallback(this.DoWork);
                    workItemsGroup.QueueWorkItem(workItemCallback);

                    MyCtrl.Instance.taskGridForm.RefreshGrid();
                }
                catch (ObjectDisposedException e)
                {
                    e.GetHashCode();
                    break;
                }
                workItemsGenerated++;
                Thread.Sleep(Convert.ToInt32(spinInterval.Value));
            }
        }
        static Random r= new Random();int i = 0;
        private object DoWork(object obj)
        {
            Thread.Sleep(Convert.ToInt32(spinConsumingTime.Value));
            var tasks = TaskManager.Instance.tasks;
            int c = tasks.Count;
            if (c > 20)
            {
                if (r.Next(100) < 50)
                {
                    TaskManager.Instance.tasks.RemoveAt(r.Next(0, c));
                    
                }
                foreach (var task in tasks)
                {
                    task.progress = new int[] { r.Next(100), r.Next(100), r.Next(100) };
                    task.fail = new int[] { r.Next(100), r.Next(100), r.Next(100) };
                    task.success = new int[] { r.Next(100), r.Next(100), r.Next(100) };
                    task.incomeToday = new int[] { r.Next(100), r.Next(100), r.Next(100) };
                    task.incomeTotal = new int[] { r.Next(1000), r.Next(1000), r.Next(1000) };

                    task.task = "" + r.Next(123456);
                    task.log = "" + r.Next(123456);
                    task.TimeSpan = (DateTime.Now - task.startTime).TotalMinutes;
                }
            }
           
            else
            {
               
                TaskItem taskitem = new TaskItem();
                taskitem.id =i++;
                taskitem.name = "" + taskitem.id;
                taskitem.ip = "" + taskitem.id;
                taskitem.status = 1;
                taskitem.comment = "" + r.Next(123456); 
              
                taskitem.task = "" + r.Next(123456);
                taskitem.log = "" + r.Next(123456);
                taskitem.startTime = DateTime.Now;
                TaskManager.Instance.tasks.Add(taskitem);
            }

            Interlocked.Increment(ref workItemsCompleted);
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
