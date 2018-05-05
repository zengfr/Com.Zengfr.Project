using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;//
namespace Builder.Page
{
  public  class JS
    {
      public static int Item = 20;
      public static int List = 20;
      public static void Build()
      {
          ItemBuild(); ListBuild();//
      }
      public static void ItemBuild()
      {
          StringBuilder sb = new StringBuilder();//
          sb.Append(FileOperate.ReadFile("template/Item.js"));
          for (int i = 0; i < Item; i++)
          {
              FileOperate.WriteFile("Js/Item/" + i + ".js", sb.ToString().Replace("{ID}", i.ToString()));//
          }
          sb.Remove(0, sb.Length);//
      }
      public static void ListBuild()
      {
          StringBuilder sb = new StringBuilder();//
          sb.Append(FileOperate.ReadFile("template/List.js"));
          for (int i = 0; i < Item; i++)
          {
              FileOperate.WriteFile("Js/List/" + i + ".js", sb.ToString().Replace("{ID}", i.ToString()));//
          }
          sb.Remove(0, sb.Length);//
      }
  }
}
