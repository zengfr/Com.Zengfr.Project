using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UniTask_UI.p1
{
    public class MyStoreUtil
    {
        public static void save<T>(String file, T obj)
        {
            using (FileStream stream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
            }
        }
        public static T load<T>(String file)
        {
            T obj = default(T);
            if (File.Exists(file)) { 
            using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = (T)formatter.Deserialize(stream);
            } }
            return obj ;
        }
    }
}
