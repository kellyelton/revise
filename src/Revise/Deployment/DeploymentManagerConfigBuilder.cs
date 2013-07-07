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
