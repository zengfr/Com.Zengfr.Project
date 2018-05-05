using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Quartz.Impl;
using Quartz;

namespace Quartz.Utils
{
    public class QuartzUtils
    {

        public static IScheduler GetScheduler(string configType)
        {
            var properties = QuartzConfig.InitXml();
            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            IScheduler sched = sf.GetScheduler();
            return sched;
        }

    }
    public class QuartzConfig
    {

        public static NameValueCollection InitAdo()
        {
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";
            properties["quartz.dataSource.default.connectionString"] = "Server=.\\SQLEXPRESS;Database=test;Trusted_Connection=True;";
            return InitAdo("SqlServer-20", false, properties);
        }
        private static NameValueCollection InitAdo(string dbProvider, bool clustered, NameValueCollection extraProperties)
        {
            NameValueCollection properties = new NameValueCollection();

            properties["quartz.scheduler.instanceName"] = "TestScheduler";
            properties["quartz.scheduler.instanceId"] = "instance_one";
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "10";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            properties["quartz.jobStore.misfireThreshold"] = "60000";
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz";
            properties["quartz.jobStore.useProperties"] = "false";
            properties["quartz.jobStore.dataSource"] = "default";
            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
            properties["quartz.jobStore.clustered"] = clustered.ToString();
            properties["quartz.dataSource.default.connectionString"] = "Server=.;Database=Quartz;Trusted_Connection=True;";

            if (extraProperties != null)
            {
                foreach (string key in extraProperties.Keys)
                {
                    properties[key] = extraProperties[key];
                }
            }

            // if (connectionStringId == "SQLServer" || connectionStringId == "SQLite")
            {
                // if running MS SQL Server we need this
                properties["quartz.jobStore.lockHandler.type"] =
                    "Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, Quartz";
            }

            //properties["quartz.dataSource.default.connectionString"] = (string)dbConnectionStrings[connectionStringId];
            properties["quartz.dataSource.default.provider"] = dbProvider;

            return properties;
        }

        public static NameValueCollection InitXml()
        {
            var xmlFile = "~/quartz_jobs.xml";
            return InitXml(xmlFile);
        }
        private static NameValueCollection InitXml(string xmlFile)
        {
            var properties = new NameValueCollection();
            properties["quartz.plugin.triggHistory.type"] = "Quartz.Plugin.History.LoggingJobHistoryPlugin";

            properties["quartz.plugin.jobInitializer.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin";
            properties["quartz.plugin.jobInitializer.fileNames"] = "quartz_jobs.xml";
            properties["quartz.plugin.jobInitializer.failOnFileNotFound"] = "false";
            properties["quartz.plugin.jobInitializer.scanInterval"] = "1";
            //或者  
            //properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";  
            //properties["quartz.plugin.xml.fileNames"] = "~/quartz_jobs.xml";  

            return properties;
        }
    }
}
