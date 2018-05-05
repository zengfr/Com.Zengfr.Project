using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace Com.Zfrong.Common.Command
{
    /// <summary>
    /// 创建一个ICommand空类型
    /// </summary>
   public class EmptyCommand : CommandBase
    {
        public override CommandFamily Family
        {
            get { return CommandFamily.Command_Empty; }
        }

        public override void CommandBody(object sender, params object[] paras)
        {
        }
    }

    
    /// <summary>
    /// Command模式
    /// </summary>
   public class CommandManager
    {
        static Dictionary<CommandFamily, CommandBase> commandlist = new Dictionary<CommandFamily, CommandBase>();
        static CommandBase emptyCommand = new EmptyCommand();


        static CommandManager()
        {
            LoadCommandFromFile();
        }

        private static void LoadCommandFromFile()
        {
           LoadCommandFromFile(Assembly.GetExecutingAssembly());
        }
        public static void LoadCommandFromFile(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                if (t.IsSubclassOf(typeof(CommandBase)))
                {
                    CommandBase newCommand = Activator.CreateInstance(t) as CommandBase;
                    if (newCommand != null)
                        RegisteCommand(newCommand);
                }
            }
        }

       public static CommandBase GetCommand(CommandFamily Family)
        {
            //有效命令
            if (commandlist.ContainsKey(Family))
                return commandlist[Family];
            //无效命令，返回一个空的方法体
            return emptyCommand;    
        }

        /// <summary>
        /// 注册一个命令
        /// </summary>
        /// <param name="Family"></param>
        /// <param name="command"></param>
       public static void RegisteCommand(CommandBase command)
        {
            if (!commandlist.ContainsKey(command.Family))
                commandlist.Add(command.Family, command);
        }
    }

  
}
