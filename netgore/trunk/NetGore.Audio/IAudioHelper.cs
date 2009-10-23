using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NetGore.IO;

namespace NetGore.Audio
{
    /// <summary>
    /// Provides helper methods for the <see cref="IAudio"/> interface.
    /// </summary>
    public static class IAudioHelper
    {
        /// <summary>
        /// The key to be used in the <see cref="IValueReader"/> and <see cref="IValueWriter"/> for the File value.
        /// </summary>
        public const string FileValueKey = "File";

        /// <summary>
        /// The key to be used in the <see cref="IValueReader"/> and <see cref="IValueWriter"/> for the Index value.
        /// </summary>
        public const string IndexValueKey = "Index";
    }
}
