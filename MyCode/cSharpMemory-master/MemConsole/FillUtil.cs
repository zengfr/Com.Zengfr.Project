using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace MemConsole
{
    public class FillUtil
    {
        public class fillArg
        {
            public string file;
            public byte flag = 0x00;
            public int minCount = 0x150;
            public int minKeepSpace = 0xa0;
            public bool flowchange = false;
            public bool isDebug = true;
        }
        private static Random r = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);
        public static void findAndFill(string file)
        {
            findAndFill(file, true);
        }
        public static void findAndFill(string file, bool isDebug)
        {
            var arg = new fillArg();
            arg.file = file;
            arg.isDebug = isDebug;
            findAndFill(arg);
        }
        public static void findAndFill(fillArg arg)
        {
            var file = arg.file;
            if (!File.Exists(file))
            {
                showLog("File is not Exists");
                return;
            }
            if (arg.minCount < 0x20)
            {
                arg.minCount = 0x20;
            }

            var stream2 = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var stream3 = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var stream = new FileStream(file, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            if (stream2.Length < arg.minCount * 2)
            {
                return;
            }
            var bw = new BinaryWriter(stream);
            var br = new BinaryReader(stream2);
            var br2 = new BinaryReader(stream3);
            using (bw)
            {
                using (br2)
                {
                    using (br)
                    {
                        findAndFill(arg, br, bw, br2);
                    }
                }
            }
        }
        public static void findAndFill(fillArg arg, BinaryReader br, BinaryWriter bw, BinaryReader br2)
        {
            var flag = arg.flag;
            var minCount = arg.minCount;

            int counter = 0;
            int startOffset = 0;
            int offset = 0;

            int lastEndOffset = 0;
            showTitle();
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                if (br.ReadByte() != flag)
                {
                    if (counter >= minCount)
                    {
                        var baseMsg = String.Format("|{0:x8}|{1:x8}|{2,10}|{3,8}", startOffset, startOffset + counter, counter.ToString("x"), counter);
                        fill(arg, bw, startOffset, counter, lastEndOffset, br2, baseMsg);
                        lastEndOffset = startOffset + counter;
                    }
                    counter = 0;
                    startOffset = offset;
                }
                else
                {
                    counter++;
                }
                offset++;
            }
            showline();

        }

        private static void fill(fillArg arg, BinaryWriter bw, int startOffset, int counter, int lastEndOffset, BinaryReader br2, string baseMsg)
        {
            var isDebug = arg.isDebug;
            int rndSpace = rnd(arg.minKeepSpace, arg.minKeepSpace + 0x10);
            var rwCounter = counter - rndSpace * 2;
            var wStartOffset = startOffset + rndSpace;
            var rStartOffset = rnd(0, (int)(br2.BaseStream.Length % int.MaxValue) - 8);

            showLog(String.Format("{0,38}|{1:x8}|{2:x8}|{3,8}|{4,8}|{5,8}|",
                baseMsg, wStartOffset, rStartOffset, rwCounter, rndSpace, counter == rwCounter + rndSpace * 2));
            process(bw, br2, wStartOffset, rStartOffset, rwCounter, isDebug);
        }
        private static void process(BinaryWriter bw, BinaryReader br2, int wStartOffset, int rStartOffset, int rwCounter, bool isDebug)
        {
            if (rwCounter>0&&!isDebug)
            {
                bw.Seek(wStartOffset, SeekOrigin.Begin);
                if (br2 != null)
                {
                    br2.BaseStream.Seek(rStartOffset, SeekOrigin.Begin);

                    long byte1;
                    for (int i = 0; i < rwCounter - 8; i += 8)
                    {
                        byte1 = br2.ReadInt64();
                        if (byte1 == 0 || (byte1 >> 2 == 0) || (byte1 << 2 == 0))
                        {
                            var rndrStartOffset = r.Next(0, (int)(br2.BaseStream.Length % int.MaxValue) - 8);
                            br2.BaseStream.Seek(rndrStartOffset, SeekOrigin.Begin);
                            continue;
                        }
                        bw.Write(byte1);
                    }
                }

            }
        }
        private static int rnd(int min, int max)
        {
            return r.Next(min, max);
        }
        private static void showTitle()
        {
            showline();
            string format = string.Format("|{0,8}|{1,8}|{2,10}|{3,8}|{4,8}|{5,8}|{6,8}|{7,8}|{8,8}|", "start", "end", "hexCount", "count"
                , "ws", "rs", "rwCount", "sp", "ok");
            showLog(format);
            showline();
        }
        private static void showline()
        {
            showLog("-".PadLeft(96, '-'));
        }
        private static void showLog(string format, params object[] args)
        {
            Console.WriteLine(String.Format(format, args));
        }
        public static string ObjectToJson(object obj)
        {
            //DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            //serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }
    }
}
