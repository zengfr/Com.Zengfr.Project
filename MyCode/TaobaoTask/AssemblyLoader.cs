using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Reflection.Emit;
using RestSharp.Extensions;

namespace ConsoleTask
{
     class AssemblyLoader
    {
        private static readonly object nullCacheLock = new object();

        private static readonly Dictionary<string, bool> nullCache = new Dictionary<string, bool>();

        private static readonly Dictionary<string, string> assemblyNames = new Dictionary<string, string>();

        private static readonly Dictionary<string, string> symbolNames = new Dictionary<string, string>();

        private static int isAttached = 0;

        private static string CultureToString(CultureInfo culture)
        {
            if (culture == null)
            {
                return "";
            }
            return culture.Name;
        }
        static AssemblyLoader()
        {
            // Note: this type is marked as 'beforefieldinit'.
            AssemblyLoader.assemblyNames.Add("apache.nms.activemq", "costura.apache.nms.activemq.dll.compressed");
            AssemblyLoader.assemblyNames.Add("apache.nms", "costura.apache.nms.dll.compressed");
            AssemblyLoader.assemblyNames.Add("beitmemcached", "costura.beitmemcached.dll.compressed");
            AssemblyLoader.symbolNames.Add("beitmemcached", "costura.beitmemcached.pdb.compressed");
            AssemblyLoader.assemblyNames.Add("bll_summay_db", "costura.bll_summay_db.dll.compressed");
            AssemblyLoader.assemblyNames.Add("bll_syndata_mq", "costura.bll_syndata_mq.dll.compressed");
            AssemblyLoader.symbolNames.Add("bll_syndata_mq", "costura.bll_syndata_mq.pdb.compressed");
            AssemblyLoader.assemblyNames.Add("dac.bll", "costura.dac.bll.dll.compressed");
            AssemblyLoader.symbolNames.Add("dac.bll", "costura.dac.bll.pdb.compressed");
            AssemblyLoader.assemblyNames.Add("dc.bll", "costura.dc.bll.dll.compressed");
            AssemblyLoader.symbolNames.Add("dc.bll", "costura.dc.bll.pdb.compressed");
            AssemblyLoader.assemblyNames.Add("dc.entity", "costura.dc.entity.dll.compressed");
            AssemblyLoader.assemblyNames.Add("gti.base.cache", "costura.gti.base.cache.dll.compressed");
            AssemblyLoader.symbolNames.Add("gti.base.cache", "costura.gti.base.cache.pdb.compressed");
            AssemblyLoader.assemblyNames.Add("iesi.collections", "costura.iesi.collections.dll.compressed");
            AssemblyLoader.assemblyNames.Add("ionic.zlib", "costura.ionic.zlib.dll.compressed");
            AssemblyLoader.assemblyNames.Add("log4net", "costura.log4net.dll.compressed");
            AssemblyLoader.assemblyNames.Add("mysql.data", "costura.mysql.data.dll.compressed");
            AssemblyLoader.assemblyNames.Add("newtonsoft.json", "costura.newtonsoft.json.dll.compressed");
            AssemblyLoader.symbolNames.Add("newtonsoft.json", "costura.newtonsoft.json.pdb.compressed");
            AssemblyLoader.assemblyNames.Add("nhibernate", "costura.nhibernate.dll.compressed");
            AssemblyLoader.assemblyNames.Add("npoi", "costura.npoi.dll.compressed");
            AssemblyLoader.assemblyNames.Add("servicestack.common", "costura.servicestack.common.dll.compressed");
            AssemblyLoader.assemblyNames.Add("servicestack.interfaces", "costura.servicestack.interfaces.dll.compressed");
            AssemblyLoader.assemblyNames.Add("servicestack.redis", "costura.servicestack.redis.dll.compressed");
            AssemblyLoader.assemblyNames.Add("servicestack.text", "costura.servicestack.text.dll.compressed");
        }

