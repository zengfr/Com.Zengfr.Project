using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Reflection;
namespace Com.Zfrong.Common.Data.AR.Base
{
    public class TypeSchema
    {
        public IList<PropertySchema> Propertys;
        public string Name = "";
        public string Namespace = "";
        public string FullName = "";
        public static TypeSchema Create(Type type)
        {
            TypeSchema s = new TypeSchema();
            s.Name = type.Name;
            s.Namespace = type.Namespace;
            s.FullName = type.FullName;
            s.Propertys = EntityHelper.GetPropertiesSchema(type);//
            return s;
        }
        public static IList<TypeSchema> Create(string file)
        {
            Assembly assembly = Assembly.LoadFrom(file);//
            return Create(assembly);//
        }
        public static IList<TypeSchema> Create(Assembly assembly)
        {
            IList<TypeSchema> objs = new List<TypeSchema>();

            foreach (Type t in assembly.GetTypes())
            {
                objs.Add(Create(t));//
            }
            return objs;
        }
    }
    public class PropertySchema
    {
        public string PropertyName;
        public string PropertyTypeF;
        public string PropertyTypeS;
        public string DisplayName;

        public bool IsGenericType;
        public bool IsSystemType;
        public bool IsCollection;
        public bool IsArray;
        public bool IsPrimaryKey;
        public PropertySchema()
        {

        }

        public object ToPropertyType(string value)
        {
            return ToPropertyType(value);
        }
        public object ToPropertyType(object value)
        {
            Type t = Type.GetType(this.PropertyTypeF);//object obj = Activator.CreateInstance(t);
            return ChangeType(value, t);
        }

        public static object ChangeType(object value, string type)
        {
            Type t = Type.GetType(type);
            return ChangeType(value, t);
        }
        public static object ChangeType(object value, Type t)
        {
            return Convert.ChangeType(value, t);
        }
    }
    /// <summary>
    /// DisplayHelper 的摘要说明
    /// </summary>
    public partial class EntityHelper
    {
        #region

        #endregion

        #region

        #endregion

        #region
        protected static List<PropertyInfo> GetPubProperties(Type t)
        {
            BindingFlags bf = BindingFlags.Default | BindingFlags.CreateInstance
               | BindingFlags.Instance | BindingFlags.Public;
            return GetPubProperties(t, bf);
        }
        protected static List<PropertyInfo> GetPubProperties(Type t, bool flag)
        {
            BindingFlags bf = BindingFlags.Default | BindingFlags.CreateInstance
                  | BindingFlags.Instance | BindingFlags.Public;
            return GetPubProperties(t, bf, flag);
        }
        protected static List<PropertyInfo> GetPubProperties(Type t, BindingFlags bf)
        {
            return GetPubProperties(t, bf, false);
        }
        protected static List<PropertyInfo> GetPubProperties(Type t, BindingFlags bf, bool flag)
        {
            List<PropertyInfo> objs = new List<PropertyInfo>();
            List<PropertyInfo> infos = new List<PropertyInfo>();
            infos.AddRange(t.GetProperties(bf));//, flag));
            Type tt;
            foreach (PropertyInfo info in infos)
            {
                tt = info.PropertyType;
                if (flag && (tt.Namespace == null || tt.Namespace.IndexOf("System") != 0))
                    objs.AddRange(GetPubProperties(tt, bf, !flag));//!flag//防止循环
                else
                    objs.Add(info);
            }
            return objs;//
        }

