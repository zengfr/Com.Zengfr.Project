using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Layout;
namespace Com.Zfrong.Common.Core.Log
{
    /// <summary>
    /// <!--自定义变量-->   log.Info(new LogContent("reason-because……"));
///     <parameter>
///        <parameterName value="@Reason" />
///        <dbType value="String" />
///        <size value="1000" />
///        <layout type="log4net.Layout.PatternLayout" >

///           <param name="ConversionPattern" value="%property{Reason}"/>

///        </layout>
///      </parameter>

///</appender>
    /// </summary>
    public class MyLayout : PatternLayout
    {
        public MyLayout()
        {
            this.AddConverter("property", typeof(MyMessagePatternConverter));
        }
    }
}
