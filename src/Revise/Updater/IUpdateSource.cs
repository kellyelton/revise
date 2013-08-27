using System;
using System.IO;
using System.Threading.Tasks;

namespace Revise.Updater
{
    public interface IUpdateSource
    {
        /// <summary>
        /// Name of this <see cref="IUpdateSource"/>. Used mostly for logging/debugging.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Current version we are at
        /// </summary>
        Version CurrentVersion { get; }

        event EventHandler<ProgressEventArgs> OnDownloadUpdateProgressChange;

        /// <summary>
        /// Checks to see if there is an update available
        /// </summary>
        /// <returns>True if there is, False if there isn't</returns>
        Task<bool> IsUpdateAvailable();

        /// <summary>
        /// Checks to see if there is a specific package with a specific version exists
        /// </summary>
        /// <param name="version">Version to see if exists</param>
        /// <returns>True if there is one, False otherwise</returns>
        Task<bool> UpdateVersionExists(Version version);

        /// <summary>
        /// Download an update 
        /// </summary>
        /// <param name="downloadLocation">Folder to download the package to</param>
        /// <param name="specificVersion">Specific version to download if specified, null for latest</param>
        /// <returns><see cref="IUpdatePackage"/>On success, otherwise null</returns>
        Task<IUpdatePackage> DownloadUpdate(DirectoryInfo downloadLocation, Version specificVersion = null);
    }

    public interface IUpdatePackage
    {
        event EventHandler<ProgressEventArgs> OnExtractProgressChange;

        /// <summary>
        /// File path of the <see cref="IUpdatePackage"/>
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// <see cref="Version"/> of the <see cref="IUpdatePackage"/>
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Extract the package into a directory
        /// </summary>
        /// <param name="extractDirectory">Directory to extract to</param>
        /// <returns>True if it succeeds, otherwise False</returns>
        Task<bool> Extract(DirectoryInfo extractDirectory);
    }

    public class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Progress out of 100%
        /// </summary>
        public int Progress { get; internal set; }

        /// <summary>
        /// Current message for the progress. Set to null if we don't want to use the last used message.
        /// </summary>
        public string Message { get; internal set; }

        public ProgressEventArgs(int progress):this(progress,null)
        {
            
        }

        public ProgressEventArgs(int progress, string message, params object[] args)
        {
            Progress = progress;
            Message = message == null ? null : string.Format(message, args);
        }
    }
}