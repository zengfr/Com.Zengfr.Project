using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate.Criterion;
using Com.Zfrong.Common.Data.AR.Base;
namespace Com.Zfrong.Common.Data.AR.Base
{
    /// <summary>
    /// PowerPageBase 的摘要说明
    /// </summary>
    public abstract class PowerPageBase : System.Web.UI.Page, System.Web.UI.IPostBackEventHandler
    {
        #region 私有字段
        protected int PageSize = 10;
        #endregion

        #region 抽象/虚拟方法virtual/abstract
        protected abstract void DoPostBackEvent(string operate, string propName, string value, int id);
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="str"></param>
        protected abstract void ShowMessage(string str);
        protected abstract void ShowMessage(string str, bool append);
        #endregion

        #region 方法 IPostBackEventHandler.RaisePostBackEvent
        /// <summary>
        /// 脚本PostBack方法 执行DoPostBackEvent
        /// </summary>
        /// <param name="eventArgument"></param>
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split(':', '$', '#');

            string operate = string.Empty; string propName = string.Empty;
            string value = string.Empty; int id = 0;
            try
            {
                operate = args[0];
                propName = args[1];
                value = args[2];

                if (args.Length > 3)
                    int.TryParse(args[3], out id);
                DoPostBackEvent(operate, propName, value, id);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }
        #endregion

