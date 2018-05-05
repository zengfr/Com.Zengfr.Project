using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Com.Zfrong.Common.Net;
namespace Com.Zfrong.Common.IO
{
   public class IOHelper
    {
       public static void SaveTextFile(ref string file, ref StringBuilder buffer)
        {
            SaveTextFile(ref file, buffer,true);//
        }
       public static void SaveTextFile(ref  string file, StringBuilder buffer,bool existUpdate)
       {
           SaveTextFile(ref file, buffer.ToString(),existUpdate);//
       }
       public static void SaveTextFile(ref string file,ref string buffer)
       {
           SaveTextFile( ref file, buffer, true);
       }
       public static void SaveTextFile(ref string file, string buffer, bool existUpdate)
       {
           if (!existUpdate)
           {
               if (File.Exists(file))
                   return;
           }
           if (!Directory.Exists(Path.GetDirectoryName(file)))
               Directory.CreateDirectory(Path.GetDirectoryName(file));//zfr
           try
           {
               StreamWriter outStream = new StreamWriter(file, false, System.Text.Encoding.Default);
               outStream.Write(buffer); outStream.Flush();
               outStream.Close(); outStream.Dispose();
           }
           catch
           {
               return;//
           }
       }
       public static void SaveBinaryFile(ref string file, ref string url)
        {
            SaveBinaryFile(ref file, ref  url, true);
        }
        public static void SaveBinaryFile(ref string file, ref string url, bool existUpdate)
        {
            SaveBinaryFile(file,url, existUpdate);//
        }
       public static void SaveBinaryFile(string file,string url, bool existUpdate)
        {
            if (!Directory.Exists(Path.GetDirectoryName(file)))
                Directory.CreateDirectory(Path.GetDirectoryName(file));//zfr
            try
            {
                MyWebRequest request = null;
                request = MyWebRequest.Create(url, request, false);

                MyWebResponse response = request.GetResponse();
                Stream outStream = File.Create(file);
                BinaryWriter writer = new BinaryWriter(outStream);
                byte[] RecvBuffer = new byte[10240];
                int nBytes, nTotalBytes = 0;
                while ((nBytes = response.socket.Receive(RecvBuffer, 0,
                                10240, SocketFlags.None)) > 0)
                {
                    nTotalBytes += nBytes; 
                    writer.Write(RecvBuffer, 0, nBytes);
                    //str+= Encoding.ASCII.GetString(RecvBuffer, 0, nBytes);
                    if (response.KeepAlive && nTotalBytes >= response.ContentLength
                                          && response.ContentLength > 0)
                        break;
                }
                if (response.KeepAlive == false)
                    response.Close();
                writer.Close();
                outStream.Close(); outStream.Dispose();
            }
            catch
            {
                return;//
            }
        }
    }
}
