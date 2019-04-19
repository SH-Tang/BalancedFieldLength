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
using Core.Common.TestUtil;

namespace Simulator.Data.TestUtil
{
    /// <summary>
    /// Creates valid instances of <see cref="AircraftData"/> which can be used for testing.
    /// </summary>
    public static class AircraftDataTestFactory
    {
        /// <summary>
        /// Generates an instance of <see cref="AircraftData"/> with random values.
        /// </summary>
        /// <returns>An <see cref="AircraftData"/> with random values.</returns>
        public static AircraftData CreateRandomAircraftData()
        {
            var random = new Random(21);
            return new AircraftData(random.Next(), random.NextDouble(),
                                    random.NextDouble(), random.NextAngle(),
                                    random.NextAngle(), random.NextDouble(),
                                    random.NextDouble(), AerodynamicsDataTestFactory.CreateAerodynamicsData());
        }
    }
}