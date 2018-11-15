using System;
using NUnit.Framework;

namespace Calculator.Data.Test
{
    [TestFixture]
    public class AerodynamicDataTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup 
            var random = new Random(21);
            double aspectRatio = random.NextDouble();
            double wingArea = random.NextDouble();
            double zeroLiftAngleOfAttack = random.NextDouble();
            double liftCoefficientGradient = random.NextDouble();
            double maximumLiftCoefficient = random.NextDouble();
            double restDragCoefficientWithoutEngineFailure = random.NextDouble();
            double restDragCoefficientWithEngineFailure = random.NextDouble();
            double oswaldFactor = random.NextDouble();

            // Call
            var aerodynamicData = new AerodynamicData(aspectRatio, wingArea,
                                                      zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                      maximumLiftCoefficient,
                                                      restDragCoefficientWithoutEngineFailure,
                                                      restDragCoefficientWithEngineFailure, oswaldFactor);

            // Assert
            Assert.AreEqual(aspectRatio, aerodynamicData.AspectRatio);
            Assert.AreEqual(wingArea, aerodynamicData.WingArea);
            Assert.AreEqual(zeroLiftAngleOfAttack, aerodynamicData.ZeroLiftAngleOfAttack);
            Assert.AreEqual(liftCoefficientGradient, aerodynamicData.LiftCoefficientGradient);
            Assert.AreEqual(maximumLiftCoefficient, aerodynamicData.MaximumLiftCoefficient);
            Assert.AreEqual(restDragCoefficientWithoutEngineFailure, aerodynamicData.RestDragCoefficientWithoutEngineFailure);
            Assert.AreEqual(restDragCoefficientWithEngineFailure, aerodynamicData.RestDragCoefficientWithEngineFailure);
            Assert.AreEqual(oswaldFactor, aerodynamicData.OswaldFactor);
        }
    }
}