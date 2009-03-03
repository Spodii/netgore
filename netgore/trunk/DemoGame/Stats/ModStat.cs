using System;
using NetGore.IO.Bits;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

// TODO: I need to find a decent way to make the StatValueTypes an immutable struct, 
// making them essentially just a wrapper of the ValueType.
// The disadvantage of a class really shows here in ModStat, where I have to use the ModHandler to
// get an int then write that int to the StatValueType just to write it into the BitStream

namespace DemoGame
{
    public static class ModStat
    {
        static readonly StatType?[] _baseToModStat;
        static readonly StatType?[] _modToBaseStat;

        static ModStat()
        {
            Array enumValues = Enum.GetValues(typeof(StatType));

            int numStats = enumValues.Length;
            _baseToModStat = new StatType?[numStats];
            _modToBaseStat = new StatType?[numStats];

            var statTypes = enumValues.Cast<StatType>();
            var modStats = statTypes.Where(statType => statType.ToString().StartsWith("Mod"));
            var nonModStats = statTypes.Except(modStats);

            foreach (StatType modStat in modStats)
            {
                string baseStatName = modStat.ToString().Substring("Mod".Length);
                StatType baseStat = nonModStats.First(nonMod => nonMod.ToString() == baseStatName);

                _baseToModStat[(int)baseStat] = modStat;
                _modToBaseStat[(int)modStat] = baseStat;
            }
        }

        /// <summary>
        /// Gets the base StatType for a mod StatType, if one exists.
        /// </summary>
        /// <param name="modStatType">Mod StatType to get the base StatType for.</param>
        /// <returns>Base stat type for the given <paramref name="modStatType"/>.</returns>
        /// <exception cref="ArgumentException">No mod stat / base stat relationship found for the specified StatType.</exception>
        public static StatType? GetBaseStat(StatType modStatType)
        {
            var st = _modToBaseStat[(int)modStatType];
            if (!st.HasValue)
                throw new ArgumentException("Could not find a base stat for the given mod stat.", "modStatType");
            return st.Value;
        }

        /// <summary>
        /// Gets the mod StatType for a base StatType, if one exists.
        /// </summary>
        /// <param name="baseStatType">Base StatType to get the mod StatType for.</param>
        /// <returns>Mod stat type for the given <paramref name="baseStatType"/>.</returns>
        /// <exception cref="ArgumentException">No mod stat / base stat relationship found for the specified StatType.</exception>
        public static StatType GetModStat(StatType baseStatType)
        {
            var st = _baseToModStat[(int)baseStatType];
            if (!st.HasValue)
                throw new ArgumentException("Could not find a mod stat for the given base stat.", "baseStatType");
            return st.Value;
        }

        public static bool TryGetBaseStat(StatType modStatType, out StatType baseStatType)
        {
            var st = _modToBaseStat[(int)modStatType];

            if (st.HasValue)
            {
                baseStatType = st.Value;
                return true;
            }
            else
            {
                baseStatType = 0;
                return false;
            }
        }

        public static bool TryGetModStat(StatType baseStatType, out StatType modStatType)
        {
            var st = _baseToModStat[(int)baseStatType];

            if (st.HasValue)
            {
                modStatType = st.Value;
                return true;
            }
            else
            {
                modStatType = 0;
                return false;
            }
        }
    }

    public class ModStat<T> : IStat where T : IStatValueType, new()
    {
        readonly ModStatHandler _modHandler;
        readonly StatType _statType;
        readonly T _value = new T();

        public ModStatHandler ModHandler
        {
            get { return _modHandler; }
        }

        public ModStat(StatType statType, ModStatHandler modHandler)
        {
            if (modHandler == null)
                throw new ArgumentNullException("modHandler");

            _statType = statType;
            _modHandler = modHandler;
        }

        #region IStat Members

        public int Value
        {
            get
            {
                _value.SetValue(_modHandler());
                return _value.GetValue();
            }
            set { throw new MethodAccessException("Can not set the Value for a ModStat."); }
        }

        public void Read(BitStream bitStream)
        {
            throw new MethodAccessException("Can not set the Value for a ModStat.");
        }

        public void Read(IDataRecord dataReader, int ordinal)
        {
            throw new MethodAccessException("Can not set the Value for a ModStat.");
        }

        public bool CanWrite
        {
            get { return false; }
        }

        public event StatChangeHandler OnChange
        {
            add { throw new NotImplementedException("Need a decent way to handle tracking the ModStat's changes"); }
            remove { throw new NotImplementedException("Need a decent way to handle tracking the ModStat's changes"); }
        }

        public void Write(BitStream bitStream)
        {
            _value.SetValue(_modHandler());
            _value.Write(bitStream);
        }

        public IStatValueType DeepCopyValueType()
        {
            return _value.DeepCopy();
        }

        public IStat DeepCopy()
        {
            var copy = new ModStat<T>(_statType, _modHandler);
            copy._value.SetValue(_value.GetValue());
            return copy;
        }

        public StatType StatType
        {
            get { return _statType; }
        }

        #endregion
    }
}