using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

namespace Com.Zengfr.CodeGenerator.Renamer
{
    class Program
    {
        static void Main(string[] args)
        {
            Process(@"E:\gitRepository\zhuizhangdashi\bin\Debug\Com.Zengfr.Proj.DataAccess.dll");
            Console.ReadLine();
        }
        static long NameIndex = 0;
        static Random NameRandom = new Random();
        static string BuildNameString()
        {
            return BuildNameString(false);
        }
        static string BuildNameString(bool onlyIndex)
        {
            if (onlyIndex)
                return "" + NameIndex++;
            var n1 = DateTime.Now.ToString("fff");
            var n2 = DateTime.Now.ToString("HHmmssfff");
            return string.Format("_{0}{1}{2}{3}", n1, n2, NameIndex++, NameRandom.Next(100000, 999999));
        }
        static void Process(string filename)
        {
            var newFileName = filename + ".reName.dll";
            Process(filename, newFileName);
        }
        static void Process(string filename, string newFileName)
        {
            ModuleDefMD mod = ModuleDefMD.Load(filename);


            var wopts = new dnlib.DotNet.Writer.ModuleWriterOptions(mod);
            wopts.WritePdb = true; wopts.Listener = new ModuleWriterListener();
            wopts.MetaDataOptions.Flags = wopts.MetaDataOptions.Flags | MetaDataFlags.KeepOldMaxStack
                ;//| MetaDataFlags.PreserveAllMethodRids | MetaDataFlags.PreserveMethodRids;

            mod.LoadPdb();
            //mod.MetaData.
            ProcessType(mod.GetTypes());
            mod.Write(newFileName, wopts);


        }
        static void ProcessType(IEnumerable<TypeDef> defs)
        {
            foreach (TypeDef def in defs)
            {
                if (!def.IsPublic)
                {
                    def.Name = string.Format("t{0}", BuildNameString());
                }
                ProcessMethod(def.Methods);
                ProcessProperty(def.Properties);
                ProcessField(def.Fields);
                //ProcessMethod(def.Methods());
            }
        }
        static void ProcessMethod(IList<MethodDef> defs)
        {
            foreach (MethodDef def in defs)
            {
                if (!def.IsPublic)
                {
                    def.Name = string.Format("m{0}", BuildNameString());
                }
                ProcessParam(def.ParamDefs);

                //def.Body.InitLocals = false;
                ProcessLocalVariables(def.Body.Variables);
                def.Body.Scope = null;

                var mb = new CilBody();
                mb.InitLocals = true;
                foreach (var loc in def.Body.Variables)
                {
                    mb.Variables.Add(loc);
                }
                foreach (Instruction ins in def.Body.Instructions)
                {
                    if(ins.OpCode.OperandType==OperandType.ShortInlineVar)
                    {
                        var variable = ins.Operand as IVariable;
                        if (variable.Index < byte.MinValue || variable.Index >= byte.MaxValue)
                        {
                            ; //
                        }
                        break;
                    }
                    //ins.Offset = 0;
                    //var ins2 = ins.Clone();
                    mb.Instructions.Add(ins); 
                }
                //def.Body.KeepOldMaxStack = false;
                //def.Body.MaxStack = (ushort)(def.Body.MaxStack * 2);
                def.Body.Variables.Clear();
                def.Body.Instructions.Clear();

                def.Body = mb;
                def.Body.OptimizeBranches();
                //def.Body.OptimizeMacros();
                def.Body.SimplifyBranches();
                // def.Body.SimplifyMacros();
                // def.Body.UpdateInstructionOffsets();
            }
        }
        static void ProcessLocalVariables(LocalList localList)
        {
            foreach (Local local in localList)
            {
                // if (!string.IsNullOrEmpty(local.Name))
                local.Name = string.Format("v{0}", BuildNameString(true));
            }
        }
        static void ProcessProperty(IList<PropertyDef> defs)
        {
            foreach (PropertyDef def in defs)
            {
                if ((!def.GetMethod.IsPublic) && (!def.SetMethod.IsPublic))
                {
                    def.Name = string.Format("pr{0}", BuildNameString());
                }
            }
        }
        static void ProcessField(IList<FieldDef> defs)
        {
            foreach (FieldDef def in defs)
            {
                if (!def.IsPublic)
                {
                    def.Name = string.Format("f{0}", BuildNameString());
                }
            }
        }
        static void ProcessParam(IList<ParamDef> defs)
        {
            foreach (ParamDef def in defs)
            {
                if (!def.HasCustomAttributes)
                {
                    def.Name = string.Format("p{0}", BuildNameString());
                }
            }
        }
        public class ModuleWriterListener : IModuleWriterListener
        {
            public void OnWriterEvent(ModuleWriterBase writer, ModuleWriterEvent evt)
            {

            }
        }
    }
}
