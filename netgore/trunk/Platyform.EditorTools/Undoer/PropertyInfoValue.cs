using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Platyform.Extensions;

namespace Platyform.EditorTools
{
    /// <summary>
    /// Contains an object value and the PropertyInfo of the property the value belongs to
    /// </summary>
    public struct PropertyInfoValue
    {
        /// <summary>
        /// Information of the property affected by the value change
        /// </summary>
        public readonly PropertyInfo PropertyInfo;

        /// <summary>
        /// Cached value
        /// </summary>
        public readonly object Value;

        /// <summary>
        /// PropertyInfoValue constructor
        /// </summary>
        /// <param name="p">Information of the property affected by the value change</param>
        /// <param name="value">Cached value</param>
        public PropertyInfoValue(PropertyInfo p, object value)
        {
            PropertyInfo = p;
            Value = value;
        }
    }
}