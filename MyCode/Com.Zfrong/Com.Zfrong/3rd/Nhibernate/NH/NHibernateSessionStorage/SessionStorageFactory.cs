/*

$Header$
$Author$
$Date$ 
$Revision$
$History$

*/

using System;
using Com.Zfrong.Common.Data.NH.SessionStorage.Cfg;

namespace Com.Zfrong.Common.Data.NH.SessionStorage
{
    /// <summary>
    /// ����ISessionStorage�Ĺ���
    /// </summary>
    public class SessionStorageFactory
    {
        /// <summary>
        /// ���ISessionStorage
        /// </summary>
        /// <returns></returns>
        public static ISessionStorage GetSessionStorage()
        {
            if (Config.SessionSourceType == "http")  //ʹ��    
            {
                return new HttpSessionSource();
            }
            else if (Config.SessionSourceType == "threadStatic")
            {
                return new ThreadSessionSource();
            }
            else
            {
                throw new NotSupportedException("��֧�ֵ�SessionSourceType��" + Config.SessionSourceType);
            }
        }
    }
}
