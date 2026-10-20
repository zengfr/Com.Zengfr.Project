using Cn.Jj.Service.Msg.Protocol;
using Extensions.Data;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CNJJ
{
    internal class PakeageUtil
    {
        public static void showHash(string s)
        {
            var h = fastHash(s);
            Console.WriteLine("s:{0},{1}", s,"".PadRight(88,'-'));
            Console.WriteLine("hash:{0},{1},{2}", h, h >> 1, h >> 2);
            Console.WriteLine("hash:{0:x},{1:x},{2:x}", h, h >> 1, h >> 2);
        }
            public static void tset()
        {
            var hash = "pb/TKMobileLordUnion.pb";
            showHash(hash);
            showHash(hash.Substring(1));
            hash = "pb/TKMobile.pb";
            showHash(hash);
            showHash(hash.Substring(1));

            hash = "pb/TKLord.pb";
            showHash(hash);
            showHash(hash.Substring(1));

            var ss = new List<String>();

            TKMobileReqMsg req = new TKMobileReqMsg();
           // req.LordReqMsg = new LordReqMsg();
           // req.LordReqMsg.LordtakeoutcardReqMsg = new  LordTakeoutCardReq (); ;
            //req.LordReqMsg.LordtakeoutcardReqMsg.Cards =   ByteString.FromBase64( "Jx==");
            //req.LordReqMsg.LordtakeoutcardReqMsg.Seat = 3333;
            //req.Param = 123456;

       

            //序列化获取二进制数据
            byte[] bs; String bss;
              bs = req.ToByteArray();
            Console.WriteLine("ToString:" + req.ToString());
              bss = byteToHexStr(req.ToByteArray());
            Console.WriteLine("ToByteString:" + bss);


            TKMobileReqMsg req2 = new TKMobileReqMsg();
            req2.MergeFrom(hexStrTobyte(bss));

            bs = req2.ToByteArray();
            Console.WriteLine("ToString:" + req2.ToString());
            bss = byteToHexStr(req2.ToByteArray());
            Console.WriteLine("ToByteString:" + bss);


            ss.Add("01480100670000004a657263200112587b226d73676964223a313638333632373737322c22706172616d73223a7b227061636b6167656964223a327d2c22636c617373223a22646973706c61795f6d6f64756c65222c226d6574686f64223a22676574736574227d0a036c75611802");
            ss.Add("01000000080000001206fa0503088101");
            ss.Add("01000000070000001205fa0502087f");
            ss.Add("014801002a0000007228c2012508ead6e299041089f6031a04080110021a04080210011a05080510e9071a06080710b0ea01");
            ss.Add("014801007d0000004a7b72790a036c756118e00830012001126b7b22636c617373223a226d7967616d65222c22706172616d73223a7b2265787072657373696f6e566572223a302c22756964223a313132373738373337307d2c226d6574686f64223a2265787072657373696f6e222c226d73676964223a2231363833353134323632227d");

            ss.Add("000000001f00000008be851882011808f2da86e7013a1008ead6e2990410bd051a0508bd051000");
            ss.Add("01480100270000001a2508d6da86e701ba031c080110011a167b226e70223a323030302c226f70223a313030307d00");
            ss.Add("e903000014000000221208d6da86e7012a0a0800100118ead6e29904");
            ss.Add("e903000037000000223508d6da86e70122250800100118ffffffffffffffffff01200130003a02271a4000480f50006a06312e30303030b801f2aca5818031");
            ss.Add("e90300002c000000222a08d6da86e701221a080010021801200030003a004000481050006a06302e30303030b801dbc3a5818031");
            ss.Add("e903000037000000223508d6da86e70122250802100018ffffffffffffffffff01200130003a02202d4000481150006a06312e30303030b801a1cda5818031");
            ss.Add("e903000036000000223408d6da86e70122240800100118ffffffffffffffffff01200130003a01234000481550006a06312e30303030b801ddbba6818031");
            ss.Add("e9030000220000001a2008d6da86e701fa021708a0f1f6fb0210001887d0dd2f200028ead6e299043000");
            ss.Add("e903000037000000223508d6da86e70122250801100218ffffffffffffffffff01200130003a0230094000480450006a06312e30303030b8019dfca2818031");
            ss.Add("e90300002c000000222a08d6da86e701221a080110001802200030003a004000480550006a06302e30303030b801bf89a3818031");
            ss.Add("e903000037000000223508d6da86e70122250800100118ffffffffffffffffff01200130003a02240a4000480650006a06312e30303030b8019599a3818031");
            ss.Add("e903000014000000221208d6da86e7012a0a0800100018ead6e29904");
            ss.Add("e90300002d00000008c3c2c9a3061a2508d6da86e70182061c080110011a167b226e70223a323030302c226f70223a313030307d00");
            //---------------------------------
            ss.Add("e9030000310000001a2f08d6da86e7011a2710d6da86e7011a13e69697e59cb0e4b8bb2de696b0e6898be59cba40c09a0c48e6c4e9a2066000");
            ss.Add("e90300006400000008a8e8d3101a5d08d6da86e70192015408021800220028849d0132483c54697020436c6173733d2231222049643d22323031303022205478743d22e6aca2e8bf8ee8bf9be585a5e2809ce69697e59cb0e4b8bb2de696b0e6898be59cbae2809d222f3e0a");
            ss.Add("e9030000320000001a3008d6da86e7019a0127080010001a00220fe4b8b4e697b653746174654e616d652a0ee4b8b4e697b6426f75744e616d65");
            ss.Add("e90300005a0500001ad70a08d6da86e7018203cd0a0aca0a3c7461736b733e0a093c7461736b206e616d653d2263767461736b22207469703d22e8bf9ee8839ce5a596e58ab1223e0a09093c74617267657420636f756e743d2231223e0a0909093c6d6f6e65792069643d2232222076616c75653d223522207469703d2235e98791e5b88122206d6561737572653d2222206e616d653d22e98791e5b881222f3e0a09093c2f7461726765743e0a09093c74617267657420636f756e743d2232223e0a0909093c6d6f6e65792069643d2232222076616c75653d22313022207469703d223130e98791e5b88122206d6561737572653d2222206e616d653d22e98791e5b881222f3e0a0909093c67726f772069643d223630303632323038222076616c75653d223122207469703d223122206d6561737572653d2222206e616d653d22222f3e0a0909093c67726f772069643d223630303632313733222076616c75653d223122207469703d223122206d6561737572653d2222206e616d653d22222f3e0a09093c2f7461726765743e0a09093c74617267657420636f756e743d2233223e0a0909093c6d6f6e65792069643d2232222076616c75653d22313522207469703d223135e98791e5b88122206d6561737572653d2222206e616d653d22e98791e5b881222f3e0a0909093c67726f772069643d223630303632313734222076616c75653d223122207469703d223122206d6561737572653d2222206e616d653d22222f3e0a09093c2f7461726765743e0a09093c74617267657420636f756e743d2234223e0a0909093c6d6f6e65792069643d2232222076616c75653d22323022207469703d223230e98791e5b88122206d6561737572653d2222206e616d653d22e98791e5b881222f3e0a0909093c67726f772069643d223630303632323130222076616c75653d223122207469703d223122206d6561737572653d2222206e616d653d22222f3e0a09093c2f7461726765743e0a09093c74617267657420636f756e743d2235223e0a0909093c6d6f6e65792069643d2232222076616c75653d22353022207469703d223530e98791e5b88122206d6561737572653d2222206e616d653d22e98791e5b881222f3e0a09093c2f7461726765743e0a09093c74617267657420636f756e743d2236223e0a0909093c6d6f6e65792069643d2232222076616c75653d2231303022207469703d22313030e98791e5b88122206d6561737572653d2222206e616d653d22e98791e5b881222f3e0a09093c2f7461726765743e0a09093c74617267657420636f756e743d2237223e0a0909093c6d6f6e65792069643d2232222076616c75653d2231353022207469703d22313530e98791e5b88122206d6561737572653d2222206e616d653d22e98791e5b881222f3e0a09093c2f7461726765743e0a09093c74617267657420636f756e743d2238223e0a0909093c6d6f6e65792069643d2232222076616c75653d2232303022207469703d22323030e98791e5b88122206d6561737572653d2222206e616d653d22e98791e5b881222f3e0a09093c2f7461726765743e0a09093c74617267657420636f756e743d2239223e0a0909093c6d6f6e65792069643d2232222076616c75653d2233303022207469703d22333030e98791e5b88122206d6561737572653d2222206e616d653d22e98791e5b881222f3e0a09093c2f7461726765743e0a09093c74617267657420636f"
                + "756e743d223130223e0a0909093c6d6f6e65792069643d2232222076616c75653d2235303022207469703d22353030e98791e5b88122206d6561737572653d2222206e616d653d22e98791e5b881222f3e0a09093c2f7461726765743e0a093c2f7461736b3e0a3c2f7461736b733e0a");
            ss.Add("e90300002e0000001a2c08d6da86e7019a02230a213c47616d65436f756e7441776172643e3c2f47616d65436f756e7441776172643e");
            ss.Add("e9030000290000001a2708d6da86e701ca031e0a04086410640a0608c80110c8010a0608ac0210ac020a0608f40310f403");
            ss.Add("000000003300000008be851882012c08d6da86e70122240a0508bf0510000a0508c00510000a0608c10510c8010a0508c20510000a050892061001");
            ss.Add("000000001600000008b8fd8b2282010e08d6da86e7011a0608e7c4e9a206");
            ss.Add("000000000c000000120ada0107089d83cf1c1014"); 
            ss.Add("000000000c000000120ada0107089f83cf1c1000");
            ss.Add("000000000c000000120ada0107089e83cf1c1014");
            ss.Add("000000000c000000120ada010708a083cf1c1000");
            ss.Add("e9030000100000001a0e08d6da86e7012206100118002003");
            ss.Add("e9030000200000001a1e08d6da86e701d2011520053a0042004a0de6af8f31e5b180e8bdaee8bdac");
            ss.Add("e9030000530000001a5108d6da86e701a203480a46312c31352c32302c312c352c372c312c312c312c302c2c2c2c2c2c2c2c2c2c2c2c302c2c352c312c322c322c322c322c322c312c2c2c2c352c332c302c2c312c2c2c2c2c2c2c");
            ss.Add("e9030000320000001a3008d6da86e701ba01270a25080110ead6e299041a0ce59da6e5858be58f8ae6a88a208702280030c80138014000488702");
            ss.Add("e9030000350000001a3308d6da86e701ba012a0a2808001082b2989a041a0fe59097e698afe882a5e882a5e586ac208d02280030d50538014000488d02");
            ss.Add("e9030000130000001a1108d6da86e701a2020810904e18c8012800");
            ss.Add("e9030000310000001a2f08d6da86e701ba01260a24080210d6cacba7011a0ce88d98e6b8a3e691a9e4baba2082022800300f38014000488202");
            ss.Add("e9030000110000001a0f08d6da86e701da0106080010011814");
            ss.Add("e90300007000000008cd9bb7ee0c1a6808d6da86e701da035f0819125b3c616374696f6e2069643d32352064656c61793d22322220756964303d333531343632373432206376303d3020756964313d31313237373837333730206376313d3020756964323d31313238363637333934206376323d30202f3e");
            ss.Add("e9030000fc00000008cd9bb7ee0c1af30108d6da86e701c201e9010ae6013c526f756e642069643d2233373834342220747970653d22312220426f757449443d223122205461626c6549443d2230222047616d6549443d22312220415052503d2230223e3c6c206e616d653d22e7a7afe58886e696b9e5bc8f222076616c75653d22e7a7afe58886c397e59fbae695b0222f3e3c6c206e616d653d22e6b8b8e6888fe59fbae695b0222076616c75653d2235222f3e3c6c206e616d653d22e5b180e588b6e5908de7a7b0222076616c75653d22222f3e3c6c206e616d653d224e6f7465222076616c75653d22e6af8f31e5b180e8bdaee8bdac222f3e3c2f526f756e643e");
            ss.Add("e90300000b0000001a0908d6da86e701ca0100");
            ss.Add("e90300001a000000221808d6da86e7019a010708bddfc1888031b801bddfc1888031");

            ss.Add("e90300000b0000001a0908d6da86e701da0200");
            ss.Add("000000002f00000082012c08d6da86e70122240a0508c00510010a0608c10510aa010a0508c20510010a0508bf0510000a050892061001");
            ss.Add("e9030000500000001a4e08d6da86e70182024508d80410041a04086410641a0608c80110c8011a0608ac0210ac021a0608f40310f40320aa0128aa01300138004000480050904e5800600068e2ffffffffffffffff017003");
            ss.Add("000000003600000008be851882012f08d6da86e70132270a2308ead6e29904120608bc0510aa01120508bd0510001a0ce59da6e5858be58f8ae6a88a2007");
            ss.Add("000000001f00000008be851882011808d6da86e7013a1008ead6e2990410bd051a0508bd051000");
            ss.Add("014801001400000082011108d6da86e701120908ead6e2990410bd05");
            System.Type t = typeof(PakeageUtil);
            var mt = typeof(IMessage); var rqt = typeof(TKMobileReqMsg);
            var types = t.Assembly.GetTypes().Where(t=> mt.IsAssignableFrom(t))
                .Where(t=>  true||t.Name.StartsWith("TKMobile")|| t.Name.Contains("Http") || t.Name.Contains("Common") || t.Name.Contains("Tcp")
                 || t.Name.Contains("Udp") || t.Name.Contains("Socket")).ToList();
            types = t.Assembly.GetTypes().Where(t => rqt.IsAssignableFrom(t))
                .Where(t =>   t.Name.StartsWith("TKMobile")  ).ToList();
            foreach (var s in ss)
            {
                Console.WriteLine("{0}{1}",""+types.Count, "".PadLeft(88, '-'));
                Console.WriteLine("l:{0:x2} {1:x2} s:{2}", s.Length/2, s.Length/2-8, s, "");
                foreach (var type in types)
                {
                        var obj = Activator.CreateInstance(type);
                        var  rtn= showMsg(obj as IMessage, s);
                    // if (rtn > 0) 
                    // break;

                    //if (req2.LordtakeoutcardReqMsg != null)
                    //{
                    //    var cards = req2.LordtakeoutcardReqMsg.Cards;
                    //    parseCards(cards);
                    //}
                    //if (req2.LordShowcardsReqMsg != null && req2.LordShowcardsReqMsg.Cards.Count > 0)
                    //{
                    //    var cards2 = req2.LordShowcardsReqMsg.Cards.First();
                    //    parseCards(cards2);
                    //}
                }
            }
 
        }
        private static void parseCards(ByteString cards)
        {
            Console.WriteLine("cards:{0}", cards.ToBase64());
            foreach (var card in cards)
            {
                var i = Convert.ToInt32(card);
                var c = i / 13;
                var p = c > 3 ? i % 13 + 0 : i % 13 + 3;
                var cc = parseCardColor(c);
                var pp = parseCardPoint(p);
                Console.WriteLine("card:{0}\t{1}{2}", i, cc, pp);
            }
        }

        private static String parseCardColor(int c)
        {
            switch (c)
            {
                case 0:
                    return "方";
                case 1:
                    return "梅";
                case 2:
                    return "红";
                case 3:
                    return "黑";
                case 4:
                    return "王";
            }
            return "X";
        }
        private static String parseCardPoint(int p)
        {
            if (p < 10)
                return "" + p;
            switch (p)
            {
                case 10:
                    return "10";
                case 11:
                    return "J";
                case 12:
                    return "Q";
                case 13:
                    return "K";
                case 14:
                    return "A";
                case 15:
                    return "2";
            }
            return "X";
        }
         
        
        private static int showMsg (IMessage  msg, String ss)  
        { 
            var bytes = hexStrTobyte(ss.Substring(0x10));
          return  showMsg(msg, bytes);

        }
        private static int showMsg (IMessage  msg, byte[] bytes)  

        {
            try
            {
                msg.MergeFrom(bytes);//msg.MergeDelimitedFrom
                if(msg.ToString().Length>5)
                Console.WriteLine("msg:{0,-32} {1}", msg.GetType().Name, msg.ToString());
                return msg.ToString().Length - 5;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("err:{0} {1}", msg.GetType(), ex.Message );
            }
            return 0;
        }
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        public static byte[] hexStrTobyte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }
        public static uint fastHash(string str) {

            byte[] input = Encoding.ASCII.GetBytes(str);     // the data to be hashed

            uint result = XXHash.XXH32(input);
            return result;
        }
    }
}
