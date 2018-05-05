//-----------------------------------------------------------------------
// <copyright file="DownloadSymbolData.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

namespace SymbolLookup.Actors.Messages
{
    public abstract class AbstractCommand
    {
        public virtual string sourceKey { get; set; }
        public virtual string sourceSubKey { get; set; }
    }
        public class HttpGetCommand: AbstractCommand
    {
      
        public string url { get; set; }
        public string encoding { get; set; }
    }
    public class HttpPostCommand: AbstractCommand
    {
        public string url { get; set; }
        public string encoding { get; set; }
         
    }
}

