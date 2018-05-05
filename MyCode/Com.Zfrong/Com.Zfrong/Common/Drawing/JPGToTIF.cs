using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
namespace Com.Zfrong.Common.Drawing
{
    class JPGToTIF
    {
        /********************************************/
 private static void ConvertJPGtoTIF (string jpgPath, string tifPath,
ImageCodecInfo ici)
 {
 byte tifPixels = 0;
 float[] brightnessValues;

 Bitmap jpgImage;
 Bitmap tifImage;
 BitmapData jpgBmd;
 BitmapData tifBmd;
 Color[] entries;
 EncoderParameters eps = new EncoderParameters(1);

 // new up a Bitmap from the source JPG file then
 // get the bytes of the jpg so we can read through them
 // 8 bits or 1 byte of the jpg is equal to 1 pixel
 // each byte represents a color value that is in the Bitmaps Palette
 jpgImage = new Bitmap (jpgPath);
jpgBmd = jpgImage.LockBits (
 new Rectangle (0, 0, jpgImage.Width, jpgImage.Height),
 ImageLockMode.ReadOnly,
 PixelFormat.Format8bppIndexed);

 // new up a Bitmap that will hold our destination TIF file then
 // get the bytes of the tif so we can write to them
 // 8 bits or 1 byte of the tif is equal to 8 pixels (i.e. 1 bit of the
 //tif is equal to 1 pixel)
 tifImage = new Bitmap (jpgImage.Width, jpgImage.Height,
 PixelFormat.Format1bppIndexed);
 tifBmd = tifImage.LockBits (
 new Rectangle (0, 0, tifImage.Width, tifImage.Height),
 ImageLockMode.WriteOnly,
 PixelFormat.Format1bppIndexed);

 // we will be doing a lookup into Palette.Entries for the color
 //reference by the JPG pixel
 // Entries is a collection of Color objects that is generated each time
 //Palette.Entries is called
 // I originally had this code inside the loop, but that was obviously
// extremely inefficient once
 // I realized how it was working. I then discovered that the
 //GetBrightness(); //call seems to be
 // calculated on each call also. So since I only need the brightness
 //value from each color,
 // I stuck those values in an array of floats that I would go after in
 //the loop
 entries = jpgImage.Palette.Entries;
 brightnessValues = new float[entries.Length];
 for (int index = 0; index < entries.Length; index++)
 brightnessValues[index] = entries[index].GetBrightness ();

 // starting point from http://www.bobpowell.net/lockingbits.htm and
 //Bob's newsgroup posts...thanks Bob!
 // for understanding of Stride see Bob's site - basically, the image is
 //wider in memory than the actual width
 for (int h = 0; h < jpgBmd.Height; h++) // iterate through all of the
 //rows of the JPG
for (int w = 0; w < jpgBmd.Stride; w++) // iterate through all of
 //the columns for the current row of the JPG
 {
 // 1 byte of the jpg is 1 pixel
 byte jpgPixel = Marshal.ReadByte (jpgBmd.Scan0, h *
 jpgBmd.Stride + w);

// get the brightness/luminescence HSB/L value for a good
 //conversion to black or white
// using .5 as black = 0.0 and white = 1.0
 bool isWhitePixel = brightnessValues[jpgPixel] < 0.5 ? false :
 true;

 // we need to translate what we know about this JPG pixel
 //(white or black) to the correct
 // bit in the current byte of the TIF pixel
 // the whole 7- thing is for writing the bits left to right
 // only do this work if it's white, otherwise the bit is
 //already 0 from the reset below
 if (isWhitePixel)
 tifPixels = (byte)(tifPixels | (1 << (7-(w % 8))));
 if (w % 8 == 7) // we only write out TIF bytes after every 8
 //JPG bytes
 {
 // divide width by 8 for proper TIF pixel
 Marshal.WriteByte (tifBmd.Scan0, h * tifBmd.Stride + w/8,
 tifPixels);
 tifPixels = 0; // reset is needed since we are not ORing 0s
 //in
 }
 }
 jpgImage.UnlockBits (jpgBmd);
 tifImage.UnlockBits (tifBmd);

 // save it out as a CCITT4 compressed TIF
 eps.Param[0] = new EncoderParameter (Encoder.Compression,
 (long)EncoderValue.CompressionCCITT4);
 tifImage.Save (tifPath, ici, eps);
 } 
    }
}
