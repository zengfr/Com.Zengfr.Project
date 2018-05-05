using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;//
using HtmlAgilityPack;
using CommonPack;
using ThreadWorker;
namespace Builder.Data
{
  public  class Builder
    {
      public static bool IsQuit;//
      public static bool IsShowMessage;

      protected string TaskName;
      protected ModeType ModeType;

      protected Regex QueueAllowPatternRegex;
      protected Regex QueueForbidPatternRegex;

      protected MyConfig Config=new MyConfig(); 
      protected bool IsTrue;//
      public delegate void DataEventHandler<T>(ICollection<T> obj);
      public Builder() { }
      protected void Init(string taskName, ModeType modeType)
      {
          TaskName = taskName;//
          ModeType = modeType;//
          this.ConfigInit();//
      }
      #region Config
      protected virtual string GetName()
      {
          return "Default";//
      }
      protected void ConfigInit()
      {
          ConfigLoad();//
          this.IsTrue = this.Config.GetParamValue_Bool("IsTrue");//

          string tmp = this.Config.GetParamValue_String("QueueAllowPattern");
          if (tmp != null && tmp.Length == 0)
              //tmp = "*";
            this.QueueAllowPatternRegex = new Regex(tmp);//

            tmp = this.Config.GetParamValue_String("QueueForbidPattern");
          if (tmp != null && tmp.Length != 0)
            this.QueueForbidPatternRegex = new Regex(tmp);//
      }
      private void ConfigLoad()
      {
          string root = System.AppDomain.CurrentDomain.BaseDirectory + @"\Config\Plugin\";//
          ConfigLoad(root + @"\default.xml");//
          ConfigLoad(root + @"\" + GetName() + @"\default.xml");//
          ConfigLoad(root + @"\" + GetName() +@"\"+ TaskName + @"\default.xml");//
          ConfigLoad(root + @"\" + GetName() +@"\"+ TaskName + @"\" + ModeType + @".xml");//
      }
      private void ConfigLoad(string file)
      {
          bool b = File.Exists(file);
          MyConfig myConfig = MyConfigHelper.Get(file);

          MyConfigHelper.CopyTo(myConfig,this.Config);//
          if (!b)
              myConfig.Save();
      }

      private void ConfigSave()
      {
          this.Config.Save();// 
      }
      private void ConfigSave(string file)
      {
         this.Config.Save(file);
      }
      #endregion
      /// <summary>
      /// œ‘ æœ˚œ¢
      /// </summary>
      /// <param name="str"></param>
      protected static void ShowMesage(string str)
      {
          if (IsShowMessage)
              Console.WriteLine(str);//
      }
      protected string Replace(string str)
      {
          MyConfig.Replace(this.Config.Get("replace"), ref str);//
          return str;//
      }
      protected void Replace(ref string str)
      {
          MyConfig.Replace(this.Config.Get("replace"), ref str);//
      }
      protected void Replace(StringBuilder sb)
      {
          MyConfig.Replace(this.Config.Get("replace"), sb);//
      }
      protected void RemoveAllChildrenNode(HtmlDocument doc)
      {
          MyConfig.RemoveAllChildrenNodeFromXPath(this.Config.Get("removeAllChildrenNode"), doc);//
      }
      protected void AppendChildNode(HtmlDocument doc)
      {
          MyConfig.AppendChildNodeFromXPath(this.Config.Get("appendChildNode"), doc);//
      }
      protected void PrependChildNode(HtmlDocument doc)
      {
          MyConfig.PrependChildNodeFromXPath(this.Config.Get("prependChildNode"), doc);//
      }
      protected static string GetTitle(HtmlDocument doc)
      {
          HtmlNode node = doc.DocumentNode.SelectSingleNode("//title");//
          if (node == null)
              node = doc.DocumentNode.SelectSingleNode("//Title");//
          if (node == null)
              return "";//

          return node.InnerText;//
      }
    }
    public interface IBuilderPlugin
    {
      void Init(ThreadWorker.ThreadWorker worker);
      void  ThreadStart();//
    }
}
