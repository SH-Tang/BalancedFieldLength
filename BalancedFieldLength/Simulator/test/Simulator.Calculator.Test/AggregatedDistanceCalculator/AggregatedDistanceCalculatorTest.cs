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
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Simulator.Calculator.AggregatedDistanceCalculator;
using Simulator.Calculator.Factories;
using Simulator.Calculator.Integrators;
using Simulator.Data;
using Simulator.Data.Exceptions;
using Simulator.Data.TestUtil;

namespace Simulator.Calculator.Test.AggregatedDistanceCalculator
{
    [TestFixture]
    public class AggregatedDistanceCalculatorTest
    {
        [Test]
        public void Constructor_DistanceCalculatorFactoryNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new Calculator.AggregatedDistanceCalculator.AggregatedDistanceCalculator(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("distanceCalculatorFactory", exception.ParamName);
        }

        [Test]
        public void Calculate_AircraftDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            var integrator = Substitute.For<IIntegrator>();
            int nrOfFailedEngines = random.Next();
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();
            CalculationSettings calculationSettings = CalculationSettingsTestFactory.CreateDistanceCalculatorSettings();

            var distanceCalculatorFactory = Substitute.For<IDistanceCalculatorFactory>();

            var aggregatedCalculator = new Calculator.AggregatedDistanceCalculator.AggregatedDistanceCalculator(distanceCalculatorFactory);

            // Call
            TestDelegate call = () => aggregatedCalculator.Calculate(null, integrator, nrOfFailedEngines, density, gravitationalAcceleration, calculationSettings);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aircraftData", exception.ParamName);
        }

        [Test]
        public void Calculate_IntegratorNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            int nrOfFailedEngines = random.Next();
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();
            CalculationSettings calculationSettings = CalculationSettingsTestFactory.CreateDistanceCalculatorSettings();

            var distanceCalculatorFactory = Substitute.For<IDistanceCalculatorFactory>();

            var aggregatedCalculator = new Calculator.AggregatedDistanceCalculator.AggregatedDistanceCalculator(distanceCalculatorFactory);

