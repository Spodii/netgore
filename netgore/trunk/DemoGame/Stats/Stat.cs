using System.Data;
using NetGore.IO;

namespace DemoGame
{
    public class Stat<T> : IStat where T : IStatValueType, new()
    {
        readonly StatType _statType;
        readonly T _value = new T();

        public Stat(StatType statType)
        {
            _statType = statType;
        }

        public override string ToString()
        {
            return _statType + ": " + Value;
        }

        #region IStat Members

        public bool CanWrite
        {
            get { return true; }
        }

        public StatType StatType
        {
            get { return _statType; }
        }

        public int Value
        {
            get { return _value.GetValue(); }
            set
            {
                if (_value.GetValue() == value)
                    return;

                _value.SetValue(value);

                if (OnChange != null)
                    OnChange((IStat)this);
            }
        }

        public void Read(BitStream bitStream)
        {
            _value.Read(bitStream);
        }

        public void Read(IDataRecord dataReader, int ordinal)
        {
            _value.Read(dataReader, ordinal);
        }

        public void Read(IDataReader dataReader, string name)
        {
            _value.Read(dataReader, dataReader.GetOrdinal(name));
        }

        public event StatChangeHandler OnChange;

        public void Write(BitStream bitStream)
        {
            _value.Write(bitStream);
        }

        public IStatValueType DeepCopyValueType()
        {
            return _value.DeepCopy();
        }

        public IStat DeepCopy()
        {
            var copy = new Stat<T>(_statType);
            copy._value.SetValue(_value.GetValue());
            return copy;
        }

        #endregion
    }
}