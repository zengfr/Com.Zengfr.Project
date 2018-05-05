using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Zfrong.Framework.Core.Service;
using Zfrong.Framework.Core.DataContract;
using Zfrong.Framework.Core.Model.Base;
using Zfrong.Framework.Utils.TokenService;
using NHibernate.Criterion;
using AgentPlatform.Contract.ServiceContract;
using AgentPlatform.Model;
namespace AgentPlatform.ServiceClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            TokenServiceTest(1);

           UserServiceTest(2);
           UserExtServiceTest(2);
            ReadLine();
        }
        static void TokenServiceTest(int count)
        {
            for (int i = 0; i < count; i++)
            {
                MyChannelFactory.GetToken("e_ITokenService");
                //System.Threading.Thread.Sleep(1000);
                //MyChannelFactory.RemoveToken("basicHttpBinding_ITokenService");
                //System.Threading.Thread.Sleep(1000);
            }
        }
        static void UserServiceTest(int count)
        {
            IUserService service = MyChannelFactory.CreateChannelService<IUserService>("e_IUserService");
            Order[] order =new Order[]{ Order.Desc("ID")};
            ICriterion c = null;
            for (int i =1; i < count+1; i++)
            {
                c=Expression.Eq("ID",(long)i);
                IList<User> objs=service.FindAll(order,c);
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine();
            }
            ReadLine();
        }
        static void UserExtServiceTest(int count)
        {
            IUserExtService service = CreateChannelService<IUserExtService>();
            ExtPagingResponse<User> response=service.SlicedFindAll("0","100",null,null,null);
        }
        static IService CreateChannelService<IService>()
        {
            return MyChannelFactory.CreateChannelService<IService>("e_"+typeof(IService).Name);
        }
        static void ReadLine()
        {
            Console.WriteLine("ReadLine...");
            Console.ReadLine();
        }
    }
}
