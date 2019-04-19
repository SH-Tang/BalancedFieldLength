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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Core.Common.TestUtil
{
    /// <summary>
    /// General test helper which can be used for testing.
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Asserts if a call results in an (instance of) <see cref="ArgumentException"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ArgumentException"/>
        /// to be thrown.</typeparam>
        /// <param name="call">The <see cref="TestDelegate"/> which results in an <see cref="ArgumentException"/>.</param>
        /// <param name="expectedMessage">The message that is expected.</param>
        /// <exception cref="Exception">Any exception that is not an instance of <see cref="ArgumentException"/>.</exception>
        /// <exception cref="AssertionException">Thrown when the exception message does not match with <paramref name="expectedMessage"/>.</exception>
        public static void AssertThrowsArgumentException<T>(TestDelegate call, string expectedMessage)
            where T : ArgumentException
        {
            var exception = Assert.Throws<T>(call);
            string message = exception.Message;
            if (exception.ParamName != null)
            {
                List<string> customMessageParts = message.Split(new[]
                                                                {
                                                                    Environment.NewLine
                                                                }, StringSplitOptions.None).ToList();
                customMessageParts.RemoveAt(customMessageParts.Count - 1);

                message = string.Join(Environment.NewLine, customMessageParts.ToArray());
            }

            Assert.AreEqual(expectedMessage, message);
        }

        /// <summary>
        /// Gets the path of the solution root.
        /// </summary>
        /// <returns>A path of the solution root.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the solution root could not be found.</exception>
        public static string GetSolutionRootPath()
        {
            const string solutionName = "BalancedFieldLength.sln";
            //get the current directory and scope up
            //TODO find a faster safer method 
            var testContext = new TestContext(new TestExecutionContext.AdhocContext());
            string curDir = testContext.TestDirectory;
            while (Directory.Exists(curDir) && !File.Exists(curDir + @"\" + solutionName))
            {
                curDir += "/../";
            }

            if (!File.Exists(Path.Combine(curDir, solutionName)))
            {
                throw new InvalidOperationException($"Solution file '{solutionName}' not found in any folder of '{Directory.GetCurrentDirectory()}'.");
            }

            return Path.GetFullPath(curDir);
        }
    }
}