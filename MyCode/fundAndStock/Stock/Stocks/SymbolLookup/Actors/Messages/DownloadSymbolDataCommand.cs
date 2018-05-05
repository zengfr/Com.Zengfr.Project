using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolLookup.Actors.Messages
{
    public class DownloadAllSymbolDataCommand : AbstractCommand
    {
        public DateTime? OpenDate { get; set; }
    }
        public class DownloadSymbolDataCommand : AbstractCommand
    {
        public string Symbol { get; set; }
        public DateTime? OpenDate { get; set; }

    }
}
