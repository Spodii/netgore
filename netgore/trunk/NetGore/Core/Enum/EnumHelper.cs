using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Provides helper methods for <see cref="Enum"/>s.
    /// </summary>
    /// <typeparam name="T">The Type of <see cref="Enum"/>.</typeparam>
    /// <exception cref="TypeInitializationException"><typeparamref name="T"/> is not an Enum.</exception>
    public static class EnumHelper<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        // ReSharper disable StaticFieldInGenericType
        static readonly byte _bitsRequired;
        static readonly Func<int, T> _fromInt;
        static readonly int _maxValue;
        static readonly int _minValue;
        static readonly Func<T, int> _toInt;
        static readonly IEnumerable<T> _values;
        // ReSharper restore StaticFieldInGenericType

        /// <summary>
        /// Initializes the <see cref="EnumHelper{T}"/> class.
        /// </summary>
        /// <exception cref="MethodAccessException"><typeparamref name="T"/> is not an enum.</exception>
        static EnumHelper()
        {
            var supportedCastTypes = new Type[] { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int) };

            // Make sure we have an enum
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            // Get the defined enum values
            _values = Enum.GetValues(typeof(T)).Cast<T>().ToCompact();

            // Check if we have an underlying enum type that supports our ToInt/FromInt operations
            var underlyingType = Enum.GetUnderlyingType(typeof(T));
            if (supportedCastTypes.Contains(underlyingType))
            {
                // Create the funcs to cast to/from an int
                _toInt = CreateToInt();
                _fromInt = CreateFromInt();

                // Get all the defined values casted to int
                var valuesAsInt = _values.Select(_toInt);

                // Find the min and max values
                _minValue = valuesAsInt.Min();
                _maxValue = valuesAsInt.Max();
                Debug.Assert(_minValue <= _maxValue);

                // Find the difference between the min and max so we can cache how many bits are required for the range
                var diff = _maxValue - _minValue;
                Debug.Assert(diff >= 0);
                Debug.Assert(diff >= uint.MinValue);

                var bitsReq = BitOps.RequiredBits((uint)diff);
                Debug.Assert(bitsReq > 0);
                Debug.Assert(bitsReq < byte.MaxValue);
                _bitsRequired = (byte)bitsReq;
            }
        }

        /// <summary>
        /// Gets the number of bits required to write the enums values.
        /// </summary>
        /// <exception cref="MethodAccessException"><see cref="SupportsCastOperations"/> is false.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static int BitsRequired
        {
            get
            {
                if (!SupportsCastOperations)
                    throw CastOperationsNotSupportedException();

                return _bitsRequired;
            }
        }

        /// <summary>
        /// Gets the maximum defined enum value.
        /// </summary>
        /// <exception cref="MethodAccessException"><see cref="SupportsCastOperations"/> is false.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static int MaxValue
        {
            get
            {
                if (!SupportsCastOperations)
                    throw CastOperationsNotSupportedException();

                return _maxValue;
            }
        }

        /// <summary>
        /// Gets the minimum defined enum value.
        /// </summary>
        /// <exception cref="MethodAccessException"><see cref="SupportsCastOperations"/> is false.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static int MinValue
        {
            get
            {
                if (!SupportsCastOperations)
                    throw CastOperationsNotSupportedException();

                return _minValue;
            }
        }

        /// <summary>
        /// Gets if the cast operations of this class for the given type <typeparamref name="T"/> are supported. If false,
        /// some operations may not be available. Whether or not the operations are supported depends on the underlying
        /// type of the enum.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static bool SupportsCastOperations
        {
            get { return _toInt != null; }
        }

        /// <summary>
        /// Gets the defined values in the Enum.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static IEnumerable<T> Values
        {
            get { return _values; }
        }

        /// <summary>
        /// Creates an <see cref="MethodAccessException"/> to use for when accessing a method that requires casting to/from
        /// and int while <see cref="SupportsCastOperations"/> is false.
        /// </summary>
        /// <returns>An <see cref="MethodAccessException"/> to use for when accessing a method that requires casting to/from
        /// and int while <see cref="SupportsCastOperations"/> is false.</returns>
        static MethodAccessException CastOperationsNotSupportedException()
        {
            return
                new MethodAccessException("Methods that require casting the enum value to or from an Int32 are not" +
                                          " supported by this enum type since the underlying value type is not supported.");
        }

        /// <summary>
        /// Creates a Func to convert an int to <typeparamref name="T"/>.
        /// </summary>
        /// <returns>A Func to convert an int to <typeparamref name="T"/>.</returns>
        static Func<int, T> CreateFromInt()
        {
            var value = Expression.Parameter(typeof(int), "value");
            var valueCast = Expression.Convert(value, typeof(T));
            var lambda = Expression.Lambda<Func<int, T>>(valueCast, value);
            return lambda.Compile();
        }

        /// <summary>
        /// Creates a <see cref="MethodAccessException"/> to use for when <typeparamref name="T"/> is not an enum.
        /// </summary>
        /// <returns>A <see cref="MethodAccessException"/> to use for when <typeparamref name="T"/> is not an enum.</returns>
        static MethodAccessException CreateGenericTypeIsNotEnumException()
        {
            const string errmsg = "Type parameter T ({0}) must be an Enum.";
            return new MethodAccessException(string.Format(errmsg, typeof(T)));
        }

        /// <summary>
        /// Creates a Func to convert <typeparamref name="T"/> to an int.
        /// </summary>
        /// <returns>A Func to convert <typeparamref name="T"/> to an int.</returns>
        static Func<T, int> CreateToInt()
        {
            var value = Expression.Parameter(typeof(T), "value");
            var valueCast = Expression.Convert(value, typeof(int));
            var lambda = Expression.Lambda<Func<T, int>>(valueCast, value);
            return lambda.Compile();
        }

        /// <summary>
        /// Casts an int to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <typeparamref name="T"/>.</returns>
        /// <exception cref="MethodAccessException"><see cref="SupportsCastOperations"/> is false.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static T FromInt(int value)
        {
            if (!SupportsCastOperations)
                throw CastOperationsNotSupportedException();

            return _fromInt(value);
        }

        /// <summary>
        /// Gets the Enum of the given type from its name. 
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Enum"/>.</typeparam>
        /// <param name="value">The name of the <see cref="Enum"/> value.</param>
        /// <returns>The parsed enum.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> is not an <see cref="Enum"/> -or-
        /// <paramref name="value"/> is an valid enum name -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static T FromName(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Gets a Func that will cast an int to <typeparamref name="T"/>.
        /// </summary>
        /// <returns>A Func that will cast an int to <typeparamref name="T"/>.</returns>
        /// <exception cref="MethodAccessException"><see cref="SupportsCastOperations"/> is false.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Func<int, T> GetFromIntFunc()
        {
            if (!SupportsCastOperations)
                throw CastOperationsNotSupportedException();

            return _fromInt;
        }

        /// <summary>
        /// Gets a Func that will cast <typeparamref name="T"/> to an int.
        /// </summary>
        /// <returns>A Func that will cast <typeparamref name="T"/> to an int.</returns>
        /// <exception cref="MethodAccessException"><see cref="SupportsCastOperations"/> is false.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Func<T, int> GetToIntFunc()
        {
            if (!SupportsCastOperations)
                throw CastOperationsNotSupportedException();

            return _toInt;
        }

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in the Enum of type
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>True if the <paramref name="value"/> exists in the Enum of type
        /// <typeparamref name="T"/>; otherwise false.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static bool IsDefined(T value)
        {
            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not equal to the name of any of
        /// the defined enum values.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">If true, ignore case; otherwise, regard case.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not equal to the name of any of
        /// the defined enum values.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static T Parse(string value, bool ignoreCase)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// Reads the Enum value using the name of the Enum instead of the underlying integer value.
        /// </summary>
        /// <param name="bitStream">The BitStream to read the value from.</param>
        /// <returns>The value read from the <paramref name="bitStream"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static T ReadName(BitStream bitStream)
        {
            var str = bitStream.ReadString();
            var value = FromName(str);
            return value;
        }

        /// <summary>
        /// Reads the Enum value using the underlying integer value of the Enum instead of the name.
        /// </summary>
        /// <param name="reader">The IValueReader to read the value from.</param>
        /// <param name="name">The name of the value to read.</param>
        /// <returns>The value read from the <paramref name="reader"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static T ReadName(IValueReader reader, string name)
        {
            var str = reader.ReadString(name);
            var value = FromName(str);
            return value;
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <see cref="BitStream"/> to read from.</param>
        /// <returns>The value read from the <see cref="bitStream"/>.</returns>
        /// <exception cref="MethodAccessException"><see cref="SupportsCastOperations"/> is false.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static T ReadValue(BitStream bitStream)
        {
            var v = (int)(bitStream.ReadUInt(_bitsRequired) + _minValue);
            return FromInt(v);
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from a <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>The value read from the <paramref name="reader"/>.</returns>
        /// <exception cref="MethodAccessException"><see cref="SupportsCastOperations"/> is false.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static T ReadValue(IValueReader reader, string name)
        {
            var v = (int)(reader.ReadUInt(name, _bitsRequired) + _minValue);
            return FromInt(v);
        }

        /// <summary>
        /// Casts a value of type <typeparamref name="T"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        /// <exception cref="MethodAccessException"><see cref="SupportsCastOperations"/> is false.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static int ToInt(T value)
        {
            if (!SupportsCastOperations)
                throw CastOperationsNotSupportedException();

            return _toInt(value);
        }

        /// <summary>
        /// Gets the name of the <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Enum"/>.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>The string name of the <paramref name="value"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static string ToName(T value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="outValue">When this method returns true, contains the parsed enum value.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static bool TryParse(string value, out T outValue)
        {
            try
            {
                outValue = (T)Enum.Parse(typeof(T), value);
            }
            catch (ArgumentException)
            {
                outValue = default(T);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">If true, ignore case; otherwise, regard case.</param>
        /// <param name="outValue">When this method returns true, contains the parsed enum value.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static bool TryParse(string value, bool ignoreCase, out T outValue)
        {
            try
            {
                outValue = (T)Enum.Parse(typeof(T), value, ignoreCase);
            }
            catch (ArgumentException)
            {
                outValue = default(T);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Writes the Enum value using the name of the Enum instead of the underlying integer value.
        /// </summary>
        /// <param name="bitStream">The BitStream to write the value to.</param>
        /// <param name="value">The value to write.</param>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static void WriteName(BitStream bitStream, T value)
        {
            var str = ToName(value);
            bitStream.Write(str);
        }

        /// <summary>
        /// Writes the Enum value using the underlying integer value of the Enum instead of the name.
        /// </summary>
        /// <param name="writer">The IValueWriter to write the value to.</param>
        /// <param name="name">The name of the value to write.</param>
        /// <param name="value">The value to write.</param>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static void WriteName(IValueWriter writer, string name, T value)
        {
            var str = ToName(value);
            writer.Write(name, str);
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <see cref="BitStream"/> to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <exception cref="MethodAccessException"><see cref="SupportsCastOperations"/> is false.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static void WriteValue(BitStream bitStream, T value)
        {
            var signedV = ToInt(value) - _minValue;
            Debug.Assert(signedV >= uint.MinValue);
            var v = (uint)signedV;
            bitStream.Write(v, _bitsRequired);
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to a <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">The value to write.</param>
        /// <exception cref="MethodAccessException"><see cref="SupportsCastOperations"/> is false.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static void WriteValue(IValueWriter writer, string name, T value)
        {
            var signedV = ToInt(value) - _minValue;
            Debug.Assert(signedV >= uint.MinValue);
            var v = (uint)signedV;
            writer.Write(name, v, _bitsRequired);
        }
    }
}