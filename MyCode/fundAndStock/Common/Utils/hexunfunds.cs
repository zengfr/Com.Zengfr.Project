using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using ConsoleApplication3.Model;
using Spring.Rest.Client;

namespace ConsoleApplication3.Utils
{
    public class hexunfunds
    {
        static RestTemplate template = new RestTemplate();

        private static RegexOptions RegexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase |
                                                   RegexOptions.Singleline;
        public static string GetzcgtjString(FundRequest request)
        {
            var url = "http://paiming.funds.hexun.com/cc/zcgtj.aspx";

            NameValueCollection form = new NameValueCollection();
            form.Add("enddate_open", request.enddate_open);
            form.Add("fund_class", request.fund_class);
            form.Add("fund_com_open", request.fund_com_open);
            form.Add("invest_custom", request.invest_custom);
            form.Add("invest_type", request.invest_type);
            var response = template.PostForMessage<string>(url, form);
            return response.Body;
        }

        public static List<zcgtj> Getzcgtj(FundRequest request)
        {
            var list = new List<zcgtj>();


            var response = GetzcgtjString(request);

            return list;
        }
        private List<zcgtj> ParseList(string source)
        {
            var list = new List<zcgtj>();
            var matches = Regex.Matches(source, "<tr align='center' onmouseover=(.*?)</td></tr>", RegexOptions);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    Parse(match.Value);
                }
            }
            return list;
        }
        private zcgtj Parse(string source)
        {
            var obj = new zcgtj();

            return obj;
        }
    }
}
