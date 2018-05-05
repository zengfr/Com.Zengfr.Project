using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
namespace Com.Zfrong.Common.Text
{
    /// <summary>
    /// ×Ö·û´®Ñ¹Ëõ½âÑ¹Àà
    /// </summary>
    public static class StringZipper
    {
        public static string Zip(string tozipstr)
        {
            MemoryStream mStream = new MemoryStream();
            GZipStream gStream = new GZipStream(mStream, CompressionMode.Compress);

            BinaryWriter bw = new BinaryWriter(gStream);
            bw.Write(Encoding.UTF8.GetBytes(tozipstr));
            bw.Close();

            gStream.Close();
            string outs = Convert.ToBase64String(mStream.ToArray());
            mStream.Close();
            return outs;
        }
        public static string UnZip(string zipedstr)
        {
            byte[] data = Convert.FromBase64String(zipedstr);
            MemoryStream mStream = new MemoryStream(data);
            GZipStream gStream = new GZipStream(mStream, CompressionMode.Decompress);
            StreamReader streamR = new StreamReader(gStream);
            string outs = streamR.ReadToEnd();
            mStream.Close();
            gStream.Close();
            streamR.Close();
            return outs;
        }
    }
}