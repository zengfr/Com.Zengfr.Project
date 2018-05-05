using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Math = System.Math;
//��װ�����ʹ�úܼ򵥣���Բ�ͬ����֤�룬��Ӧ�̳��޸�ĳЩ���������ɼ򵥼������Ϳ���ʵ��ͼƬʶ���ˣ�
           // GrayByPixels(); //�Ҷȴ���
           // GetPicValidByValue(128, 4); //�õ���Ч�ռ�
           // Bitmap[] pics = GetSplitPics(4, 1);     //�ָ�
           // string code = GetSingleBmpCode(pics[i], 128);   //�õ����봮
namespace Com.Zfrong.Common.Drawing//BallotAiying2
{
   public class UnCodeBase
    {
        public Bitmap bmpobj;
        public UnCodeBase(Bitmap pic)
        {
            //       if (pic.PixelFormat == PixelFormat.Format8bppIndexed)
            bmpobj = new Bitmap(pic);    //ת��ΪFormat32bppRgb
        }
         /**//// <summary>
        /// ͼ����������
        /// </summary>
        /// <param name="b">ԭʼͼ</param>
        /// <param name="degree">����[-255, 255]</param>
        /// <returns></returns>
        public static Bitmap KiLighten(Bitmap b, int degree)
        {
            if (b == null)
            {
                return null;
            }

            if (degree < -255) degree = -255;
            if (degree > 255) degree = 255;

            try
            {

                int width = b.Width;
                int height = b.Height;

                int pix = 0;

                BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* p = (byte*)data.Scan0;
                    int offset = data.Stride - width * 3;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // ����ָ��λ�����ص�����
                            for (int i = 0; i < 3; i++)
                            {
                                pix = p[i] + degree;

                                if (degree < 0) p[i] = (byte)System.Math.Max(0, pix);
                                if (degree > 0) p[i] = (byte)System.Math.Min(255, pix);

                            } // i
                            p += 3;
                        } // x
                        p += offset;
                    } // y
                }

                b.UnlockBits(data);

                return b;
            }
            catch
            {
                return null;
            }

        } // end of Lighten
/// <summary>
        /// ͼ��Աȶȵ���
        /// </summary>
        /// <param name="b">ԭʼͼ</param>
        /// <param name="degree">�Աȶ�[-100, 100]</param>
        /// <returns></returns>
        public static Bitmap KiContrast(Bitmap b, int degree)
        {
            if (b == null)
            {
                return null;
            }

            if (degree < -100) degree = -100;
            if (degree > 100) degree = 100;

            try
            {

                double pixel = 0;
                double contrast = (100.0 + degree) / 100.0;
                contrast *= contrast;
                int width = b.Width;
                int height = b.Height;
                BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                unsafe
                {
                    byte* p = (byte*)data.Scan0;
                    int offset = data.Stride - width * 3;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // ����ָ��λ�����صĶԱȶ�
                            for (int i = 0; i < 3; i++)
                            {
                                pixel = ((p[i] / 255.0 - 0.5) * contrast + 0.5) * 255;
                                if (pixel < 0) pixel = 0;
                                if (pixel > 255) pixel = 255;
                                p[i] = (byte)pixel;
                            } // i
                            p += 3;
                        } // x
                        p += offset;
                    } // y
                }
                b.UnlockBits(data);
                return b;
            }
            catch
            {
                return null;
            }
        } // end of Contrast
