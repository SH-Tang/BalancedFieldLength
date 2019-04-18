using System.Reflection;
using NUnit.Framework;

namespace Core.Common.Version.Test
{
    [TestFixture]
    public class GlobalAssemblyInfoTest
    {
        [Test]
        public void GlobalAssemblySettings_Always_ReturnsExpectedValues()
        {
            // Setup
            Assembly assembly = Assembly.GetAssembly(typeof(GlobalAssemblyInfoTest));

            // Call 
            string assemblyProduct = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
            string assemblyTitle = assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;

            string assemblyFileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

            string assemblyDescription = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
            string assemblyConfiguration = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>().Configuration;
            string assemblyCompany = assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
            string assemblyCopyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            string assemblyTrademark = assembly.GetCustomAttribute<AssemblyTrademarkAttribute>().Trademark;
            
            // Assert
            Assert.AreEqual("Core.Common.Version.Test", assemblyProduct);
            Assert.AreEqual("Core.Common.Version.Test", assemblyTitle);

            Assert.AreEqual("1.0.0.0", assemblyFileVersion);
            Assert.AreEqual("Copyright Dennis Tang ©  2019", assemblyCopyright);

            Assert.IsEmpty(assemblyDescription);
            Assert.IsEmpty(assemblyConfiguration);
            Assert.IsEmpty(assemblyCompany);
            Assert.IsEmpty(assemblyTrademark);
        }
    }
}