using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.IO.PropertySync;

namespace NetGore.IO
{
    /// <summary>
    /// Provides helper methods for the <see cref="IPersistable"/> interface.
    /// </summary>
    public static class PersistableHelper
    {
        /// <summary>
        /// Reads all properties for the given <paramref name="obj"/> that have the <see cref="SyncValueAttribute"/>
        /// attribute from the given <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="obj">The object to read the persistent values for.</param>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public static void Read(object obj, IValueReader reader)
        {
            var propertySyncs = PropertySyncHelper.GetPropertySyncs(obj.GetType());
            foreach (var ps in propertySyncs)
            {
                ps.ReadValue(obj, reader);
            }
        }

        /// <summary>
        /// Writes all properties for the given <paramref name="obj"/> that have the <see cref="SyncValueAttribute"/>
        /// attribute to the given <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="obj">The object to write the persistent values for.</param>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public static void Write(object obj, IValueWriter writer)
        {
            var propertySyncs = PropertySyncHelper.GetPropertySyncs(obj.GetType());
            foreach (var ps in propertySyncs)
            {
                ps.WriteValue(obj, writer);
            }
        }
    }
}