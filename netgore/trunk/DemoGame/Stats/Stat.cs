using System.Data;
using System.Linq;
using DemoGame;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Describes a single IStat, containing the StatType and value of the IStat.
    /// </summary>
    public class Stat : IStat
    {
        /// <summary>
        /// The type of Stat.
        /// </summary>
        readonly StatType _statType;

        /// <summary>
        /// The Stat's value.
        /// </summary>
        IStatValueType _value;

        /// <summary>
        /// Stat constructor.
        /// </summary>
        /// <param name="istatToCopy">The IStat to copy the values from.</param>
        public Stat(IStat istatToCopy)
        {
            _statType = istatToCopy.StatType;
            _value = istatToCopy.DeepCopyValueType();
        }

        /// <summary>
        /// Stat constructor.
        /// </summary>
        /// <param name="istatToCopy">The IStat to copy the values from.</param>
        /// <param name="initialValue">The initial value to assign to this Stat. If not specified, the initial value
        /// will end up being equal to the Value of <paramref name="istatToCopy"/>.</param>
        public Stat(IStat istatToCopy, int initialValue) : this(istatToCopy)
        {
            Value = initialValue;
        }

        /// <summary>
        /// Stat constructor.
        /// </summary>
        /// <param name="statType">The StatType of this Stat.</param>
        /// <param name="statValueType">The IStatValueType to store the stat value in.</param>
        public Stat(StatType statType, IStatValueType statValueType)
        {
            _statType = statType;
            _value = statValueType;
        }

        /// <summary>
        /// Stat constructor.
        /// </summary>
        /// <param name="statType">The StatType of this Stat.</param>
        /// <param name="statValueType">The IStatValueType to store the stat value in.</param>
        /// <param name="initialValue">The initial value to assign to this Stat. If not specified, the initial value
        /// will end up being the current value of the <paramref name="statValueType"/>.</param>
        public Stat(StatType statType, IStatValueType statValueType, int initialValue) : this(statType, statValueType)
        {
            Value = initialValue;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return _statType + ": " + Value;
        }

        #region IStat Members

        /// <summary>
        /// Gets the StatType of this IStat.
        /// </summary>
        public StatType StatType
        {
            get { return _statType; }
        }

        /// <summary>
        /// Gets or sets the value of this IStat as an integer.
        /// </summary>
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
        /// Reads the value for the IStat into the Value property using the specified BitStream. The BitStream
        /// must not be null and must already be positioned at the start (first byte) of the value to be read.
        /// </summary>
        /// <param name="bitStream">BitStream to acquire the value from.</param>
        public void Read(BitStream bitStream)
        {
            _value = _value.Read(bitStream);
        }

        /// <summary>
        /// Reads the value for the IStat into the Value property using the specified IDataRecord. The IDataReader
        /// must not be null and currently have a row being read.
        /// </summary>
        /// <param name="dataReader">IDataRecord to acquire the value from.</param>
        /// <param name="ordinal">Ordinal of the field to read the value from.</param>
        public void Read(IDataRecord dataReader, int ordinal)
        {
            _value = _value.Read(dataReader, ordinal);
        }

        /// <summary>
        /// Reads the value for the IStat into the Value property using the specified IDataRecord. The IDataReader
        /// must not be null and currently have a row being read.
        /// </summary>
        /// <param name="dataReader">IDataRecord to acquire the value from.</param>
        /// <param name="name">Name of the field to read the value from.</param>
        public void Read(IDataReader dataReader, string name)
        {
            _value = _value.Read(dataReader, dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Notifies listeners that the value of the stat has changed.
        /// </summary>
        public event IStatEventHandler OnChange;

        /// <summary>
        /// Writes the Value property of the IStat directly into the specified BitStream. The BitStream must not be null,
        /// be in Write mode, and be positioned at the location where the value is to be written.
        /// </summary>
        /// <param name="bitStream">BitStream to write the value of the IStat to.</param>
        public void Write(BitStream bitStream)
        {
            _value.Write(bitStream);
        }

        /// <summary>
        /// Creates a deep copy of the IStat's IStatValueType, resulting in a new IStatValueType object of the same
        /// type as this IStat's IStatValueType, and containing the same value.
        /// </summary>
        /// <returns>The deep copy of the IStat's IStatValueType.</returns>
        public IStatValueType DeepCopyValueType()
        {
            return _value.DeepCopy();
        }

        /// <summary>
        /// Creates a deep copy of the IStat, resulting in a new IStat object of the same type as this IStat, and
        /// containing the same IStatValueType with the same value, and same StatType.
        /// </summary>
        /// <returns>The deep copy of the IStat.</returns>
        public virtual IStat DeepCopy()
        {
            return new Stat(StatType, _value, Value);
        }

        #endregion
    }

    /// <summary>
    /// Describes a single IStat, containing the StatType and value of the IStat.
    /// </summary>
    /// <typeparam name="T">The Type of IStatValueType.</typeparam>
    public class Stat<T> : Stat where T : IStatValueType, new()
    {
        /// <summary>
        /// Stat constructor.
        /// </summary>
        /// <param name="statType">The StatType of this Stat.</param>
        /// <param name="initialValue">The initial value to assign to this Stat.</param>
        public Stat(StatType statType, int initialValue) : base(statType, new T(), initialValue)
        {
        }

        /// <summary>
        /// Stat constructor.
        /// </summary>
        /// <param name="statType">The StatType of this Stat.</param>
        public Stat(StatType statType) : base(statType, new T())
        {
        }
    }
}