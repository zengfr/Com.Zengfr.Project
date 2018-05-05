using System;
using System.Threading;

namespace ThreadWorker
{
	
	public class Done 
	{

		/// <summary>
		/// The number of SpiderWorker object
		/// threads that are currently working
		/// on something.
		/// </summary>
		private int m_activeThreads = 0;

		/// <summary>
		/// This boolean keeps track of if
		/// the very first thread has started
		/// or not. This prevents this object
		/// from falsely reporting that the spider
		/// is done, just because the first thread
		/// has not yet started.
		/// </summary>
		private bool m_started = false;


        public object SyncObj = new object();//
		/// <summary>
        /// �ȴ�ֱ��ȫ���߳�ֹͣ
		/// This method can be called to block
		/// the current thread until the spider
		/// is done.
		/// </summary>
		public void WaitDone()
		{
			Monitor.Enter(this);
			while ( m_activeThreads>0 ) 
			{
              Monitor.Wait(this);
			}
			Monitor.Exit(this);
		}

		/// <summary>
        /// �ȴ���һ���߳̿�ʼ
		/// Called to wait for the first thread to
		/// start. Once this method returns the
		/// spidering process has begun.
		/// </summary>
		public void WaitBegin()
		{
			Monitor.Enter(this);
			while ( !m_started )
            {
                Monitor.Wait(this);
			}
			Monitor.Exit(this);
		}


		/// <summary>
        /// ĳ�߳̿�ʼ
		/// Called by a SpiderWorker object
		/// to indicate that it has begun
		/// working on a workload.
		/// </summary>
		public void WorkerBegin()
		{
			Monitor.Enter(this);
			m_activeThreads++;
			m_started = true;
          Monitor.Pulse(this);
			Monitor.Exit(this);
		}

		/// <summary>
        /// ĳ�߳̽���
		/// Called by a SpiderWorker object to
		/// indicate that it has completed a
		/// workload.
		/// </summary>
		public void WorkerEnd()
		{
			Monitor.Enter(this);
			m_activeThreads--;
           Monitor.Pulse(this);
			Monitor.Exit(this);
		}

		/// <summary>
        /// ��ʼ��
		/// Called to reset this object to
		/// its initial state.
		/// </summary>
		public void Reset()
		{
			Monitor.Enter(this);
           m_activeThreads = 0;
			Monitor.Exit(this);
		}
	}
}
