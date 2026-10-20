using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Commons.Collections;
using NVelocity.Runtime;

namespace Com.Zengfr.CodeGenerator.Common.Engine
{
    public class ZPropertiesFactory
    {
        private static ZPropertiesFactory instance;
        public static ZPropertiesFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ZPropertiesFactory();
                }
                return instance;
            }
        }
        public virtual ExtendedProperties Create4File(String templatDir)
        {
            ExtendedProperties props = new ExtendedProperties();
            props.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            props.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, templatDir);
            return props;
        }
        public virtual ExtendedProperties Create4FileInWeb(String templatDir)
        {
            ExtendedProperties props = new ExtendedProperties();
            props.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            props.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, HttpContext.Current.Server.MapPath(templatDir));
            return props;
        }
        public virtual ExtendedProperties Create4AssemblyReource(String assemblyName)
        {
            ExtendedProperties props = new ExtendedProperties();
            props.SetProperty(RuntimeConstants.RESOURCE_LOADER, "assembly");
            props.SetProperty("assembly.resource.loader.class", "NVelocity.Runtime.Resource.Loader.AssemblyResourceLoader;NVelocity");
            props.SetProperty("assembly.resource.loader.assembly", assemblyName);

            return props;
        }

    }
}
