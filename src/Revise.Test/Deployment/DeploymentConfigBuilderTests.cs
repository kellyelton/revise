namespace Revise.Test.Deployment
{
    using System.IO;

    using Revise.Deployment;
    using NUnit.Framework;

    public class DeploymentConfigBuilderTests
    {
        [Test]
        public void FindIncludeFile_Works()
        {
            var builder = new DeploymentManagerConfigBuilder();

            builder.FindIncludeFile(SearchOption.TopDirectoryOnly,"*.dll");

            Assert.AreEqual(1,builder.Config.IncludeFiles.Count);
        }

        [Test]
        public void FindExcludeFile_Works()
        {
            var builder = new DeploymentManagerConfigBuilder();

            builder.FindExcludeFile(SearchOption.TopDirectoryOnly,"*.pdb");

            Assert.AreEqual(1,builder.Config.ExcludeFiles.Count);
        }

        [Test]
        public void IncludeFile_Works()
        {
            var builder = new DeploymentManagerConfigBuilder();

            builder.IncludeFile("Revise.dll");

            Assert.AreEqual(1, builder.Config.IncludeFiles.Count);
        }

        [Test]
        public void ExcludeFile_Works()
        {
            var builder = new DeploymentManagerConfigBuilder();

            builder.ExcludeFile("Revise.dll");

            Assert.AreEqual(1, builder.Config.ExcludeFiles.Count);
        }
    }
}