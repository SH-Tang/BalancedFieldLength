namespace Simulator.Data
{
    public class DistanceCalculatorSettings
    {
        public DistanceCalculatorSettings(int failureSpeed, int nrOfTimeSteps, double timeStep)
        {
            FailureSpeed = failureSpeed;
            NrOfTimeSteps = nrOfTimeSteps;
            TimeStep = timeStep;
        }

        public int FailureSpeed { get; }
        public int NrOfTimeSteps { get; }
        public double TimeStep { get; }
    }
}