            // Call
            TestDelegate call = () => aggregatedCalculator.Calculate(aircraftData, null, nrOfFailedEngines, density, gravitationalAcceleration, calculationSettings);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("integrator", exception.ParamName);
        }

        [Test]
        public void Calculate_CalculationSettingsNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            var integrator = Substitute.For<IIntegrator>();
            int nrOfFailedEngines = random.Next();
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();

            var distanceCalculatorFactory = Substitute.For<IDistanceCalculatorFactory>();

            var aggregatedCalculator = new Calculator.AggregatedDistanceCalculator.AggregatedDistanceCalculator(distanceCalculatorFactory);

            // Call
            TestDelegate call = () => aggregatedCalculator.Calculate(aircraftData, integrator, nrOfFailedEngines, density, gravitationalAcceleration, null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("calculationSettings", exception.ParamName);
        }

        [Test]
        public static void Calculate_WithArguments_ExecutesDistanceCalculatorsAndReturnsExpectedOutput()
        {
            // Setup
            var random = new Random(21);

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            var integrator = Substitute.For<IIntegrator>();
            int nrOfFailedEngines = random.Next();
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();
            CalculationSettings calculationSettings = CalculationSettingsTestFactory.CreateDistanceCalculatorSettings();

            var abortedTakeOffDistanceCalculator = Substitute.For<IDistanceCalculator>();
            var abortedTakeOffDistanceOutput = new DistanceCalculatorOutput(calculationSettings.FailureSpeed, random.NextDouble());
            abortedTakeOffDistanceCalculator.Calculate().Returns(abortedTakeOffDistanceOutput);

            var continuedTakeOffDistanceCalculator = Substitute.For<IDistanceCalculator>();
            var continuedTakeOffDistanceOutput = new DistanceCalculatorOutput(calculationSettings.FailureSpeed, random.NextDouble());
            continuedTakeOffDistanceCalculator.Calculate().Returns(continuedTakeOffDistanceOutput);

            var distanceCalculatorFactory = Substitute.For<IDistanceCalculatorFactory>();
            distanceCalculatorFactory.CreateAbortedTakeOffDistanceCalculator(Arg.Is(aircraftData),
                                                                             Arg.Is(integrator),
                                                                             Arg.Is(density),
                                                                             Arg.Is(gravitationalAcceleration),
                                                                             Arg.Is(calculationSettings))
                                     .Returns(abortedTakeOffDistanceCalculator);
            distanceCalculatorFactory.CreateContinuedTakeOffDistanceCalculator(Arg.Is(aircraftData),
                                                                               Arg.Is(integrator),
                                                                               Arg.Is(nrOfFailedEngines),
                                                                               Arg.Is(density),
                                                                               Arg.Is(gravitationalAcceleration),
                                                                               Arg.Is(calculationSettings))
                                     .Returns(continuedTakeOffDistanceCalculator);

            var aggregatedCalculator = new Calculator.AggregatedDistanceCalculator.AggregatedDistanceCalculator(distanceCalculatorFactory);

            // Call 
            AggregatedDistanceOutput output = aggregatedCalculator.Calculate(aircraftData, integrator, nrOfFailedEngines, density, gravitationalAcceleration, calculationSettings);

            // Assert
            Assert.AreEqual(calculationSettings.FailureSpeed, output.FailureSpeed);
            Assert.AreEqual(abortedTakeOffDistanceOutput.Distance, output.AbortedTakeOffDistance);
            Assert.AreEqual(continuedTakeOffDistanceOutput.Distance, output.ContinuedTakeOffDistance);
        }

        [Test]
        public static void Calculate_AbortedTakeOffDistanceCalculatorThrowsCalculatorException_ThrowsCalculatorException()
        {
            // Setup
            var random = new Random(21);

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            var integrator = Substitute.For<IIntegrator>();
            int nrOfFailedEngines = random.Next();
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();
            CalculationSettings calculationSettings = CalculationSettingsTestFactory.CreateDistanceCalculatorSettings();

            var calculatorException = new CalculatorException();
            var abortedTakeOffDistanceCalculator = Substitute.For<IDistanceCalculator>();
            abortedTakeOffDistanceCalculator.Calculate().Throws(calculatorException);

            var continuedTakeOffDistanceCalculator = Substitute.For<IDistanceCalculator>();
            var continuedTakeOffDistanceOutput = new DistanceCalculatorOutput(calculationSettings.FailureSpeed, random.NextDouble());
            continuedTakeOffDistanceCalculator.Calculate().Returns(continuedTakeOffDistanceOutput);

            var distanceCalculatorFactory = Substitute.For<IDistanceCalculatorFactory>();
            distanceCalculatorFactory.CreateAbortedTakeOffDistanceCalculator(null, null, 0, 0, null).ReturnsForAnyArgs(abortedTakeOffDistanceCalculator);

            var aggregatedCalculator = new Calculator.AggregatedDistanceCalculator.AggregatedDistanceCalculator(distanceCalculatorFactory);

            // Call 
            TestDelegate call = () => aggregatedCalculator.Calculate(aircraftData, integrator, nrOfFailedEngines, density, gravitationalAcceleration, calculationSettings);

            // Assert
            var exception = Assert.Throws<CalculatorException>(call);
            Assert.AreSame(calculatorException, exception);
        }

        [Test]
        public static void Calculate_ContinuedTakeOffDistanceCalculatorThrowsCalculatorException_ThrowsCalculatorException()
        {
            // Setup
            var random = new Random(21);

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            var integrator = Substitute.For<IIntegrator>();
            int nrOfFailedEngines = random.Next();
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();
            CalculationSettings calculationSettings = CalculationSettingsTestFactory.CreateDistanceCalculatorSettings();

            var calculatorException = new CalculatorException();

            var continuedTakeOffDistanceCalculator = Substitute.For<IDistanceCalculator>();
            continuedTakeOffDistanceCalculator.Calculate().Throws(calculatorException);

            var distanceCalculatorFactory = Substitute.For<IDistanceCalculatorFactory>();
            distanceCalculatorFactory.CreateAbortedTakeOffDistanceCalculator(null, null, 0, 0, null).ReturnsForAnyArgs(continuedTakeOffDistanceCalculator);

            var aggregatedCalculator = new Calculator.AggregatedDistanceCalculator.AggregatedDistanceCalculator(distanceCalculatorFactory);

            // Call 
            TestDelegate call = () => aggregatedCalculator.Calculate(aircraftData, integrator, nrOfFailedEngines, density, gravitationalAcceleration, calculationSettings);

            // Assert
            var exception = Assert.Throws<CalculatorException>(call);
            Assert.AreSame(calculatorException, exception);
        }
    }
}