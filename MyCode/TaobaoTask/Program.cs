using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTask;
using ConsoleTask.proj;
using TaobaoTask.KC;
using TaobaoTask.ximalaya;

namespace TaobaoTask
{
    class Program
    {

        public static bool isEnabledShutdown(int sec)
        {
            return false;
        }
        public static void shutdown(int sec) {

            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.StandardInput.WriteLine("shutdown -s -t " + sec);
            process.StandardInput.WriteLine("exit");
             
            process.WaitForExit();
            process.Close();
            
        }
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            sps.Main2(args);
            return;
            ximalayaDownloaderV2 ximalaya2 = new ximalayaDownloaderV2();
            ximalaya2.downloadAlbum("君主论", 14287089, "君主论");
            return;
            a1.gg(); return;
            ImageUtil.Process(args);
            return;
            AssemblyLoader.saveto2();

            if (args == null || args.Length <= 0)
                args = new string[] { "oschinablog" };
            //args = new string[] { "github" };
            log4net.Config.XmlConfigurator.Configure();
            logger.Info("configure");
            //linkedin.relationshipsdiscovery();
            switch (args[0]) {
                case "gitee":
            gitee.SearchProject();break;
                case "github":
                    github.SearchProject(); break;
                case "liepin":
                    liepin.search(); break;
                case "tjg"://统计局
                    regionApp.process(); break;
                case "bli":
                    bilibiliApp.process();break;
                case "oschinablog":
                    oschinablog.getLISTs(); break;
                default:break;
            }
            Console.ReadLine();
            return;


            // test();
            ///return;
            ximalayaDownloaderV2 ximalaya = new ximalayaDownloaderV2();
            ximalaya.downloadAlbum("君主论", 14287089, "君主论");
            ximalaya.downloadAlbum("吉姆罗恩", 16591562, "快乐致富 吉姆·罗恩（全集）");
            ximalaya.downloadAlbum("吉姆罗恩", 21059050, "快乐致富(七大策略）（全）");
            ximalaya.downloadAlbum("富爸爸", 19103685, "爱读书的螺丝钉系列");
      
            return;

            ximalaya.downloadAlbum("NLP", 5623211, "语言的魔力-NLP");
            ximalaya.downloadAlbum("NLP", 3607870, "nlp12条前提假设");
            ximalaya.downloadAlbum("NLP", 17581099, "nlp12条前提假设");
            ximalaya.downloadAlbum("NLP", 18386296, "改变-问题形成和解决的原则[完本]");
            ximalaya.downloadAlbum("人脉", 6128908, "基本人际关系技巧");
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

            //ximalaya.downloadAlbum("识人",15005430, "曾国藩的识人和用人");
            //ximalaya.downloadAlbum("识人",4932387,"【曾国藩】识人用人之道");

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

            /* ximalaya.downloadAlbum("饭局", 4640946, "好饭局是设计出来的");
             ximalaya.downloadAlbum("饭局", 6893636, "饭局攻心术");
             ximalaya.downloadAlbum("饭局", 6734032, "中国饭局里的潜规则");
             ximalaya.downloadAlbum("饭局", 11218304, "饭局对了是就成了");
             ximalaya.downloadAlbum("饭局", 11264488, "中国式饭局人脉学");
             ximalaya.downloadAlbum("饭局", 6809768, "饭局商学院");

            ximalaya.downloadAlbum("人性", 7823176, "扎扎木读《人性论》");
            ximalaya.downloadAlbum("人性", 12377597, "人性的控制.心理控制术MS马尔茨");
            ximalaya.downloadAlbum("人性", 4936420, "了解你我他－－谈人性");
            ximalaya.downloadAlbum("人性", 4154350, "人性【卡耐基】");
            ximalaya.downloadAlbum("创业", 3232138, "如何把小公司做大");*/

            //ximalaya.downloadAlbum("人脉", 4972751, "教你打造超级人脉");
            //ximalaya.downloadAlbum("逻辑", 4934246, "逻辑学导论");
            //ximalaya.downloadAlbum("心理", 17012, "做个明白人 | 清醒思考的艺术");

