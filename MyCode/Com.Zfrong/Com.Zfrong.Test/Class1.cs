using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace Test
{
      public partial class VCODE 
    {
       // IniFile config = null;

        //查看两个颜色是不是一样，注意这里有一定误差也算相同
        public bool IsSameColor(Color c1, Color c2)
        {
            if (Math.Abs(c1.R - c2.R) < 10
                && Math.Abs(c1.G - c2.G) < 10
                && Math.Abs(c1.B - c2.B) < 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //计算一个Region中，像素的个数
        public int RegionPointCount(Region r,int width,int height)
        { 
            int count = 0;
            for(int h = 0; h<width; ++h)
            {
                for(int v = 0; v< height; ++v)
                {
                    if(r.IsVisible(h,v))
                    {
                        ++count;
                    }
                }
            }

            return count;
        }
        
        //初始化每个字符的Region
        public void InitPictureCharInfo()
        {
            Bitmap bmp = (Bitmap)Bitmap.FromFile("chars.bmp");
            List<BitmapCharInfo> bcil = charRgnList;
            Region rgn = new Region();
            rgn.MakeEmpty();
            if (bmp.Height > 0 && bmp.Width > 0)
            {
                Color bkColor = bmp.GetPixel(0, 0);
                bool bInWorking = false;
                int nNextStartPos = 0;
                for (int h = 0; h < bmp.Width; ++h)
                {
                    bool bFindColor = false;
                    for (int v = 0; v < bmp.Height; ++v)
                    {
                        if (!IsSameColor(bkColor, bmp.GetPixel(h, v)))
                        {
                            rgn.Union(new Rectangle(h, v, 1, 1));
                            bFindColor = true;
                        }
                    }

                    if (bInWorking)
                    {
                        if (!bFindColor)
                        {
                            bInWorking = false;
                            rgn.Translate(-nNextStartPos, 0);
                            BitmapCharInfo bci = new BitmapCharInfo(rgn, h - nNextStartPos, bmp.Height);
                            bci.orgPos = nNextStartPos;
                            bcil.Add(bci);
                            rgn = new Region();
                            rgn.MakeEmpty();
                        }
                    }
                    else
                    {
                        if (bFindColor)
                        {
                            bInWorking = true;
                            nNextStartPos = h;
                        }
                    }
                }

                chars.AddRange("0123456789".ToCharArray());
            }
        }

        //扫描并识别验证码
        public void ScanValidCode()
        {
            Bitmap bmp = this.bmpValidCode;
            List<BitmapCharInfo> bcil = new List<BitmapCharInfo>();
            Region rgn = new Region();
            rgn.MakeEmpty();
            if (bmp.Height > 0 && bmp.Width > 0)
            {
                Color bkColor = bmp.GetPixel(0, 0);
                bool bInWorking = false;
                int nNextStartPos = 0;
                for (int h = 0; h < bmp.Width; ++h)
                {
                    bool bFindColor = false;
                    for (int v = 0; v < bmp.Height; ++v)
                    {
                        if (!IsSameColor(bkColor, bmp.GetPixel(h, v)))
                        {
                            rgn.Union(new Rectangle(h, v, 1, 1));
                            bFindColor = true;
                        }
                    }

                    if (bInWorking)
                    {
                        if (!bFindColor)
                        {
                            bInWorking = false;
                            rgn.Translate(-nNextStartPos, 0);
                            BitmapCharInfo bci = new BitmapCharInfo(rgn, h - nNextStartPos, bmp.Height);
                            bci.orgPos = nNextStartPos;
                            bcil.Add(bci);
                            rgn = new Region();
                            rgn.MakeEmpty();
                        }
                    }
                    else
                    {
                        if (bFindColor)
                        {
                            bInWorking = true;
                            nNextStartPos = h;
                        }
                    }
                }

                List<char> chs = new List<char>();

                Graphics gh = Graphics.FromImage(bmp);
                foreach (BitmapCharInfo bci in bcil)
                {
                    int minPos = -1;
                    int minLng = -1;
                    for (int i = 0; i < charRgnList.Count; ++i)
                    {
                        Region r = bci.rgn.Clone();
                        r.Union(charRgnList[i].rgn);
                        r.Exclude(bci.rgn);

                        int lng = RegionPointCount(r, bci.width, bci.height);

                        if (minLng == -1)
                        {
                            minLng = lng;
                            minPos = i;
                        }
                        else
                        {
                            if (lng < minLng)
                            {
                                minLng = lng;
                                minPos = i;
                            }
                        }


                    }

                    if (minPos != -1)
                    {
                        chs.Add(chars[minPos]);
                    }
                }

                string str = new string(chs.ToArray(), 0, chs.Count);
                //MessageBox.Show(str);
                this.currScanValidCode = str;

            }
        }
        Bitmap bmpValidCode = null;
        List<BitmapCharInfo> charRgnList = new List<BitmapCharInfo>();
        List<char> chars = new List<char>();
        string currScanValidCode;
        //public void GetValidCodePicture(CookieContainer cc, WebProxy wp )
        //{ 
        //    //this.pbValidCode.ImageLocation = this.pbValidCode.ImageLocation;
        //    Exception exp = null;
        //    for (int i = 0; i < 3; ++i)
        //    {
        //        try
        //        {
        //            Stream s = GetDataNonProxy(this.tbValidPicUrl.Text, cc,wp);
        //            Bitmap bmp = (Bitmap)Bitmap.FromStream(s);
        //            s.Close();

        //            bmpValidCode = bmp;

        //            return;
        //        }
        //        catch (Exception ex)
        //        {
        //            exp = ex;
        //        }
        //    }
        
        //    MessageBox.Show("猎取图片失败:" + exp == null?"unkown":exp.Message);
        //}

       
        //private void btnLoadPic_Click(object sender, EventArgs e)
        //{

        //    this.GetValidCodePicture(null,null);
        //    currValidCode = this.currScanValidCode;
        //    this.UpdateDate(false);
        //}

        
        //private void btnInit_Click(object sender, EventArgs e)
        //{
        //    InitPictureCharInfo();
        //}



        //private void btnScanValidCode_Click(object sender, EventArgs e)
        //{
        //    ScanValidCode();
        //    this.currValidCode = this.currScanValidCode;
        //    this.UpdateDate(false);

        //}

        

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    this.UpdateDate(true);
        //    InitPictureCharInfo();
        //    config = new IniFile(Application.ExecutablePath + ".ini");
        //    lastPwdPosInDict = config.GetInt("Process", "PwdPostion", 0);
        //    InitPwdDict();
        //    UpdateProcessText();
        //}

    }

    public class BitmapCharInfo 
    {
        public BitmapCharInfo()
        {
            this.rgn = new Region();
            this.rgn.MakeEmpty();
        }
        public BitmapCharInfo(Region r, int w, int h)
        {
            this.rgn = r;
            this.width = w;
            this.height = h;
        }
        public Region rgn;
        public int width = 0;
        public int height = 0;
        public int orgPos = 0;

    }
}
