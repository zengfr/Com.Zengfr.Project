using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Zfrong.Common.Command
{
    /// <summary>
    /// ����������������
    /// </summary>
    public  enum CoreDataType
    {
       App,
       Session,
       AppData,
       SessionData,
       Command,
       Log
    }

   public enum CommandFamily
    {
        Command_App,
        Command_Session,
        Command_AppData,
        Command_SessionData,
        Command,
        Command_Log,
        Command_Empty
    }
}
