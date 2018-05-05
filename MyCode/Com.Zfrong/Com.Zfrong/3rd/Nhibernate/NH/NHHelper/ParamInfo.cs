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
        private string _name; // ��������
        private object _value; // ����ֵ
        private IType _type; // ��������

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