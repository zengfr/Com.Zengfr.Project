using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Zengfr.Proj.Common
{
    public class FilterHelper
    {
        public class FilterExpression
        {
            public virtual string PropertyName { get; set; }
            public virtual string ExpressionType { get; set; }
            public virtual object PropertyValue { get; set; }
        }
        public enum FilterExpressionType
        {
            Between, Eq, EqProperty,
            Ge, GeProperty, Gt, GtProperty,
            In, InsensitiveLike, IsEmpty, IsNotEmpty,
            IsNotNull, IsNull, Le, LeProperty,
            Like, Lt, LtProperty, NotEqProperty,
            Sql,
            NotEq, NotIn, NotBetween, NotLike, NotInsensitiveLike

        }
        protected IList<FilterExpression> Filters = new List<FilterExpression>();

        public string[] FilterNames
        {
            get { return Filters.Select(t => t.PropertyName).ToArray(); }
        }
        public string[] FilterTypes
        {
            get { return Filters.Select(t => t.ExpressionType).ToArray(); }
        }
        public object[] FilterValues
        {
            get { return Filters.Select(t => t.PropertyValue).ToArray(); }
        }
        public void AddExpression(FilterExpressionType type, string name, object value)
        {
            AddExpression(name, type, value);
        }
        public void AddExpression(string name, FilterExpressionType type, object value)
        {
            AddExpression(name, type.ToString(), value);
        }
        public void AddExpression(string name, string type, object value)
        {
            Filters.Add(new FilterExpression() { PropertyName = name, ExpressionType = type, PropertyValue = value });
        }
        public void AddEq(string name, object value)
        {
            AddExpression(name, FilterExpressionType.Eq, value);
        }
        public void AddLike(string name, string value)
        {
            AddExpression(name, FilterExpressionType.Like, value);
        }
        public void AddBtw(string name, object minValue, object maxValue)
        {
            AddExpression(name, FilterExpressionType.Between, new object[] { minValue, maxValue });
        }
        public void AddIN(string name, object[] values)
        {
            AddExpression(name, FilterExpressionType.In, values);
        }


    }
}
