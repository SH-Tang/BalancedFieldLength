using Application.BalancedFieldLength.Controls;
using NUnit.Framework;

namespace Application.BalancedFieldLength.Test.Controls
{
    [TestFixture]
    public class OutputViewModelTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var viewModel = new OutputViewModel();

            // Assert
            Assert.That(viewModel.BalancedFieldLengthDistance, Is.NaN);
            Assert.That(viewModel.BalancedFieldLengthVelocity, Is.NaN);
        }
    }
}