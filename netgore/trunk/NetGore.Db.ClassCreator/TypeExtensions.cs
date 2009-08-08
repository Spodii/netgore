using System;
using System.ComponentModel;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the underlying Type from a Nullable Type.
        /// </summary>
        /// <param name="type">The Nullable Type to get the underlying Type for.</param>
        /// <returns>The underlying Type from the Nullable Type.</returns>
        public static Type GetNullableUnderlyingType(this Type type)
        {
            NullableConverter c = new NullableConverter(type);
            return c.UnderlyingType;
        }

        /// <summary>
        /// Checks if the given Type is a Nullable Type.
        /// </summary>
        /// <param name="type">The Type to check.</param>
        /// <returns>True if the Type is Nullable; otherwise false.</returns>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}