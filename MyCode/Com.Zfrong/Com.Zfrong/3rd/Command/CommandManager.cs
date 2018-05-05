using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace Com.Zfrong.Common.Command
{
    /// <summary>
    /// ����һ��ICommand������
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
    /// Commandģʽ
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
            //��Ч����
            if (commandlist.ContainsKey(Family))
                return commandlist[Family];
            //��Ч�������һ���յķ�����
            return emptyCommand;    
        }

        /// <summary>
        /// ע��һ������
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
