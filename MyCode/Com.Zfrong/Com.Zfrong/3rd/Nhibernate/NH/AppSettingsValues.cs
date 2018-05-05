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

namespace Com.Zfrong.Common.Data.NH
{
    public class AppSettingsValues
    {
        private string _value;
        private string _name;
        private string _defaultValue;

        public AppSettingsValues(string value, string name, string defaultValue)
        {
            this._value = value;
            this._name = name;
            this._defaultValue = defaultValue;
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
    }
}