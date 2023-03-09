using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public class WatcherFileComponent: Entity, IAwake<string>, IDestroy
    {
        public FileSystemWatcher FileWatcher;
    }
}