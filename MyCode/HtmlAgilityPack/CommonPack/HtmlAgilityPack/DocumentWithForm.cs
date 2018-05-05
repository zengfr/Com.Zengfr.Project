using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;//
using HtmlAgilityPack;
namespace CommonPack.HtmlAgilityPack
{
    /// <summary>
    /// 获取form提交需要提供的参数集合
    /// </summary>
   public class DocumentWithForm
    {
       private string _postUrl="";
       private Dictionary<string, string> _params = new Dictionary<string, string>();
		private HtmlDocument _doc;
       private string _url;//
       private int _formIndex = 0;
       public DocumentWithForm(string url)
       {
           _url = url; Init();//
       }
       public DocumentWithForm(string url,int formIndex)
       {
           _formIndex = formIndex;
           _url = url; Init();//
       }
       private void Init()
       {
           HtmlWeb hw = new HtmlWeb();
           hw.UseCookies = true;//
           _doc = hw.Load(_url);
           if (_doc == null)
           {
               throw new ArgumentNullException("doc");
           }
           GetPostUrl();
           //GetParams();
       }
       private void GetPostUrl()
       {
           HtmlNodeCollection nodes = _doc.DocumentNode.SelectNodes("//form");
           if (nodes == null || nodes.Count == 0)
               return;

           HtmlAttribute att = nodes[0].Attributes["action"];
           if (att != null)
               _postUrl = new Uri(new Uri(_url), att.Value).ToString();//
           if (_postUrl == "")
           {
               _postUrl = _url;//
           }
       }
       public void GetParams()
       { 
           GetParams(ref _params,"input");
           GetParams(ref _params, "textarea");
           GetParams(ref _params, "select");
       }
       public void GetParams(ParamType type)
       {
           switch (type)
           {
               case ParamType.Input:
                   GetParams(ref _params, "input"); break;
               case ParamType.TextArea:
                   GetParams(ref _params, "textarea"); break;
               case ParamType.Select:
                   GetParams(ref _params, "select"); break;
               case ParamType.All:
                   GetParams(ref _params, "input");
                   GetParams(ref _params, "textarea");
                   GetParams(ref _params, "select"); 
                   break;
           }
       }
       
       /// <summary>
       /// name="input","textarea","select"
       /// </summary>
       /// <param name="Params"></param>
       /// <param name="name"></param>
       public void GetParams(ref Dictionary<string, string> Params, string name)
		{
            HtmlNodeCollection nodes = _doc.DocumentNode.SelectNodes("//form")[_formIndex].SelectNodes("..//"+name);
			if (nodes == null)
				return;

			foreach(HtmlNode n in nodes)
			{
				Parse(ref Params,n);
			}
		}
		private void Parse(ref Dictionary<string, string> Params,HtmlNode node)
		{
            HtmlAttribute pName = node.Attributes["Name"];

            if (pName == null)
                return;
            Params.Add(pName.Value, "");

            HtmlAttribute pValue = node.Attributes["Value"];
            if (pValue == null)
                return;
            Params[pName.Value]=pValue.Value;
        }

		/// <summary>
		/// 参数集合
		/// </summary>
       public Dictionary<string, string> Params
		{
			get
			{
				return _params;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string PostUrl
		{
			get
			{
				return _postUrl;
			}
		}
       public static DataSet ConvertHTMLTablesToDataSet(string HTML)
       {
           DataTable dt;
           DataSet ds = new DataSet();
           dt = new DataTable();
           string TableExpression = "<table[^>]*>(.*?)</table>";
           string HeaderExpression = "<th[^>]*>(.*?)</th>";
           string RowExpression = "<tr[^>]*>(.*?)</tr>";
           string ColumnExpression = "<td[^>]*>(.*?)</td>";
           bool HeadersExist = false;
           int iCurrentColumn = 0;
           int iCurrentRow = 0;

           MatchCollection Tables = Regex.Matches(HTML,
           TableExpression,
           RegexOptions.Singleline |
           RegexOptions.Multiline |
           RegexOptions.IgnoreCase);


           foreach (Match Table in Tables)
           {
               iCurrentRow = 0;
               HeadersExist = false;
               dt = new DataTable();

               if (Table.Value.Contains("<th"))
               {
                   HeadersExist = true;

                   MatchCollection Headers = Regex.Matches(Table.Value,
                   HeaderExpression,
                   RegexOptions.Singleline |
                   RegexOptions.Multiline |
                   RegexOptions.IgnoreCase);

                   foreach (Match Header in Headers)
                   {
                       dt.Columns.Add(Header.Groups[1].ToString());
                   }

               }
               else
               {

                   int myvar2222 = Regex.Matches(
                   Regex.Matches(
                   Regex.Matches(
                   Table.Value,
                   TableExpression,
                   RegexOptions.Singleline
                   | RegexOptions.Multiline |
                   RegexOptions.IgnoreCase)[0].ToString(),
                   RowExpression, RegexOptions.Singleline |
                   RegexOptions.Multiline |
                   RegexOptions.IgnoreCase)[0].ToString(),
                   ColumnExpression,
                   RegexOptions.Singleline |
                   RegexOptions.Multiline |
                   RegexOptions.IgnoreCase).Count;

                   for (int iColumns = 1; iColumns <= myvar2222; iColumns++)
                   {
                       dt.Columns.Add("Column " + System.Convert.ToString(iColumns));
                   }

               }

               MatchCollection Rows = Regex.Matches(Table.Value,
               RowExpression,
               RegexOptions.Singleline |
               RegexOptions.Multiline | RegexOptions.IgnoreCase);

               foreach (Match Row in Rows)
               {

                   if (!((iCurrentRow == 0) & HeadersExist))
                   {
                       DataRow dr = dt.NewRow();
                       iCurrentColumn = 0;

                       MatchCollection Columns = Regex.Matches(Row.Value,
                       ColumnExpression,
                       RegexOptions.Singleline |
                       RegexOptions.Multiline |
                       RegexOptions.IgnoreCase);

                       foreach (Match Column in Columns)
                       {
                           dr[iCurrentColumn] = Column.Groups[1].ToString();
                           iCurrentColumn++;
                       }

                       dt.Rows.Add(dr);
                   }
                   iCurrentRow++;
               }
               ds.Tables.Add(dt);

           }

           return ds;
       } 

    }
  public  enum ParamType:byte
    {   
        All,
        Input,
        Select,
        TextArea
    }
}
