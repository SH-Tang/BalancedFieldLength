using System;

namespace Calculator.Data.Helpers
{
    /// <summary>
    /// Helper class which can be used to calculate the aerodynamic forces.
    /// </summary>
    public static class AerodynamicForceHelper
    {
        /// <summary>
        /// Calculates the lift based on the input.
        /// </summary>
        /// <param name="aerodynamicData">The <see cref="AerodynamicData"/>
        /// containing all the aerodynamic properties.</param>
        /// <param name="angleOfAttack">The angle of attack. [deg]</param>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <param name="velocity">The true airspeed (Vtas). [m/s]</param>
        /// <returns>The generated lift. [N]</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aerodynamicData"/>
        /// is <c>null</c>.</exception>
        public static double CalculateLift(AerodynamicData aerodynamicData, 
                                           double angleOfAttack, 
                                           double density, 
                                           double velocity)
        {
            if (aerodynamicData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicData));
            }

            double liftCoefficient = aerodynamicData.LiftCoefficientGradient *
                                  DegreesToRadians(angleOfAttack - aerodynamicData.ZeroLiftAngleOfAttack);
            
            return 0.5 * liftCoefficient * density * Math.Pow(velocity, 2) * aerodynamicData.WingArea;
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }
    }
}