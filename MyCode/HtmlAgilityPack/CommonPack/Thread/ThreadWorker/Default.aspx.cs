using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Threading;
using System.IO;
using DownLoadComponent;


/// <summary>
/// Author: [ ChengKing(ZhengJian) ] 
/// Blog:   Http://blog.csdn.net/ChengKing
//该源码下载自www.51aspx.com(５１ａｓｐｘ．ｃｏｍ)
/// 扩展如下功能: 
///   1. 解决一些线程相关的Bug; 
///   2.扩展用控制文件实现断点续传功能.
/// </summary>
public partial class _Default : System.Web.UI.Page 
{    
    //全局变量
    private static object _SyncLockObject = new object();

    protected void Page_Load(object sender, EventArgs e)
    {
        Utility.RegisterTypeForAjax(typeof(_Default));
        this.TextBox1.Text = "http://download.csdn.net/filedown/aHR0cDovL2Rvd25sb2FkMS5jc2RuLm5ldC9kb3duMy8yMDA3MDQxOS8xOTE1NTU0NzgwMS5yYXI=!169105";
        
           }
    protected void btOK_Click(object sender, EventArgs e)
    {
        this.Label1.Text = "状态: 正在下载...";
        
        DownLoadComponent.HttpWebClient x = new DownLoadComponent.HttpWebClient();

        //注册 DataReceive　事件
        x.DataReceive += new DownLoadComponent.HttpWebClient.DataReceiveEventHandler(this.x_DataReceive);
        //注册 ExceptionOccurrs　事件
        x.ExceptionOccurrs += new DownLoadComponent.HttpWebClient.ExceptionEventHandler(this.x_ExceptionOccurrs);

        string Source = this.TextBox1.Text.Trim();
        string FileName = Source.Substring(Source.LastIndexOf("/") + 1);
        string Location= System.IO.Path.Combine( this.TextBox2.Text.Trim() , FileName);
        
        //F: 源服务器文件;  _f: 保存路径;  10: 自设定一个文件有几个线程下载.
        x.DownloadFile(Source,Location , int.Parse(this.TextBox3.Text));

        //Response.Write("正在下载文件...");
        this.btOK.Enabled = false;
        this.btCancel.Enabled = true;
    }

    private void x_DataReceive(DownLoadComponent.HttpWebClient Sender, DownLoadComponent.DownLoadEventArgs e)
    {

        string f = e.DownloadState.FileName;
        if (e.DownloadState.AttachmentName != null)
            f = System.IO.Path.GetDirectoryName(f) + @"\" + e.DownloadState.AttachmentName;       
        
        using (System.IO.FileStream sw = new System.IO.FileStream(f, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite))
        {
            sw.Position = e.DownloadState.Position;                
            sw.Write(e.DownloadState.Data, 0, e.DownloadState.Data.Length); 
            sw.Close();
        }            
    }   

    private void x_ExceptionOccurrs(DownLoadComponent.HttpWebClient Sender, DownLoadComponent.ExceptionEventArgs e)
    {
        System.Console.WriteLine(e.Exception.Message);
        //发生异常重新下载相当于断点续传,你可以自己自行选择处理方式或自行处理
        DownLoadComponent.HttpWebClient x = new DownLoadComponent.HttpWebClient();
        x.DataReceive += new DownLoadComponent.HttpWebClient.DataReceiveEventHandler(this.x_DataReceive);
        //订阅 ExceptionOccurrs　事件
        //x.ExceptionOccurrs += new DownLoadComponent.HttpWebClient.ExceptionEventHandler(this.x_ExceptionOccurrs);

        x.DownloadFileChunk(e.DownloadState.RequestURL, e.DownloadState.FileName, e.DownloadState.Position, e.DownloadState.Length);
        e.ExceptionAction = DownLoadComponent.ExceptionActions.Ignore;
    }
    protected void btCancel_Click(object sender, EventArgs e)
    {
        if (DownLoadComponent.HttpWebClient.threads != null)
        {
            foreach (Thread t in DownLoadComponent.HttpWebClient.threads)
            {
                if (t.IsAlive)
                {
                    t.Abort();
                }
            }

            DownLoadComponent.HttpWebClient.threads.Clear();
        }
        System.Diagnostics.Process myproc = new System.Diagnostics.Process();
        Process[] procs = (Process[])Process.GetProcessesByName("DW20.exe");  //得到所有打开的进程
        try
        {
            foreach (Process proc in procs)
            {
                if (proc.CloseMainWindow() == false)
                {
                    proc.Kill();
                }
            }
        }
        catch
        { }
        KillAllThreads();
        this.btOK.Enabled = true;
        this.btCancel.Enabled = false;
        GC.Collect();
       
    }

    /// <summary>
    /// 定期检查控制文件
    /// </summary>
    /// <param name="str"></param>
    /// <returns>是否还继续监视(1: 正在下载中，继续监视; 0: 表示已经下载完毕,不用再检视)</returns>
    //[AjaxMethod()]// or [AjaxPro.AjaxMethod] 
    public bool CheckControlFiles(string strObjPath)
    {
        if (!WhetherDownloadFinished(strObjPath))
        {
            return true;
        }
        return false;
    }

    private bool WhetherDownloadFinished(string strObjPath)
    {
        DirectoryInfo df = new DirectoryInfo(strObjPath);
        FileInfo[] fi = (FileInfo[])df.GetFiles("*.txt", SearchOption.TopDirectoryOnly);
        HttpWebClient hwc = new HttpWebClient();        
        for (int i = 0; i < fi.Length; i++)
        {
            if (fi[i].FullName.Length > 12 && fi[i].FullName.Substring(fi[i].FullName.Length - 12) == "_Control.txt")
            {                
                if (hwc.JudgeControlFileIfFinished(fi[i].FullName) == true)
                {
                    hwc.DeleteControlFile(fi[i].FullName);
                    KillAllThreads();
                    return true;
                }
            }
        }
        return false;
    }

    private void KillAllThreads()
    {
        foreach (Thread t in HttpWebClient.threads)
        {
            if (t.IsAlive)
            {
                t.Abort();
            }
        }
        HttpWebClient.threads.Clear();
    }

}
