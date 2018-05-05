using System;
using System.Collections.Generic;
using System.Text;
namespace CommonPack
{
    public class PowerUri : Uri
    {
        public PowerUri(string url) : base(url) { }
        public PowerUri(Uri uri, string url)
            : base(uri, url)
        {
            this.BaseUri = uri;
        }
        //public PowerUri(PowerUri uri, string url)
        //    : base(uri, url)
        //{
        //    this.BaseUri = uri;
        //    this.Depth = uri.Depth + 1;
        //}
        public Uri BaseUri;
        public int Depth;
    }
}