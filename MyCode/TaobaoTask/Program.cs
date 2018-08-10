using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaobaoTask.KC;
using TaobaoTask.ximalaya;

namespace TaobaoTask
{
    class Program
    {
        static void Main(string[] args)
        {
            ximalayaDownloader ximalaya = new ximalayaDownloader();
            //ximalaya.downloadAlbum(6005962, "卡耐基 沟通的艺术-人际交往智慧");
            //ximalaya.downloadAlbum(6481070, "卡耐基 人性的优点—克服忧虑");
            //ximalaya.downloadAlbum(7629850, "卡耐基 语言的突破 -有效沟通，成功演讲宝典（完本）");
            //ximalaya.downloadAlbum(11501536, "蔡康永的201堂情商课");
            //ximalaya.downloadAlbum(6219183, "冷读术 石真语");
            //ximalaya.downloadAlbum(268665, "冷读术——瞬间赢得他人信任的终极社交技巧");
            //ximalaya.downloadAlbum(4309644, "有限与无限的游戏");
            //ximalaya.downloadAlbum(12134364, "权力论 罗素著");
            //ximalaya.downloadAlbum(8750163, "领导力21法则");
            //ximalaya.downloadAlbum(6474188, "解读领导力21法则");
            //ximalaya.downloadAlbum(9605515, "长篇解读《孙子兵法》");
            //ximalaya.downloadAlbum(6244506, "《孙子兵法》解读");

            //ximalaya.downloadAlbum(5166290, "金字塔原理语音解读版");
            //ximalaya.downloadAlbum(11082589, "《金字塔原理》精读＋解读");

            //ximalaya.downloadAlbum(6332064, "（完本）《洞察人性》作者：奥地利――阿德勒");
            //ximalaya.downloadAlbum(5662517,"【全】伯恩《人生脚本》");

            //ximalaya.downloadAlbum(2685546, "边干边学做老板：一个小公司老板的日常管理-黄中强");
            //ximalaya.downloadAlbum(12149808, "创业老板必备企业法公司法常识");
            //ximalaya.downloadAlbum(15774619, "如何创业当老板[完本]");

            //ximalaya.downloadAlbum(6060723, "做生意的艺术");
            //ximalaya.downloadAlbum(4777296, "金钱论《金钱真相》");
            // ximalaya.downloadAlbum(5005493, "财富在金钱之外");

            //ximalaya.downloadAlbum(6784161, "富爸爸投资指南[完本]");
            //ximalaya.downloadAlbum(10541949, "《富爸爸不公平的优势》[完本]");
            //ximalaya.downloadAlbum(7298741, "《富爸爸：财务自由之路》[完本]");
            //ximalaya.downloadAlbum(7029695, "富爸爸-如何创办自己的公司[完本]");
            //ximalaya.downloadAlbum(3696496, "富爸爸穷爸爸辞职创业前的十堂课》[完本]");
            //ximalaya.downloadAlbum(8454790, "富爸爸点石成金（罗伯特•清崎;唐纳德•特朗普）[完本]");

            //ximalaya.downloadAlbum(5119189, "《赚钱如此简单快速》--克亚营销理论");

            //ximalaya.downloadAlbum(15005430, "曾国藩的识人和用人");
            //ximalaya.downloadAlbum(4932387,"【曾国藩】识人用人之道");

            //ximalaya.downloadAlbum(410667, "穷爸爸富爸爸之选择富有[完本]");
            //ximalaya.downloadAlbum(9221001, "富爸爸 不公平的优势"); 未下载 重复
            //ximalaya.downloadAlbum(6933780, "耶格系统培训课程---领导力课程");
            //ximalaya.downloadAlbum(7591066, "《提高你的财商》");
            //ximalaya.downloadAlbum(10011183, "《致富需要做的6件事》");

            /*ximalaya.downloadAlbum("权力", 3573708, "(完)《权力》[完本]");
            ximalaya.downloadAlbum("权力", 11380051, "权力的48条法则");
            ximalaya.downloadAlbum("权力", 12134364, "权力论 罗素著[完本]");
            ximalaya.downloadAlbum("权力", 4889423, "破解权力方程");
            ximalaya.downloadAlbum("权力", 13511744, "权力：为什么只为某些人所拥有[完本]");*/
            //return;


            youzanApp2.MainT(args);
            return;
            // KonsoleSample.MainT(args);
            //return;
            //Craw.Program.Main2(args);
            youzanApp.process();
            //TaobaoKeyword TaobaoKeyword = new TaobaoKeyword();
            //    TaobaoKeyword.Run();
            Console.ReadLine();
        }
    }
}
