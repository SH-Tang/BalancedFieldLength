using System;
using Core.Common.Data;
using Core.Common.TestUtil;
using NUnit.Framework;
using Simulator.Calculator.Integrators;
using Simulator.Data;

namespace Simulator.Calculator.Test.Integrators
{
    [TestFixture]
    public class EulerIntegratorTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var integrator = new EulerIntegrator();

            // Assert
            Assert.IsInstanceOf<IIntegrator>(integrator);
        }

        [Test]
        public void Integrate_AircraftStateNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call
            TestDelegate call = () =>
                new EulerIntegrator().Integrate(null, CreateAircraftAccelerations(), random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("state", exception.ParamName);
        }

        [Test]
        public void Integrate_AircraftAccelerationsNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call
            TestDelegate call = () => new EulerIntegrator().Integrate(CreateAircraftState(), null, random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("accelerations", exception.ParamName);
        }

        [Test]
        public void Integrate_Always_PerformsExpectedIntegration()
        {
            // Setup
            var random = new Random(21);
            var state = CreateAircraftState();
            var accelerations = CreateAircraftAccelerations();
            var timeStep = random.NextDouble();

            // Call
            var resultingState = new EulerIntegrator().Integrate(state, accelerations, timeStep);

            // Assert
            Assert.AreEqual(ExpectedIntegratedValue(state.Height, accelerations.ClimbRate, timeStep),
                resultingState.Height);
            Assert.AreEqual(ExpectedIntegratedValue(state.TrueAirspeed, accelerations.TrueAirSpeedRate, timeStep),
                resultingState.TrueAirspeed);
            Assert.AreEqual(ExpectedIntegratedValue(state.Distance, state.TrueAirspeed, timeStep),
                resultingState.Distance);
            Assert.AreEqual(ExpectedIntegratedValue(state.FlightPathAngle, accelerations.FlightPathRate, timeStep),
                resultingState.FlightPathAngle);
            Assert.AreEqual(ExpectedIntegratedValue(state.PitchAngle, accelerations.PitchRate, timeStep),
                resultingState.PitchAngle);
        }

        private static double ExpectedIntegratedValue(double state, double timeDerivative, double timeStep)
        {
            return state + timeDerivative * timeStep;
        }

        private static Angle ExpectedIntegratedValue(Angle state, Angle timeDerivative, double timeStep)
        {
            var integratedAngleInRadians = ExpectedIntegratedValue(state.Radians, timeDerivative.Radians, timeStep);
            return Angle.FromRadians(integratedAngleInRadians);
        }


        private static AircraftState CreateAircraftState()
        {
            var random = new Random(21);
            return new AircraftState(random.NextAngle(),
                random.NextAngle(),
                random.NextDouble(),
                random.NextDouble(),
                random.NextDouble());
        }

        private static AircraftAccelerations CreateAircraftAccelerations()
        {
            var random = new Random(21);
            return new AircraftAccelerations(random.NextAngle(),
                random.NextDouble(),
                random.NextDouble(),
                random.NextAngle());
        }
    }

    /// <summary>
    /// Class which solves the dynamic system by means of Euler integration.
    /// </summary>
    public class EulerIntegrator : IIntegrator
    {
        public AircraftState Integrate(AircraftState state, AircraftAccelerations accelerations, double timeStep)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (accelerations == null)
            {
                throw new ArgumentNullException(nameof(accelerations));
            }
            
            return new AircraftState(IntegrateState(state.PitchAngle, accelerations.PitchRate, timeStep),
                IntegrateState(state.FlightPathAngle, accelerations.FlightPathRate, timeStep),
                IntegrateState(state.TrueAirspeed, accelerations.TrueAirSpeedRate, timeStep),
                IntegrateState(state.Height, accelerations.ClimbRate, timeStep),
                IntegrateState(state.Distance, state.TrueAirspeed, timeStep));
        }

        private static double IntegrateState(double state, double timeDerivative, double timeStep)
        {
            return state + timeDerivative * timeStep;
        }

        private static Angle IntegrateState(Angle state, Angle timeDerivative, double timeStep)
        {
            var integratedAngleInRadians = IntegrateState(state.Radians, timeDerivative.Radians, timeStep);
            return Angle.FromRadians(integratedAngleInRadians);
        }
    }
}