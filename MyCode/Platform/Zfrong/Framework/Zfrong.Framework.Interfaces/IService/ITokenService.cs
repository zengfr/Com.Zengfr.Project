using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using Zfrong.Framework.Core.DataContract;
using Zfrong.Framework.Core.IService.Plugin;
namespace Zfrong.Framework.Core.IService
{
    [ServiceContract]
    public interface ITokenService 
    {
        [OperationContract]
        TokenResponse GetToken(TokenRequest getTokenRequest);
        [OperationContract]
        TokenResponse VerifyToken(long userId,string token);
        [OperationContract]
        bool RemoveToken(long userId, string token);
    }
    
}