            /* ximalaya.downloadAlbum("曾仕强", 257010, "曾仕强论三国智慧");
             ximalaya.downloadAlbum("曾仕强", 272617, "曾仕强 - 胡雪岩的启示");
             ximalaya.downloadAlbum("曾仕强", 4173769, "曾仕强-易品三国");
             ximalaya.downloadAlbum("曾仕强", 3791817, "曾仕强-评胡雪岩");
             ximalaya.downloadAlbum("鬼谷子", 4626644, "鬼谷子 中国智慧第一书");
             ximalaya.downloadAlbum("鬼谷子", 290946, "翟杰-话说鬼谷子");
             ximalaya.downloadAlbum("鬼谷子", 5712642, "鬼谷子说服术 - 颜率钩引说服术");

             ximalaya.downloadAlbum("鬼谷子", 7798285, "《天下第一诡术》～鬼谷子智慧");
             ximalaya.downloadAlbum("鬼谷子", 11713812, "大白话鬼谷子");*/

            /* ximalaya.downloadAlbum("心理分析", 4829078, "讲书会《人间游戏：人际关系心理学》");
             ximalaya.downloadAlbum("心理分析", 3643803, "人间游戏——TA沟通分析技术");
             ximalaya.downloadAlbum("心理分析", 16609487, "人间游戏――人际关系心理学");

             ximalaya.downloadAlbum("心理分析", 17007860, "人生脚本：改变命运的心理学");
             ximalaya.downloadAlbum("心理分析", 18373021, "《人生脚本》说完你好，说什么");
             ximalaya.downloadAlbum("心理分析", 6092526, "人生脚本——Eric Berne");*/

            /*ximalaya.downloadAlbum("心理分析", 7215104, "生命的轨迹-深度心理分析手册");
            ximalaya.downloadAlbum("心理分析", 8364738, "生命的轨迹（深度心理分析手册）[完本]");
            ximalaya.downloadAlbum("心理分析", 7877148, "弗里兹·李曼.直面内心的恐惧[完本]");
            ximalaya.downloadAlbum("心理分析", 15890988, "直面内心的恐惧[完本]");*/
            //ximalaya.downloadAlbum("心理分析", 6455234, "心理控制方法");
            //ximalaya.downloadAlbum("心理", 5101522, "影响力心理学");
            //ximalaya.downloadAlbum("识人", 3088334, "冰鉴 中国识人第一书[完本]");
            //ximalaya.downloadAlbum("识人", 15085364, "冰鉴·识人的智慧 赵知易");
            //ximalaya.downloadAlbum("识人", 15848157, "曾国藩《冰鉴》：千古识人第一书");
            // ximalaya.downloadAlbum("PUA", 9707580, "现实世界的诱惑术-六格(完)");
            /*ximalaya.downloadAlbum("说话", 11046863, "说话技巧—怎么说话 增加个人魅力");
            ximalaya.downloadAlbum("说话", 4659714, "跟我学说话");
            ximalaya.downloadAlbum("说话", 3276045, "学会说话，学会交际");
            ximalaya.downloadAlbum("说话", 5715071, "说话艺术全知道");

            ximalaya.downloadAlbum("说话", 12186177, "打动人心的78个说话技巧");
            ximalaya.downloadAlbum("说话", 8120831, "会聊天，懂交际，说话技巧");
            ximalaya.downloadAlbum("说话", 307363, "练说话，练口才");
            ximalaya.downloadAlbum("说话", 6154283, "敢演讲•会说话");
            ximalaya.downloadAlbum("说话", 11851018, "《说话之道》做最会说话的人");*/

