using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Zengfr.Proj.Common
{
    public class IOSerialization
    {

        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {
                    ;
                }
            }
            using (Stream stream =new FileStream(filePath, append ? FileMode.Append : FileMode.Create,FileAccess.Write,FileShare.Read))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }


        public static T ReadFromBinaryFile<T>(string filePath)
        {
            if (File.Exists(filePath))
                using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return (T)binaryFormatter.Deserialize(stream);
                }
        
               return default(T);

        }
}
}