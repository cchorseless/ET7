using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class WatcherFileComponentSystem
    {
        [ObjectSystem]
        public class WatcherFileComponentSystemAwakeSystem: AwakeSystem<WatcherFileComponent, string>
        {
            protected override void Awake(WatcherFileComponent self, string path)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var FileWatcher = new FileSystemWatcher();
                FileWatcher.Path = path;
                FileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.DirectoryName;
                FileWatcher.IncludeSubdirectories = false;
                FileWatcher.Created += new FileSystemEventHandler(self.OnFileCreated);
                FileWatcher.Changed += new FileSystemEventHandler(self.OnFileChanged);
                // FileWatcher.Deleted += new FileSystemEventHandler(self.OnFileDeleted);
                // FileWatcher.Renamed += new RenamedEventHandler(self.OnFileRenamed);
                self.FileWatcher = FileWatcher;
                self.FileWatcher.EnableRaisingEvents = true;
            }
        }

        [ObjectSystem]
        public class WatcherFileComponentSystemDestroySystem: DestroySystem<WatcherFileComponent>
        {
            protected override void Destroy(WatcherFileComponent self)
            {
                self.FileWatcher.EnableRaisingEvents = false;
                self.FileWatcher.Dispose();
                self.FileWatcher = null;
            }
        }

        static void OnFileCreated(this WatcherFileComponent self, object sender, FileSystemEventArgs e)
        {
        }

        static void OnFileChanged(this WatcherFileComponent self, object sender, FileSystemEventArgs e)
        {
            if (e.Name.Contains("WatcherLog"))
            {
                var txt = File.ReadAllLines(e.FullPath, Encoding.UTF8);
                var lastline = txt.Last();
                if (lastline.Contains("Reload"))
                {
                    self.DelayReload().Coroutine();
                }
            }
        }

        static async ETTask DelayReload(this WatcherFileComponent self)
        {
            // 需要延时，不然有BUG读不到文件
            await TimerComponent.Instance.WaitAsync(1000);
            await ReloadDllConsoleHandler.Handle();
        }

        static void OnFileDeleted(this WatcherFileComponent self, object sender, FileSystemEventArgs e)
        {
        }

        static void OnFileRenamed(this WatcherFileComponent self, object sender, RenamedEventArgs e)
        {
        }
    }
}