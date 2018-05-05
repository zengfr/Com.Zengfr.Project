using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Common.Logging;
using Zfrong.Framework.Core.DataContract;
using Zfrong.Framework.Core.IService;
using Zfrong.Framework.Utils.Token.AgentService;
namespace Zfrong.Framework.Utils.Token
{
    public partial class MyChannelFactory
    {
        public static ILog Log { get; set; }
        public static IService CreateChannelService<IService>(string endpointConfigName)
        {
            ChannelFactory<IService> factory = new ChannelFactory<IService>(endpointConfigName);
            AddEvent(factory);
            return factory.CreateChannel();
        }
        public static IService CreateChannelService<IService>(Binding binding,string url)
        {
            ChannelFactory<IService> factory = new ChannelFactory<IService>(binding,url);
            AddEvent(factory);
            return factory.CreateChannel();
        }
        static void AddEvent(IChannelFactory factory)
        {
            factory.Opening += new EventHandler(factory_Opening);
            factory.Opened += new EventHandler(factory_Opened);
            factory.Faulted += new EventHandler(factory_Faulted);
        }
        static void factory_Opening(object sender, EventArgs e)
        {
            Show("Channel,Opening:{0}", sender.GetType().GetGenericArguments()[0].Name);
        }

        static void factory_Faulted(object sender, EventArgs e)
        {
            Show("Channel,Faulted:{0}", sender.GetType().GetGenericArguments()[0].Name);
        }

        static void factory_Opened(object sender, EventArgs e)
        {
            Show("Channel,Opened:{0}", sender.GetType().GetGenericArguments()[0].Name);
        }
        static void Show(string format, params object[] args)
        {
            if (Log != null)
            {
                Log.Info(string.Format("{0} :{1}", DateTime.Now.ToString("MMdd HH:mm:ss fff"), String.Format(format, args)));
            }
            else
                Console.WriteLine(string.Format("{0} :{1}", DateTime.Now.ToString("MMdd HH:mm:ss fff"), String.Format(format, args)));
        }
    }
        public partial class MyChannelFactory
    {
        public static TokenResponse GetToken(string endpointConfigName,string user,string pwd)
        {
            ITokenService tokenService = CreateChannelService<ITokenService>(endpointConfigName);
            TokenRequest request = new TokenRequest();
            request.User = user;
            request.Pwd = pwd;
            Show("GetToken,REQ:{0}", request.User);
            TokenResponse response = tokenService.GetToken(request);
            Show("GetToken,RTN:{0}", response.Message);

            return response;
        }
        public static bool RemoveToken(string endpointConfigName,long userId,string token)
        {
            ITokenService tokenService = CreateChannelService<ITokenService>(endpointConfigName);
           
            Show("RemoveToken,REQ:{0}", userId);
            bool rtn = tokenService.RemoveToken(userId, token);
            Show("RemoveToken,RTN:{0}", rtn);
            return rtn;
        }
        static string Agent = System.Configuration.ConfigurationManager.AppSettings["agent"];
        static string Pwd = System.Configuration.ConfigurationManager.AppSettings["pwd"];
        public static long AgentID
        {get{
            return AgentTokenMessageInspector.AgentID;
        }
        }
        public static string AgentToken
        {
            get
            {
                return AgentTokenMessageInspector.AgentToken;
            }
        }
        public static bool GetAgentToken(string endpointConfigName)
        {
           
            TokenResponse response = GetToken(endpointConfigName,Agent,Pwd);
            if (response.Message == "success")
            {
                AgentTokenMessageInspector.AgentToken = response.Token;
                AgentTokenMessageInspector.AgentID = response.UserID;
                return true;
            }
            return false;
        }
        public static bool RemoveAgentToken(string endpointConfigName)
        {
            long agentID = AgentTokenMessageInspector.AgentID;
            string token = AgentTokenMessageInspector.AgentToken;
            bool rtn = RemoveToken(endpointConfigName,agentID,token);
            if (rtn)
            {
                AgentTokenMessageInspector.AgentToken = string.Empty;
                AgentTokenMessageInspector.AgentID = 0;
            }
            return rtn;
        }
    }
}