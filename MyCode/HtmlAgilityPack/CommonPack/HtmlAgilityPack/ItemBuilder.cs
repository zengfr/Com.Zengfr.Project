using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Com.Zfrong.Algorithms.LCS;
namespace CommonPack.LCS
{
    /// <summary>
    /// ����ģ���ĳurl��ȡ itemsֵ
    /// </summary>
    public class ItemBuilder
    {
        #region ���췽��
        public ItemBuilder()
        {
            Diff = new Dictionary<int, string>();
        }
        public ItemBuilder(string text, string template)
        {
            Diff = new Dictionary<int, string>();
            Text = text;
            Template = template;//
        }
        #endregion
        #region �ֶ�
        /// <summary>
        /// ģ��string
        /// </summary>
        public string Template;
        /// <summary>
        /// ���ɼ���url����
        /// </summary>
        public string Text;
        /// <summary>
        /// items�� key=ģ��ƫ���� value=ֵ
        /// </summary>
        public IDictionary<int, string> Diff;
        #endregion
        #region
        /// <summary>
        /// Ҫ�ɼ���url
        /// </summary>
        /// <param name="url"></param>
        public void SetText(string url)
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);
            this.Text = doc.Text;
            doc = null;//
            hw = null;//
        }
        /// <summary>
        /// ����ģ���ļ���ַ
        /// </summary>
        /// <param name="templateFile"></param>
        public void SetTemplate(string templateFile)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(templateFile);//
            this.Template = doc.Text;//
            doc = null;//
        }
        #endregion
        #region
        public IDictionary<int, string> GetAllItemValue()
        {
            this.Diff.Clear();//
            LCSFinder.GetLCS(ref Template,ref Text, null, this.Diff);
            return this.Diff;//
        }
        public string GetItemValue(int index)
        {
            return this.Diff[index];//
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetItemValue(int[] index)
        {
            string tmp = "";
            foreach (int i in index)
            {
                tmp += this.Diff[i];
            }
            return tmp;//
        }
        #endregion
        #region
        public string GetItemValue(string itemName, int starStart, int starEnd)
        {
            return GetItemValue0(itemName, starStart, starEnd);//
        }
        private string GetItemValue0(string itemName, int starStart, int starEnd)
        {
            string pattern = GetItemStart(itemName, starStart);//
            pattern += "(*)" + GetItemEnd(itemName, starEnd);
            IDictionary<int, string> diff = new Dictionary<int, string>();
            LCSFinder.GetLCS(ref pattern, ref Text, null, diff);
            return diff[starStart] + diff[starEnd];
        }
        /// <summary>
        /// ��ȡͷ
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="starIndex"></param>
        /// <returns></returns>
        private string GetItemStart(string itemName, int starIndex)
        {
            int startIndex2 = Template.IndexOf("(" + itemName + ")");
            string tmp = Template.Substring(0, startIndex2);//
            int startIndex1 = tmp.Length; int index = 0;
            for (int i = tmp.Length - 1; i > 1; i--)
            {
                if (tmp[i] == ')' && tmp[i - 1] == '*' && tmp[i - 2] == '(')
                {
                    index++;//
                    startIndex1 = i + 1;
                }
                if (index >= starIndex)
                    break;
            }
            //startIndex2 = startIndex2 + itemName.Length + 2;

            string startStr = Template.Substring(startIndex1, startIndex2 - startIndex1);//
            return startStr;//
        }
        /// <summary>
        /// ��ȡβ
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="starIndex"></param>
        /// <returns></returns>
        private string GetItemEnd(string itemName, int starIndex)
        {
            int endIndex1 = Template.IndexOf("(" + itemName + ")");
            endIndex1 = endIndex1 + itemName.Length + 2;

            string tmp = Template.Substring(endIndex1);//
            int length = tmp.Length; int index = 0;
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i] == '(' && tmp[i + 1] == '*' && tmp[i + 2] == ')')
                {
                    index++;//
                    length = i;
                }
                if (index >= starIndex)
                    break;
            }

            string endStr = Template.Substring(endIndex1, length);//
            return endStr;//
        }
        #endregion
        
    }
}
