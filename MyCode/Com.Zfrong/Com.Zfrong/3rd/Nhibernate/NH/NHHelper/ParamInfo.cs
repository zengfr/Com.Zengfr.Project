/*

$Header$
$Author$
$Date$ 
$Revision$
$History$

*/


using System;
using NHibernate.Type;

/// <summary>
/// ParamInfo 
/// </summary>
namespace Com.Zfrong.Common.Data.NH
{
    public class ParamInfo
    {
        private string _name; // 参数名称
        private object _value; // 参数值
        private IType _type; // 参数类型

        public ParamInfo(string name, object value)
            : this(name, value, null)
        {
        }

        public ParamInfo(string name, object value, IType type)
        {
            this._name = name;
            this._value = value;
            this._type = type;
        }

        public string Name
        {
            get { return _name; }
        }

        public object Value
        {
            get { return _value; }
        }

        public IType Type
        {
            get { return _type; }
        }
    }
}