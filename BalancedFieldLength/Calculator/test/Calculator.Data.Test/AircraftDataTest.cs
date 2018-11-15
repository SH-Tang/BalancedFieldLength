using System;
using NUnit.Framework;

namespace Calculator.Data.Test
{
    [TestFixture]
    public class AircraftDataTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var random = new Random(21);
            int nrOfEngines = random.Next();
            double aspectRatio = random.NextDouble();
            double wingArea = random.NextDouble();
            double takeOffWeight = random.NextDouble();
            double maximumThrust = random.NextDouble();

            
            // Call
            var aircraftData = new AircraftData();

            // Assert
            
        }
    }
}