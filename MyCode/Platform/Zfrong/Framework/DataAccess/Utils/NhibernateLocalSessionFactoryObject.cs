using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Spring.Data.NHibernate;
namespace Zfrong.Framework.Repository.Utils
{
   using System.Reflection;
   using NHibernate.Cfg;
    using NHibernate.Mapping.ByCode;
   public class NHibernateLocalSessionFactoryObject: LocalSessionFactoryObject
        {
           public string[] ByCodeMappingAssemblies { get; set; }
          public string   SchemaAction{ get; set; }
            protected override void PostProcessConfiguration(Configuration config)
            {
                base.PostProcessConfiguration(config);
                if (ByCodeMappingAssemblies != null) 
                {
                    List<Type> types=new List<Type>();
                    foreach (string assemblyName in ByCodeMappingAssemblies)
                    {
                        Assembly assembly = Assembly.Load(assemblyName);
                        if (assembly != null)
                        {
                            types.AddRange(assembly.GetExportedTypes());
                        }
                    }
                    if (types.Count != 0)
                    {
                        var mappers = new ModelMapper();
                        mappers.AddMappings(types);
                        var hbmMapping = mappers.CompileMappingForAllExplicitlyAddedEntities();
                        config.AddMapping(hbmMapping); mappers = null;
                    }
                    types = null;
                }
                SchemaAutoAction action=null;
                switch (this.SchemaAction)
                {
                    case "Create": action = SchemaAutoAction.Create; break;
                    case "Recreate": action = SchemaAutoAction.Recreate; break;
                    case "Update": action = SchemaAutoAction.Update; break;
                    case "Validate": action = SchemaAutoAction.Validate; break;
                    default: break;
                }
                if (action != null)
                {
                    //config.DataBaseIntegration(c => c.SchemaAction = action);
                }
                NHibernate.Tool.hbm2ddl.SchemaExport se =new NHibernate.Tool.hbm2ddl.SchemaExport(config);
                var sb = new StringBuilder();
                TextWriter output = new StringWriter(sb);
                se.SetDelimiter(";");
                se.Execute(true,false,false,null,output);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+"\\SchemaExport.sql", sb.ToString(), Encoding.UTF8);
                sb = null; output.Close(); se = null; action = null;
            }
        }
    }