        public static IList<PropertySchema> GetPropertiesSchema(Type t)
        {
            return GetPropertiesSchema(t, false);
        }
        public static IList<PropertySchema> GetPropertiesSchema(Type t, bool flag)
        {
            return GetPropertiesSchema(t, flag, null, null);
        }
        public static IList<PropertySchema> GetPropertiesSchema(Type t, bool flag, string baseDisplayName, string basePropertyName)
        {
            List<PropertySchema> objs = new List<PropertySchema>();
            List<PropertyInfo> infos = GetPubProperties(t);
            PropertySchema obj; Type tt;
            if (baseDisplayName != null) { baseDisplayName += "."; }
            if (basePropertyName != null) { basePropertyName += "."; }
            foreach (PropertyInfo info in infos)
            {
                obj = new PropertySchema();
                obj.DisplayName = GetDisplayName(info, baseDisplayName);
                obj.PropertyName = basePropertyName + info.Name;
                obj.PropertyTypeF = info.PropertyType.FullName;
                obj.PropertyTypeS = info.PropertyType.Name;
                obj.IsGenericType = info.PropertyType.IsGenericType;
                obj.IsPrimaryKey = IsPrimaryKey(info);//
                if (info.PropertyType.Namespace != null)
                {
                    obj.IsSystemType = info.PropertyType.Namespace.IndexOf("System") == 0;
                    obj.IsCollection = info.PropertyType.Namespace.IndexOf("System.Collections") == 0;
                }
                obj.IsArray = info.PropertyType.Name.IndexOf("Array") == 0 || info.PropertyType.BaseType != null && info.PropertyType.BaseType.Name == "Array";

                tt = info.PropertyType;
                if (flag && (tt.Namespace == null || tt.Namespace.IndexOf("System") != 0))
                    objs.AddRange(GetPropertiesSchema(tt, !flag, obj.DisplayName, obj.PropertyName));//!flag//防止循环
                else
                {
                    objs.Add(obj);//
                }
            }
            return objs;//

            //PropertyHelper obj = new PropertyHelper(t, flag);
            //for (int i = 0; i < obj.Count; i++)
            //    objs.Add(new PropertySchema(obj.Properties[i], obj.DisplayNames[i], obj.PropertyTypes[i]));//
            //return objs;//
        }

        #region
        //protected static IList<string> GetPropertyTypes(Type t)
        //{
        //    return GetPropertyTypes(GetPubProperties(t));
        //}
        //protected static IList<string> GetPropertyTypes(Type t, bool flag)
        //{
        //    int totalCount = 0;
        //    return GetPropertyTypes(GetPubProperties(t), flag,ref totalCount);
        //}
        //protected static IList<string> GetPropertyTypes(Type t, bool flag, ref int totalCount)
        //{
        //    return GetPropertyTypes(GetPubProperties(t), flag,ref totalCount);
        //}
        //protected static IList<string> GetPropertyTypes(List<PropertyInfo> infos)
        //{
        //    return GetPropertyTypes(infos, false);
        //}
        //protected static IList<string> GetPropertyTypes(List<PropertyInfo> infos, bool flag)
        //{
        //    int totalCount = 0;
        //    return GetPropertyTypes(infos, flag,ref totalCount);
        //}
        //protected static IList<string> GetPropertyTypes(List<PropertyInfo> infos,bool flag,ref int totalCount)
        //{
        //    List<string> objs = new List<string>(); Type t;
        //    for (int i = 0; i < infos.Count; i++)
        //    {
        //        t = infos[i].PropertyType;
        //        if (flag && (t.Namespace == null || t.Namespace.IndexOf("System") != 0))
        //            objs.AddRange(GetPropertyTypes(infos[i].PropertyType, !flag, ref totalCount));//!flag//防止循环
        //        else
        //            objs.Add(infos[i].PropertyType.Name);
        //    }

        //    return objs;
        //}

        //protected static IList<string> GetPropertiesName(Type t)
        //{
        //    return GetPropertiesName(GetPubProperties(t));
        //}
        //protected static IList<string> GetPropertiesName(Type t,bool flag)
        //{
        //    return GetPropertiesName(GetPubProperties(t),flag,t.Name);
        //}
        //protected static IList<string> GetPropertiesName(Type t, bool flag, string baseTypeName)
        //{
        //    return GetPropertiesName(GetPubProperties(t), flag, baseTypeName);
        //}
        //protected static IList<string> GetPropertiesName(List<PropertyInfo> infos)
        //{
        //    return GetPropertiesName(infos, false, null);
        //}
        //protected static IList<string> GetPropertiesName(List<PropertyInfo> infos, bool flag, string baseName)
        //{
        //    List<string> objs = new List<string>(); Type t;
        //    if (baseName != null) { baseName += "."; }
        //    for (int i = 0; i < infos.Count; i++)
        //    {
        //        t = infos[i].PropertyType;
        //        if (flag && (t.Namespace == null || t.Namespace.IndexOf("System") != 0))//&&baseTypeName.IndexOf(t.Name)==-1)
        //             objs.AddRange(GetPropertiesName(infos[i].PropertyType, !flag, baseName + infos[i].Name));//!flag//防止循环
        //            else
        //        objs.Add(baseName+infos[i].Name);
        //    }
        //    return objs;
        //}

