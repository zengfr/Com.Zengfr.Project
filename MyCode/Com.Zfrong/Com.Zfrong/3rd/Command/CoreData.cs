using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Com.Zfrong.Common.Command
{
    public class Core
    {
        static NotifiedDictionary coreData;

        /// <summary>
        /// ��ȡ�������ú�������
        /// </summary>
        public static NotifiedDictionary CoreData
        {
            get { return Core.coreData; }
            set { Core.coreData = value; }
        }

        static Core()
        {
            CoreData = new NotifiedDictionary();
            InitCoreData();

        }

        /// <summary>
        /// ִ��һЩ���ܵ����ݳ�ʼ��
        /// </summary>
        static void InitCoreData()
        {
            //����ö�٣���ӵ��ֵ���
            foreach (CoreDataType Key in System.Enum.GetValues(typeof(CoreDataType)))
            {
                CoreData.Add(Key, null);
            }
            //CoreData[CoreDataType.HasCourse] = false;
            //CoreData[CoreDataType.Dispatched] = false;
            //CoreData[CoreDataType.HasNewVersion] = false;
        }
        
    }
}
