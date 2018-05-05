using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zfrong.Framework.Core.DataContract;
using Zfrong.Framework.Core.IService;
using Spring.Transaction.Interceptor;
namespace Zfrong.Framework.Utils.Token.AgentService
{

    public class AgentTokenService : ITokenService
    {
        AgentTokenService() {
            TokenHelper.PermissionsHelper =AgentPermissionsHelper.Current;
        }
         [Transaction(ReadOnly = false)]
        public virtual TokenResponse GetToken(TokenRequest getTokenRequest)
        {
            return TokenHelper.Current.GetToken(getTokenRequest);
        }
         [Transaction(ReadOnly = false)]
         public virtual TokenResponse VerifyToken(long agentId, string token) 
        {
            return TokenHelper.Current.VerifyToken(agentId, token);
        }
          [Transaction(ReadOnly = false)]
         public virtual bool RemoveToken(long agentId, string token)
        {
            return TokenHelper.Current.RemoveToken(agentId, token);
        }
    }
}
