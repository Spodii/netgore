#region Copyright & License

//
// Copyright 2001-2005 The Apache Software Foundation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

#endregion

using System.IO;
using System.Linq;
using System.Security;
using System.Threading;

namespace log4net.Util.PatternStringConverters
{
    /// <summary>
    /// Write the current thread identity to the output
    /// </summary>
    /// <remarks>
    /// <para>
    /// Write the current thread identity to the output writer
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    sealed class IdentityPatternConverter : PatternConverter
    {
        /// <summary>
        /// Write the current thread identity to the output
        /// </summary>
        /// <param name="writer">the writer to write to</param>
        /// <param name="state">null, state is not set</param>
        /// <remarks>
        /// <para>
        /// Writes the current thread identity to the output <paramref name="writer"/>.
        /// </para>
        /// </remarks>
        protected override void Convert(TextWriter writer, object state)
        {
            try
            {
                if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null &&
                    Thread.CurrentPrincipal.Identity.Name != null)
                    writer.Write(Thread.CurrentPrincipal.Identity.Name);
            }
            catch (SecurityException)
            {
                // This security exception will occur if the caller does not have 
                // some undefined set of SecurityPermission flags.
                LogLog.Debug(
                    "IdentityPatternConverter: Security exception while trying to get current thread principal. Error Ignored.");

                writer.Write(SystemInfo.NotAvailableText);
            }
        }
    }
}