        #region 静态方法（模板）
        protected static void BuildSchema<T>(GridView gv)
        {
            gv.Columns.Clear();
            IList<PropertySchema> list = EntityHelper.GetPropertiesSchema(typeof(T), true);//
            TemplateField f;

            f = BuildTemplateField();
            f.HeaderTemplate = new GridViewCommandTemplate(DataControlRowType.Header);
            f.ItemTemplate = new GridViewCommandTemplate(DataControlRowType.DataRow);
            gv.Columns.Add(f);

            foreach (PropertySchema s in list)
            {
                f = BuildTemplateField(); //f.HeaderText = s.DisplayName;
                f.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, s);
                f.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, s);
                //if (s.PropertyName.IndexOf('.') == -1 || s.IsPrimaryKey)
                //    f.SortExpression = s.PropertyName;
                gv.Columns.Add(f);
            }
        }
        private static TemplateField BuildTemplateField()
        {
            TemplateField f = new TemplateField(); f.Visible = true;
            f.ItemStyle.Wrap = false; f.HeaderStyle.Wrap = false;
            f.ShowHeader = true;//
            return f;
        }
        #endregion

        #region 静态方法（数据）
        protected static void BindData<T>(GridView gv, int pageIndex, int pageSize, Order[] orders, params ICriterion[] criterion) where T : class
        {
            int first = pageIndex * pageSize;
            if (criterion != null)
                gv.DataSource = DB<T>.SlicedFindAll(first, pageSize, orders, criterion);
            else
                gv.DataSource = DB<T>.SlicedFindAll(first, pageSize, orders);
            gv.DataBind();
        }
        #endregion

        #region 静态方法（控件/转换/消息处理）


        protected static string GetBinderPropertyValue(object container, string propName, string format)
        {
            return DataBinder.GetPropertyValue(container, propName, format);
        }
        protected static string GetBinderPropertyValue(object container, string propName)
        {
            return DataBinder.GetPropertyValue(container, propName).ToString();
        }

        /// <summary>
        /// 状态转换
        /// </summary>
        /// <param name="container"></param>
        /// <param name="statepropName">状态</param>
        /// <param name="idpropName">记录ID</param>
        /// <returns></returns>
        public static string GetOperate_State(object container, string propName, string IDpropName)
        {
            string Return = "";
            //字段
            string propValue = GetBinderPropertyValue(container, propName);
            //记录ID
            string Id = GetBinderPropertyValue(container, IDpropName);
            switch (propValue)
            {   //启用　让该帐号可以使用
                case "0":
                    Return = "<a href=\"#\" onclick=\"Change(" + Id + ",'" + propName + "',2);\" title='启用，允许使用'>启用</a>";
                    break;
                case "1":
                    Return = "<a href=\"#\" onclick=\"Change(" + Id + ",'" + propName + "',2);\" title='启用，允许使用'>启用</a>";
                    Return += "|<a href=\"#\" onclick=\"Change(" + Id + ",'" + propName + "',0);\" title='锁定，禁止使用'>停用</a>";
                    break;
                //拒绝　让该帐号停止使用
                case "2":
                    Return = "<a href=\"#\" onclick=\"Change(" + Id + ",'" + propName + "',0);\" title='锁定，禁止使用'>停用</a>";
                    break;
            }
            return Return;
        }
        /// <summary>
        /// 排序文本转换 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="stypepropName">审核状态</param>
        /// <returns></returns>
        public static string GetOperate_Sort(object container, string propName, string IDpropName)
        {
            int propValue = int.Parse(GetBinderPropertyValue(container, propName));
            //记录ID
            string IdValue = GetBinderPropertyValue(container, IDpropName);
            string Return = "[" + propValue.ToString() + "]";
            Return += "<a href='#' onclick=\"__doPostBack('__Page','Change:" + IdValue + ":" + propName + ":100');\">＾</a>";
            Return += "<a href='#' onclick=\"__doPostBack('__Page','Change:" + IdValue + ":" + propName + ":0');\">0</a>";

            Return += "<a href='#' onclick=\"__doPostBack('__Page','Change:" + IdValue + ":" + propName + ":" + (propValue + 1) + "');\">↑</a>";
            if (propValue > 0) Return += "<a href='#' onclick=\"__doPostBack('__Page','Change:" + IdValue + ":" + propName + ":" + (propValue - 1) + "');\">↓</a>";
            return Return;
        }
        /// <summary>
        /// 审核文本转换 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="propName">审核状态</param>
        /// <returns></returns>
        public static string GetStateText(object container, string propName)
        {
            string Return = "";
            string propValue = GetBinderPropertyValue(container, propName);
            switch (propValue)
            {
                case "0":
                    Return = "[" + propValue + "]<img src='Images/erro.gif'>"; break;
                case "1":
                    Return = "[" + propValue + "]<img src='Images/sh.gif'>"; break;
                case "2":
                    Return = "[" + propValue + "]<img src='Images/ok.gif'>"; break;
            }
            return Return;
        }

        #endregion

        #region 静态方法（修改/编辑）
        /// <summary>
        /// 批量修改
        /// </summary>
        protected static bool ChangeMultiple<K>(string propName, bool value, GridView gv) where K : class
        {
            return ChangeMultiple<K>(propName, value as object, gv);
        }
        protected static bool ChangeMultiple<K>(string propName, byte value, GridView gv) where K : class
        {
            return ChangeMultiple<K>(propName, value as object, gv);
        }
        protected static bool ChangeMultiple<K>(string propName, string value, GridView gv) where K : class
        {
            return ChangeMultiple<K>(propName, value as object, gv);
        }
        protected static bool ChangeMultiple<K>(string propName, DateTime value, GridView gv) where K : class
        {
            return ChangeMultiple<K>(propName, value as object, gv);
        }
        /// <summary>
        /// 批量修改
        /// </summary>
        protected static bool ChangeMultiple<K>(string propName, object value, GridView gv) where K : class
        {
            int id = 0;
            CheckBox cbcheck = null;
            try
            {
                for (int i = 0; i < gv.Rows.Count; i++)
                {
                    cbcheck = (CheckBox)gv.Rows[i].Cells[0].FindControl("opcheck");
                    if (cbcheck != null && cbcheck.Checked)
                    {
                        id = int.Parse(gv.Rows[i].Cells[1].Text);
                        ChangeOne<K>(id, propName, value);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        protected static bool ChangeOne<K>(int id, string propName, bool value) where K : class
        {
            return ChangeOne<K>(id, propName, value as object);
        }
        protected static bool ChangeOne<K>(int id, string propName, byte value) where K : class
        {
            return ChangeOne<K>(id, propName, value as object);
        }
        protected static bool ChangeOne<K>(int id, string propName, string value) where K : class
        {
            return ChangeOne<K>(id, propName, value as object);
        }
        protected static bool ChangeOne<K>(int id, string propName, DateTime value) where K : class
        {
            return ChangeOne<K>(id, propName, value as object);
        }
        /// <summary>
        /// 修改
        /// </summary>
        protected static bool ChangeOne<K>(int id, string propName, object value) where K : class
        {
            try
            {
                K obj = DB<K>.Find(id);
                Type t = typeof(K);
                value = Convert.ChangeType(value, t.GetProperty(propName).PropertyType);
                object v2 = t.GetProperty(propName).GetValue(obj, null);//
                if (value != v2)
                {
                    t.GetProperty(propName).SetValue(obj, value, null);//
                    DB<K>.Update(obj);//
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private static bool IsEqual<K>(object obj1, object obj2) where K : IComparable
        {
            return ((K)obj1).CompareTo((K)obj2) != 0;
        }
        #endregion

        #region 静态方法（删除）
        /// <summary>
        /// 批量修改
        /// </summary>
        protected static bool DeleteMultiple<K>(GridView gv) where K : class
        {
            int id = 0;
            CheckBox cbcheck = null;
            try
            {
                for (int i = 0; i < gv.Rows.Count; i++)
                {
                    cbcheck = (CheckBox)gv.Rows[i].Cells[0].FindControl("opcheck");
                    if (cbcheck.Checked)
                    {
                        id = int.Parse(gv.Rows[i].Cells[1].Text);
                        DeleteOne<K>(id);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        protected static bool DeleteOne<K>(int id) where K : class
        {
            try
            {
                K obj = DB<K>.Find(id);
                DB<K>.Delete(obj);//
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 静态方法（信息/显示）

        protected static void ShowMessage(string str, Label label)
        {
            ShowMessage(str, true, label);//
        }
        protected static void ShowMessage(string str, bool append, Label label)
        {
            string tmp = "<div class=dvNone>{0} Time:{1}</div>";
            if (append)
                label.Text += string.Format(tmp, str, DateTime.Now.ToString());
            else
                label.Text = string.Format(tmp, str, DateTime.Now.ToString());
        }
        #endregion

        #region Page_Init()
        protected void Page_Init()
        {

            RegisterJSInit();//
        }
        #endregion

        #region JScript
        public void RegisterJSInit()
        {
            ClientScript.GetPostBackEventReference(this, null);//注册page

            string s = "$.blockUI({css: {border:'none',padding:'15px',backgroundColor:'#000','-webkit-border-radius':'10px','-moz-border-radius':'10px',opacity:'.5',color:'#fff'},message:\"操作执行中...\"});";
            RegisterOnSubmitStatement("blockUI", s);

            System.Text.StringBuilder script = new System.Text.StringBuilder();

            script.Append("function JS_DoPostBack(eventArgument) {__doPostBack('__Page',eventArgument);}");
            script.Append("function JS_DoPostBackC(eventArgument) {if(confirm(\"您确定执行此操作?\")){JS_DoPostBack(eventArgument);}}");
            script.Append("function JS_GoTo(url){window.location=url;}");////跳转到
            script.Append("function JS_Change(id,field,value){if(confirm(\"您确定执行此更改?\")){JS_DoPostBack('Change:' + id+':'+field+':'+value);}}");
            script.Append("function JS_Sort(col){JS_DoPostBack(col);}");//
            script.Append("function JS_Select(id){JS_GoTo(\"?do=Select&ID=\"+id);}");//
            script.Append("function JS_AddOrEdit(id){JS_GoTo(\"?do=AddOrEdit&ID=\"+id);}");///编辑/添加
            script.Append("function JS_Delete(id){if(confirm(\"您确定执行此删除?\")){JS_GoTo(\"?do=Delete&ID=\"+id);}}");//删除
            script.Append("function JS_CheckedAll(e,id){  var form=document.form1; for (i = 0,n = form.elements.length;i< n;i++) { if(form.elements[i].type == \"checkbox\"&&form.elements[i].id.indexOf(id)!=-1) {form.elements[i].checked = e.checked;}}}");
            script.Append("$.unblockUI();"); //'页面载入时隐藏 blockUI

            RegisterJSStartTag();
            RegisterJSBody("RegisterJSInit", script.ToString());//CheckBox
            RegisterJSEndTag();
        }

        protected void RegisterJSAlert(string key, params string[] text)
        {
            string script = "alert('";
            foreach (string s in text)
            {
                script += "$" + s;
            }
            script += "');";
            RegisterJS(key, script);//
        }
        protected void RegisterJSStartTag()
        {
            ClientScript.RegisterStartupScript(typeof(Page), "JSStartTag", "<script type=\"text/javascript\" language=\"javascript\">");
        }
        protected void RegisterJSEndTag()
        {
            ClientScript.RegisterStartupScript(typeof(Page), "JSEndTag", "</script>");
        }
        protected void RegisterJSBody(string key, string script)
        {
            ClientScript.RegisterStartupScript(typeof(Page), key, script);//
        }
        protected void RegisterJS(string key, string script)
        {
            ClientScript.RegisterStartupScript(typeof(Page), key, script, true);//string.Format("<script type=\"text/javascript\" language=\"javascript\">{0}</script>",
        }
        #endregion

        #region Common/静态
        protected static string BuildCurrentURL(string name, int value)
        {
            return BuildCurrentURL(name, value.ToString());
        }
        protected static string BuildCurrentURL(string name, string value)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(); sb.Append("?");
            sb.Append("&" + name + "=" + value);//
            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                if (HttpContext.Current.Request.QueryString.GetKey(i) != name)
                    sb.Append("&" + HttpContext.Current.Request.QueryString.GetKey(i) + "=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.QueryString.Get(i)));//
            }
            return sb.ToString();
        }
        #endregion
    }
}