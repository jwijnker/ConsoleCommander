using Microsoft.Extensions.Configuration;
using System;
using WT.ConsoleCommander;

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
            registerCommand("1", "watchSystemdrive", watchSystemdrive);

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
