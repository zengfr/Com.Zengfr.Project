using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.IO.Compression;
namespace Com.Zfrong.Common.Configuration
{
    /// <summary>
    /// 设置保存和提取类
    /// </summary>
    public class ConfigX
    {
        #region 字段
        /// <summary>
        /// 默认编码
        /// </summary>
        public static Encoding Encoding = Encoding.UTF8;
        #endregion
        #region 保存
        public static void SaveDictionaryEnumerator(string filePath, IDictionaryEnumerator idenum, bool isCompress, bool isAppend)
        {
            FileStream fs; GZipStream zip; StreamWriter sw;
            BuildStreamWriter(filePath, isCompress, out sw, out  zip, out fs, isAppend);
            while (idenum.MoveNext())
            {
                sw.WriteLine(idenum.Key.ToString());
                sw.WriteLine(idenum.Value.ToString());
            }
            sw.Flush();//
            sw.Dispose();
            sw.Close();
            if (zip != null) zip.Close();
            if (fs != null) fs.Close();//
        }
        public static void SaveEnumerator(string filePath, IEnumerator idenum, bool isCompress, bool isAppend)
        {
            FileStream fs; GZipStream zip; StreamWriter sw;
            BuildStreamWriter(filePath, isCompress, out sw, out  zip, out fs, isAppend);
            while (idenum.MoveNext())
            {
                sw.WriteLine(idenum.Current.ToString());
            }
            sw.Flush();//
            sw.Dispose();
            sw.Close();
            if (zip != null) zip.Close();
            if (fs != null) fs.Close();//
        }
        public static void SaveBitArray(string filePath, BitArray array, bool isCompress, bool isAppend)
        {
            FileStream fs; GZipStream zip; BinaryWriter sw;
            BuildBinaryWriter(filePath, isCompress, out sw, out  zip, out fs, isAppend);
            for (int i = 0; i < array.Length; i++)
            {
                sw.Write(array.Get(i));
            }
            sw.Flush();//
            sw.Close();
            if (zip != null) zip.Close();
            if (fs != null) fs.Close();//
        }
        #endregion
        #region 提取
        #region Queue
        public static void LoadQueue(string filePath, bool isDecompress, Queue<string> list)
        {

            FileStream fs; GZipStream zip; StreamReader sr;
            BuildStreamReader(filePath, isDecompress, out sr, out  zip, out fs);
            string str = sr.ReadLine();
            while (str != null && str.Length!= 0)
            {
                try
                {
                    list.Enqueue(str);
                }
                catch { }
                str = sr.ReadLine();
            }
            sr.Dispose();
            sr.Close();
            if (zip != null) zip.Close();
            if (fs != null) fs.Close();//
        }
        #endregion
        #region List
        public static void LoadList(string filePath, bool isDecompress,IList<string> list)
        {
            FileStream fs; GZipStream zip; StreamReader sr;
            BuildStreamReader(filePath, isDecompress, out sr, out  zip, out fs);
            string str = sr.ReadLine();

            while (str != null && str.Length!= 0)
            {
               try
                {
                    list.Add(str);
                }
                catch { }
                str = sr.ReadLine();
            }
            sr.Dispose();
            sr.Close();
            if (zip != null) zip.Close();
            if (fs != null) fs.Close();//
        }
        public static void LoadList(string filePath, IList<string> list)
        {
            FileStream fs; GZipStream zip; StreamReader sr;
            BuildStreamReader(filePath, true, out sr, out  zip, out fs);
            while (!sr.EndOfStream)
            {
                list.Add(sr.ReadLine());
            }
            sr.Dispose();
            sr.Close();
            if (zip != null) zip.Close();
            if (fs != null) fs.Close();//
        }
        #endregion
        public static void LoadBitArray(BitArray items, string filePath, bool isDecompress)
        {
            FileStream fs; GZipStream zip; BinaryReader sr;
            BuildBinaryReader(filePath, isDecompress, out sr, out  zip, out fs);
            for (int i = 0; i < items.Length; i++)
            {
                if (sr.PeekChar() != -1)
                    items.Set(i, sr.ReadBoolean());
            }
            sr.Close();
            if (zip != null) zip.Close();
            if (fs != null) fs.Close();//
        }
        #endregion
        #region 私有方法
        private static void BuildStreamWriter(string filePath, bool isCompress, out StreamWriter sw, out GZipStream zip, out FileStream fs, bool isAppend)
        {
            if (isCompress)
            {
                FileInfo f = new FileInfo(filePath);
                if (!Directory.Exists(f.DirectoryName))
                    Directory.CreateDirectory(f.DirectoryName);//
                if (isAppend)
                    fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                else
                    fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                zip = new GZipStream(fs, CompressionMode.Compress);
                sw = new StreamWriter(zip, Encoding);
            }
            else
            {
                fs = null; zip = null;
                sw = new StreamWriter(filePath, false, Encoding);
            }
        }
        private static void BuildStreamReader(string filePath, bool isDecompress, out StreamReader sr, out GZipStream zip, out FileStream fs)
        {
            FileInfo f = new FileInfo(filePath);
            if (!Directory.Exists(f.DirectoryName))
                Directory.CreateDirectory(f.DirectoryName);//
            fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

            if (isDecompress)
            {
                zip = new GZipStream(fs, CompressionMode.Decompress);
                sr = new StreamReader(zip, Encoding);
            }
            else
            {

                // sr = new StreamReader(filePath, Encoding);
                sr = new StreamReader(fs, Encoding); fs = null; zip = null;
            }
        }
        private static void BuildBinaryWriter(string filePath, bool isCompress, out BinaryWriter sw, out GZipStream zip, out FileStream fs, bool isAppend)
        {
            FileInfo f = new FileInfo(filePath);
            if (!Directory.Exists(f.DirectoryName))
                Directory.CreateDirectory(f.DirectoryName);//
            if (isAppend)
                fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            else
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            if (isCompress)
            {
                zip = new GZipStream(fs, CompressionMode.Compress);
                sw = new BinaryWriter(zip, Encoding);
            }
            else
            {
                fs = null; zip = null;
                sw = new BinaryWriter(fs, Encoding);
            }
        }
        private static void BuildBinaryReader(string filePath, bool isDecompress, out BinaryReader sr, out GZipStream zip, out FileStream fs)
        {
            FileInfo f = new FileInfo(filePath);
            if (!Directory.Exists(f.DirectoryName))
                Directory.CreateDirectory(f.DirectoryName);//
            fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

            if (isDecompress)
            {
                zip = new GZipStream(fs, CompressionMode.Decompress);
                sr = new BinaryReader(zip, Encoding);
            }
            else
            {
                fs = null; zip = null;
                sr = new BinaryReader(fs, Encoding);
            }
        }
        #endregion
    }
}