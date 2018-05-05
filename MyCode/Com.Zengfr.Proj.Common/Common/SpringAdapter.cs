
//
// 创建标识:Copyright (C) 2014-2015 zengfr 版权所有
// 创建描述:
// 创建时间:2015-9-15 23:42:20
// 功能描述:
// 修改标识: 无
// 修改描述: 无
//

using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;


using NHibernate;
using ServiceStack.Common.Extensions;
using ServiceStack.ServiceInterface;

using ServiceStack.Configuration;
using ServiceStack.WebHost.Endpoints;
using System.Web;
using Spring.Context;
using Spring.Context.Support;
namespace Com.Zengfr.Proj.Common.Web
{

    public class SpringAdapter : IContainerAdapter, IDisposable
    {
        private IApplicationContext ctx;

        public SpringAdapter(IApplicationContext _ctx)
        {
            // IApplicationContext container = ContextRegistry.GetContext();  
            if (_ctx == null)
            {
                _ctx = new XmlApplicationContext("~/SpringConfig/spring.xml");
            }
            ctx = _ctx;
        }

        public T Resolve<T>()
        {
            // var names = GetObject<T>();
            //return ctx.GetObject(names[0]) as T;

            return (T)ctx.GetObject(typeof(T).Name);
        }

        public T TryResolve<T>()
        {
            // var names = GetObject<T>();
            //return ctx.GetObject(names[0]) as T;

            return (T)ctx.GetObject(typeof(T).Name);
        }
        //private  string[] GetObject<T>()
        //{
        //    var names = ctx.GetObjectNamesForType(typeof(T));
        //    return names;
        //}
        public void Dispose()
        {
            ctx.Dispose();
        }

    }
}
