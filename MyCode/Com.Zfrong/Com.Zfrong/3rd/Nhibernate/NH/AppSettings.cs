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
using System.Configuration;

namespace Com.Zfrong.Common.Data.NH
{
    public class AppSettings
    {

        #region Fields

        private static AppSettingsValues nHibernateSessionSourceType = new AppSettingsValues(null, "NHibernateSessionSourceType", "threadStatic");// "http");
        private static AppSettingsValues nHibernateHttpSessionSourceItemName = new AppSettingsValues(null, "NHibernateHttpSessionSourceItemName", "NHSession");
        private static AppSettingsValues nHibernateUserSessionSource = new AppSettingsValues(null, "NHibernateUserSessionSource", "true");

        #endregion

        #region Constructors

        private AppSettings() { }

        #endregion

        #region Properties

        public static string NHibernateSessionSourceType
        {
            get { return GetValue(nHibernateSessionSourceType); }
        }

        public static string NHibernateHttpSessionSourceItemName
        {
            get { return GetValue(nHibernateHttpSessionSourceItemName); }
        }

        public static string NHibernateUserSessionSource
        {
            get { return GetValue(nHibernateUserSessionSource); }
        }

        #endregion

        #region Methods

        private static string GetValue(AppSettingsValues settingValue)
        {
            if (settingValue == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(settingValue.Name))
            {
                return string.IsNullOrEmpty(settingValue.DefaultValue) ? string.Empty : settingValue.DefaultValue;
            }
            else
            {
                settingValue.Value = ConfigurationManager.AppSettings[settingValue.Name];
            }

            return string.IsNullOrEmpty(settingValue.Value) ? settingValue.DefaultValue : settingValue.Value;
        }

        #endregion

    }
}
