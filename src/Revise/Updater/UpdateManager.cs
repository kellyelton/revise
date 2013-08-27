using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace Revise.Updater
{
    public class UpdateManager
    {
        internal static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Singleton

        internal static UpdateManager SingletonContext { get; set; }

        private static readonly object UpdateManagerSingletonLocker = new object();

        public static UpdateManager Instance
        {
            get
            {
                if (SingletonContext == null)
                {
                    lock (UpdateManagerSingletonLocker)
                    {
                        if (SingletonContext == null)
                        {
                            SingletonContext = new UpdateManager();
                        }
                    }
                }
                return SingletonContext;
            }
        }

        #endregion Singleton

        internal List<IUpdateSource> UpdateSources { get; set; }

        internal UpdateManager()
        {
            UpdateSources = new List<IUpdateSource>();
        }

        public void AddSource(IUpdateSource source)
        {
            lock (UpdateSources)
                UpdateSources.Add(source);
        }

        public async Task<IEnumerable<IUpdateSource>> CheckForUpdates()
        {
            IUpdateSource[] localList = null;
            lock (UpdateSources)
            {
                localList = UpdateSources.ToArray();
            }
            var ret = new List<IUpdateSource>();
            foreach (var source in localList)
            {
                try
                {
                    var avail = await source.IsUpdateAvailable();
                    if (avail)
                    {
                        Log.Debug("[CheckForUpdates] Update available for " + source.Name);
                    }
                    else
                    {
                        Log.Debug("[CheckForUpdates] No update available for " + source.Name);
                    }
                }
                catch (System.Exception e)
                {
                    Log.Warn("[CheckForUpdates] Error checking for update for " + source.Name, e);
                } 
            }
            return ret;
        }

        public async Task<IUpdatePackage> DownloadUpdate(IUpdateSource source)
        {
            try
            {
                var tempDir = Path.GetTempPath();
                var location = new DirectoryInfo(Path.Combine(tempDir,"Revise",source.Name,Guid.NewGuid().ToString()));
                return await source.DownloadUpdate(location);
            }
            catch (Exception e)
            {
                Log.Warn("[DownloadUpdate] Error download update for " + source.Name,e);
                return null;
            }
        }
    }
}