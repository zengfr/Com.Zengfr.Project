using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;

namespace Zfrong.Framework.Utils.Token.AgentService
{
    public class AgentTokenBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(AgentTokenBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new AgentTokenBehavior();
        }
        #region IEndpointBehavior 成员

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new AgentTokenMessageInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new AgentTokenMessageInspector());
        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        #endregion
    }
}
