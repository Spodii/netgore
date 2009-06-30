using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Extensions for the IDataReader.
    /// </summary>
    public static class IDataReaderExtensions
    {
        /// <summary>
        /// Gets the GetMapEntityIndex from the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The GetMapEntityIndex from the specified field.</returns>
        public static MapEntityIndex GetMapEntityIndex(this IDataReader dataReader, int i)
        {
            return MapEntityIndex.Read(dataReader, i);
        }

        /// <summary>
        /// Gets the GetMapEntityIndex from the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The GetMapEntityIndex from the specified field.</returns>
        public static MapEntityIndex GetMapEntityIndex(this IDataReader dataReader, string name)
        {
            return MapEntityIndex.Read(dataReader, name);
        }

        /// <summary>
        /// Gets the GetMapIndex from the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The GetMapIndex from the specified field.</returns>
        public static MapIndex GetMapIndex(this IDataReader dataReader, int i)
        {
            return MapIndex.Read(dataReader, i);
        }

        /// <summary>
        /// Gets the GetMapIndex from the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The GetMapIndex from the specified field.</returns>
        public static MapIndex GetMapIndex(this IDataReader dataReader, string name)
        {
            return MapIndex.Read(dataReader, name);
        }
    }
}