using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Com.Zengfr.Proj.Common
{
    public class HtmlRenderToImageUtils
    {
        //public static Image HtmlRenderToImage(Uri baseUri, string html, int width, int height)
        //{
        //    Image image = TheArtOfDev.HtmlRenderer.WinForms.HtmlRender.RenderToImage(
        //        html, new Size(width, height),
        //        Color.Empty, null,
        //        (sender, e) =>
        //        {
        //            e.SetSrc = new Uri(baseUri, e.Src).ToString();
        //            if (e.Attributes.ContainsKey("src"))
        //            {
        //                e.Attributes["src"] = new Uri(baseUri, e.Src).ToString();
        //            }
        //        },
        //        (sender, e) =>
        //        {
        //            if (e.Attributes.ContainsKey("src"))
        //            {
        //                e.Attributes["src"] = new Uri(baseUri, e.Src).ToString();
        //            }
        //        });
        //    return image;
        //}
    }
}