/// <summary>
        /// ����Ƕ���ת
        /// </summary>
        /// <param name="bmp">ԭʼͼBitmap</param>
        /// <param name="angle">��ת�Ƕ�</param>
        /// <param name="bkColor">����ɫ</param>
        /// <returns>���Bitmap</returns>
        public static Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 2;
            int h = bmp.Height + 2;

            PixelFormat pf;

            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();

            tmp.Dispose();

            return dst;
        }

         //// <summary>
        /// ɫ�ʵ���
        /// </summary>
        /// <param name="bmp">ԭʼͼ</param>
        /// <param name="rVal">r����</param>
        /// <param name="gVal">g����</param>
        /// <param name="bVal">b����</param>
        /// <returns>������ͼ</returns>
        public static Bitmap KiColorBalance(Bitmap bmp, int rVal, int gVal, int bVal)
        {

            if (bmp == null)
            {
                return null;
            }


            int h = bmp.Height;
            int w = bmp.Width;

            try
            {
                if (rVal > 255 || rVal < -255 || gVal > 255 || gVal < -255 || bVal > 255 || bVal < -255)
                {
                    return null;
                }

                BitmapData srcData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* p = (byte*)srcData.Scan0.ToPointer();

                    int nOffset = srcData.Stride - w * 3;
                    int r, g, b;

                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {

                            b = p[0] + bVal;
                            if (bVal >= 0)
                            {
                                if (b > 255) b = 255;
                            }
                            else
                            {
                                if (b < 0) b = 0;
                            }

                            g = p[1] + gVal;
                            if (gVal >= 0)
                            {
                                if (g > 255) g = 255;
                            }
                            else
                            {
                                if (g < 0) g = 0;
                            }

                            r = p[2] + rVal;
                            if (rVal >= 0)
                            {
                                if (r > 255) r = 255;
                            }
                            else
                            {
                                if (r < 0) r = 0;
                            }

                            p[0] = (byte)b;
                            p[1] = (byte)g;
                            p[2] = (byte)r;

                            p += 3;
                        }

                        p += nOffset;


                    }
                } // end of unsafe

                bmp.UnlockBits(srcData);

                return bmp;
            }
            catch
            {
                return null;
            }

        } // end of color
        //// <summary>
        /// �ữ
        /// <param name="b">ԭʼͼ</param>
        /// <returns>���ͼ</returns>
        public static Bitmap KiBlur(Bitmap b)
        {

            if (b == null)
            {
                return null;
            }

            int w = b.Width;
            int h = b.Height;

            try
            {

                Bitmap bmpRtn = new Bitmap(w, h, PixelFormat.Format24bppRgb);

                BitmapData srcData = b.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                BitmapData dstData = bmpRtn.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* pIn = (byte*)srcData.Scan0.ToPointer();
                    byte* pOut = (byte*)dstData.Scan0.ToPointer();
                    int stride = srcData.Stride;
                    byte* p;

                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            //ȡ��Χ9���ֵ
                            if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
                            {
                                //����
                                pOut[0] = pIn[0];
                                pOut[1] = pIn[1];
                                pOut[2] = pIn[2];
                            }
                            else
                            {
                                int r1, r2, r3, r4, r5, r6, r7, r8, r9;
                                int g1, g2, g3, g4, g5, g6, g7, g8, g9;
                                int b1, b2, b3, b4, b5, b6, b7, b8, b9;

                                float vR, vG, vB;

                                //����
                                p = pIn - stride - 3;
                                r1 = p[2];
                                g1 = p[1];
                                b1 = p[0];

                                //����
                                p = pIn - stride;
                                r2 = p[2];
                                g2 = p[1];
                                b2 = p[0];

                                //����
                                p = pIn - stride + 3;
                                r3 = p[2];
                                g3 = p[1];
                                b3 = p[0];

                                //���
                                p = pIn - 3;
                                r4 = p[2];
                                g4 = p[1];
                                b4 = p[0];

                                //�Ҳ�
                                p = pIn + 3;
                                r5 = p[2];
                                g5 = p[1];
                                b5 = p[0];

                                //����
                                p = pIn + stride - 3;
                                r6 = p[2];
                                g6 = p[1];
                                b6 = p[0];

                                //����
                                p = pIn + stride;
                                r7 = p[2];
                                g7 = p[1];
                                b7 = p[0];

                                //����
                                p = pIn + stride + 3;
                                r8 = p[2];
                                g8 = p[1];
                                b8 = p[0];

                                //�Լ�
                                p = pIn;
                                r9 = p[2];
                                g9 = p[1];
                                b9 = p[0];

                                vR = (float)(r1 + r2 + r3 + r4 + r5 + r6 + r7 + r8 + r9);
                                vG = (float)(g1 + g2 + g3 + g4 + g5 + g6 + g7 + g8 + g9);
                                vB = (float)(b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8 + b9);

                                vR /= 9;
                                vG /= 9;
                                vB /= 9;

                                pOut[0] = (byte)vB;
                                pOut[1] = (byte)vG;
                                pOut[2] = (byte)vR;

                            }

                            pIn += 3;
                            pOut += 3;
                        }// end of x

                        pIn += srcData.Stride - w * 3;
                        pOut += srcData.Stride - w * 3;
                    } // end of y
                }

                b.UnlockBits(srcData);
                bmpRtn.UnlockBits(dstData);

                return bmpRtn;
            }
            catch
            {
                return null;
            }

        } // end of KiBlur