        public static void Attach()
        {
            if (Interlocked.Exchange(ref AssemblyLoader.isAttached, 1) == 1)
            {
                return;
            }
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyLoader.ResolveAssembly);
        }
        private static Assembly ReadExistingAssembly(AssemblyName name)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                AssemblyName name2 = assembly.GetName();
                if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(AssemblyLoader.CultureToString(name2.CultureInfo), AssemblyLoader.CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
                {
                    return assembly;
                }
            }
            return null;
        }
        public static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
        {
            object obj = AssemblyLoader.nullCacheLock;
            lock (obj)
            {
                if (AssemblyLoader.nullCache.ContainsKey(e.Name))
                {
                    Assembly result = null;
                    return result;
                }
            }
            AssemblyName assemblyName = new AssemblyName(e.Name);
            Assembly assembly = AssemblyLoader.ReadExistingAssembly(assemblyName);
            if (assembly != null)
            {
                return assembly;
            }
            assembly = AssemblyLoader.ReadFromEmbeddedResources(AssemblyLoader.assemblyNames, AssemblyLoader.symbolNames, assemblyName);
            if (assembly == null)
            {
                obj = AssemblyLoader.nullCacheLock;
                lock (obj)
                {
                    AssemblyLoader.nullCache[e.Name] = true;
                }
                if (assemblyName.Flags == AssemblyNameFlags.Retargetable)
                {
                    assembly = Assembly.Load(assemblyName);
                }
            }
            return assembly;
        }
        // public static Assembly ReadFromEmbeddedResources(string requestedAssemblyName,string path)
        //  {
        //AssemblyName assemblyName = new AssemblyName(requestedAssemblyName);
        //var assembly = AssemblyLoader.ReadFromEmbeddedResources(AssemblyLoader.assemblyNames, AssemblyLoader.symbolNames, assemblyName);
        // AssemblyBuilder ab = new AssemblyBuilder(); AssemblyBuilder.DefineDynamicAssembly
        // ; ; ab.Save(path);
        // return assembly;

        // }
        public static void saveto()
        {
            var fullname = "costura.bll_summay_db.dll.compressed";
            var savePath = "E:\\Download\\";
            var dllPath = "E:\\Download\\DAC_SumService32.exe";
            MemoryStream memoryStream=null;
            if (fullname.EndsWith(".compressed"))
            {
                var assembly=Assembly.Load(""+dllPath);
                using (Stream manifestResourceStream = assembly.GetManifestResourceStream(fullname))
                {
                    using (DeflateStream deflateStream = new DeflateStream(manifestResourceStream, CompressionMode.Decompress))
                    {
                          memoryStream = new MemoryStream();
                        AssemblyLoader.CopyTo(deflateStream, memoryStream);
                        memoryStream.Position = 0L;

                    }
                }
            }
            // save to local disk
            using (FileStream fileStream = new FileStream(@"" + savePath+"/"+fullname.Replace(".compressed","orgi.dll"),FileMode.Create))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(fileStream);
            }
        }
        public static void saveto2()
        {
            var fullname = "C:\\Users\\zengfr\\Desktop\\apk\\2\\costura.dc.entity.dll.compressed";
            var savePath = "C:\\Users\\zengfr\\Desktop\\apk\\2\\";
        
            MemoryStream memoryStream = null;
            if (fullname.EndsWith(".compressed"))
            { 
                using (Stream manifestResourceStream = new FileStream(fullname, FileMode.Open))
                {
                    using (DeflateStream deflateStream = new DeflateStream(manifestResourceStream, CompressionMode.Decompress))
                    {
                        memoryStream = new MemoryStream();
                        AssemblyLoader.CopyTo(deflateStream, memoryStream);
                        memoryStream.Position = 0L;

                    }
                }
            }
            // save to local disk
            using (FileStream fileStream = new FileStream(@"" + savePath + "/" +Path.GetFileNameWithoutExtension( fullname).Replace(".compressed", "orgi.dll").Replace("costura.", "").ToPascalCase(CultureInfo.CurrentCulture), FileMode.Create))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(fileStream);
            }
        }

        private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
        {
            string text = requestedAssemblyName.Name.ToLowerInvariant();
            if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
            {
                text = string.Format("{0}.{1}", requestedAssemblyName.CultureInfo.Name, text);
            }
            byte[] rawAssembly;
            using (Stream stream = AssemblyLoader.LoadStream(assemblyNames, text))
            {
                if (stream == null)
                {
                    Assembly result = null;
                    return result;
                }
                rawAssembly = AssemblyLoader.ReadStream(stream);
            }
            using (Stream stream2 = AssemblyLoader.LoadStream(symbolNames, text))
            {
                if (stream2 != null)
                {
                    byte[] rawSymbolStore = AssemblyLoader.ReadStream(stream2);
                    Assembly result = Assembly.Load(rawAssembly, rawSymbolStore);
                    return result;
                }
            }
            return Assembly.Load(rawAssembly);
        }
        private static void CopyTo(Stream source, Stream destination)
        {
            byte[] array = new byte[81920];
            int count;
            while ((count = source.Read(array, 0, array.Length)) != 0)
            {
                destination.Write(array, 0, count);
            }
        }

        private static Stream LoadStream(string fullname)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            if (fullname.EndsWith(".compressed"))
            {
                using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(fullname))
                {
                    using (DeflateStream deflateStream = new DeflateStream(manifestResourceStream, CompressionMode.Decompress))
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        AssemblyLoader.CopyTo(deflateStream, memoryStream);
                        memoryStream.Position = 0L;
                        return memoryStream;
                    }
                }
            }
            return executingAssembly.GetManifestResourceStream(fullname);
        }

        private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
        {
            string fullname;
            if (resourceNames.TryGetValue(name, out fullname))
            {
                return AssemblyLoader.LoadStream(fullname);
            }
            return null;
        }

        private static byte[] ReadStream(Stream stream)
        {
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            return array;
        }

    }
}
