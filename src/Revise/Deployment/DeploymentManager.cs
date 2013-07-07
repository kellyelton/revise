using System;

namespace Revise.Deployment
{
	public static class DeploymentManager
	{
		public static void Setup(Action<DeploymentManagerConfigBuilder> setup)
		{
			var builder = new DeploymentManagerConfigBuilder();
			setup(builder);
		}
	}
}