///<summary>
/// ��
/// </summary>
/// <param name="b">ԭʼBitmap</param>
/// <param name="val">�񻯳̶ȡ�ȡֵ[0,1]��ֵԽ���񻯳̶�Խ��</param>
/// <returns>�񻯺��ͼ��</returns>
public static Bitmap KiSharpen(Bitmap b, float val)
{
    if (b == null)
    {
 return null;
    }

    int w = b.Width;
    int h = b.Height;

    try
    {

 Bitmap bmpRtn = new Bitmap(w, h, PixelFormat.Format24bppRgb);

 BitmapData srcData = b.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
 BitmapData dstData = bmpRtn.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

 unsafe
 {
     byte* pIn = (byte*)srcData.Scan0.ToPointer();
     byte* pOut = (byte*)dstData.Scan0.ToPointer();
     int stride = srcData.Stride;
     byte* p;

     for (int y = 0; y < h; y++)
     {
  for (int x = 0; x < w; x++)
  {
      //ȡ��Χ9���ֵ��λ�ڱ�Ե�ϵĵ㲻���ı䡣
      if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
      {
   //����
   pOut[0] = pIn[0];
   pOut[1] = pIn[1];
   pOut[2] = pIn[2];
      }
      else
      {
   int r1, r2, r3, r4, r5, r6, r7, r8, r0;
   int g1, g2, g3, g4, g5, g6, g7, g8, g0;
   int b1, b2, b3, b4, b5, b6, b7, b8, b0;

   float vR, vG, vB;

   //����
   p = pIn - stride - 3;
   r1 = p[2];
   g1 = p[1];
   b1 = p[0];

   //����
   p = pIn - stride;
   r2 = p[2];
   g2 = p[1];
   b2 = p[0];

   //����
   p = pIn - stride + 3;
   r3 = p[2];
   g3 = p[1];
   b3 = p[0];

   //���
   p = pIn - 3;
   r4 = p[2];
   g4 = p[1];
   b4 = p[0];

   //�Ҳ�
   p = pIn + 3;
   r5 = p[2];
   g5 = p[1];
   b5 = p[0];

   //����
   p = pIn + stride - 3;
   r6 = p[2];
   g6 = p[1];
   b6 = p[0];

   //����
   p = pIn + stride;
   r7 = p[2];
   g7 = p[1];
   b7 = p[0];

   //����
   p = pIn + stride + 3;
   r8 = p[2];
   g8 = p[1];
   b8 = p[0];

   //�Լ�
   p = pIn;
   r0 = p[2];
   g0 = p[1];
   b0 = p[0];

   vR = (float)r0 - (float)(r1 + r2 + r3 + r4 + r5 + r6 + r7 + r8) / 8;
   vG = (float)g0 - (float)(g1 + g2 + g3 + g4 + g5 + g6 + g7 + g8) / 8;
   vB = (float)b0 - (float)(b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8) / 8;

   vR = r0 + vR * val ;
   vG = g0 + vG * val;
   vB = b0 + vB * val;

   if (vR > 0)
   {
       vR = System.Math.Min(255, vR);
   }
   else
   {
       vR = System.Math.Max(0, vR);
   }

   if (vG > 0)
   {
       vG = System.Math.Min(255, vG);
   }
   else
   {
       vG = System.Math.Max(0, vG);
   }

   if (vB > 0)
   {
       vB = System.Math.Min(255, vB);
   }
   else
   {
       vB = System.Math.Max(0, vB);
   }

   pOut[0] = (byte)vB;
   pOut[1] = (byte)vG;
   pOut[2] = (byte)vR;

      }

      pIn += 3;
      pOut += 3;
  }// end of x

  pIn += srcData.Stride - w * 3;
  pOut += srcData.Stride - w * 3;
     } // end of y
 }

 b.UnlockBits(srcData);
 bmpRtn.UnlockBits(dstData);

 return bmpRtn;
    }
    catch
    {
 return null;
    }

} // end of KiSharpen
        /// <summary>
