using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Platyform.Extensions;

namespace Platyform
{
    /// <summary>
    /// Interface for getting the current time.
    /// </summary>
    public interface IGetTime
    {
        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        int GetTime();
    }
}