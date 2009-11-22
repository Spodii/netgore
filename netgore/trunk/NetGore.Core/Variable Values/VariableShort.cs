using System.ComponentModel;
using System.Linq;

namespace NetGore
{
    [TypeConverter(typeof(VariableShortConverter))]
    public struct VariableShort : IVariableValue<short>
    {
        short _max;
        short _min;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableShort"/> struct.
        /// </summary>
        /// <param name="value">The value for both the <see cref="Min"/> and <see cref="Max"/>.</param>
        public VariableShort(short value)
        {
            _min = value;
            _max = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableShort"/> struct.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        public VariableShort(short min, short max)
        {
            if (min <= max)
            {
                _min = min;
                _max = max;
            }
            else
            {
                _min = max;
                _max = min;
            }
        }

        #region IVariableValue<short> Members

        /// <summary>
        /// Gets or sets the inclusive minimum possible value. If this value is set to greater than <see cref="IVariableValue{T}.Max"/>,
        /// then <see cref="IVariableValue{T}.Max"/> will be raised to equal this value.
        /// </summary>
        [Description("The inclusive minimum possible value.")]
        [Category("Variable Value")]
        [EditorBrowsable]
        public short Min
        {
            get { return _min; }
            set
            {
                _min = value;
                if (_max < _min)
                    _max = _min;
            }
        }

        /// <summary>
        /// Gets or sets the inclusive maximum possible value. If this value is set to less than <see cref="IVariableValue{T}.Min"/>,
        /// then <see cref="IVariableValue{T}.Min"/> will be lowered to equal this value.
        /// </summary>
        [Description("The inclusive maximum possible value.")]
        [Category("Variable Value")]
        [EditorBrowsable]
        public short Max
        {
            get { return _max; }
            set
            {
                _max = value;
                if (_min > _max)
                    _min = _max;
            }
        }

        /// <summary>
        /// Gets the next value, based off of the <see cref="IVariableValue{T}.Min"/> and <see cref="IVariableValue{T}.Max"/>.
        /// </summary>
        /// <returns>The next value, based off of the <see cref="IVariableValue{T}.Min"/> and <see cref="IVariableValue{T}.Max"/>.</returns>
        public short GetNext()
        {
            return (short)RandomHelper.NextInt(Min, Max + 1);
        }

        #endregion
    }
}