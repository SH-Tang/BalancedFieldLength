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

namespace Core.Common.Data
{
    /// <summary>
    /// Represents an angle.
    /// </summary>
    public struct Angle
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Angle"/>
        /// with a given angle.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        private Angle(double radians)
        {
            Degrees = RadiansToDegrees(radians);
            Radians = radians;
        }

        /// <summary>
        /// Gets the angle in degrees.
        /// </summary>
        public double Degrees { get; }

        /// <summary>
        /// Gets the angle in radians.
        /// </summary>
        public double Radians { get; }

        /// <summary>
        /// Creates a new instance of <see cref="Angle"/>
        /// based on <paramref name="degrees"/>.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>A <see cref="Angle"/>.</returns>
        public static Angle FromDegrees(double degrees)
        {
            return new Angle(DegreesToRadians(degrees));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Angle"/>
        /// based on <paramref name="radians"/>.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>A <see cref="Angle"/>.</returns>
        public static Angle FromRadians(double radians)
        {
            return new Angle(radians);
        }

        public static Angle operator +(Angle leftAngle, Angle rightAngle)
        {
            return new Angle(leftAngle.Radians + rightAngle.Radians);
        }

        public static Angle operator -(Angle leftAngle, Angle rightAngle)
        {
            return new Angle(leftAngle.Radians - rightAngle.Radians);
        }

        public static bool operator ==(Angle leftAngle, Angle rightAngle)
        {
            return leftAngle.Equals(rightAngle);
        }

        public static bool operator !=(Angle leftAngle, Angle rightAngle)
        {
            return !leftAngle.Equals(rightAngle);
        }

        public static bool operator >(Angle leftAngle, Angle rightAngle)
        {
            return leftAngle.Radians > rightAngle.Radians;
        }

        public static bool operator >=(Angle leftAngle, Angle rightAngle)
        {
            return leftAngle.Radians >= rightAngle.Radians;
        }

        public static bool operator <(Angle leftAngle, Angle rightAngle)
        {
            return leftAngle.Radians < rightAngle.Radians;
        }

        public static bool operator <=(Angle leftAngle, Angle rightAngle)
        {
            return leftAngle.Radians <= rightAngle.Radians;
        }

        public bool Equals(Angle other)
        {
            return Radians.Equals(other.Radians);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Radians.GetHashCode();
        }

        private static double RadiansToDegrees(double radians)
        {
            return (radians * 180) / Math.PI;
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }
    }
}