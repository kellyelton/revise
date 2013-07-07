using System;
using System.IO;
using System.Linq;

namespace Revise.Deployment
{
	public class DeploymentManagerConfigBuilder
	{
		internal DeploymentManagerConfig Config { get; private set; }

		public DeploymentManagerConfigBuilder()
		{
			Config = new DeploymentManagerConfig();
		}

        /// <summary>
        /// Include file or files using wildcards and searching
        /// </summary>
        /// <param name="search"><see cref="SearchOption"/></param>
        /// <param name="findRelativePath">Relative paths(to Working Directory) of files to search for</param>
        /// <returns><see cref="DeploymentManagerConfigBuilder"/></returns>
		public DeploymentManagerConfigBuilder FindIncludeFile(SearchOption search, params string[] findRelativePath)
		{
			foreach (var frp in findRelativePath)
			{
				var fList = new DirectoryInfo(Config.WorkingDirectory.AbsolutePath).GetFiles(frp, search);
				foreach (var f in fList)
				{
					Config.IncludeFiles.Add(new Uri(f.FullName));
				}
			}
			return this;
		}

        /// <summary>
        /// Exclude file or files using wildcards and searching
        /// </summary>
        /// <param name="search"><see cref="SearchOption"/></param>
        /// <param name="findRelativePath">Relative paths(to Working Directory) of files to search for</param>
        /// <returns><see cref="DeploymentManagerConfigBuilder"/></returns>
		public DeploymentManagerConfigBuilder FindExcludeFile(SearchOption search, params string[] findRelativePath)
		{
			foreach (var frp in findRelativePath)
			{
				var fList = new DirectoryInfo(Config.WorkingDirectory.AbsolutePath).GetFiles(frp, search);
				foreach (var f in fList)
				{
					Config.ExcludeFiles.Add(new Uri(f.FullName));
				}
			}
			return this;
		}

        /// <summary>
        /// Include a file into the deployment
        /// </summary>
        /// <param name="relativePath">Relative path to include file, relative to Working Directory</param>
        /// <returns><see cref="DeploymentManagerConfigBuilder"/></returns>
		public DeploymentManagerConfigBuilder IncludeFile(params string[] relativePath)
		{
			foreach (var path in relativePath.Select(rPath => Path.Combine(Config.WorkingDirectory.AbsolutePath, rPath)))
			{
				if (!Directory.Exists(path))
					throw new DirectoryNotFoundException("Directory " + path + " now found.");
				if (!File.Exists(path))
					throw new FileNotFoundException("File " + path + " not found.");

				Config.IncludeFiles.Add(new Uri(path));
			}
			return this;
		}

        /// <summary>
        /// Exclude a file from the deployment.
        /// </summary>
        /// <param name="relativePath">Relative path to exclude file, relative to Working Directory</param>
        /// <returns><see cref="DeploymentManagerConfigBuilder"/></returns>
		public DeploymentManagerConfigBuilder ExcludeFile(params string[] relativePath)
		{
			foreach (var path in relativePath.Select(rPath => Path.Combine(Config.WorkingDirectory.AbsolutePath, rPath)))
			{
				if (!Directory.Exists(path))
					throw new DirectoryNotFoundException("Directory " + path + " now found.");
				if (!File.Exists(path))
					throw new FileNotFoundException("File " + path + " not found.");

				Config.ExcludeFiles.Add(new Uri(path));
			}
			return this;
		}

        /// <summary>
        /// Set the working directory of your deployment
        /// </summary>
        /// <param name="fullPath">Full path to your deployment</param>
        /// <returns><see cref="DeploymentManagerConfigBuilder"/></returns>
		public DeploymentManagerConfigBuilder SetWorkingDirectory(string fullPath)
		{
			if(!Directory.Exists(fullPath))
				throw new DirectoryNotFoundException("Directory " + fullPath + " now found.");
			if (File.Exists(fullPath))
				throw new InvalidOperationException("Cannot set working directory to file.");

			Config.WorkingDirectory = new Uri(fullPath);
			return this;
		}
	}
}
