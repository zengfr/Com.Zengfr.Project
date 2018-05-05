using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.AdoJobStore.Common;
using System.Threading;

namespace TaskApplication
{
    public class AdoSchedulerFactory
    {
       
        public static  void CleanUpScheduler(IScheduler inScheduler)
        {
            
            // unschedule jobs
            string[] groups = inScheduler.TriggerGroupNames;
            for (int i = 0; i < groups.Length; i++)
            {
                String[] names = inScheduler.GetTriggerNames(groups[i]);
                for (int j = 0; j < names.Length; j++)
                    inScheduler.UnscheduleJob(names[j], groups[i]);
            }

            // delete jobs
            groups = inScheduler.JobGroupNames;
            for (int i = 0; i < groups.Length; i++)
            {
                String[] names = inScheduler.GetJobNames(groups[i]);
                for (int j = 0; j < names.Length; j++)
                    inScheduler.DeleteJob(names[j], groups[i]);
            }
        }

        private static DbMetadata GetMySqlDbMetadata()
        {
            var metadata = new DbMetadata();
            metadata.ConnectionType = typeof(MySql.Data.MySqlClient.MySqlConnection);
            metadata.CommandType = typeof(MySql.Data.MySqlClient.MySqlCommand);
            metadata.ParameterDbType = typeof(MySql.Data.MySqlClient.MySqlDbType);
            metadata.ParameterType = typeof(MySql.Data.MySqlClient.MySqlParameter);
            metadata.CommandBuilderType = typeof(MySql.Data.MySqlClient.MySqlCommandBuilder);
            metadata.ExceptionType = typeof(MySql.Data.MySqlClient.MySqlException);
            metadata.UseParameterNamePrefixInParameterCollection = true;
            metadata.ParameterNamePrefix = "?";
            metadata.Init();
            return metadata;
        }
        public static  ISchedulerFactory GetFormAdoConfig()
        {

            DbProvider.RegisterDbMetadata("MySql-6", GetMySqlDbMetadata()); 

            NameValueCollection properties = new NameValueCollection();

            properties["quartz.scheduler.instanceName"] = "TestScheduler";
            properties["quartz.scheduler.instanceId"] = "instance_one";
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            properties["quartz.jobStore.misfireThreshold"] = "60000";
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            //properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz";
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz";
            properties["quartz.jobStore.useProperties"] = "false";
            properties["quartz.jobStore.dataSource"] = "default";
            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
            properties["quartz.jobStore.clustered"] = "true";
            // if running MS SQL Server we need this
           // properties["quartz.jobStore.selectWithLockSQL"] = "SELECT * FROM {0}LOCKS UPDLOCK WHERE LOCK_NAME = @lockName";

            //properties["quartz.dataSource.default.connectionString"] = @"Server=LIJUNNIN-PC\SQLEXPRESS;Database=quartz;Trusted_Connection=True;";
            //properties["quartz.dataSource.default.provider"] = "SqlServer-20";

            properties["quartz.dataSource.default.connectionString"] = @"Server=127.0.0.1;database=fw;uid=root;pwd=123456;Max Pool Size = 512;";
            properties["quartz.dataSource.default.provider"] = "MySql-6";// "SqlServer-20";
            // First we must get a reference to a scheduler
            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            return sf;
        }
    }
}
