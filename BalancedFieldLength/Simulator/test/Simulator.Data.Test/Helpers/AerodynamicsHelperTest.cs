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
using Core.Common.Data;
using Core.Common.TestUtil;
using NUnit.Framework;
using Simulator.Data.Exceptions;
using Simulator.Data.Helpers;
using Simulator.Data.TestUtil;

namespace Simulator.Data.Test.Helpers
{
    [TestFixture]
    public class AerodynamicsHelperTest
    {
        private const double airDensity = SimulationConstants.Density;
        private const double tolerance = SimulationConstants.Tolerance;

        [Test]
        public static void CalculateDragWithoutEngineFailure_AerodynamicsDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateDragWithoutEngineFailure(null,
                                                                                           random.NextDouble(),
                                                                                           random.NextDouble(),
                                                                                           random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicsData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidLiftCoefficientTestCases))]
        public static void CalculateDragWithoutEngineFailure_InvalidLiftCoefficient_ThrowsInvalidCalculationException(AerodynamicsData aerodynamicsData,
                                                                                                                      double invalidLiftCoefficient)
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateDragWithoutEngineFailure(aerodynamicsData,
                                                                                           invalidLiftCoefficient,
                                                                                           random.NextDouble(),
                                                                                           random.NextDouble());

            // Assert
            var exception = Assert.Throws<InvalidCalculationException>(call);
            Assert.AreEqual("Lift coefficient must be in the range of [0, CLMax].", exception.Message);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicsDataTestCases))]
        public static void CalculateDragWithoutEngineFailure_WithValidParameters_ReturnsExpectedValues(AerodynamicsData aerodynamicsData)
        {
            // Setup
            var random = new Random(21);
            double liftCoefficient = random.NextDouble();
            double velocity = random.NextDouble();

            // Call 
            double drag = AerodynamicsHelper.CalculateDragWithoutEngineFailure(aerodynamicsData, liftCoefficient, airDensity, velocity);

            // Assert
            double expectedDrag = CalculateExpectedDrag(aerodynamicsData, liftCoefficient, airDensity, velocity, false);
            Assert.AreEqual(expectedDrag, drag, tolerance);
        }

        [Test]
        public static void CalculateDragWithEngineFailure_AerodynamicsDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateDragWithEngineFailure(null,
                                                                                        random.NextDouble(),
                                                                                        random.NextDouble(),
                                                                                        random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicsData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidLiftCoefficientTestCases))]
        public static void CalculateDragWithEngineFailure_InvalidLiftCoefficient_ThrowsInvalidCalculationException(AerodynamicsData aerodynamicsData,
                                                                                                                   double invalidLiftCoefficient)
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateDragWithEngineFailure(aerodynamicsData,
                                                                                        invalidLiftCoefficient,
                                                                                        random.NextDouble(),
                                                                                        random.NextDouble());

            // Assert
            var exception = Assert.Throws<InvalidCalculationException>(call);
            Assert.AreEqual("Lift coefficient must be in the range of [0, CLMax].", exception.Message);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicsDataTestCases))]
        public static void CalculateDragWithEngineFailure_WithValidParameters_ReturnsExpectedValues(AerodynamicsData aerodynamicsData)
        {
            // Setup
            var random = new Random(21);
            double liftCoefficient = random.NextDouble();
            double velocity = random.NextDouble();

            // Call 
            double drag = AerodynamicsHelper.CalculateDragWithEngineFailure(aerodynamicsData, liftCoefficient, airDensity, velocity);

            // Assert
            double expectedDrag = CalculateExpectedDrag(aerodynamicsData, liftCoefficient, airDensity, velocity, true);
            Assert.AreEqual(expectedDrag, drag, tolerance);
        }

        [Test]
        public static void CalculateStallSpeed_AerodynamicsDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateStallSpeed(null,
                                                                             random.NextDouble(),
                                                                             random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicsData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicsDataTestCases))]
        public static void CalculateStallSpeed_WithValidParametersWithinLimits_ReturnsExpectedValues(AerodynamicsData aerodynamicsData)
        {
            // Setup
            const double weight = 500e3; // N

            // Call 
            double stallSpeed = AerodynamicsHelper.CalculateStallSpeed(aerodynamicsData, weight, airDensity);

            // Assert
            double expectedStallSpeed = Math.Sqrt(2 * weight / (aerodynamicsData.MaximumLiftCoefficient * airDensity * aerodynamicsData.WingArea));
            Assert.AreEqual(expectedStallSpeed, stallSpeed, tolerance);
        }

        [Test]
        public void CalculateLiftCoefficient_AerodynamicsDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call
            TestDelegate call = () => AerodynamicsHelper.CalculateLiftCoefficient(null, random.NextAngle());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicsData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidAngleOfAttackTestCases))]
        public static void CalculateLiftCoefficient_WithInvalidAngleOfAttack_ThrowsInvalidCalculationException(AerodynamicsData aerodynamicsData,
                                                                                                               Angle invalidAngleOfAttack,
                                                                                                               string expectedExceptionMessage)
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateLiftCoefficient(aerodynamicsData,
                                                                                  invalidAngleOfAttack);

            // Assert
            var exception = Assert.Throws<InvalidCalculationException>(call);
            Assert.AreEqual(expectedExceptionMessage, exception.Message);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicsDataTestCases))]
        public static void CalculateLiftCoefficient_WithAerodynamicsData_ReturnsExpectedLiftCoefficient(AerodynamicsData aerodynamicsData)
        {
            // Setup
            var random = new Random(21);
            Angle angleOfAttack = random.NextAngle();

            // Call 
            double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aerodynamicsData,
                                                                                 angleOfAttack);

            // Assert
            double expectedLiftCoefficient = CalculateExpectedLiftCoefficient(aerodynamicsData, angleOfAttack);
            Assert.AreEqual(expectedLiftCoefficient, liftCoefficient, tolerance);
        }

        [Test]
        public static void CalculateLift_AerodynamicsDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateLift(null,
                                                                       new Angle(),
                                                                       random.NextDouble(),
                                                                       random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicsData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidAngleOfAttackTestCases))]
        public static void CalculateLift_WithInvalidAngleOfAttack_ThrowsInvalidCalculationException(AerodynamicsData aerodynamicsData,
                                                                                                    Angle invalidAngleOfAttack,
                                                                                                    string expectedExceptionMessage)
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateLift(aerodynamicsData,
                                                                       invalidAngleOfAttack,
                                                                       airDensity,
                                                                       random.NextDouble());

            // Assert
            var exception = Assert.Throws<InvalidCalculationException>(call);
            Assert.AreEqual(expectedExceptionMessage, exception.Message);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicsDataTestCases))]
        public static void CalculateLift_WithValidParametersAndWithinLimits_ReturnsExpectedValues(AerodynamicsData aerodynamicsData)
        {
            // Setup
            Angle angleOfAttack = Angle.FromDegrees(3.0);
            const int velocity = 10; // m/s

            // Call 
            double lift = AerodynamicsHelper.CalculateLift(aerodynamicsData,
                                                           angleOfAttack,
                                                           airDensity,
                                                           velocity);
            // Assert
            Assert.AreEqual(CalculateExpectedLift(aerodynamicsData, angleOfAttack, airDensity, velocity), lift);
        }

        [Test]
        [TestCaseSource(nameof(GetNegativeVelocityTestCases))]
        public static void VariousCalculations_NegativeVelocity_ThrowsInvalidCalculationException(Action helperCalculationAction)
        {
            // Call 
            TestDelegate call = () => helperCalculationAction();

            // Assert
            var exception = Assert.Throws<InvalidCalculationException>(call);
            Assert.AreEqual("Velocity must be larger or equal to 0.", exception.Message);
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidDensityTestCases))]
        public static void VariousCalculations_InvalidDensity_ThrowsInvalidCalculationException(Action helperCalculationAction)
        {
            // Call 
            TestDelegate call = () => helperCalculationAction();

            // Assert
            var exception = Assert.Throws<InvalidCalculationException>(call);
            Assert.AreEqual("Density must be larger than 0.", exception.Message);
        }

        private static double CalculateExpectedLift(AerodynamicsData aerodynamicsData,
                                                    Angle angleOfAttack,
                                                    double density,
                                                    double velocity)
        {
            double liftCoefficient = CalculateExpectedLiftCoefficient(aerodynamicsData, angleOfAttack);
            return liftCoefficient * CalculateDynamicPressure(velocity, density, aerodynamicsData.WingArea);
        }

        private static double CalculateExpectedLiftCoefficient(AerodynamicsData aerodynamicsData, Angle angleOfAttack)
        {
            return aerodynamicsData.LiftCoefficientGradient *
                   (angleOfAttack.Radians - aerodynamicsData.ZeroLiftAngleOfAttack.Radians);
        }

        private static double CalculateExpectedDrag(AerodynamicsData aerodynamicsData,
                                                    double liftCoefficient,
                                                    double density,
                                                    double velocity,
                                                    bool hasEngineFailed)
        {
            double staticDragCoefficient = hasEngineFailed
                                               ? aerodynamicsData.RestDragCoefficientWithEngineFailure
                                               : aerodynamicsData.RestDragCoefficientWithoutEngineFailure;

            double inducedDragCoefficient = Math.Pow(liftCoefficient, 2) / (Math.PI * aerodynamicsData.AspectRatio * aerodynamicsData.OswaldFactor);

            double totalDragCoefficient = staticDragCoefficient + inducedDragCoefficient;
            return totalDragCoefficient * CalculateDynamicPressure(velocity, density, aerodynamicsData.WingArea);
        }

        private static double CalculateDynamicPressure(double velocity, double density, double wingArea)
        {
            return 0.5 * density * Math.Pow(velocity, 2) * wingArea;
        }

        #region TestCases

        private static IEnumerable<TestCaseData> GetNegativeVelocityTestCases()
        {
            var random = new Random(21);
            AerodynamicsData aerodynamicsData = AerodynamicsDataTestFactory.CreateAerodynamicsData();
            Angle angleOfAttack = random.NextAngle();
            double liftCoefficient = random.NextDouble();
            double density = random.NextDouble();
            double invalidVelocity = -random.NextDouble();

            yield return new TestCaseData(new Action(() => AerodynamicsHelper.CalculateLift(aerodynamicsData,
                                                                                            angleOfAttack,
                                                                                            density,
                                                                                            invalidVelocity)))
                .SetName("CalculateLift");
            yield return new TestCaseData(new Action(() => AerodynamicsHelper.CalculateDragWithEngineFailure(aerodynamicsData,
                                                                                                             liftCoefficient,
                                                                                                             density,
                                                                                                             invalidVelocity)))
                .SetName("CalculateDragWithEngineFailure");
            yield return new TestCaseData(new Action(() => AerodynamicsHelper.CalculateDragWithoutEngineFailure(aerodynamicsData,
                                                                                                                liftCoefficient,
                                                                                                                density,
                                                                                                                invalidVelocity)))
                .SetName("CalculateDragWithoutEngineFailure");
        }

        private static IEnumerable<TestCaseData> GetInvalidDensityTestCases()
        {
            var random = new Random(21);
            AerodynamicsData aerodynamicsData = AerodynamicsDataTestFactory.CreateAerodynamicsData();
            Angle angleOfAttack = random.NextAngle();
            double liftCoefficient = random.NextDouble();
            double invalidDensity = -random.NextDouble();
            double velocity = random.NextDouble();
            double takeOffWeight = random.NextDouble();

            yield return new TestCaseData(new Action(() => AerodynamicsHelper.CalculateLift(aerodynamicsData,
                                                                                            angleOfAttack,
                                                                                            invalidDensity,
                                                                                            velocity)))
                .SetName("CalculateLift - Negative");
            yield return new TestCaseData(new Action(() => AerodynamicsHelper.CalculateLift(aerodynamicsData,
                                                                                            angleOfAttack,
                                                                                            0,
                                                                                            velocity)))
                .SetName("CalculateLift - Zero");

            yield return new TestCaseData(new Action(() => AerodynamicsHelper.CalculateDragWithEngineFailure(aerodynamicsData,
                                                                                                             liftCoefficient,
                                                                                                             invalidDensity,
                                                                                                             velocity)))
                .SetName("CalculateDragWithEngineFailure - Negative");
            yield return new TestCaseData(new Action(() => AerodynamicsHelper.CalculateDragWithEngineFailure(aerodynamicsData,
                                                                                                             liftCoefficient,
                                                                                                             0,
                                                                                                             velocity)))
                .SetName("CalculateDragWithEngineFailure - Zero");

            yield return new TestCaseData(new Action(() => AerodynamicsHelper.CalculateDragWithoutEngineFailure(aerodynamicsData,
                                                                                                                liftCoefficient,
                                                                                                                invalidDensity,
                                                                                                                velocity)))
                .SetName("CalculateDragWithoutEngineFailure - Negative");
            yield return new TestCaseData(new Action(() => AerodynamicsHelper.CalculateDragWithoutEngineFailure(aerodynamicsData,
                                                                                                                liftCoefficient,
                                                                                                                0,
                                                                                                                velocity)))
                .SetName("CalculateDragWithoutEngineFailure - Zero");

            yield return new TestCaseData(new Action(() => AerodynamicsHelper.CalculateStallSpeed(aerodynamicsData,
                                                                                                  takeOffWeight,
                                                                                                  invalidDensity)))
                .SetName("CalculateStallSpeed - Negative");
            yield return new TestCaseData(new Action(() => AerodynamicsHelper.CalculateStallSpeed(aerodynamicsData,
                                                                                                  takeOffWeight,
                                                                                                  0)))
                .SetName("CalculateStallSpeed - Zero");
        }

        private static IEnumerable<TestCaseData> GetInvalidAngleOfAttackTestCases()
        {
            var random = new Random(21);
            var aerodynamicsData = new AerodynamicsData(random.NextDouble(),
                                                        random.NextDouble(),
                                                        new Angle(),
                                                        1.0, 10.0,
                                                        random.NextDouble(),
                                                        random.NextDouble(),
                                                        random.NextDouble());

            Angle angleOfAttackExceedingClMax = Angle.FromRadians(11.0);

            yield return new TestCaseData(aerodynamicsData, aerodynamicsData.ZeroLiftAngleOfAttack - random.NextAngle(),
                                          "Angle of attack must be larger than zero lift angle of attack.")
                .SetName("Angle of Attack < Zero Lift Angle of Attack");

            yield return new TestCaseData(aerodynamicsData, angleOfAttackExceedingClMax,
                                          "Angle of attack results in a lift coefficient larger than the maximum lift coefficient CLMax.")
                .SetName("Angle of Attack - Larger than ClMax");
        }

        private static IEnumerable<TestCaseData> GetInvalidLiftCoefficientTestCases()
        {
            var random = new Random(21);
            AerodynamicsData aerodynamicsData = AerodynamicsDataTestFactory.CreateAerodynamicsData();

            yield return new TestCaseData(aerodynamicsData, -random.NextDouble())
                .SetName("Lift Coefficient < 0");

            yield return new TestCaseData(aerodynamicsData, aerodynamicsData.MaximumLiftCoefficient + random.NextDouble())
                .SetName("Lift Coefficient > Maximum lift coefficient CLMax");
        }

        #endregion
    }
}