using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Platyform.Extensions;

namespace Platyform.EditorTools
{
    /// <summary>
    /// Contains an object and the list of PropertyInfoValues representing the changes made to the item
    /// </summary>
    public struct ObjectVersion<T>
    {
        /// <summary>
        /// List of changes applied to the object properties
        /// </summary>
        public readonly List<PropertyInfoValue> Changes;

        /// <summary>
        /// Object the changes were applied to
        /// </summary>
        public readonly T Object;

        /// <summary>
        /// ObjectVersion constructor
        /// </summary>
        /// <param name="obj">Object the changes were applied to</param>
        /// <param name="diff">List of changes applied to the object properties</param>
        public ObjectVersion(T obj, List<PropertyInfoValue> diff)
        {
            Object = obj;
            Changes = diff;
        }
    }
}