using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Zfrong.Framework.Core.DataContract;

namespace Zfrong.Framework.Utils.Token
{

    public interface ITokenHelper
    {
        TokenResponse GetToken(TokenRequest getTokenRequest);
        TokenResponse VerifyToken(long agentId, string token);
        bool RemoveToken(long agentId, string token);
    }
    public partial class TokenHelper : ITokenHelper
    {
        private TokenHelper() { }
        public static ITokenHelper Current
        {
            get { return new TokenHelper(); }
        }
        public static IPermissionsHelper PermissionsHelper
        {
            get;
            set;
        }
        #region 接口实现
        public virtual TokenResponse GetToken(TokenRequest GetTokenRequest)
        {
            TokenResponse response = new TokenResponse();
            response.Message = "请重试";
            bool isIPLimited = TokenHelper.IsIPLimited();
            if (!isIPLimited)
            {
                long agentID = TokenHelper.Login(GetTokenRequest.User, GetTokenRequest.Pwd);
                if (agentID > 0)
                {
                    string token = string.Empty;
                    DateTime validDateTime = DateTime.Now.AddHours(4);
                    DateTime loginDateTime = DateTime.Now;
                    string permissions = TokenHelper.GetPermissions(agentID);


                    //缓存登录数据
                    TokenCacheData cacheData = new TokenCacheData();
                    if (LoginDataCache.ContainsKey(agentID))
                        cacheData = LoginDataCache[agentID];
                    cacheData.Agent = GetTokenRequest.User;
                    cacheData.LoginIP = GetClientIP();
                    cacheData.LoginDateTime = loginDateTime;
                    cacheData.ValidDateTime = validDateTime;
                    cacheData.RequestCount += 1;

                    string dataKey = cacheData.GetKey(); //加解密钥匙
                    token = string.Format("{0};{1};{2};{3};{4};",
                       agentID, cacheData.Agent, cacheData.LoginIP, cacheData.ValidDateTime,
                       permissions);
                    token = MyEncryptionHelper.EncryptoData(dataKey, token);

                    response.Token = token;
                    response.UserID = agentID;
                    response.DateTimeLimit = validDateTime;
                    response.RequestCount = cacheData.RequestCount;

                    cacheData.Token = token;
                    LoginDataCache[agentID] = cacheData;

                    response.Message = "success";
                }
                else
                    response.Message = "登录失败";
            }
            else
                response.Message = "IP受限";
            return response;
        }
        public virtual TokenResponse VerifyToken(long agentId, string token)
        {
            TokenResponse response = new TokenResponse();
            response.Message = "请重试";
            response.UserID = agentId;
            response.Token = token;
            if (agentId <= 0)
            {
                response.Message = "ID无效"; return response;
            }
            if (string.IsNullOrEmpty(token))
            {
                response.Message = "Token无效"; return response;
            }
            bool isIPLimited = TokenHelper.IsIPLimited();
            if (isIPLimited)
            {
                response.Message = "IP受限"; return response;
            }
            TokenCacheData cacheData = new TokenCacheData();
            if (!LoginDataCache.ContainsKey(agentId))
            {
                response.Message = "ID未登录"; return response;
            }
            cacheData = LoginDataCache[agentId];
            if (cacheData.LoginIP != GetClientIP())
            {
                response.Message = "IP与登录不符"; return response;
            }
            if (cacheData.Token != token)
            {
                response.Message = "Token与登录不符"; return response;
            }
            else
            {
                cacheData.RequestCount += 1;
            }
            if (cacheData.RequestLimit != 0 && cacheData.RequestCount > cacheData.RequestLimit)
            {
                response.Message = "访问次数限制"; return response;
            }
            if (cacheData.ValidDateTime < DateTime.Now)
            {
                response.Message = "Token过期";
                LoginDataCache.Remove(agentId);
                return response;
            }
            string dataKey = DateTime.Now.ToString();
            dataKey = cacheData.GetKey();//解密钥匙
            token = MyEncryptionHelper.DecryptoData(dataKey, token);
            string[] tokens = token.Split(';');
            if (tokens.Length < 5)
            {
                response.Message = "Token假冒"; return response;
            }
            long userID;
            long.TryParse(tokens[0], out userID);
            if (agentId == userID)
            {
                string permissions = tokens[4];
                response.Message = "success";
            }
            return response;
        }
        public virtual bool RemoveToken(long userId, string token)
        {
            if (LoginDataCache.ContainsKey(userId))
            {
                if (token == LoginDataCache[userId].Token)
                {
                    LoginDataCache.Remove(userId);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Common static
        public static IDictionary<string, IPCacheData> IPCacheData = new Dictionary<string, IPCacheData>();
       public static IDictionary<long, TokenCacheData> LoginDataCache = new Dictionary<long, TokenCacheData>();
       static bool IsIPLimited()
       {
           string ip = GetClientIP();
           bool rtn =false;
           if(!IPCacheData.ContainsKey(ip)){
               IPCacheData[ip] = new IPCacheData();
           }
           IPCacheData data = IPCacheData[ip];
            data.RequestCount+= 1;
            if (data.RequestLimit > 0)
            {
                if (data.RequestCount > data.RequestLimit)
                    rtn = true;
            }
           return rtn;
       }
        static string GetClientIP()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties properties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return endpoint.Address.ToString();

        }
        
        #endregion
    }
    public partial class TokenHelper : ITokenHelper
    {
        #region DB访问 static
        static long Login(string user, string pwd)
        {
            long userID = 0;
            if (PermissionsHelper != null)
                userID = PermissionsHelper.Login(user, pwd);
            return userID;
        }
        static string GetPermissions(long userID)
        {
            string permissions = string.Empty;
            if (PermissionsHelper != null)
                permissions = PermissionsHelper.GetPermissions(userID);
            return permissions;
        }
        #endregion
    }
    public class IPCacheData
    {
        public long RequestCount { get; set; }
        public long RequestLimit { get; set; }
    }
    public class TokenCacheData
    { 
        public string Token { get; set; }
        public long RequestCount { get; set; }
        public long RequestLimit { get; set; }

        public string Agent { get; set; }
        public string LoginIP { get; set; }
        public DateTime LoginDateTime { get; set; }
        public DateTime ValidDateTime { get; set; }
        
        public string GetKey()
        {
            string rtn = string.Format("{0}_{1}_{2}_{3}_{4}",
                "Cache", Agent, LoginIP,
                LoginDateTime.ToString("MMddHHmmssfff"),
                ValidDateTime.ToString("MMddHHmmssfff"));
            return rtn;
        }
    }
}
