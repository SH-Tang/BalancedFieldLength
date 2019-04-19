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
using Core.Common.Data;
using Core.Common.TestUtil;
using NUnit.Framework;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Components.TakeOffDynamics;
using Simulator.Data;
using Simulator.Data.Helpers;
using Simulator.Data.TestUtil;

namespace Simulator.Components.Test.TakeOffDynamics
{
    [TestFixture]
    public class AbortedTakeOffDynamicsCalculatorTest
    {
        private const double gravitationalAcceleration = SimulationConstants.GravitationalAcceleration;
        private const double airDensity = SimulationConstants.Density;
        private const double tolerance = SimulationConstants.Tolerance;

        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            // Call
            var calculator = new AbortedTakeOffDynamicsCalculator(aircraftData, random.NextDouble(), random.NextDouble());

            // Assert
            Assert.IsInstanceOf<TakeOffDynamicsCalculatorBase>(calculator);
            Assert.IsInstanceOf<IFailureTakeOffDynamicsCalculator>(calculator);
        }

        [Test]
        public static void Calculate_Always_ReturnsExpectedZeroPitchRate()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            Angle angleOfAttack = AerodynamicsDataTestHelper.GetValidAngleOfAttack(aircraftData.AerodynamicsData);
            var aircraftState = new AircraftState(angleOfAttack,
                                                  new Angle(),
                                                  random.NextDouble(),
                                                  random.NextDouble(),
                                                  random.NextDouble());

            var calculator = new AbortedTakeOffDynamicsCalculator(aircraftData, random.NextDouble(), random.NextDouble());

            // Call 
            AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

            // Assert
            Assert.Zero(accelerations.PitchRate.Radians);
        }

        [TestFixture]
        public class CalculateTrueAirspeedRate
        {
            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAircraftStateHeightEqualToThresholdAndLiftLowerThanTakeOffWeight_ReturnsExpectedTrueAirspeedRate(AircraftData aircraftData)
            {
                // Setup
                const double airspeed = 10.0;
                const double threshold = 0.01;

                var random = new Random(21);
                var aircraftState = new AircraftState(aircraftData.MaximumPitchAngle,
                                                      random.NextAngle(),
                                                      airspeed,
                                                      threshold,
                                                      random.NextDouble());

                Angle angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle;

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicsData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               airspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift < takeOffWeightNewton);

                var calculator = new AbortedTakeOffDynamicsCalculator(aircraftData, airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aircraftData.AerodynamicsData, angleOfAttack);
                double dragForce = AerodynamicsHelper.CalculateDragWithEngineFailure(aircraftData.AerodynamicsData,
                                                                                     liftCoefficient,
                                                                                     airDensity,
                                                                                     airspeed);

                const double thrustForce = 0;
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(aircraftState.FlightPathAngle.Radians);
                double expectedAcceleration = (gravitationalAcceleration * (thrustForce - dragForce - horizontalWeightComponent))
                                              / takeOffWeightNewton;
                Assert.AreEqual(expectedAcceleration, accelerations.TrueAirSpeedRate, tolerance);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAircraftStateHeightLowerThanThresholdAndLiftHigherThanTakeOffWeight_ReturnsExpectedTrueAirspeedRate(AircraftData aircraftData)
            {
                // Setup
                const double airspeed = 100.0;
                const double threshold = 0.01;

                var random = new Random(21);
                var aircraftState = new AircraftState(aircraftData.MaximumPitchAngle,
                                                      random.NextAngle(),
                                                      airspeed,
                                                      threshold - random.NextDouble(),
                                                      random.NextDouble());

                Angle angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle;

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicsData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               airspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift > takeOffWeightNewton);

                var calculator = new AbortedTakeOffDynamicsCalculator(aircraftData, airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aircraftData.AerodynamicsData, angleOfAttack);
                double dragForce = AerodynamicsHelper.CalculateDragWithEngineFailure(aircraftData.AerodynamicsData,
                                                                                     liftCoefficient,
                                                                                     airDensity,
                                                                                     airspeed);

                const double thrustForce = 0;
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(aircraftState.FlightPathAngle.Radians);
                double expectedAcceleration = (gravitationalAcceleration * (thrustForce - dragForce - horizontalWeightComponent))
                                              / takeOffWeightNewton;
                Assert.AreEqual(expectedAcceleration, accelerations.TrueAirSpeedRate, tolerance);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAircraftStateHeightSmallerThanThresholdAndLiftSmallerThanTakeOffWeight_ReturnsExpectedTrueAirspeedRate(AircraftData aircraftData)
            {
                // Setup
                const double airspeed = 10.0;
                const double threshold = 0.01;

                var random = new Random(21);
                var aircraftState = new AircraftState(aircraftData.MaximumPitchAngle,
                                                      random.NextAngle(),
                                                      airspeed,
                                                      threshold - random.NextDouble(),
                                                      random.NextDouble());

                Angle angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle;

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicsData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               airspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift < takeOffWeightNewton);

                var calculator = new AbortedTakeOffDynamicsCalculator(aircraftData, airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aircraftData.AerodynamicsData, angleOfAttack);
                double normalForce = takeOffWeightNewton - lift;
                double dragForce = AerodynamicsHelper.CalculateDragWithEngineFailure(aircraftData.AerodynamicsData,
                                                                                     liftCoefficient,
                                                                                     airDensity,
                                                                                     airspeed) + normalForce * aircraftData.BrakingResistanceCoefficient;

                const double thrustForce = 0;
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(aircraftState.FlightPathAngle.Radians);
                double expectedAcceleration = (gravitationalAcceleration * (thrustForce - dragForce - horizontalWeightComponent))
                                              / takeOffWeightNewton;
                Assert.AreEqual(expectedAcceleration, accelerations.TrueAirSpeedRate, tolerance);
            }
        }
    }
}