using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Rest;
using Spring.Rest.Client.Support;
using Spring.Rest.Client;
using Spring.Http;
using Spring.Http.Converters;
using Com.Zfrong.CommonLib.Spring.Http;
namespace Com.Zfrong.CommonLib.Spring.Rest
{
    class RestTemplateHelper
    {
        void Post(string url, string body)
        {

            FormHttpMessageConverter converter = new FormHttpMessageConverter();
            MockHttpOutputMessage message = new MockHttpOutputMessage();
            converter.Write(body, MediaType.APPLICATION_FORM_URLENCODED, message);
            RestTemplate t=new RestTemplate(); t.PostForMessageAsync(url, message,
                 delegate(RestOperationCompletedEventArgs<HttpResponseMessage> args)
                 {

                 });
        }
    }
}