        //protected static IList<string> GetDisplayNames(Type t)
        //{
        //    return GetDisplayNames(GetPubProperties(t),false,null);
        //}
        //protected static IList<string> GetDisplayNames(Type t,bool flag)
        //{
        //    return GetDisplayNames(GetPubProperties(t),flag,t.Name);
        //}
        //protected static IList<string> GetDisplayNames(Type t, bool flag, string baseTypeName)
        //{
        //    return GetDisplayNames(GetPubProperties(t), flag, baseTypeName);
        //}
        //protected static IList<string> GetDisplayNames(List<PropertyInfo> infos)
        //{
        //    return GetDisplayNames(infos, false,null);
        //}
        //protected static IList<string> GetDisplayNames(List<PropertyInfo> infos,bool flag,string baseName)
        //{
        //    List<string> objs = new List<string>(); Type t;
        //    if (baseName != null) { baseName += "."; }
        //    for (int i = 0; i < infos.Count; i++)
        //    {
        //        t = infos[i].PropertyType;
        //        if (flag && (t.Namespace == null || t.Namespace.IndexOf("System") != 0))
        //            objs.AddRange(GetDisplayNames(infos[i].PropertyType, !flag, GetDisplayName(infos[i], baseName)));//!flag//防止循环
        //            else
        //            objs.Add(GetDisplayName(infos[i],baseName));
        //    }
        //    return objs;
        //}
        #endregion
        protected static bool IsPrimaryKey(PropertyInfo info)
        {
            Castle.ActiveRecord.PrimaryKeyAttribute[] attrs;
            attrs = info.GetCustomAttributes(typeof(Castle.ActiveRecord.PrimaryKeyAttribute), false) as Castle.ActiveRecord.PrimaryKeyAttribute[];
            return attrs.Length != 0;
        }
        protected static string GetDisplayName(PropertyInfo info, string baseName)
        {
            string s = GetDisplayName(info);
            if (baseName == null || s == null)
                return s;
            else
                return baseName + s;
        }
        protected static string GetDisplayName(PropertyInfo info)
        {
            DisplayAttribute[] attrs; string str;
            attrs = info.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            //if (attrs == null)
            //    return "NULL";
            if (attrs.Length == 0)
                return null;
            str = attrs[0].Name;
            if (str.Length == 0)
                return info.Name;
            return str;

        }
        #endregion

        public static void Test()
        {
            Type t = null;// = typeof(AD);
            IList<string> strs;

            //strs = PropertyHelper.GetDisplayNames(t, false, null);//
            //strs = PropertyHelper.GetPropertiesName(t, false, null);//
            //strs = PropertyHelper.GetPropertyTypes(t, false);//

            //strs = PropertyHelper.GetDisplayNames(t, true, null);//
            //strs = PropertyHelper.GetPropertiesName(t, true, null);//
            //strs = PropertyHelper.GetPropertyTypes(t, true);//

            IList<PropertySchema> list;
            list = GetPropertiesSchema(t);//
            list = GetPropertiesSchema(t, true);//
            list[0].ToPropertyType(1);//
        }
    }

    public class GridViewTemplate : System.Web.UI.ITemplate
    {
        private DataControlRowType TemplateType;
        private PropertySchema PropertySchema;

