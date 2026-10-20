using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;

namespace Com.Zengfr.CodeGenerator.Host
{
    [Serializable]
    public class FileBaseHost : BaseHost, ITextTemplatingEngineHost
    {
        #region

        public virtual CompilerErrorCollection ErrorCollection { get; set; }

        public virtual bool OutputSingleFile { get; set; }
        public virtual bool GenImplementEmptyFile { get; set; }
        public virtual string OutputRootDir { set; get; }
        public virtual string OutputFolder { set; get; }
        #endregion

        public virtual string NamespacePrefix { set; get; }
        public virtual string NamespaceSuffix { set; get; }
        public virtual string ProjectName { set; get; }

        public virtual string FileName { set; get; }
        public virtual string FileNamePrefix { set; get; }
        public virtual string FileNameSuffix { get; set; }


        public virtual string FileExtention { get; set; }

        #region ITextTemplatingEngineHost

        public virtual object GetHostOption(string optionName)
        {
            object result = null;
            if (optionName != null && optionName == "CacheAssemblies")
            {
                result = true;
            }

            return result;
        }

        public virtual bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = string.Empty;
            location = string.Empty;
            if (File.Exists(requestFileName))
            {
                content = File.ReadAllText(requestFileName);
                return true;
            }
            return false;
        }

        public virtual void LogErrors(System.CodeDom.Compiler.CompilerErrorCollection errors)
        {
            this.ErrorCollection = errors;
        }

        public virtual AppDomain ProvideTemplatingAppDomain(string content)
        {
            // return AppDomain.CreateDomain("CodeGenerator AppDomain");
            return AppDomain.CurrentDomain;
        }

        public virtual string ResolveAssemblyReference(string assemblyReference)
        {
            if (File.Exists(assemblyReference))
            {
                return assemblyReference;
            }

            string text = Path.Combine(Path.GetDirectoryName(this.TemplateFile), assemblyReference);
            if (File.Exists(text))
            {
                return text;
            }
            else
            {
                var assembly = AppDomain.CurrentDomain.Load(assemblyReference);
                if (assembly != null)
                {
                    text = assembly.Location;
                    if (File.Exists(text))
                    {
                        return text;
                    }
                }
            }

            return string.Empty;
        }

        public virtual Type ResolveDirectiveProcessor(string processorName)
        {
            string.Compare(processorName, "XYZ", StringComparison.OrdinalIgnoreCase);
            throw new Exception("没有找到指令处理器");
        }

        public virtual string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            if (directiveId == null)
            {
                throw new ArgumentNullException("the directiveId cannot be null");
            }

            if (processorName == null)
            {
                throw new ArgumentNullException("the processorName cannot be null");
            }

            if (parameterName == null)
            {
                throw new ArgumentNullException("the parameterName cannot be null");
            }

            return string.Empty;
        }

        public virtual string ResolvePath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("the path cannot be null");
            }

            if (File.Exists(path))
            {
                return path;
            }

            string text = Path.Combine(Path.GetDirectoryName(this.TemplateFile), path);
            if (File.Exists(text))
            {
                return text;
            }

            return path;
        }

        public virtual void SetFileExtension(string extension)
        {
            this.FileExtention = extension;
        }


        public virtual void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            this.OutputEncoding = encoding;
        }


        #endregion
    }
}
