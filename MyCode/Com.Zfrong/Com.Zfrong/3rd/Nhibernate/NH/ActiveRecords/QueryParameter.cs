/*

$Header$
$Author$
$Date$ 
$Revision$
$History$

*/


using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Type;

namespace Com.Zfrong.Common.Data.NH.ActiveRecords
{
    public class QueryParameter
    {
        private string _name;
        private object _value;
        private IType _type;

        public QueryParameter(string name, object value)
            : this(name, value, null)
        {
        }

        public QueryParameter(string name, object value, IType type)
        {
            this._name = name;
            this._value = value;
            this._type = type;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public IType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public static bool IsAvail(QueryParameter parameter)
        {
            return (parameter != null && !string.IsNullOrEmpty(parameter.Name) && parameter.Value != null);
        }
    }
}