��������/// GammaУ��
��������/// </summary>
��������/// <param name="bmp">����Bitmap</param>
������/// <param name="val">[0 <-��- 1 -��-> 2]</param>
��������/// <returns>���Bitmap</returns>
������public static Bitmap KiGamma(Bitmap bmp, float val)
��������{
������������if (bmp == null)
������������{
����������������return null;
������������}

������������// 1��ʾ�ޱ仯���Ͳ���
������������if (val == 1.0000f) return bmp;

������������try
����������{
���������������� Bitmap b = new Bitmap(bmp.Width, bmp.Height);
����������������Graphics g = Graphics.FromImage(b);
����������������ImageAttributes attr = new ImageAttributes();

����������������attr.SetGamma(val, ColorAdjustType.Bitmap);
����������������g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attr);
����������������g.Dispose();
����������������return b;
������������}
������������catch
������������{
����������������return null;
������������}
��������}
        /// <summary>
        /// ����RGB������Ҷ�ֵ
        /// </summary>
        /// <param name="posClr">Colorֵ</param>
        /// <returns>�Ҷ�ֵ������</returns>
        private int GetGrayNumColor(System.Drawing.Color posClr)
        {
            return (posClr.R * 19595 + posClr.G * 38469 + posClr.B * 7472) >> 16;
        }

        /// <summary>
        /// �Ҷ�ת��,��㷽ʽ
        /// </summary>
        public void GrayByPixels()
        {
            for (int i = 0; i < bmpobj.Height; i++)
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    int tmpValue = GetGrayNumColor(bmpobj.GetPixel(j, i));
                    bmpobj.SetPixel(j, i, Color.FromArgb(tmpValue, tmpValue, tmpValue));
                }
            }
        }

        /// <summary>
        /// ȥͼ�α߿�
        /// </summary>
        /// <param name="borderWidth"></param>
        public void ClearPicBorder(int borderWidth)
        {
            for (int i = 0; i < bmpobj.Height; i++)
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    if (i < borderWidth || j < borderWidth || j > bmpobj.Width - 1 - borderWidth || i > bmpobj.Height - 1 - borderWidth)
                        bmpobj.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }
            }
        }

        /// <summary>
        /// �Ҷ�ת��,���з�ʽ
        /// </summary>
        public void GrayByLine()
        {
            Rectangle rec = new Rectangle(0, 0, bmpobj.Width, bmpobj.Height);
            BitmapData bmpData = bmpobj.LockBits(rec, ImageLockMode.ReadWrite, bmpobj.PixelFormat);// PixelFormat.Format32bppPArgb);
            //    bmpData.PixelFormat = PixelFormat.Format24bppRgb;
            IntPtr scan0 = bmpData.Scan0;
            int len = bmpobj.Width * bmpobj.Height;
            int[] pixels = new int[len];
            Marshal.Copy(scan0, pixels, 0, len);

            //��ͼƬ���д���
            int GrayValue = 0;
            for (int i = 0; i < len; i++)
            {
                GrayValue = GetGrayNumColor(Color.FromArgb(pixels[i]));
                pixels[i] = (byte)(Color.FromArgb(GrayValue, GrayValue, GrayValue)).ToArgb();      //Colorתbyte
            }

            bmpobj.UnlockBits(bmpData);

            ////���
            //GCHandle gch = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            //bmpOutput = new Bitmap(bmpobj.Width, bmpobj.Height, bmpData.Stride, bmpData.PixelFormat, gch.AddrOfPinnedObject());
            //gch.Free();
        }

        /// <summary>
        /// �õ���Чͼ�β�����Ϊ��ƽ���ָ�Ĵ�С
        /// </summary>
        /// <param name="dgGrayValue">�Ҷȱ����ֽ�ֵ</param>
        /// <param name="CharsCount">��Ч�ַ���</param>
        /// <returns></returns>
        public void GetPicValidByValue(int dgGrayValue, int CharsCount)
        {
            int posx1 = bmpobj.Width; int posy1 = bmpobj.Height;
            int posx2 = 0; int posy2 = 0;
            for (int i = 0; i < bmpobj.Height; i++)      //����Ч��
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    int pixelValue = bmpobj.GetPixel(j, i).R;
                    if (pixelValue < dgGrayValue)     //���ݻҶ�ֵ
                    {
                        if (posx1 > j) posx1 = j;
                        if (posy1 > i) posy1 = i;

                        if (posx2 < j) posx2 = j;
                        if (posy2 < i) posy2 = i;
                    };
                };
            };
            // ȷ��������
            int Span = CharsCount - (posx2 - posx1 + 1) % CharsCount;   //�������Ĳ����
            if (Span < CharsCount)
            {
                int leftSpan = Span / 2;    //���䵽��ߵĿ��� ����spanΪ����,���ұ߱���ߴ�1
                if (posx1 > leftSpan)
                    posx1 = posx1 - leftSpan;
                if (posx2 + Span - leftSpan < bmpobj.Width)
                    posx2 = posx2 + Span - leftSpan;
            }
            //������ͼ
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            bmpobj = bmpobj.Clone(cloneRect, bmpobj.PixelFormat);
        }
        
        /// <summary>
        /// �õ���Чͼ��,ͼ��Ϊ�����
        /// </summary>
        /// <param name="dgGrayValue">�Ҷȱ����ֽ�ֵ</param>
        /// <param name="CharsCount">��Ч�ַ���</param>
        /// <returns></returns>
        public void GetPicValidByValue(int dgGrayValue)
        {
            int posx1 = bmpobj.Width; int posy1 = bmpobj.Height;
            int posx2 = 0; int posy2 = 0;
            for (int i = 0; i < bmpobj.Height; i++)      //����Ч��
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    int pixelValue = bmpobj.GetPixel(j, i).R;
                    if (pixelValue < dgGrayValue)     //���ݻҶ�ֵ
                    {
                        if (posx1 > j) posx1 = j;
                        if (posy1 > i) posy1 = i;

                        if (posx2 < j) posx2 = j;
                        if (posy2 < i) posy2 = i;
                    };
                };
            };
            //������ͼ
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            bmpobj = bmpobj.Clone(cloneRect, bmpobj.PixelFormat);
        }

        /// <summary>
        /// �õ���Чͼ��,ͼ�������洫��
        /// </summary>
        /// <param name="dgGrayValue">�Ҷȱ����ֽ�ֵ</param>
        /// <param name="CharsCount">��Ч�ַ���</param>
        /// <returns></returns>
        public Bitmap GetPicValidByValue(Bitmap singlepic, int dgGrayValue)
        {
            int posx1 = singlepic.Width; int posy1 = singlepic.Height;
            int posx2 = 0; int posy2 = 0;
            for (int i = 0; i < singlepic.Height; i++)      //����Ч��
            {
                for (int j = 0; j < singlepic.Width; j++)
                {
                    int pixelValue = singlepic.GetPixel(j, i).R;
                    if (pixelValue < dgGrayValue)     //���ݻҶ�ֵ
                    {
                        if (posx1 > j) posx1 = j;
                        if (posy1 > i) posy1 = i;

                        if (posx2 < j) posx2 = j;
                        if (posy2 < i) posy2 = i;
                    };
                };
            };
            //������ͼ
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            return singlepic.Clone(cloneRect, singlepic.PixelFormat);
        }
        
        /// <summary>
        /// ƽ���ָ�ͼƬ
        /// </summary>
        /// <param name="RowNum">ˮƽ�Ϸָ���</param>
        /// <param name="ColNum">��ֱ�Ϸָ���</param>
        /// <returns>�ָ�õ�ͼƬ����</returns>
        public Bitmap [] GetSplitPics(int RowNum,int ColNum)
        {
            if (RowNum == 0 || ColNum == 0)
                return null;
            int singW = bmpobj.Width / RowNum;
            int singH = bmpobj.Height / ColNum;
            Bitmap [] PicArray=new Bitmap[RowNum*ColNum];

            Rectangle cloneRect;
            for (int i = 0; i < ColNum; i++)      //����Ч��
            {
                for (int j = 0; j < RowNum; j++)
                {
                    cloneRect = new Rectangle(j*singW, i*singH, singW , singH);
                    PicArray[i*RowNum+j]=bmpobj.Clone(cloneRect, bmpobj.PixelFormat);//����С��ͼ
                }
            }
            return PicArray;
        }

        /// <summary>
        /// ���ػҶ�ͼƬ�ĵ��������ִ���1��ʾ�ҵ㣬0��ʾ����
        /// </summary>
        /// <param name="singlepic">�Ҷ�ͼ</param>
        /// <param name="dgGrayValue">��ǰ����ɫ����</param>
        /// <returns></returns>
        public string GetSingleBmpCode(Bitmap singlepic, int dgGrayValue)
        {
            Color piexl;
            string code = "";
            for (int posy = 0; posy < singlepic.Height; posy++)
                for (int posx = 0; posx < singlepic.Width; posx++)
                {
                    piexl = singlepic.GetPixel(posx, posy);
                    if (piexl.R < dgGrayValue)    // Color.Black )
                        code = code + "1";
                    else
                        code = code + "0";
                }
            return code;
        }
 /// <summary>
        /// �õ��Ҷ�ͼ��ǰ���������ٽ�ֵ �����䷽���yuanbao,2007.08
        /// </summary>
        /// <returns>ǰ���������ٽ�ֵ</returns>
        public int GetDgGrayValue()
        {
            int[] pixelNum = new int[256];           //ͼ��ֱ��ͼ����256����
            int n, n1, n2;
            int total;                              //totalΪ�ܺͣ��ۼ�ֵ
            double m1, m2, sum, csum, fmax, sb;     //sbΪ��䷽�fmax�洢��󷽲�ֵ
            int k, t, q;
            int threshValue = 1;                      // ��ֵ
            int step = 1;
            //����ֱ��ͼ
            for (int i =0; i < bmpobj.Width ; i++)
            {
                for (int j = 0; j < bmpobj.Height; j++)
                {
                    //���ظ��������ɫ����RGB��ʾ
                    pixelNum[bmpobj.GetPixel(i,j).R]++;            //��Ӧ��ֱ��ͼ��1
                }
            }
            //ֱ��ͼƽ����
            for (k = 0; k <= 255; k++)
            {
                total = 0;
                for (t = -2; t <= 2; t++)              //�븽��2���Ҷ���ƽ������tֵӦȡ��С��ֵ
                {
                    q = k + t;
                    if (q < 0)                     //Խ�紦��
                        q = 0;
                    if (q > 255)
                        q = 255;
                    total = total + pixelNum[q];    //totalΪ�ܺͣ��ۼ�ֵ
                }
                pixelNum[k] = (int)((float)total / 5.0 + 0.5);    //ƽ���������2��+�м�1��+�ұ�2���Ҷȣ���5���������ܺͳ���5�������0.5��������ֵ
            }
            //����ֵ
            sum = csum = 0.0;
            n = 0;
            //�����ܵ�ͼ��ĵ����������أ�Ϊ����ļ�����׼��
            for (k = 0; k <= 255; k++)
            {
                sum += (double)k * (double)pixelNum[k];     //x*f(x)�����أ�Ҳ����ÿ���Ҷȵ�ֵ�������������һ����Ϊ���ʣ���sumΪ���ܺ�
                n += pixelNum[k];                       //nΪͼ���ܵĵ�������һ��������ۻ�����
            }

            fmax = -1.0;                          //��䷽��sb������Ϊ��������fmax��ʼֵΪ-1��Ӱ�����Ľ���
            n1 = 0;
            for (k = 0; k < 256; k++)                  //��ÿ���Ҷȣ���0��255������һ�ηָ�����䷽��sb
            {
                n1 += pixelNum[k];                //n1Ϊ�ڵ�ǰ��ֵ��ǰ��ͼ��ĵ���
                if (n1 == 0) { continue; }            //û�зֳ�ǰ����
                n2 = n - n1;                        //n2Ϊ����ͼ��ĵ���
                if (n2 == 0) { break; }               //n2Ϊ0��ʾȫ�����Ǻ�ͼ����n1=0������ƣ�֮��ı���������ʹǰ���������ӣ����Դ�ʱ�����˳�ѭ��
                csum += (double)k * pixelNum[k];    //ǰ���ġ��Ҷȵ�ֵ*����������ܺ�
                m1 = csum / n1;                     //m1Ϊǰ����ƽ���Ҷ�
                m2 = (sum - csum) / n2;               //m2Ϊ������ƽ���Ҷ�
                sb = (double)n1 * (double)n2 * (m1 - m2) * (m1 - m2);   //sbΪ��䷽��
                if (sb > fmax)                  //����������䷽�����ǰһ���������䷽��
                {
                    fmax = sb;                    //fmaxʼ��Ϊ�����䷽�otsu��
                    threshValue = k;              //ȡ�����䷽��ʱ��Ӧ�ĻҶȵ�k���������ֵ
                }
            }
            return threshValue;
        }
 /// <summary>
        ///  ȥ���ӵ㣨�ʺ��ӵ�/���ߴ�Ϊ1��
        /// </summary>
        /// <param name="dgGrayValue">��ǰ����ɫ����</param>
        /// <returns></returns>
        public void ClearNoise(int dgGrayValue, int MaxNearPoints)
        {
            Color piexl;
            int nearDots = 0;
            int XSpan, YSpan, tmpX, tmpY;
            //����ж�
            for (int i = 0; i < bmpobj.Width; i++)
                for (int j = 0; j < bmpobj.Height; j++)
                {
                    piexl = bmpobj.GetPixel(i, j);
                    if (piexl.R < dgGrayValue)
                    {
                        nearDots = 0;
                        //�ж���Χ8�����Ƿ�ȫΪ��
                        if (i == 0 || i == bmpobj.Width - 1 || j == 0 || j == bmpobj.Height - 1)  //�߿�ȫȥ��
                        {
                            bmpobj.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            if (bmpobj.GetPixel(i - 1, j - 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i, j - 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i + 1, j - 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i - 1, j).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i + 1, j).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i - 1, j + 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i, j + 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i + 1, j + 1).R < dgGrayValue) nearDots++;
                        }

                        if (nearDots < MaxNearPoints)
                            bmpobj.SetPixel(i, j, Color.FromArgb(255, 255, 255));   //ȥ������ && ��ϸС3�ڱߵ�
                    }
                    else  //����
                        bmpobj.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
        }/// <summary>
        /// 3��3��ֵ�˲����ӣ�yuanbao,2007.10
        /// </summary>
        /// <param name="dgGrayValue"></param>
        public void ClearNoise(int dgGrayValue)
        {
            int x, y;
            byte[] p = new byte[9]; //��С������3*3
            byte s;
            //byte[] lpTemp=new BYTE[nByteWidth*nHeight];
            int i, j;

            //--!!!!!!!!!!!!!!���濪ʼ����Ϊ3��3��ֵ�˲�!!!!!!!!!!!!!!!!
            for (y = 1; y < bmpobj.Height - 1; y++) //--��һ�к����һ���޷�ȡ����
            {
                for (x = 1; x < bmpobj.Width - 1; x++)
                {
                    //ȡ9�����ֵ
                    p[0] = bmpobj.GetPixel(x - 1, y - 1).R;
                    p[1] = bmpobj.GetPixel(x, y - 1).R;
                    p[2] = bmpobj.GetPixel(x + 1, y - 1).R;
                    p[3] = bmpobj.GetPixel(x - 1, y).R;
                    p[4] = bmpobj.GetPixel(x, y).R;
                    p[5] = bmpobj.GetPixel(x + 1, y).R;
                    p[6] = bmpobj.GetPixel(x - 1, y + 1).R;
                    p[7] = bmpobj.GetPixel(x, y + 1).R;
                    p[8] = bmpobj.GetPixel(x + 1, y + 1).R;
                    //������ֵ
                    for (j = 0; j < 5; j++)
                    {
                        for (i = j + 1; i < 9; i++)
                        {
                            if (p[j] > p[i])
                            {
                                s = p[j];
                                p[j] = p[i];
                                p[i] = s;
                            }
                        }
                    }
              //      if (bmpobj.GetPixel(x, y).R < dgGrayValue)
                        bmpobj.SetPixel(x, y, Color.FromArgb(p[4], p[4], p[4]));    //����Чֵ����ֵ
                }
            }
        }


