using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

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
    }
}