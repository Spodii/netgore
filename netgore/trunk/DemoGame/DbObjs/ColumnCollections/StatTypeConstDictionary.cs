/********************************************************************
                   DO NOT MANUALLY EDIT THIS FILE!

This file was automatically generated using the DbClassCreator
program. The only time you should ever alter this file is if you are
using an automated code formatter. The DbClassCreator will overwrite
this file every time it is run, so all manual changes will be lost.
If there is something in this file that you wish to change, you should
be able to do it through the DbClassCreator arguments.

Make sure that you re-run the DbClassCreator every time you alter your
game's database.

For more information on the DbClassCreator, please see:
    http://www.netgore.com/wiki/DbClassCreator
********************************************************************/

using System;
using System.Linq;
using NetGore;
using NetGore.IO;
using System.Collections.Generic;
using System.Collections;

using DemoGame.DbObjs;
namespace DemoGame.DbObjs
{
/// <summary>
/// A Dictionary-like lookup table for the Enum values of the type collection `Stat` for the
/// table that this class represents. Majority of the code for this class was automatically generated and
/// only other automatically generated code should be using this class.
/// </summary>
public class StatTypeConstDictionary : System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>>, IPersistable
{
    /// <summary>
    /// Name of the node that contains all the values.
    /// </summary>
    const string _valuesNodeName = "Values";

    /// <summary>
    /// Name of the key for the value's key.
    /// </summary>
    const string _keyKeyName = "Key";

    /// <summary>
    /// Name of the key for the value's value.
    /// </summary>
    const string _valueKeyName = "Value";
    
    /// <summary>
    /// Array that takes in the enum's value (casted to an int) as the array index, and spits out the
    /// corresponding index for the instanced <see name="_values"/> array. This allows us to build an array
    /// of values without wasting any indicies even if the defined enum skips values.
    /// </summary>
    static readonly int[] _enumToValueIndex;

    /// <summary>
    /// Array that takes in the <see cref="_values"/> array index and spits out the enum value that the
    /// index is for. This is to allow for a reverse-lookup on the <see cref="_enumToValueIndex"/>.
    /// </summary>
    static readonly DemoGame.StatType[] _valueIndexToKey;

    /// <summary>
    /// The total number of unique defined enum values. Each instanced <see cref="_values"/> array
    /// will have a length equal to this value.
    /// </summary>
    static readonly int _numEnumValues;

    /// <summary>
    /// Array containing the actual values. The index of this array is found through the value returned
    /// from the _lookupTable.
    /// </summary>
    readonly System.Int16[] _values;

    /// <summary>
    /// Gets the <see cref="Type"/> used internally to store the values. This may or may not be the same as the
    /// type used to expose the values.
    /// </summary>
    public static Type InternalType { get { return typeof(System.Int16); } }

    /// <summary>
    /// Initializes the <see cref="StatTypeConstDictionary"/> class.
    /// </summary>
    static StatTypeConstDictionary()
    {
        _valueIndexToKey = EnumHelper<DemoGame.StatType>.Values.ToArray();
        _numEnumValues = _valueIndexToKey.Length;
        _enumToValueIndex = new int[EnumHelper<DemoGame.StatType>.MaxValue + 1];

        for (int i = 0; i < _valueIndexToKey.Length; i++)
        {
            var key = (int)_valueIndexToKey[i];
            _enumToValueIndex[key] = i;
        }
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="StatTypeConstDictionary"/> class.
    /// </summary>
    public StatTypeConstDictionary()
    {
        _values = new System.Int16[_numEnumValues];
    }
    
    /// <summary>
    /// Gets or sets an item's value using the <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key for the value to get or set.</param>
    /// <returns>The item's value for the corresponding <paramref name="key"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than <see cref="System.Int16.MinValue"/>
    /// or greater than <see cref="System.Int16.MaxValue"/>.</exception>
    public System.Int32 this[DemoGame.StatType key]
    {
        get 
        { 
			return (System.Int32)_values[_enumToValueIndex[(int)key]]; 
		}
        set 
        {
			if (value > System.Int16.MaxValue || value < System.Int16.MinValue)
				throw new ArgumentOutOfRangeException("value", "Value must be between " + System.Int16.MinValue + " and " + System.Int16.MaxValue + ".");
				 
			_values[_enumToValueIndex[(int)key]] = (System.Int16)value; 
		}
    }

    #region IEnumerable<KeyValuePair<DemoGame.StatType,System.Int32>> Members

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<KeyValuePair<DemoGame.StatType, System.Int32>> GetEnumerator()
    {
        for (int i = 0; i < _values.Length; i++)
        {
            yield return new KeyValuePair<DemoGame.StatType, System.Int32>(_valueIndexToKey[i], (System.Int32)_values[i]);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    /// <summary>
    /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
    /// same order as they were written.
    /// </summary>
    /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
    public void ReadState(IValueReader reader)
    {
        // Zero all the existing values
        for (int i = 0; i < _values.Length; i++)
            _values[i] = default(System.Int16);

        // Read and set the values
        var values = reader.ReadManyNodes<KeyValuePair<DemoGame.StatType, System.Int32>>(_valuesNodeName, ReadValueHandler);
        foreach (var value in values)
        {
            this[value.Key] = value.Value;
        }
    }

    /// <summary>
    /// Writes the state of the object to an <see cref="IValueWriter"/>.
    /// </summary>
    /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
    public void WriteState(IValueWriter writer)
    {
        writer.WriteManyNodes(_valuesNodeName, this.Where(x => x.Value != default(System.Int32)), WriteValueHandler);
    }

    /// <summary>
    /// Reads a <see cref="KeyValuePair{Key, Value}"/>.
    /// </summary>
    /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
    /// <returns>The read <see cref="KeyValuePair{Key, Value}"/>.</returns>
    static KeyValuePair<DemoGame.StatType, System.Int32> ReadValueHandler(IValueReader reader)
    {
        var key = reader.ReadEnum<DemoGame.StatType>(_keyKeyName);
        var value = (System.Int32)reader.ReadShort(_valueKeyName);
        return new KeyValuePair<DemoGame.StatType, System.Int32>(key, value);
    }

    /// <summary>
    /// Writes a <see cref="KeyValuePair{Key, Value}"/>.
    /// </summary>
    /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
    /// <param name="value">The <see cref="KeyValuePair{Key, Value}"/> to write.</param>
    static void WriteValueHandler(IValueWriter writer, KeyValuePair<DemoGame.StatType, System.Int32> value)
    {
        writer.WriteEnum(_keyKeyName, value.Key);
        writer.Write(_valueKeyName, (System.Int16)value.Value);
    }
}
}
