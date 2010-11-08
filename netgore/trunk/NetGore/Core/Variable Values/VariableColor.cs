using System;
using System.ComponentModel;
using System.Linq;
using SFML.Graphics;

namespace NetGore
{
    [TypeConverter(typeof(VariableColorConverter))]
    public struct VariableColor : IVariableValue<Color>, IEquatable<VariableColor>
    {
        Color _max;
        Color _min;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableColor"/> struct.
        /// </summary>
        /// <param name="value">The value for both the <see cref="Min"/> and <see cref="Max"/>.</param>
        public VariableColor(Color value)
        {
            _min = value;
            _max = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableColor"/> struct.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        public VariableColor(Color min, Color max)
        {
            _min = min;
            _max = max;

            if (min.R > max.R)
            {
                _min.R = max.R;
                _max.R = min.R;
            }

            if (min.G > max.G)
            {
                _min.G = max.G;
                _max.G = min.G;
            }

            if (min.B > max.B)
            {
                _min.B = max.B;
                _max.B = min.B;
            }

            if (min.A > max.A)
            {
                _min.A = max.A;
                _max.A = min.A;
            }
        }

        /// <summary>
        /// Gets the next value, based off of the <see cref="IVariableValue{T}.Min"/> and <see cref="IVariableValue{T}.Max"/>.
        /// </summary>
        /// <param name="value">The next value, based off of the
        /// <see cref="IVariableValue{T}.Min"/> and <see cref="IVariableValue{T}.Max"/>.</param>
        public void GetNext(ref Color value)
        {
            var a = (byte)RandomHelper.NextInt(_min.A, _max.A);
            var r = (byte)RandomHelper.NextInt(_min.R, _max.R);
            var g = (byte)RandomHelper.NextInt(_min.G, _max.G);
            var b = (byte)RandomHelper.NextInt(_min.B, _max.B);
            value = new Color(r, g, b, a);
        }

        /// <summary>
        /// Gets the next value, based off of the <see cref="IVariableValue{T}.Min"/> and <see cref="IVariableValue{T}.Max"/>.
        /// </summary>
        /// <returns>The next value, based off of the <see cref="IVariableValue{T}.Min"/> and <see cref="IVariableValue{T}.Max"/>.</returns>
        public Vector4 GetNextAsVector4()
        {
            var a = RandomHelper.NextFloat(_min.A, _max.A) / 255f;
            var r = RandomHelper.NextFloat(_min.R, _max.R) / 255f;
            var g = RandomHelper.NextFloat(_min.G, _max.G) / 255f;
            var b = RandomHelper.NextFloat(_min.B, _max.B) / 255f;
            return new Vector4(r, g, b, a);
        }

        #region IVariableValue<Color> Members

        /// <summary>
        /// Gets or sets the inclusive maximum possible value. If this value is set to less than <see cref="IVariableValue{T}.Min"/>,
        /// then <see cref="IVariableValue{T}.Min"/> will be lowered to equal this value.
        /// </summary>
        [Description("The inclusive maximum possible value.")]
        [Category("Variable Value")]
        [EditorBrowsable]
        public Color Max
        {
            get { return _max; }
            set
            {
                _max = value;

                if (_min.A > _max.A)
                    _min.A = _max.A;

                if (_min.R > _max.R)
                    _min.R = _max.R;

                if (_min.G > _max.G)
                    _min.G = _max.G;

                if (_min.B > _max.B)
                    _min.B = _max.B;
            }
        }

        /// <summary>
        /// Gets or sets the inclusive minimum possible value. If this value is set to greater than <see cref="IVariableValue{T}.Max"/>,
        /// then <see cref="IVariableValue{T}.Max"/> will be raised to equal this value.
        /// </summary>
        [Description("The inclusive minimum possible value.")]
        [Category("Variable Value")]
        [EditorBrowsable]
        public Color Min
        {
            get { return _min; }
            set
            {
                _min = value;

                if (_max.A < _min.A)
                    _max.A = _min.A;

                if (_max.R < _min.R)
                    _max.R = _min.R;

                if (_max.G < _min.G)
                    _max.G = _min.G;

                if (_max.B < _min.B)
                    _max.B = _min.B;
            }
        }

        /// <summary>
        /// Gets the next value, based off of the <see cref="IVariableValue{T}.Min"/> and <see cref="IVariableValue{T}.Max"/>.
        /// </summary>
        /// <returns>The next value, based off of the <see cref="IVariableValue{T}.Min"/> and <see cref="IVariableValue{T}.Max"/>.</returns>
        public Color GetNext()
        {
            var a = (byte)RandomHelper.NextInt(_min.A, _max.A);
            var r = (byte)RandomHelper.NextInt(_min.R, _max.R);
            var g = (byte)RandomHelper.NextInt(_min.G, _max.G);
            var b = (byte)RandomHelper.NextInt(_min.B, _max.B);
            return new Color(r, g, b, a);
        }

        #endregion

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(VariableColor other)
        {
            return other._max.Equals(_max) && other._min.Equals(_min);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is VariableColor && this == (VariableColor)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_max.GetHashCode() * 397) ^ _min.GetHashCode();
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(VariableColor left, VariableColor right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(VariableColor left, VariableColor right)
        {
            return !left.Equals(right);
        }
    }
}