using System;
using Core.Common.Data;
using NUnit.Framework;

namespace Simulator.Data.Test
{
    [TestFixture]
    public class AerodynamicsDataTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup 
            var random = new Random(21);
            double aspectRatio = random.NextDouble();
            double wingArea = random.NextDouble();
            Angle zeroLiftAngleOfAttack = Angle.FromDegrees(random.NextDouble());
            double liftCoefficientGradient = random.NextDouble();
            double maximumLiftCoefficient = random.NextDouble();
            double restDragCoefficientWithoutEngineFailure = random.NextDouble();
            double restDragCoefficientWithEngineFailure = random.NextDouble();
            double oswaldFactor = random.NextDouble();

            // Call
            var data = new AerodynamicsData(aspectRatio, wingArea,
                                                      zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                      maximumLiftCoefficient,
                                                      restDragCoefficientWithoutEngineFailure,
                                                      restDragCoefficientWithEngineFailure, oswaldFactor);

            // Assert
            Assert.AreEqual(aspectRatio, data.AspectRatio);
            Assert.AreEqual(wingArea, data.WingArea);
            Assert.AreSame(zeroLiftAngleOfAttack, data.ZeroLiftAngleOfAttack);
            Assert.AreEqual(liftCoefficientGradient, data.LiftCoefficientGradient);
            Assert.AreEqual(maximumLiftCoefficient, data.MaximumLiftCoefficient);
            Assert.AreEqual(restDragCoefficientWithoutEngineFailure, data.RestDragCoefficientWithoutEngineFailure);
            Assert.AreEqual(restDragCoefficientWithEngineFailure, data.RestDragCoefficientWithEngineFailure);
            Assert.AreEqual(oswaldFactor, data.OswaldFactor);
        }
    }
}