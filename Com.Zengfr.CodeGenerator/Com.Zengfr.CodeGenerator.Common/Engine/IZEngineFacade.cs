using System.Collections.Generic;
using System.IO;
using Commons.Collections;

namespace Com.Zengfr.CodeGenerator.Common.Engine
{
    public interface IZEngineFacade
    {
        void Init(string templateDir, bool isInWeb);
        void Init(ExtendedProperties props);
        string Process(string templatFileName, IDictionary<string, object> kvs);
        void Process(TextWriter writer, string templatFileName, IDictionary<string, object> kvs);
    }
}