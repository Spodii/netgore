using System;
using System.Data;
using System.Linq;
using NetGore.IO;

namespace NetGore.Stats
{
    /// <summary>
    /// Describes a single <see cref="IStat{TStatType}"/>, containing the <typeparamref name="TStatType"/>
    /// and value current stat value.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public class Stat<TStatType> : IStat<TStatType> where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// The type of Stat.
        /// </summary>
        readonly TStatType _statType;

        /// <summary>
        /// The Stat's value.
        /// </summary>
        IStatValueType _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat{TStatType}"/> class.
        /// </summary>
        /// <param name="istatToCopy">The <see cref="IStat{StatType}"/> to copy the values from.</param>
        public Stat(IStat<TStatType> istatToCopy)
        {
            _statType = istatToCopy.StatType;
            _value = istatToCopy.DeepCopyValueType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat{TStatType}"/> class.
        /// </summary>
        /// <param name="istatToCopy">The <see cref="IStat{StatType}"/> to copy the values from.</param>
        /// <param name="initialValue">The initial value to assign to this <see cref="Stat{StatType}"/>.
        /// If not specified, the initial value
        /// will end up being equal to the Value of <paramref name="istatToCopy"/>.</param>
        public Stat(IStat<TStatType> istatToCopy, int initialValue) : this(istatToCopy)
        {
            Value = initialValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat{TStatType}"/> class.
        /// </summary>
        /// <param name="statType">The <see cref="StatType"/> of this <see cref="Stat{StatType}"/>.</param>
        /// <param name="statValueType">The <see cref="IStatValueType"/> to store the stat value in.</param>
        public Stat(TStatType statType, IStatValueType statValueType)
        {
            _statType = statType;
            _value = statValueType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat{TStatType}"/> class.
        /// </summary>
        /// <param name="statType">The <typeparamref name="TStatType"/> of this <see cref="Stat{StatType}"/>.</param>
        /// <param name="statValueType">The <see cref="IStatValueType"/> to store the stat value in.</param>
        /// <param name="initialValue">The initial value to assign to this <see cref="Stat{StatType}"/>.
        /// If not specified, the initial value will end up being the current value of the
        /// <paramref name="statValueType"/>.</param>
        public Stat(TStatType statType, IStatValueType statValueType, int initialValue) : this(statType, statValueType)
        {
            Value = initialValue;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return _statType + ": " + Value;
        }

        #region IStat<TStatType> Members

        /// <summary>
        /// Gets the <typeparamref name="TStatType"/> of this <see cref="IStat{TStatType}"/>.
        /// </summary>
        /// <value></value>
        public TStatType StatType
        {
            get { return _statType; }
        }

        /// <summary>
        /// Gets or sets the value of this <see cref="IStat{TStatType}"/> as an integer.
        /// </summary>
        /// <value></value>
        public int Value
        {
            get { return _value.GetValue(); }
            set
            {
                // Check that the value has changed
                if (_value.GetValue() == value)
                    return;

                // Set the new value, and invoke the OnChange event
                _value = _value.SetValue(value);

                if (OnChange != null)
                    OnChange(this);
            }
        }

        /// <summary>
        /// Reads the value for the <see cref="IStat{TStatType}"/> into the <see cref="IStat{StatType}.Value"/> property using
        /// the specified <see cref="BitStream"/>. The <see cref="BitStream"/> must not be null, be in
        /// <see cref="BitStreamMode.Read"/> mode, and must already be positioned at the start of the value to be read.
        /// </summary>
        /// <param name="bitStream"><see cref="BitStream"/> to acquire the value from.</param>
        public void Read(BitStream bitStream)
        {
            _value = _value.Read(bitStream);
        }

        /// <summary>
        /// Reads the value for the <see cref="IStat{TStatType}"/> into the Value property using the specified
        /// <see cref="IDataRecord"/>. The <see cref="IDataRecord"/> must not be null and currently have a row being read.
        /// </summary>
        /// <param name="dataReader"><see cref="IDataRecord"/> to acquire the value from.</param>
        /// <param name="ordinal">Ordinal of the field to read the value from.</param>
        public void Read(IDataRecord dataReader, int ordinal)
        {
            _value = _value.Read(dataReader, ordinal);
        }

        /// <summary>
        /// Reads the value for the <see cref="IStat{TStatType}"/> into the Value property using the specified
        /// <see cref="IDataReader"/>. The <see cref="IDataReader"/> must not be null and currently have a row being read.
        /// </summary>
        /// <param name="dataReader"><see cref="IDataReader"/> to acquire the value from.</param>
        /// <param name="name">Name of the field to read the value from.</param>
        public void Read(IDataReader dataReader, string name)
        {
            _value = _value.Read(dataReader, dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Notifies listeners that the value of this <see cref="IStat{TStatType}"/> has changed.
        /// </summary>
        public event IStatEventHandler<TStatType> OnChange;

        /// <summary>
        /// Writes the <see cref="IStat{StatType}.Value"/> property of the <see cref="IStat{TStatType}"/> directly into the
        /// specified <see cref="BitStream"/>. The <see cref="BitStream"/> must not be null, be in
        /// <see cref="BitStreamMode.Write"/> mode, and be positioned at the location where the value is to be written.
        /// </summary>
        /// <param name="bitStream"><see cref="BitStream"/> to write the value of the <see cref="IStat{TStatType}"/> to.</param>
        public void Write(BitStream bitStream)
        {
            _value.Write(bitStream);
        }

        /// <summary>
        /// Creates a deep copy of the <see cref="IStat{TStatType}"/>'s <see cref="IStatValueType"/>, resulting in a new
        /// <see cref="IStatValueType"/> object of the same type as this <see cref="IStat{TStatType}"/>'s
        /// <see cref="IStatValueType"/>, and containing the same value.
        /// </summary>
        /// <returns>
        /// The deep copy of the <see cref="IStat{TStatType}"/>'s <see cref="IStatValueType"/>.
        /// </returns>
        public IStatValueType DeepCopyValueType()
        {
            return _value.DeepCopy();
        }

        /// <summary>
        /// Creates a deep copy of the <see cref="IStat{TStatType}"/>, resulting in a new <see cref="IStat{TStatType}"/>
        /// object of the same type as this <see cref="IStat{TStatType}"/>, and containing the same <see cref="IStatValueType"/>
        /// with the same value, and same <typeparamref name="TStatType"/>.
        /// </summary>
        /// <returns>
        /// The deep copy of the <see cref="IStat{TStatType}"/>.
        /// </returns>
        public virtual IStat<TStatType> DeepCopy()
        {
            return new Stat<TStatType>(StatType, _value, Value);
        }

        #endregion
    }

    /// <summary>
    /// Describes a single <see cref="IStat{StatType}"/>, containing the <typeparamref name="TStatType"/> and value
    /// of the stat.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    /// <typeparam name="TStatValueType">The type of <see cref="IStatValueType"/>.</typeparam>
    public class Stat<TStatType, TStatValueType> : Stat<TStatType>
        where TStatType : struct, IComparable, IConvertible, IFormattable where TStatValueType : IStatValueType, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Stat{TStatType,TStatValueType}"/> class.
        /// </summary>
        /// <param name="statType">The type of stat.</param>
        /// <param name="initialValue">The initial value to assign to this
        /// <see cref="Stat{TStatType,TStatValueType}"/>.</param>
        public Stat(TStatType statType, int initialValue) : base(statType, new TStatValueType(), initialValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat{TStatType, TStatValueType}"/> class.
        /// </summary>
        /// <param name="statType">The type of stat.</param>
        public Stat(TStatType statType) : base(statType, new TStatValueType())
        {
        }
    }
}