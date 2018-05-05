using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using Zfrong.Framework.Core.DataContract;
namespace Zfrong.Framework.Repository.Utils
{
   public class ExtFilterItemHelper
    {
      
       protected static void ToCriteria(DetachedCriteria criteria, ExtFilterItem[] items)
       {
            IList<ICriterion> objs =ToCriterion(items);
            foreach (ICriterion c in objs)
            {
                criteria.Add(c);
            }
       }
       public static ICriterion[] ToCriterion(ExtFilterItem[] items)
        {
            IList<ICriterion> objs = new List<ICriterion>();
            ICriterion criterion = null;
            if (items != null)
            {
                foreach (ExtFilterItem f in items)
                {
                    if (f != null)
                    {
                        criterion = ToCriterion(f);
                        if (criterion != null)
                        {
                            objs.Add(criterion);
                        }
                    }
                }
            }
            return objs.ToArray();
        }
        static ICriterion ToCriterion(ExtFilterItem f)
        {
            ICriterion e=null;
            if (f.Data != null)
            {
                switch (f.Data.Comparison)
                {
                    case "eq": e = Expression.Eq("" + f.Field, GetDataValue(f)); break;
                    case "ge": e = Expression.Ge("" + f.Field, GetDataValue(f)); break;
                    case "gt": e = Expression.Gt("" + f.Field, GetDataValue(f)); break;
                    case "le": e = Expression.Le("" + f.Field, GetDataValue(f)); break;
                    case "lt": e = Expression.Lt("" + f.Field, GetDataValue(f)); break;
                    case "like": e = Expression.Like("" + f.Field, GetDataValue(f)); break;
                    default:
                        e = Expression.Eq("" + f.Field, GetDataValue(f));
                        break;

                }
            }
            return e;
        }
        static object GetDataValue(ExtFilterItem f)
        {
            string value = f.Data.Value;

            switch (f.Data.Type.ToLower())
            {
                default:
                case "string": return value;
                case "int": return int.Parse(value);
                case "int32": return int.Parse(value);
                case "bool": return bool.Parse(value);
                case "boolean": return bool.Parse(value);
                case "date": return DateTime.Parse(value);
                case "datetime": return DateTime.Parse(value);
            }
        }
    }
}
