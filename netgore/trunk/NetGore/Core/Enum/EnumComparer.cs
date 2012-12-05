using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

// EnumComparer code base credit goes to: http://www.codeproject.com/KB/cs/EnumComparer.aspx

namespace NetGore
{
    public class EnumComparer
    {
        /// <summary>
        /// The underlying Enum types that are supported.
        /// </summary>
        protected static readonly ICollection<Type> _supportedUnderlyingTypes = new Type[] { 
            typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong) 
        };
    }

    /// <summary>
    /// Comparer that is specifically for Enums.
    /// </summary>
    /// <remarks>
    /// The <see cref="EnumComparer{T}"/> should always return the same comparison result as the default comparer, but uses
    /// dynamically generated methods to implement a very fast implementation of Equals and GetHashCode.
    /// </remarks>
    /// <typeparam name="T">Type of Enum to compare.</typeparam>
    public sealed class EnumComparer<T> : EnumComparer, IEqualityComparer<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        static readonly EnumComparer<T> _instance;

        readonly Func<T, T, bool> _equals;
        readonly Func<T, int> _getHashCode;

        /// <summary>
        /// Initializes the <see cref="EnumComparer{T}"/> class.
        /// </summary>
        /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not an enum.</exception>
        /// <exception cref="NotSupportedException"><typeparamref name="T"/> uses an underlying type that is not supported.</exception>
        static EnumComparer()
        {
            _instance = new EnumComparer<T>();

            // Ensure T is an Enum
            if (!typeof(T).IsEnum)
            {
                const string errmsg = "The type parameter `{0}` is not an Enum. Only Enums may be used.";
                Debug.Fail(string.Format(errmsg, typeof(T)));
                throw new NotSupportedException(string.Format(errmsg, typeof(T)));
            }

            // Make sure the underlying type is supported
            var underlyingType = Enum.GetUnderlyingType(typeof(T));

            if (!_supportedUnderlyingTypes.Contains(underlyingType))
            {
                const string errmsg = "Enum `{0}` contains an unsupported underlying Enum type of `{1}`.";
                Debug.Fail(string.Format(errmsg, typeof(T), underlyingType));
                throw new NotSupportedException(string.Format(errmsg, typeof(T), underlyingType));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumComparer{T}"/> class.
        /// </summary>
        EnumComparer()
        {
            _getHashCode = GenerateGetHashCode();
            _equals = GenerateEquals();
        }

        /// <summary>
        /// Gets the EnumComparer used to compare an Enum of type <typeparam name="T">T</typeparam>.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static EnumComparer<T> Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Generates the code for the Equals method.
        /// </summary>
        /// <returns>The code for the Equals method.</returns>
        static Func<T, T, bool> GenerateEquals()
        {
            var xParam = Expression.Parameter(typeof(T), "x");
            var yParam = Expression.Parameter(typeof(T), "y");
            var equalExpression = Expression.Equal(xParam, yParam);
            return Expression.Lambda<Func<T, T, bool>>(equalExpression, new[] { xParam, yParam }).Compile();
        }

        /// <summary>
        /// Generates the code for the GetHashCode method.
        /// </summary>
        /// <returns>The code for the GetHashCode method.</returns>
        static Func<T, int> GenerateGetHashCode()
        {
            var objParam = Expression.Parameter(typeof(T), "obj");
            var underlyingType = Enum.GetUnderlyingType(typeof(T));
            var convertExpression = Expression.Convert(objParam, underlyingType);
            var getHashCodeMethod = underlyingType.GetMethod("GetHashCode");
            var getHashCodeExpression = Expression.Call(convertExpression, getHashCodeMethod);
            return Expression.Lambda<Func<T, int>>(getHashCodeExpression, new[] { objParam }).Compile();
        }

        #region IEqualityComparer<T> Members

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <typeparamref name="T"/> to compare.
        ///                 </param><param name="y">The second object of type <typeparamref name="T"/> to compare.
        ///                 </param>
        public bool Equals(T x, T y)
        {
            return _equals(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.
        ///                 </param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        ///                 </exception>
        public int GetHashCode(T obj)
        {
            return _getHashCode(obj);
        }

        #endregion
    }
}