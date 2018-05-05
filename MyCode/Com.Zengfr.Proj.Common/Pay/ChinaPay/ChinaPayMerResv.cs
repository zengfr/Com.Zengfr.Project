
using Newtonsoft.Json;
namespace Com.Zengfr.Proj.Common.Pay.ChinaPay
{
    public class ChinaPayMerResv
    {
        [JsonProperty("R")]
        public virtual string Referred { get; set; }
        [JsonProperty("P")]
        public virtual string PayBankType { get; set; }
        [JsonProperty("A")]
        public virtual string ActionType { get; set; }
        [JsonProperty("H")]
        public virtual long HandlerID { get; set; }
        [JsonProperty("HC")]
        public virtual long HandlerCompanyID { get; set; }
        [JsonProperty("L")]
        public virtual long LawyerID { get; set; }
        [JsonProperty("M")]
        public virtual long MarketerID { get; set; }

        public ChinaPayMerResv()
        {
            //this.Referred = string.Empty;//
            //if (HttpContext.Current.Request.UrlReferrer != null)
            //    this.Referred = HttpContext.Current.Request.UrlReferrer.PathAndQuery;

            //this.RemoteAddr = HttpContext.Current.Request.UserHostAddress ?? string.Empty;
            //this.RemoteAddrXFORWARDED = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? string.Empty;
        }
        static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            FloatFormatHandling = FloatFormatHandling.String,
        };
        public string SerializeObject(bool compress = true)
        {
            var json = JsonConvert.SerializeObject(this, jsonSerializerSettings);
            if (compress)
                json = GZipUtil.CompressString(json);
            return json;
        }
        public static ChinaPayMerResv DeserializeObject(string jsonCompressString, bool decompress = true)
        {
            if (decompress)
                jsonCompressString = GZipUtil.DecompressString(jsonCompressString);
            return JsonConvert.DeserializeObject<ChinaPayMerResv>(jsonCompressString, jsonSerializerSettings);
        }
    }
}
