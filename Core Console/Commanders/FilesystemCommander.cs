using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleCommander;

namespace Sample_Console.Commanders
{
    public class FilesystemCommander : CommanderBase
    {
        #region Properties

        private IConfiguration config { get; }

        #endregion

        #region Constructor(s)

        public FilesystemCommander(IConfiguration config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            registerCommand("x", "watchSystemdrive", watchSystemdrive);
            registerCommand("1", nameof(ListDirectoryContents), ListDirectoryContents);
        }

        private void ListDirectoryContents()
        {
            var curDir = Directory.GetCurrentDirectory();
            var dir = this.requestValue("Dir: ", curDir);

            if (Directory.Exists(dir))
            {
                var contents = Directory.GetFileSystemEntries(dir)
                    .OrderBy(s => s);

                this.WriteAsTable(contents, new Dictionary<string, Func<string, object>>() {
                    { "Name", s => s.StringLimit(15) },
                    { "Length", s => s.Length },
                    { "Lower", s => s.StringLimit(15).ToLower() },
                    { "Upper", s => s.StringLimit(15).ToUpper() },
                    { "PadLeft", s => s.StringLimit(15).PadLeft(20, '.') },
                    { "PadRight", s => s.StringLimit(15).PadRight(20, '.') }
                });
            }
            else {
                this.Error($"Directory '{dir}' doesn't exist!");
                return;
            }
        }

        #endregion

        private bool IsWatching = false;

        public void watchSystemdrive()
        {
            var fsw = new System.IO.FileSystemWatcher("c:\\");

            if (!IsWatching)
            {
                fsw.Changed += Fsw_Changed;

                IsWatching = true;

                while (IsWatching)
                {
                    //fsw.SynchronizingObject.
                    var result = fsw.WaitForChanged(System.IO.WatcherChangeTypes.All);
                    this.Warning($"{result.ChangeType}: '{result.Name}'.");
                }

            }
        }

        private void Fsw_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            this.Info($"Something changed: '{e.FullPath}'.");
        }
    }
}
