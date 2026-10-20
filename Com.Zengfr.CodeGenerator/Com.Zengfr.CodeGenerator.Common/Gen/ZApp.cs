using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Com.Zengfr.CodeGenerator.Common.Meta;
using Com.Zengfr.CodeGenerator.Common.Meta.Builder;
using Com.Zengfr.CodeGenerator.Common.Util;
using SchemaExplorer;

namespace Com.Zengfr.CodeGenerator.Common.Gen
{
    public class ZApp
    {
        public static void Main(string[] args)
        {
            Test2.B.Main2();

            Console.WindowWidth = 160;
            Console.BufferWidth = Console.WindowWidth * 10;

            ZMeta meta = null;

            var templateRootDir = string.Empty;
            var outputRootDir = string.Empty;
            var namespacePrefix = string.Empty;

            var assemblyFile = @"E:\csharpproject\Com.Zengfr.Project\MyCode\fundAndStock\Entity\bin\Debug\Entity.dll";
            meta = BuildMetaByAssembly(templateRootDir, outputRootDir, assemblyFile);

            //var connectionString = @"Server=(local)\NetSDK;Database=Northwind;Integrated Security=true;";
            //var connectionProviderType = "sqlserver";
            //string tablePrefix = "";
            //meta = BuildMetaByDatabase(templateRootDir, outputRootDir, connectionString, connectionProviderType, tablePrefix, namespaces);

            GenByMeta(templateRootDir, outputRootDir, meta, namespacePrefix);

            Console.ReadLine();
        }
        private static void GenByMeta(String templateRootDir, String outputRootDir, ZMeta meta, string namespacePrefix)
        {
            ZSysConfig sysConfig = ZGenerator.BuildSysConfig(false, templateRootDir, outputRootDir);
            ZInputConfig inConfig = ZGenerator.BuildInputConfig(sysConfig, meta, namespacePrefix);
            ZGenerator.Process(sysConfig, meta, inConfig);
        }
        public static ZMeta BuildMetaByXmlFile(String templateRootDir, String outputRootDir, String xmlFile)
        {
            var xmlContent = File.ReadAllText(xmlFile);
            ZMeta meta = BuildMetaByXml(templateRootDir, outputRootDir, xmlContent);
            return meta;
        }
        public static ZMeta BuildMetaByXml(String templateRootDir, String outputRootDir, String xmlContent)
        {
            XmlSerializer se = new XmlSerializer(typeof(ZMeta));
            byte[] arry = Encoding.UTF8.GetBytes(xmlContent);
            System.IO.Stream me = new System.IO.MemoryStream(arry);
            ZMeta meta = se.Deserialize(me) as ZMeta;
            return meta;
        }
        public static ZMeta BuildMetaByAssembly(String templateRootDir, String outputRootDir, String assemblyFile)
        {
            ZMeta meta = ZMeta4AssemblyBuilder.BuildMeta(assemblyFile, true);
            return meta;
        }
        public static ZMeta BuildMetaByDatabase(String templateRootDir, String outputRootDir,
            string connectionString, string connectionProviderType, string tablePrefix, string namespaces)
        {

            DatabaseSchema databaseSchema = null;
            switch (connectionProviderType)
            {
                case "sqlserver":
                    databaseSchema = new DatabaseSchema(new SqlSchemaProvider(), connectionString);
                    break;
                case "mysql":
                    databaseSchema = new DatabaseSchema(new SqlSchemaProvider(), connectionString);
                    break;
                case "":
                    databaseSchema = new DatabaseSchema(new SqlSchemaProvider(), connectionString);
                    break;
                default:
                    break;
            }
            ZMeta meta = null;
            if (databaseSchema != null)
            {
                meta = ZMeta4DBBuilder.BuildMeta(databaseSchema, tablePrefix, namespaces);

            }
            return meta;
        }

    }
}