        public GridViewTemplate(DataControlRowType type, PropertySchema propertySchema)
        {
            TemplateType = type;
            PropertySchema = propertySchema;
        }
        public void InstantiateIn(System.Web.UI.Control container)
        {
            Label obj = new Label();//
            // obj.ID = container.UniqueID + "_c" +container.Controls.Count +"";
            switch (TemplateType)
            {

                case DataControlRowType.Header:
                    obj.Text = "<B>" + PropertySchema.DisplayName + "</B>";
                    container.Controls.Add(obj);
                    break;

                case DataControlRowType.DataRow:
                    obj.DataBinding += new EventHandler(this.DataBinding);
                    container.Controls.Add(obj);


                    break;
                default:
                    break;
            }
        }
        private void DataBinding(Object sender, EventArgs e)
        {
            Label obj = (Label)sender;
            GridViewRow row = (GridViewRow)obj.NamingContainer;
            string id = System.Web.UI.DataBinder.Eval(row.DataItem, "ID").ToString();
            string value = System.Web.UI.DataBinder.Eval(row.DataItem, PropertySchema.PropertyName).ToString();

            LiteralControl lc = new LiteralControl(value);
            obj.Controls.Add(lc);


            if (PropertySchema.PropertyName == "State")
            {
                int v = 0; int.TryParse(value, out v);
                LiteralControl lc2;
                for (int i = 0; i < 3; i++)
                {
                    lc2 = new LiteralControl("|");
                    obj.Controls.Add(lc2);

                    System.Web.UI.HtmlControls.HtmlAnchor a = new System.Web.UI.HtmlControls.HtmlAnchor();
                    a.HRef = "javascript:JS_Change('" + id + "','" + PropertySchema.PropertyName + "'," + i + ")";//
                    a.InnerText = "" + i.ToString() + "";//
                    obj.Controls.Add(a);
                }
            }
        }
    }
    public class GridViewCommandTemplate : System.Web.UI.ITemplate
    {
        private DataControlRowType TemplateType;

        public GridViewCommandTemplate(DataControlRowType type)
        {
            TemplateType = type;
        }
        public void InstantiateIn(System.Web.UI.Control container)
        {

            switch (TemplateType)
            {

                case DataControlRowType.Header:
                    HtmlInputCheckBox cb1 = new HtmlInputCheckBox();
                    cb1.ID = "opcheckAll";
                    cb1.Attributes.Add("onclick", "JS_CheckedAll(this,\"opcheck\")");
                    container.Controls.Add(cb1);

                    Label obj1 = new Label();//
                    obj1.Text = "<B>管理/操作</B>";
                    container.Controls.Add(obj1);
                    break;

                case DataControlRowType.DataRow:
                    HtmlInputCheckBox cb = new HtmlInputCheckBox();
                    cb.ID = "opcheck";
                    container.Controls.Add(cb);

                    HtmlAnchor obj = new HtmlAnchor();
                    obj.HRef = "javascript:JS_Select({0});";
                    obj.InnerText = "详";
                    obj.DataBinding += new EventHandler(this.DataBinding);
                    container.Controls.Add(obj);

                    LiteralControl lc2 = new LiteralControl("|");
                    container.Controls.Add(lc2);

                    obj = new HtmlAnchor();
                    obj.HRef = "javascript:JS_AddOrEdit({0});";
                    obj.InnerText = "改";
                    obj.DataBinding += new EventHandler(this.DataBinding);
                    container.Controls.Add(obj);

                    LiteralControl lc3 = new LiteralControl("|");
                    container.Controls.Add(lc3);

                    obj = new HtmlAnchor();
                    obj.HRef = "javascript:JS_Delete({0});";
                    obj.InnerText = "删";
                    obj.DataBinding += new EventHandler(this.DataBinding);
                    container.Controls.Add(obj);


                    break;
                default:
                    break;
            }
        }
        private void DataBinding(Object sender, EventArgs e)
        {
            HtmlAnchor obj = (HtmlAnchor)sender;
            GridViewRow row = (GridViewRow)obj.NamingContainer;
            obj.HRef = string.Format(obj.HRef, System.Web.UI.DataBinder.Eval(row.DataItem, "ID").ToString());
        }
    }

