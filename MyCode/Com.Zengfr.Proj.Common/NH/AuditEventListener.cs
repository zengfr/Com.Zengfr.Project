using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate;
using NHibernate.Persister.Entity;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Com.Zengfr.Proj.Common.NH.Audit
{
    public class AuditLogEventListener : IPostUpdateEventListener, ISaveOrUpdateEventListener, IPreUpdateEventListener
    {
        public void OnSaveOrUpdate(SaveOrUpdateEvent @event)
        {

        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            try
            {
                SaveAuditLog(@event.Entity, @event.Id, @event.OldState, @event.State, @event.Persister, @event.Session);
            }
            catch { }
            return false;
        }
        public void OnPostUpdate(PostUpdateEvent @event)
        {
            try
            {
                SaveAuditLog(@event.Entity, @event.Id, @event.OldState, @event.State, @event.Persister, @event.Session);
            }
            catch { }
        }
        private const string _noValueString = "[NOV]";

        private static string getStringValueFromStateArray(object[] stateArray, int position)
        {
            var value = stateArray[position];
            if (value == null || value.ToString() == string.Empty)
            {
                return _noValueString;
            }
            return Serialize(value);
        }
        private static string Serialize(object t)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = string.Empty;
                settings.NewLineChars = string.Empty;
                settings.Encoding = Encoding.UTF8;
                settings.OmitXmlDeclaration = true; // 不生成声明头
                XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                xns.Add(string.Empty, string.Empty);
                using (XmlWriter xmlWriter = XmlWriter.Create(sw, settings))
                {
                    XmlSerializer xz = new XmlSerializer(t.GetType());
                    {
                        xz.Serialize(xmlWriter, t, xns);
                        return sw.ToString();
                    }
                }
            }
        }
        public void SaveAuditLog(object Entity, object Id, object[] OldState, object[] State,
            IEntityPersister Persister, IEventSource Session)
        {
            if (Entity is AuditMetadataLog)
            {
                return;
            }
            var entityFullName = Entity.GetType().FullName;

            if (OldState == null)
            {
                //                throw new ArgumentNullException(
                //                    string.Format(@"No old state available for entity type '{0}':{1}. 
                //Make sure you're loading it into Session before modifying and saving it.",
                //                                                entityFullName, @event.Id));
                return;
            }
            var dirtyFieldIndexes = Persister.FindDirty(State, OldState, Entity, Session);

            using (var session = Session.GetSession(EntityMode.Poco))
            {
                if (!session.IsConnected || !session.IsOpen)
                    return;

                foreach (var dirtyFieldIndex in dirtyFieldIndexes)
                {
                    var oldValue = getStringValueFromStateArray(OldState, dirtyFieldIndex);
                    var newValue = getStringValueFromStateArray(State, dirtyFieldIndex);

                    if (oldValue == newValue)
                    {
                        continue;
                    }
                    var auditMetadataLog = new AuditMetadataLog
                    {
                        EntityName = Entity.GetType().Name,
                        FieldName = Persister.PropertyNames[dirtyFieldIndex],
                        OldValue = oldValue,
                        NewValue = newValue,

                        EntityId = (long)Id,
                        AuditType = "U",
                        Timestamp = DateTime.Now
                    };
                    session.Save(auditMetadataLog);
                }
                session.Flush();
            }

        }


    }
}
