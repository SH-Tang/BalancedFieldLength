// Copyright (C) 2018 Dennis Tang. All rights reserved.
//
// This file is part of Balanced Field Length.
//
// Balanced Field Length is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

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

            Assert.AreEqual("0.2.1.0", assemblyFileVersion);
            Assert.AreEqual("Copyright Dennis Tang ©  2019", assemblyCopyright);

            Assert.IsEmpty(assemblyDescription);
            Assert.IsEmpty(assemblyConfiguration);
            Assert.IsEmpty(assemblyCompany);
            Assert.IsEmpty(assemblyTrademark);
        }
    }
}