/**//// <summary>
          /// �ú������ڶ�ͼ����и�ʴ���㡣�ṹԪ��Ϊˮƽ�����ֱ����������㣬
         /// �м��λ��ԭ�㣻�������û��Լ�����3��3�ĽṹԪ�ء�
          /// </summary>
          /// <param name="dgGrayValue">ǰ���ٽ�ֵ</param>
          /// <param name="nMode">��ʴ��ʽ��0��ʾˮƽ����1��ֱ����2�Զ���ṹԪ�ء�</param>
         /// <param name="structure"> �Զ����3��3�ṹԪ��</param>
          public void ErosionPic(int dgGrayValue, int nMode, bool[,] structure)
          {
            int lWidth = bmpobj.Width;
            int lHeight = bmpobj.Height;
           Bitmap newBmp = new Bitmap(lWidth, lHeight);

           int i, j, n, m;            //ѭ������
            Color pixel;    //������ɫֵ

            if (nMode == 0)
            {
                //ʹ��ˮƽ����ĽṹԪ�ؽ��и�ʴ
                // ����ʹ��1��3�ĽṹԪ�أ�Ϊ��ֹԽ�磬���Բ���������ߺ����ұ�
                // ����������
                for (j = 0; j < lHeight; j++)
                 {
                   for (i = 1; i < lWidth - 1; i++)
                    {
                        //Ŀ��ͼ���еĵ�ǰ���ȸ��ɺ�ɫ
                        newBmp.SetPixel(i, j, Color.Black);

                        //���Դͼ���е�ǰ���������������һ���㲻�Ǻ�ɫ��
                       //��Ŀ��ͼ���еĵ�ǰ�㸳�ɰ�ɫ
                       if (bmpobj.GetPixel(i - 1, j).R > dgGrayValue ||
                            bmpobj.GetPixel(i, j).R > dgGrayValue ||
                            bmpobj.GetPixel(i + 1, j).R > dgGrayValue)
                            newBmp.SetPixel(i, j, Color.White);
                     }
                }
            }
            else if (nMode == 1)
            {
                //ʹ�ô��淽��ĽṹԪ�ؽ��и�ʴ
                 // ����ʹ��3��1�ĽṹԪ�أ�Ϊ��ֹԽ�磬���Բ��������ϱߺ����±�
                // ����������
               for (j = 1; j < lHeight - 1; j++)
                {
                    for (i = 0; i < lWidth; i++)
                    {
                       //Ŀ��ͼ���еĵ�ǰ���ȸ��ɺ�ɫ
                        newBmp.SetPixel(i, j, Color.Black);

                        //���Դͼ���е�ǰ���������������һ���㲻�Ǻ�ɫ��
                       //��Ŀ��ͼ���еĵ�ǰ�㸳�ɰ�ɫ
                        if (bmpobj.GetPixel(i, j - 1).R > dgGrayValue ||
                            bmpobj.GetPixel(i, j).R > dgGrayValue ||
                            bmpobj.GetPixel(i, j + 1).R > dgGrayValue)
                            newBmp.SetPixel(i, j, Color.White);
                    }
                }
            }
            else
           {
                if (structure.Length != 9)  //����Զ���ṹ
                    return;
                //ʹ���Զ���ĽṹԪ�ؽ��и�ʴ
                // ����ʹ��3��3�ĽṹԪ�أ�Ϊ��ֹԽ�磬���Բ���������ߺ����ұ�
               // ���������غ����ϱߺ����±ߵ���������
               for (j = 1; j < lHeight - 1; j++)
               {
                   for (i = 1; i < lWidth - 1; i++)
                   {
                        //Ŀ��ͼ���еĵ�ǰ���ȸ��ɺ�ɫ
                        newBmp.SetPixel(i, j, Color.Black);
                        //���ԭͼ���ж�Ӧ�ṹԪ����Ϊ��ɫ����Щ������һ�����Ǻ�ɫ��
                       //��Ŀ��ͼ���еĵ�ǰ�㸳�ɰ�ɫ
                       for (m = 0; m < 3; m++)
                       {
                            for (n = 0; n < 3; n++)
                            {
                                if (!structure[m, n])
                                    continue;
                               if (bmpobj.GetPixel(i + m - 1, j + n - 1).R > dgGrayValue)
                                {
                                    newBmp.SetPixel(i, j, Color.White);
                                    break;
                                 }
                            }
                        }
                    }
                 }
            }

            bmpobj = newBmp;
         }
 

        /**//// <summary>
        /// �ú������ڶ�ͼ�����ϸ�����㡣Ҫ��Ŀ��ͼ��Ϊ�Ҷ�ͼ��
        /// </summary>
        /// <param name="dgGrayValue"></param>
        public void ThiningPic(int dgGrayValue)
        {
            int lWidth = bmpobj.Width;
            int lHeight = bmpobj.Height;
         //   Bitmap newBmp = new Bitmap(lWidth, lHeight);

            bool bModified;            //����    
            int i, j, n, m;            //ѭ������
            Color pixel;    //������ɫֵ

            //�ĸ�����
           bool bCondition1;
           bool bCondition2;
            bool bCondition3;
          bool bCondition4;

            int nCount;    //������    
          int[,] neighbour = new int[5, 5];    //5��5������������ֵ


          bModified = true;
           while (bModified)
           {
                bModified = false;

                //����ʹ��5��5�ĽṹԪ�أ�Ϊ��ֹԽ�磬���Բ�������Χ�ļ��кͼ�������
                for (j = 2; j < lHeight - 2; j++)
                {
                    for (i = 2; i < lWidth - 2; i++)
                    {
                        bCondition1 = false;
                        bCondition2 = false;
                        bCondition3 = false;
                        bCondition4 = false;

                        if (bmpobj.GetPixel(i, j).R > dgGrayValue)  
                        {
                            if(bmpobj.GetPixel(i, j).R<255)
                                bmpobj.SetPixel(i, j, Color.White);
                            continue;
                        }

                        //��õ�ǰ�����ڵ�5��5����������ֵ����ɫ��0������ɫ��1����
                        for (m = 0; m < 5; m++)
                        {
                           for (n = 0; n < 5; n++)
                            {
                                neighbour[m, n] = bmpobj.GetPixel(i + m - 2, j + n - 2).R < dgGrayValue ? 1 : 0;
                            }
                        }

                        //����ж�������
                        //�ж�2<=NZ(P1)<=6
                        nCount = neighbour[1, 1] + neighbour[1, 2] + neighbour[1, 3]
                                + neighbour[2, 1] + neighbour[2, 3] +
                               +neighbour[3, 1] + neighbour[3, 2] + neighbour[3, 3];
                        if (nCount >= 2 && nCount <= 6)
                        {
                            bCondition1 = true;
                        }

                        //�ж�Z0(P1)=1
                        nCount = 0;
                       if (neighbour[1, 2] == 0 && neighbour[1, 1] == 1)
                            nCount++;
                       if (neighbour[1, 1] == 0 && neighbour[2, 1] == 1)
                           nCount++;
                        if (neighbour[2, 1] == 0 && neighbour[3, 1] == 1)
                            nCount++;
                       if (neighbour[3, 1] == 0 && neighbour[3, 2] == 1)
                            nCount++;
                        if (neighbour[3, 2] == 0 && neighbour[3, 3] == 1)
                           nCount++;
                        if (neighbour[3, 3] == 0 && neighbour[2, 3] == 1)
                            nCount++;
                        if (neighbour[2, 3] == 0 && neighbour[1, 3] == 1)
                            nCount++;
                        if (neighbour[1, 3] == 0 && neighbour[1, 2] == 1)
                            nCount++;
                        if (nCount == 1)
                            bCondition2 = true;

                        //�ж�P2*P4*P8=0 or Z0(p2)!=1
                        if (neighbour[1, 2] * neighbour[2, 1] * neighbour[2, 3] == 0)
                       {
                           bCondition3 = true;
                        }
                        else
                        {
                            nCount = 0;
                           if (neighbour[0, 2] == 0 && neighbour[0, 1] == 1)
                                nCount++;
                            if (neighbour[0, 1] == 0 && neighbour[1, 1] == 1)
                               nCount++;
                            if (neighbour[1, 1] == 0 && neighbour[2, 1] == 1)
                               nCount++;
                            if (neighbour[2, 1] == 0 && neighbour[2, 2] == 1)
                                nCount++;
                            if (neighbour[2, 2] == 0 && neighbour[2, 3] == 1)
                                nCount++;
                           if (neighbour[2, 3] == 0 && neighbour[1, 3] == 1)
                              nCount++;
                         if (neighbour[1, 3] == 0 && neighbour[0, 3] == 1)
                                nCount++;
                          if (neighbour[0, 3] == 0 && neighbour[0, 2] == 1)
                                nCount++;
                           if (nCount != 1)
                               bCondition3 = true;
                       }

                        //�ж�P2*P4*P6=0 or Z0(p4)!=1
                       if (neighbour[1, 2] * neighbour[2, 1] * neighbour[3, 2] == 0)
                        {
                           bCondition4 = true;
                        }
                        else
                        {
                            nCount = 0;
                           if (neighbour[1, 1] == 0 && neighbour[1, 0] == 1)
                                nCount++;
                           if (neighbour[1, 0] == 0 && neighbour[2, 0] == 1)
                                nCount++;
                            if (neighbour[2, 0] == 0 && neighbour[3, 0] == 1)
                                nCount++;
                            if (neighbour[3, 0] == 0 && neighbour[3, 1] == 1)
                               nCount++;
                            if (neighbour[3, 1] == 0 && neighbour[3, 2] == 1)
                                nCount++;
                           if (neighbour[3, 2] == 0 && neighbour[2, 2] == 1)
                                nCount++;
                           if (neighbour[2, 2] == 0 && neighbour[1, 2] == 1)
                                nCount++;
                          if (neighbour[1, 2] == 0 && neighbour[1, 1] == 1)
                                nCount++;
                           if (nCount != 1)
                                bCondition4 = true;
                        }

                        if (bCondition1 && bCondition2 && bCondition3 && bCondition4)
                        {
                            bmpobj.SetPixel(i, j, Color.White);
                            bModified = true;
                        }
                        else
                        {
                            bmpobj.SetPixel(i, j, Color.Black);
                        }
                    }
                }
            }
            // ����ϸ�����ͼ��
       //    bmpobj = newBmp;
       }

    }

}
