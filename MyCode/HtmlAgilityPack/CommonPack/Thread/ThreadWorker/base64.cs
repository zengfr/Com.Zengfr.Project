using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.IO.Compression;
namespace spider_demo
{  //  < * ? > : "  /\ | ���ܽ�����windows�ļ�����Ŀ¼
    class Base64_my
        {
        public string Base64Code(string Message)
        {
            StringBuilder tmp = new StringBuilder();
            byte[] asciiBytes = Encoding.ASCII.GetBytes("http://");
            tmp.Append("[");
            for (int i = 0; i <= asciiBytes.Length-1; i++)
            {   
                tmp.Append(asciiBytes[i].ToString());
            }
            tmp .Append("]");
            Message = Message.Replace("http://", tmp.ToString());
            
            tmp.Remove(0, tmp.Length);
            asciiBytes = Encoding.ASCII.GetBytes("\\");
            tmp.Append("[");
            for (int i = 0; i <= asciiBytes.Length - 1; i++)
            {
                tmp.Append(asciiBytes[i].ToString());
            }
            tmp.Append("]");
            Message = Message.Replace("\\", tmp.ToString());

            tmp.Remove(0, tmp.Length);
            asciiBytes = Encoding.ASCII.GetBytes("/");
            tmp.Append("[");
            for (int i = 0; i <= asciiBytes.Length - 1; i++)
            {
                tmp.Append(asciiBytes[i].ToString());
            }
            tmp.Append("]");
            Message = Message.Replace("/", tmp.ToString());

            tmp.Remove(0, tmp.Length);
            asciiBytes = Encoding.ASCII.GetBytes("?");
            tmp.Append("[");
            for (int i = 0; i <= asciiBytes.Length - 1; i++)
            {
                tmp.Append(asciiBytes[i].ToString());
            }
            tmp.Append("]");
            Message = Message.Replace("?", tmp.ToString());

            tmp.Remove(0, tmp.Length);
            asciiBytes = Encoding.ASCII.GetBytes("*");
            tmp.Append("[");
            for (int i = 0; i <= asciiBytes.Length - 1; i++)
            {
                tmp.Append(asciiBytes[i].ToString());
            }
            tmp.Append("]");
            Message = Message.Replace("*", tmp.ToString());

            tmp.Remove(0, tmp.Length);
            asciiBytes = Encoding.ASCII.GetBytes("+");
            tmp.Append("[");
            for (int i = 0; i <= asciiBytes.Length - 1; i++)
            {
                tmp.Append(asciiBytes[i].ToString());
            }
            tmp.Append("]");
            Message = Message.Replace("+", tmp.ToString());

            tmp.Remove(0, tmp.Length);
            asciiBytes = Encoding.ASCII.GetBytes("|");
            tmp.Append("[");
            for (int i = 0; i <= asciiBytes.Length - 1; i++)
            {
                tmp.Append(asciiBytes[i].ToString());
            }
            tmp.Append("]");
            Message = Message.Replace("|", tmp.ToString());

            tmp.Remove(0, tmp.Length);
            asciiBytes = Encoding.ASCII.GetBytes(":");
            tmp.Append("[");
            for (int i = 0; i <= asciiBytes.Length - 1; i++)
            {
                tmp.Append(asciiBytes[i].ToString());
            }
            tmp.Append("]");
            Message = Message.Replace(":", tmp.ToString());

            tmp.Remove(0, tmp.Length);
            asciiBytes = Encoding.ASCII.GetBytes("<");
            tmp.Append("[");
            for (int i = 0; i <= asciiBytes.Length - 1; i++)
            {
                tmp.Append(asciiBytes[i].ToString());
            }
            tmp.Append("]");
            Message = Message.Replace("<", tmp.ToString());

            tmp.Remove(0, tmp.Length);
            asciiBytes = Encoding.ASCII.GetBytes(">");
            tmp.Append("[");
            for (int i = 0; i <= asciiBytes.Length - 1; i++)
            {
                tmp.Append(asciiBytes[i].ToString());
            }
            tmp.Append("]");
            Message = Message.Replace(">", tmp.ToString());

            tmp.Remove(0, tmp.Length);
            asciiBytes = Encoding.ASCII.GetBytes("\"");
            tmp.Append("[");
            for (int i = 0; i <= asciiBytes.Length - 1; i++)
            {
                tmp.Append(asciiBytes[i].ToString());
            }
            tmp.Append("]");
            Message = Message.Replace("\"", tmp.ToString());

            return Message;

        }
         //   public string Base64Code(string Message)
         //   {
         //       //Message=ZipString(Message);
         //       char[] Base64Code = new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T',
         //'U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n',
         //'o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
         //'8','9','='};
         //       byte empty = (byte)0;
         //       System.Collections.ArrayList byteMessage = new System.Collections.ArrayList(System.Text.Encoding.Default.GetBytes(Message));
         //       System.Text.StringBuilder outmessage;
         //       int messageLen = byteMessage.Count;
         //       int page = messageLen / 3;
         //       int use = 0;
         //       if ((use = messageLen % 3) > 0)
         //       {
         //           for (int i = 0; i < 3 - use; i++)
         //               byteMessage.Add(empty);
         //           page++;
         //       }
         //       outmessage = new System.Text.StringBuilder(page * 4);
         //       for (int i = 0; i < page; i++)
         //       {
         //           byte[] instr = new byte[3];
         //           instr[0] = (byte)byteMessage[i * 3];
         //           instr[1] = (byte)byteMessage[i * 3 + 1];
         //           instr[2] = (byte)byteMessage[i * 3 + 2];
         //           int[] outstr = new int[4];
         //           outstr[0] = instr[0] >> 2;
         //           outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
         //           if (!instr[1].Equals(empty))
         //               outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
         //           else
         //               outstr[2] = 61;
         //           if (!instr[2].Equals(empty))
         //               outstr[3] = (instr[2] & 0x3f);
         //           else
         //               outstr[3] = 61;
         //           outmessage.Append(Base64Code[outstr[0]]);
         //           outmessage.Append(Base64Code[outstr[1]]);
         //          //outmessage.Append(Base64Code[outstr[2]]);
         //          //outmessage.Append(Base64Code[outstr[3]]);
         //       }
         //       return outmessage.ToString();
         //       //return ZipString(outmessage.ToString());
         //   }


