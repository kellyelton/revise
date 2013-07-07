using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Revise.Deployment
{
	public class DeploymentManagerConfig
	{
		public Uri WorkingDirectory { get; internal set; }
		public List<Uri> IncludeFiles { get; internal set; }
		public List<Uri> ExcludeFiles { get; internal set; }
		
		internal DeploymentManagerConfig()
		{
			IncludeFiles = new List<Uri>();
			ExcludeFiles = new List<Uri>();
			WorkingDirectory = new Uri(new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName);
		}
	}
}