            /*ximalaya.downloadAlbum("矛盾实践", 10070929, "《实践论》[完本]");
            ximalaya.downloadAlbum("矛盾实践", 10077601, "《矛盾论》[完本]");
            ximalaya.downloadAlbum("矛盾实践", 11341955, "毛泽东主要著作及重要观点论断");

            ximalaya.downloadAlbum("矛盾实践", 13105312, "《毛泽东的领导智慧》");
            ximalaya.downloadAlbum("矛盾实践", 12063797, "逆境中的毛泽东[完本]");
            ximalaya.downloadAlbum("矛盾实践", 9212861, "毛泽东之谋[全20集]");
            ximalaya.downloadAlbum("矛盾实践", 4742269, "毛泽东风雨沉浮四十年[完本]");
            ximalaya.downloadAlbum("矛盾实践", 12237963, "独领风骚：毛泽东心路解读[完本]");*/
            //ximalaya.downloadAlbum("经理管理", 15813346, "中中层领导力");
            //ximalaya.downloadAlbum("经理管理", 10986445, "40天学会做中层");
            //ximalaya.downloadAlbum("经理管理", 19551180, "中层经理管理技能提升");
            ximalaya.downloadAlbum("矛盾实践", 16120366, "《实践论》《矛盾论》[完本]");
            ximalaya.downloadAlbum("矛盾实践", 4867381, "团队建设，论十大关系");
            ximalaya.downloadAlbum("矛盾实践", 10460106, "论经理人与老板的十大关系");
            ximalaya.downloadAlbum("曾仕强", 16760113, "百家讲坛曾仕强《易经的奥秘》[完本]");
            //ximalaya.downloadAlbum("南怀瑾", 16435882, "南怀瑾易经系传别讲(上下传)[完本]");
            //ximalaya.downloadAlbum("南怀瑾", 15772958, "南怀瑾易经杂说[完本]");
            ximalaya.downloadAlbum("洗脑", 8257791, "洗脑术[完本]");
            ximalaya.downloadAlbum("安东尼罗宾", 3693966, "安东尼罗宾《激发个人潜能Ⅱ》中文版[完本]");
            ximalaya.downloadAlbum("安东尼罗宾", 20117611, "安东尼罗宾-激发个人潜能2驱动力[完本]");
            ximalaya.downloadAlbum("安东尼罗宾", 7571029, "安东尼罗宾掌握优势有声课程中文版[15CD]");

            ximalaya.downloadAlbum("安东尼罗宾", 11661002, "唤醒心中的巨人[完本]");
            ximalaya.downloadAlbum("安东尼罗宾", 6677104, "巨人的脚步--每天安利一点进步");

            ximalaya.downloadAlbum("马克思主义", 16910127, "马克思主义基本原理概论2018版[完本]");
            ximalaya.downloadAlbum("马克思主义", 18590181, "第4版-马克思主义哲学原理[完本]");
            ximalaya.downloadAlbum("马克思主义", 18513284, "第4版-马克思主义政治经济学原理");

            ximalaya.downloadAlbum("642系统", 18617103, "642系统成功的秘密[完本]");

            ximalaya.downloadAlbum("决策", 19089962, "系统思考决策者的系统思考[完本]");
            ximalaya.downloadAlbum("决策", 3721438, "决策与判断");
            ximalaya.downloadAlbum("决策", 5660259, "《决策与判断》精读版");

            ximalaya.downloadAlbum("人格", 2917117, "精神分析诊断-理解人格结构[完本]");
            ximalaya.downloadAlbum("人格", 12472595, "心理疏导-《九型人格》");
            ximalaya.downloadAlbum("人格", 5809723, "九型人格心理学-快速了解自我[完本]");

            return;


            //youzanApp2.MainT(args);
            return;
            // KonsoleSample.MainT(args);
            //return;
            //Craw.Program.Main2(args);
            youzanApp.process();
            //TaobaoKeyword TaobaoKeyword = new TaobaoKeyword();
            //    TaobaoKeyword.Run();
            Console.ReadLine();
        }
        static void test()
        {
            var url = "http://audio.xmcdn.com/group52/M05/79/C3/wKgLcFwbNjOCd_39AYfmrdQaE4A516.m4a";
            var baseUrl = "http://audio.xmcdn.com";
            var restClient2 = RestClientUtil.GetRestClient(baseUrl, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");

            RestClientUtil.DownloadData(restClient2, url.Replace(baseUrl, ""), RestSharp.Method.GET, null,
                "321.m4a", (t, f, c) =>
                {
                    Console.WriteLine(string.Format("{0},{1},{2}", t, f, c));
                }
                );
            Console.ReadLine();
        }
    }
}
