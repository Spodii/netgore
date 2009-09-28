using System;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Base class for a class that helps with writing and reading enum values.
    /// </summary>
    /// <typeparam name="T">The Type of Enum.</typeparam>
    public abstract class EnumValuesHelperBase<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        static readonly Type[] _supportedTypes = new Type[]
        { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int) };

        readonly int _bitsRequired;
        readonly int _maxValue;
        readonly int _minValue;

        /// <summary>
        /// Gets the number of bits required to write the enums values.
        /// </summary>
        public int BitsRequired
        {
            get { return _bitsRequired; }
        }

        /// <summary>
        /// Gets the maximum defined enum value.
        /// </summary>
        public int MaxValue
        {
            get { return _maxValue; }
        }

        /// <summary>
        /// Gets the minimum defined enum value.
        /// </summary>
        public int MinValue
        {
            get { return _minValue; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValuesHelperBase&lt;T&gt;"/> class.
        /// </summary>
        protected EnumValuesHelperBase()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type parameter must be an enum.");

            Type underlyingType = Enum.GetUnderlyingType(typeof(T));
            if (!_supportedTypes.Contains(underlyingType))
                throw new ArgumentException("The given enum type parameter's underlying type is not supported.");

            var values = Enum.GetValues(typeof(T)).Cast<T>().Select(x => Convert.ToInt32(x));

            _minValue = values.Min();
            _maxValue = values.Max();
            Debug.Assert(_minValue <= _maxValue);

            int diff = _maxValue - _minValue;
            Debug.Assert(diff >= 0);
            Debug.Assert(diff >= uint.MinValue && diff <= uint.MaxValue);

            _bitsRequired = BitOps.RequiredBits((uint)diff);
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <typeparamref name="T"/>.</returns>
        protected abstract T FromInt(int value);

        /// <summary>
        /// When overridden in the derived class, casts type <typeparamref name="T"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected abstract int ToInt(T value);

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <see cref="BitStream"/> to write to.</param>
        /// <param name="value">The value to write.</param>
        public void Write(BitStream bitStream, T value)
        {
            uint v = (uint)(ToInt(value) - _minValue);
            bitStream.Write(v, _bitsRequired);
        }


        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to a <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">The value to write.</param>
        public void Write(IValueWriter writer, string name, T value)
        {
            uint v = (uint)(ToInt(value) - _minValue);
            writer.Write(name, v, _bitsRequired);
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <see cref="BitStream"/> to read from.</param>
        /// <returns>The value read from the <see cref="bitStream"/>.</returns>
        public T Read(BitStream bitStream)
        {
            int v = (int)(bitStream.ReadUInt(_bitsRequired) + _minValue);
            return FromInt(v);
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from a <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>The value read from the <paramref name="reader"/>.</returns>
        public T Read(IValueReader reader, string name)
        {
            int v = (int)(reader.ReadUInt(name, _bitsRequired) + _minValue);
            return FromInt(v);
        }
    }
}