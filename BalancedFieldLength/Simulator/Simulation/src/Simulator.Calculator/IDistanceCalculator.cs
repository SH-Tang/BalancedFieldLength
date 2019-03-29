using Simulator.Data.Exceptions;

namespace Simulator.Calculator
{
    /// <summary>
    /// Interface describing the calculator for calculating the traversed distance.
    /// </summary>
    public interface IDistanceCalculator
    {
        /// <summary>
        /// Calculates the distance which is needed before the aircraft reaches either the screen height
        /// or comes to a standstill.
        /// </summary>
        /// <returns>The <see cref="DistanceCalculatorOutput"/> with the calculated result.</returns>
        /// <exception cref="CalculatorException">Thrown when the calculator cannot calculate the covered distance.</exception>
        DistanceCalculatorOutput Calculate();
    }
}