    public partial class EntityHelper
    {
        public static void BindPropertyList<T>(DropDownList obj, bool showType)
        {
            IList<PropertySchema> list = EntityHelper.GetPropertiesSchema(typeof(T), true);//
            ListItem item; string tmp = "";
            obj.Items.Add(new ListItem("选择...", ""));//
            foreach (PropertySchema s in list)
            {
                item = new ListItem();
                if (showType)
                    tmp = "[" + s.PropertyTypeS + "]";
                if (s.IsPrimaryKey)
                {
                    item.Attributes.Add("style", "color:red");
                }
                item.Text = s.DisplayName + tmp;
                item.Value = s.PropertyName;
                obj.Items.Add(item);//
            }
        }
        public static void BindConditions<T>(Panel obj)
        {
            IList<PropertySchema> list = EntityHelper.GetPropertiesSchema(typeof(T), true);//
            HtmlTable table = new HtmlTable();

            HtmlTableRow row; HtmlTableCell cell;
            foreach (PropertySchema s in list)
            {
                row = new HtmlTableRow();

                cell = new HtmlTableCell();
                cell.Controls.Add(BuildLiteralControl(s.PropertyName, s.DisplayName + ":"));
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Controls.Add(BuildConditions("C1_" + s.PropertyName));
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Controls.Add(BuildHtmlInputText("V1_" + s.PropertyName, s));
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Controls.Add(BuildOrAnd("B_" + s.PropertyName));
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Controls.Add(BuildConditions("C2_" + s.PropertyName));
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Controls.Add(BuildHtmlInputText("V2_" + s.PropertyName, s));
                row.Cells.Add(cell);

                table.Rows.Add(row);//
            }
            obj.Controls.Add(table);//
        }
        public static LiteralControl BuildLiteralControl(string id, string text)
        {
            LiteralControl obj = new LiteralControl();
            obj.Text = text;
            obj.ID = id;
            return obj;
        }
        public static HtmlInputText BuildHtmlInputText(string id, PropertySchema s)
        {
            HtmlInputText obj = new HtmlInputText();
            obj.ID = id; obj.Attributes.Add("style", "width:86px;");
            switch (s.PropertyTypeS)
            {
                case "DateTime":
                    //obj.Attributes.Add("onfocus", "JS_SetDate(this);");//
                    obj.Attributes.Add("onclick", "$(\"#" + id + "\").datepicker({showOn: \"both\"}).attr(\"readonly\", \"readonly\");"); break;//
                case "Byte":
                case "Int32":
                    obj.Attributes.Add("onclick", "$(\"#" + id + "\").spinner();"); break;//
            }
            return obj;
        }
        public static DropDownList BuildConditions(string id)
        {
            DropDownList obj = new DropDownList(); obj.ID = id;//

            obj.Items.Add(new ListItem("选择...", ""));//

            obj.Items.Add(new ListItem("等于", "="));//
            obj.Items.Add(new ListItem("不等于", "="));//

            obj.Items.Add(new ListItem("大于等于", "="));//
            obj.Items.Add(new ListItem("小于等于", "="));//

            obj.Items.Add(new ListItem("大于", "="));//
            obj.Items.Add(new ListItem("小于", "="));//

            obj.Items.Add(new ListItem("Like", "="));//
            obj.Items.Add(new ListItem("Not Like", "="));//

            obj.Items.Add(new ListItem("Empty", "="));//
            obj.Items.Add(new ListItem("Not Empty", "="));//

            obj.Items.Add(new ListItem("NULL", "="));//
            obj.Items.Add(new ListItem("Not NULL", "="));//
            return obj;
        }
        public static DropDownList BuildOrAnd(string id)
        {
            DropDownList obj = new DropDownList(); obj.ID = id;//

            obj.Items.Add(new ListItem("选择...", ""));//

            obj.Items.Add(new ListItem("并且", "OR"));//
            obj.Items.Add(new ListItem("或者", "AND"));//
            return obj;
        }
    }
}
