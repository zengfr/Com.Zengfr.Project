using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Zfrong.Framework.Core.DataContract;
namespace Zfrong.Framework.Utils.Token.AgentService
{
    public class AgentTokenMessageInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        private static string Ns="";
        public static long AgentID = 0;
        public static string AgentToken = string.Empty;
        #region IClientMessageInspector 成员

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {

        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            MessageHeader agentHeader = MessageHeader.CreateHeader("AgentID", Ns, AgentID); request.Headers.Add(agentHeader);
            MessageHeader tokenHeader = MessageHeader.CreateHeader("AgentToken", Ns, AgentToken); request.Headers.Add(tokenHeader);
            return null;
        }

        #endregion

        #region IDispatchMessageInspector 成员

        static string GetHeaderValue(string key)
        {
            int index = OperationContext.Current.IncomingMessageHeaders.FindHeader(key, Ns);
            if (index >= 0)
            {
                return OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(index).ToString();
            }
            return null;
        }
        static string GetClientIP()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties properties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return endpoint.Address.ToString();

        }
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            string ip = GetClientIP();
            string agent = GetHeaderValue("AgentID");
            string token = GetHeaderValue("AgentToken");
            long agentId = 0;
            long.TryParse(agent, out agentId);
            TokenResponse tokenResponse = TokenHelper.Current.VerifyToken(agentId, token);
            if (tokenResponse.Message != "success")
            {
                //失败
                throw new FaultException(string.Format("IP:{0} AgentID:{1} Message:{2}!",
                    ip, agentId, tokenResponse.Message));
            }
            else
            {
                //成功
            }
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {

        }

        #endregion
    }
    
}
