using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
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
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The type of Stat.
        /// </summary>
        readonly TStatType _statType;

        /// <summary>
        /// The Stat's value.
        /// </summary>
        StatValueType _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat{TStatType}"/> class.
        /// </summary>
        /// <param name="istatToCopy">The <see cref="IStat{TStatType}"/> to copy the values from.</param>
        public Stat(IStat<TStatType> istatToCopy)
        {
            _statType = istatToCopy.StatType;
            _value = (StatValueType)istatToCopy.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat{TStatType}"/> class.
        /// </summary>
        /// <param name="istatToCopy">The <see cref="Stat{TStatType}"/> to copy the values from.</param>
        public Stat(Stat<TStatType> istatToCopy)
        {
            _statType = istatToCopy._statType;
            _value = istatToCopy._value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat{TStatType}"/> class.
        /// </summary>
        /// <param name="statType">The type of the stat.</param>
        /// <param name="initialValue">The initial stat value.</param>
        public Stat(TStatType statType, int initialValue) : this(statType, (StatValueType)initialValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat{TStatType}"/> class.
        /// </summary>
        /// <param name="statType">The type of the stat.</param>
        /// <param name="initialValue">The initial stat value.</param>
        public Stat(TStatType statType, StatValueType initialValue)
        {
            _statType = statType;
            _value = initialValue;
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
        /// Notifies listeners that the value of this <see cref="IStat{TStatType}"/> has changed.
        /// </summary>
        public event IStatEventHandler<TStatType> Changed;

        /// <summary>
        /// Gets the <typeparamref name="TStatType"/> of this <see cref="IStat{TStatType}"/>.
        /// </summary>
        public TStatType StatType
        {
            get { return _statType; }
        }

        /// <summary>
        /// Gets or sets the value of this <see cref="IStat{TStatType}"/>.
        /// </summary>
        public StatValueType Value
        {
            get { return _value; }
            set
            {
                // Check that the value has changed
                if (_value == value)
                    return;

                // Set the new value, and invoke the Changed event
                _value = value;

                if (Changed != null)
                    Changed(this);
            }
        }

        /// <summary>
        /// Creates a deep copy of the <see cref="IStat{TStatType}"/>, resulting in a new <see cref="IStat{TStatType}"/>
        /// object of the same type and stat value as this <see cref="IStat{TStatType}"/>.
        /// </summary>
        /// <returns>
        /// The deep copy of the <see cref="IStat{TStatType}"/>.
        /// </returns>
        public virtual IStat<TStatType> DeepCopy()
        {
            return new Stat<TStatType>(StatType, _value);
        }

        /// <summary>
        /// Reads the value for the <see cref="IStat{TStatType}"/> into the <see cref="IStat{StatType}.Value"/> property using
        /// the specified <see cref="BitStream"/>. The <see cref="BitStream"/> must not be null, be in
        /// <see cref="BitStreamMode.Read"/> mode, and must already be positioned at the start of the value to be read.
        /// </summary>
        /// <param name="bitStream"><see cref="BitStream"/> to acquire the value from.</param>
        public void Read(BitStream bitStream)
        {
            _value = bitStream.ReadStatValueType();
        }

        /// <summary>
        /// Reads the value for the <see cref="IStat{TStatType}"/> into the Value property using the specified
        /// <see cref="IDataRecord"/>. The <see cref="IDataRecord"/> must not be null and currently have a row being read.
        /// </summary>
        /// <param name="dataReader"><see cref="IDataRecord"/> to acquire the value from.</param>
        /// <param name="ordinal">Ordinal of the field to read the value from.</param>
        public void Read(IDataRecord dataReader, int ordinal)
        {
            _value = dataReader.GetStatValueType(ordinal);
        }

        /// <summary>
        /// Reads the value for the <see cref="IStat{TStatType}"/> into the Value property using the specified
        /// <see cref="IDataReader"/>. The <see cref="IDataReader"/> must not be null and currently have a row being read.
        /// </summary>
        /// <param name="dataReader"><see cref="IDataReader"/> to acquire the value from.</param>
        /// <param name="name">Name of the field to read the value from.</param>
        public void Read(IDataReader dataReader, string name)
        {
            _value = StatValueType.Read(dataReader, dataReader.GetOrdinal(name));
        }

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

        #endregion
    }
}