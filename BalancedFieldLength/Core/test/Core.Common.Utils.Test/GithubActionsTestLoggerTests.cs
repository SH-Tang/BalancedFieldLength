using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Core.Common.Utils.Test
{
    [TestFixture]
    public class GithubActionsTestLoggerTests
    {
        [Test]
        public void TestGithubActionTestLogger_With_FailingTest()
        {
            Assert.Fail("This test will fail.");
        }

        [Test]
        public void TestGithubActionTestLogger_With_TestThrowingException()
        {
            throw new Exception("Exception test");
        }

        [Test]
        [Ignore("Test must be explicitly run")]
        public void TestGithubActionTestLogger_With_IgnoredTest()
        {
            Assert.Fail("This test will fail.");
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases))]
        public void TestGithubActionTestLogger_WithVariousTestCases_ReturnsExpectedResult(bool mustFail)
        {
            if (mustFail)
            {
                Assert.Fail("This test will fail.");
            }
            else
            {
                Assert.Pass("Test passed");
            }
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(true);
            yield return new TestCaseData(false);
        }
    }
}