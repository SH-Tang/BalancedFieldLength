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
using NUnit.Framework;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Components.Factories;
using Simulator.Components.TakeOffDynamics;
using Simulator.Data;
using Simulator.Data.TestUtil;

namespace Simulator.Components.Test.Factories
{
    [TestFixture]
    public class TakeOffDynamicsCalculatorFactoryTest
    {
        [Test]
        public void CreateNormalTakeOffDynamics_Always_ReturnsExpectedCalculator()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var factory = new TakeOffDynamicsCalculatorFactory();

            // Call
            INormalTakeOffDynamicsCalculator calculator = factory.CreateNormalTakeOffDynamics(aircraftData,
                                                                                              random.NextDouble(),
                                                                                              random.NextDouble());

            // Assert
            Assert.IsInstanceOf<NormalTakeOffDynamicsCalculator>(calculator);
        }

        [Test]
        public void CreateAbortedTakeOffDynamics_Always_ReturnsExpectedCalculator()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var factory = new TakeOffDynamicsCalculatorFactory();

            // Call
            IFailureTakeOffDynamicsCalculator calculator = factory.CreateAbortedTakeOffDynamics(aircraftData,
                                                                                                random.NextDouble(),
                                                                                                random.NextDouble());

            // Assert
            Assert.IsInstanceOf<AbortedTakeOffDynamicsCalculator>(calculator);
        }

        [Test]
        public void CreateContinuedTakeOffDynamics_Always_ReturnsExpectedCalculator()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var factory = new TakeOffDynamicsCalculatorFactory();

            // Call
            IFailureTakeOffDynamicsCalculator calculator = factory.CreateContinuedTakeOffDynamicsCalculator(aircraftData,
                                                                                                            random.Next(),
                                                                                                            random.NextDouble(),
                                                                                                            random.NextDouble());

            // Assert
            Assert.IsInstanceOf<ContinuedTakeOffDynamicsCalculator>(calculator);
        }
    }
}