         //   public string Base64Decode(string Message)
         //   {
         //       if ((Message.Length % 4) != 0)
         //       {
         //           throw new ArgumentException("������ȷ��BASE64���룬���顣", "Message");
         //       }
         //       if (!System.Text.RegularExpressions.Regex.IsMatch(Message, "^[A-Z0-9/+=]*$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
         //       {
         //           throw new ArgumentException("��������ȷ��BASE64���룬���顣", "Message");
         //       }
         //       string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
         //       int page = Message.Length / 4;
         //       System.Collections.ArrayList outMessage = new System.Collections.ArrayList(page * 3);
         //       char[] message = Message.ToCharArray();
         //       for (int i = 0; i < page; i++)
         //       {
         //           byte[] instr = new byte[4];
         //           instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
         //           instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
         //           instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
         //           instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
         //           byte[] outstr = new byte[3];
         //           outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
         //           if (instr[2] != 61)
         //           {
         //               outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
         //           }
         //           else
         //           {
         //               outstr[2] = 0;
         //           }
         //           if (instr[3] != 62)
         //           {
         //               outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
         //           }
         //           else
         //           {
         //               outstr[2] = 0;
         //           }
         //           outMessage.Add(outstr[0]);
         //           if (outstr[1] != 0)
         //               outMessage.Add(outstr[1]);
         //           if (outstr[2] != 0)
         //               outMessage.Add(outstr[2]);
         //       }
         //       byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
         //       return System.Text.Encoding.Default.GetString(outbyte);
         //   }
            //public static string ZipString(string unCompressedString)
            //{

            //    byte[] bytData = System.Text.Encoding.UTF8.GetBytes(unCompressedString);
            //    MemoryStream ms = new MemoryStream();
            //    Stream s = new GZipStream(ms, CompressionMode.Compress);
            //    s.Write(bytData, 0, bytData.Length);
            //    s.Close();
            //    byte[] compressedData = (byte[])ms.ToArray();
            //    return System.Convert.ToBase64String(compressedData, 0, compressedData.Length);
            //}
            //public static string UnzipString(string unCompressedString)
            //{
            //    System.Text.StringBuilder uncompressedString = new System.Text.StringBuilder();
            //    byte[] writeData = new byte[4096];

            //    byte[] bytData = System.Convert.FromBase64String(unCompressedString);
            //    int totalLength = 0;
            //    int size = 0;

            //    Stream s = new GZipStream(new MemoryStream(bytData), CompressionMode.Decompress);
            //    while (true)
            //    {
            //        size = s.Read(writeData, 0, writeData.Length);
            //        if (size > 0)
            //        {
            //            totalLength += size;
            //            uncompressedString.Append(System.Text.Encoding.UTF8.GetString(writeData, 0, size));
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //    s.Close();
            //    return uncompressedString.ToString();
            //}
        }

}
