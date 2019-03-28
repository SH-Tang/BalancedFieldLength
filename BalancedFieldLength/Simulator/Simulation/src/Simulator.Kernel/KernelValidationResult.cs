using System;
using System.Collections.Generic;

namespace Simulator.Kernel
{
    /// <summary>
    /// Class which holds the validation result after validating data that is used by the kernel.
    /// </summary>
    public class KernelValidationResult
    {
        /// <summary>
        /// Creates a new instance of <see cref="KernelValidationResult"/>.
        /// </summary>
        /// <param name="isValid">Indicator whether the data was valid.</param>
        /// <param name="validationErrors">A collection containing the errors that occurred during the validation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validationErrors"/>
        /// is <c>null</c>.</exception>
        internal KernelValidationResult(bool isValid, IEnumerable<KernelValidationError> validationErrors)
        {
            ValidationErrors = validationErrors;
            IsValid = isValid;
        }

        /// <summary>
        /// Indicator whether the input is valid.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the validation messages.
        /// </summary>
        public IEnumerable<KernelValidationError> ValidationErrors { get; }
    }
}