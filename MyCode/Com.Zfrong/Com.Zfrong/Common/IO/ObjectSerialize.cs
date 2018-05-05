using System;
using System.Collections.Generic;
using System.Text;
using System.IO;//
namespace Com.Zfrong.Common.IO
{
    public class ObjectSerialize
    {
        #region#对象序列化与保存、读取1
        private static byte[] SerializeObject(object pObj)
        {
            MemoryStream stream = new MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter xs = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();//
            //XmlSerializer xs = new XmlSerializer(pObj.GetType());
            xs.Serialize(stream, pObj);
            stream.Position = 0;
            byte[] read = new byte[stream.Length];
            stream.Read(read, 0, read.Length);
            stream.Close();
          
            return read;
        }
        private static T DeserializeObject<T>(Stream stream) where T : class
        {
            T _newOjb = null;
            stream.Position = 0;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter xs = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();//
            //XmlSerializer xs = new XmlSerializer(typeof(T));
            _newOjb = xs.Deserialize(stream) as T;
            stream.Close();
            return _newOjb;
        }

        public static T Deserialize<T>(string file) where T : class
        {
                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                
                return DeserializeObject<T>(fs);//
        }
        public static void Serialize<T>(T pObj, string file) where T : class
        {
            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
                 byte[] data = SerializeObject(pObj);
                fs.Write(data, 0, data.Length);//
                fs.Close(); fs.Dispose();//
            
        }
        #endregion
    }
}
