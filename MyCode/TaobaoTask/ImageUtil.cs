using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;

namespace ConsoleTask
{
    public class ImageUtil
    {

       public static void Process(string[] args)
        {
            var fs = Directory.EnumerateFiles("E:\\javaproject\\conuniframework\\conuniframework-packages\\conuniframework-core-parent\\conuniframework-pcany\\conuniframework-pcany-trans\\conuniframework-pcany-trans-client\\src\\main\\resources\\tk","*.xml");

            foreach (var f in fs)
            {
                if (f.Contains("logback")) {
                    continue;
                }
                XmlDocument doc = new XmlDocument();
               
                doc.Load(f); 
         
                 var nodes = doc.SelectNodes("//*[@bounds]");
                String img = f.Replace("xml", "jpg").Replace("_dh","");
                string imgFName = Path.GetFileNameWithoutExtension(img);
                String dir =   Path.GetDirectoryName(img) + "\\" +imgFName;
                
                var image = Image.FromStream(new FileStream(img, FileMode.Open));
                foreach (XmlNode node in nodes)
                {
                    String cls = node.Attributes["class"].Value;
                    String clickable = node.Attributes["clickable"].Value;
                    String longclickable = node.Attributes["long-clickable"].Value;
                    String bounds = node.Attributes["bounds"].Value;
                    String[] bb = bounds.Replace("[", "").Replace("]", ",").Split(',');
                    int x1; 
                    int y1;
                    int x2;
                    int y2;
                    int.TryParse(bb[0],out x1);
                    int.TryParse(bb[1], out y1);
                    int.TryParse(bb[2], out x2);
                    int.TryParse(bb[3], out y2);
                    int w = x2 - x1;
                    int h = y2 - y1;


                    string tFile = dir + "\\"+bounds+ clickable+longclickable+".jpg";
                    Console.WriteLine(bounds);
                    Console.WriteLine(tFile);
                    if (tFile.Contains("true")|| cls.Contains("UIImage")||cls.Contains("UISvg"))
                    {
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        MakeThumbnailImage(image, tFile, w, h, w, h, x1, y1);
                    }

                }
                
            }
            Console.ReadLine();
        }

        /// <summary>
        /// 裁剪图片并保存
        /// </summary>
        /// <param name="Image">图片信息</param>
        /// <param name="maxWidth">缩略图宽度</param>
        /// <param name="maxHeight">缩略图高度</param>
        /// <param name="cropWidth">裁剪宽度</param>
        /// <param name="cropHeight">裁剪高度</param>
        /// <param name="X">X轴</param>
        /// <param name="Y">Y轴</param>
        public static Bitmap MakeThumbnailImage(Image originalImage,String outFile, int maxWidth, int maxHeight, int cropWidth, int cropHeight, int X, int Y)
        {
            Bitmap b = new Bitmap(cropWidth, cropHeight);
            try
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    //清空画布并以透明背景色填充
                    g.Clear(Color.Transparent);
                    //在指定位置并且按指定大小绘制原图片的指定部分
                    g.DrawImage(originalImage, new Rectangle(0, 0, cropWidth, cropHeight), X, Y, cropWidth, cropHeight, GraphicsUnit.Pixel);
                    Image displayImage = new Bitmap(b, maxWidth, maxHeight);

                   // Image displayImage = b.Clone(new Rectangle(0, 0, maxWidth, maxHeight), PixelFormat.Format8bppIndexed);

                    displayImage.Save(outFile, System.Drawing.Imaging.ImageFormat.Bmp);
                    Bitmap bit = new Bitmap(b, maxWidth, maxHeight);
                    return bit;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                //originalImage.Dispose();
                b.Dispose();
            }
        }
    }

}
