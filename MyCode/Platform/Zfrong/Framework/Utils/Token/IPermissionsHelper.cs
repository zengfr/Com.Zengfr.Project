using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
namespace Zfrong.Framework.Utils.Token
{

    public interface IPermissionsHelper
    {
        long Login(string user, string pwd);
        string GetPermissions(long userID);
    }
}
