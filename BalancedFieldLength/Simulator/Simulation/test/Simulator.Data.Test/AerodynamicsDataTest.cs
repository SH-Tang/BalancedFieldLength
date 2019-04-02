using System;
using System.Collections.Generic;
using Core.Common.Data;
using Core.Common.TestUtil;
using NUnit.Framework;

namespace Simulator.Data.Test
{
    [TestFixture]
    public class AerodynamicsDataTest
    {
        [Test]
        [TestCaseSource(nameof(GetInvalidValuesForPropertiesLargerThanZero))]
        public void Constructor_InvalidValuesForPropertiesLargerThanZero_ThrowsArgumentOutOfRangeException(Action constructorAction,
                                                                                                           string propertyName)
        {
            // Call
            TestDelegate call = () => constructorAction();

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger than 0.");
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidValuesForPropertiesLargerOrEqualToZero))]
        public void Constructor_InvalidValuesForPropertiesLargerOrEqualToZero_ThrowsArgumentOutOfRangeException(Action constructorAction,
                                                                                                                string propertyName)
        {
            // Call
            TestDelegate call = () => constructorAction();

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger or equal to 0.");
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidConcreteValuesForProperties))]
        public void Constructor_ValuesNaNOrInfinity_ThrowsArgumentException(Action constructorAction, string propertyName)
        {
            // Call
            TestDelegate call = () => constructorAction();

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, $"{propertyName} must be a concrete number and cannot be NaN or Infinity.");
        }

        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup 
            var random = new Random(21);
            double aspectRatio = random.NextDouble();
            double wingArea = random.NextDouble();
            Angle zeroLiftAngleOfAttack = random.NextAngle();
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
            Assert.AreEqual(zeroLiftAngleOfAttack.Degrees, data.ZeroLiftAngleOfAttack.Degrees);
            Assert.AreEqual(liftCoefficientGradient, data.LiftCoefficientGradient);
            Assert.AreEqual(maximumLiftCoefficient, data.MaximumLiftCoefficient);
            Assert.AreEqual(restDragCoefficientWithoutEngineFailure, data.RestDragCoefficientWithoutEngineFailure);
            Assert.AreEqual(restDragCoefficientWithEngineFailure, data.RestDragCoefficientWithEngineFailure);
            Assert.AreEqual(oswaldFactor, data.OswaldFactor);
        }

        #region Test Data

        private static IEnumerable<TestCaseData> GetInvalidValuesForPropertiesLargerOrEqualToZero()
        {
            var random = new Random(21);
            double aspectRatio = random.NextDouble();
            double wingArea = random.NextDouble();
            Angle zeroLiftAngleOfAttack = random.NextAngle();
            double liftCoefficientGradient = random.NextDouble();
            double maximumLiftCoefficient = random.NextDouble();
            double restDragCoefficientWithoutEngineFailure = random.NextDouble();
            double restDragCoefficientWithEngineFailure = random.NextDouble();
            double oswaldFactor = random.NextDouble();

            const string dragCoefficientWithoutEngineFailurePropertyName = "restDragCoefficientWithoutEngineFailure";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                -1e-1,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          dragCoefficientWithoutEngineFailurePropertyName)
                .SetName($"{dragCoefficientWithoutEngineFailurePropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                double.NegativeInfinity,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          dragCoefficientWithoutEngineFailurePropertyName)
                .SetName($"{dragCoefficientWithoutEngineFailurePropertyName} NegativeInfinity");

            const string dragCoefficientWithEngineFailurePropertyName = "restDragCoefficientWithEngineFailure";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                -1e-1, oswaldFactor)),
                                          dragCoefficientWithEngineFailurePropertyName)
                .SetName($"{dragCoefficientWithEngineFailurePropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                double.NegativeInfinity, oswaldFactor)),
                                          dragCoefficientWithEngineFailurePropertyName)
                .SetName($"{dragCoefficientWithEngineFailurePropertyName} NegativeInfinity");
        }

        private static IEnumerable<TestCaseData> GetInvalidValuesForPropertiesLargerThanZero()
        {
            var random = new Random(21);
            double aspectRatio = random.NextDouble();
            double wingArea = random.NextDouble();
            Angle zeroLiftAngleOfAttack = random.NextAngle();
            double liftCoefficientGradient = random.NextDouble();
            double maximumLiftCoefficient = random.NextDouble();
            double restDragCoefficientWithoutEngineFailure = random.NextDouble();
            double restDragCoefficientWithEngineFailure = random.NextDouble();
            double oswaldFactor = random.NextDouble();

            const string aspectRatioPropertyName = "aspectRatio";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(0, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          aspectRatioPropertyName)
                .SetName($"{aspectRatioPropertyName} Zero");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(-1e-6, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          aspectRatioPropertyName)
                .SetName($"{aspectRatioPropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(double.NegativeInfinity, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          aspectRatioPropertyName)
                .SetName($"{aspectRatioPropertyName} NegativeInfinity");

            const string wingAreaPropertyName = "wingArea";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, 0,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          wingAreaPropertyName)
                .SetName($"{wingAreaPropertyName} Zero");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, -1e-6,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          wingAreaPropertyName)
                .SetName($"{wingAreaPropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, double.NegativeInfinity,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          wingAreaPropertyName)
                .SetName($"{wingAreaPropertyName} NegativeInfinity");

            const string liftCoefficientGradientPropertyName = "liftCoefficientGradient";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, 0,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          liftCoefficientGradientPropertyName)
                .SetName($"{liftCoefficientGradientPropertyName} Zero");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, -1e-6,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          liftCoefficientGradientPropertyName)
                .SetName($"{liftCoefficientGradientPropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, double.NegativeInfinity,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          liftCoefficientGradientPropertyName)
                .SetName($"{liftCoefficientGradientPropertyName} NegativeInfinity");

            const string maximumLiftCoefficientPropertyName = "maximumLiftCoefficient";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                0,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          maximumLiftCoefficientPropertyName)
                .SetName($"{maximumLiftCoefficientPropertyName} Zero");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                -1e-6,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          maximumLiftCoefficientPropertyName)
                .SetName($"{maximumLiftCoefficientPropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                double.NegativeInfinity,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          maximumLiftCoefficientPropertyName)
                .SetName($"{maximumLiftCoefficientPropertyName} NegativeInfinity");

            const string oswaldFactorPropertyName = "oswaldFactor";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, 0)),
                                          oswaldFactorPropertyName)
                .SetName($"{oswaldFactorPropertyName} Zero");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, -1e-6)),
                                          oswaldFactorPropertyName)
                .SetName($"{oswaldFactorPropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, double.NegativeInfinity)),
                                          oswaldFactorPropertyName)
                .SetName($"{oswaldFactorPropertyName} NegativeInfinity");
        }

        private static IEnumerable<TestCaseData> GetInvalidConcreteValuesForProperties()
        {
            var random = new Random(21);
            double aspectRatio = random.NextDouble();
            double wingArea = random.NextDouble();
            Angle zeroLiftAngleOfAttack = random.NextAngle();
            double liftCoefficientGradient = random.NextDouble();
            double maximumLiftCoefficient = random.NextDouble();
            double restDragCoefficientWithoutEngineFailure = random.NextDouble();
            double restDragCoefficientWithEngineFailure = random.NextDouble();
            double oswaldFactor = random.NextDouble();

            const string aspectRatioPropertyName = "aspectRatio";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(double.NaN, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          aspectRatioPropertyName)
                .SetName($"{aspectRatioPropertyName}  NaN");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(double.PositiveInfinity, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          aspectRatioPropertyName)
                .SetName($"{aspectRatioPropertyName} PositiveInfinity");

            const string wingAreaPropertyName = "wingArea";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, double.NaN,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          wingAreaPropertyName)
                .SetName($"{wingAreaPropertyName} NaN");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, double.PositiveInfinity,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          wingAreaPropertyName)
                .SetName($"{wingAreaPropertyName} PositiveInfinity");

            const string zeroLiftAngleOfAttackPropertyName = "zeroLiftAngleOfAttack";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                Angle.FromRadians(double.NaN), liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          zeroLiftAngleOfAttackPropertyName)
                .SetName($"{zeroLiftAngleOfAttackPropertyName} NaN");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                Angle.FromRadians(double.PositiveInfinity), liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          zeroLiftAngleOfAttackPropertyName)
                .SetName($"{zeroLiftAngleOfAttackPropertyName} PositiveInfinity");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                Angle.FromRadians(double.NegativeInfinity), liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          zeroLiftAngleOfAttackPropertyName)
                .SetName($"{zeroLiftAngleOfAttackPropertyName} NegativeInfinity");

            const string liftCoefficientGradientPropertyName = "liftCoefficientGradient";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, double.NaN,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          liftCoefficientGradientPropertyName)
                .SetName($"{liftCoefficientGradientPropertyName} NaN");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, double.PositiveInfinity,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          liftCoefficientGradientPropertyName)
                .SetName($"{liftCoefficientGradientPropertyName} PositiveInfinity");

            const string maximumLiftCoefficientPropertyName = "maximumLiftCoefficient";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                double.NaN,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          maximumLiftCoefficientPropertyName)
                .SetName($"{maximumLiftCoefficientPropertyName} Zero");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                double.PositiveInfinity,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          maximumLiftCoefficientPropertyName)
                .SetName($"{maximumLiftCoefficientPropertyName} PositiveInfinity");

            const string dragCoefficientWithoutEngineFailurePropertyName = "restDragCoefficientWithoutEngineFailure";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                double.NaN,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          dragCoefficientWithoutEngineFailurePropertyName)
                .SetName($"{dragCoefficientWithoutEngineFailurePropertyName} NaN");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                double.PositiveInfinity,
                                                                                restDragCoefficientWithEngineFailure, oswaldFactor)),
                                          dragCoefficientWithoutEngineFailurePropertyName)
                .SetName($"{dragCoefficientWithoutEngineFailurePropertyName} PositiveInfinity");

            const string dragCoefficientWithEngineFailurePropertyName = "restDragCoefficientWithEngineFailure";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                double.NaN, oswaldFactor)),
                                          dragCoefficientWithEngineFailurePropertyName)
                .SetName($"{dragCoefficientWithEngineFailurePropertyName}NaN");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                double.PositiveInfinity, oswaldFactor)),
                                          dragCoefficientWithEngineFailurePropertyName)
                .SetName($"{dragCoefficientWithEngineFailurePropertyName}PositiveInfinity");

            const string oswaldFactorPropertyName = "oswaldFactor";
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, double.NaN)),
                                          oswaldFactorPropertyName)
                .SetName($"{oswaldFactorPropertyName} NaN");
            yield return new TestCaseData(new Action(() => new AerodynamicsData(aspectRatio, wingArea,
                                                                                zeroLiftAngleOfAttack, liftCoefficientGradient,
                                                                                maximumLiftCoefficient,
                                                                                restDragCoefficientWithoutEngineFailure,
                                                                                restDragCoefficientWithEngineFailure, double.PositiveInfinity)),
                                          oswaldFactorPropertyName)
                .SetName($"{oswaldFactorPropertyName} PositiveInfinity");
        }

        #endregion
    }
}