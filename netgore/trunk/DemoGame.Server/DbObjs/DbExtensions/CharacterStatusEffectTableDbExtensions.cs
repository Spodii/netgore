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
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Contains extension methods for class CharacterStatusEffectTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class CharacterStatusEffectTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this ICharacterStatusEffectTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["character_id"] = (System.Int32)source.CharacterID;
paramValues["id"] = (System.Int32)source.ID;
paramValues["power"] = (System.UInt16)source.Power;
paramValues["status_effect_id"] = (System.Byte)source.StatusEffect;
paramValues["time_left_secs"] = (System.UInt16)source.TimeLeftSecs;
}

/// <summary>
/// Reads the values from an <see cref="IDataRecord"/> and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataRecord"/> to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this CharacterStatusEffectTable source, System.Data.IDataRecord dataRecord)
{
System.Int32 i;

i = dataRecord.GetOrdinal("character_id");

source.CharacterID = (DemoGame.CharacterID)(DemoGame.CharacterID)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("id");

source.ID = (DemoGame.ActiveStatusEffectID)(DemoGame.ActiveStatusEffectID)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("power");

source.Power = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("status_effect_id");

source.StatusEffect = (DemoGame.StatusEffectType)(DemoGame.StatusEffectType)dataRecord.GetByte(i);

i = dataRecord.GetOrdinal("time_left_secs");

source.TimeLeftSecs = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
}

/// <summary>
/// Reads the values from an <see cref="IDataReader"/> and assigns the read values to this
/// object's properties. Unlike ReadValues(), this method not only doesn't require
/// all values to be in the <see cref="IDataReader"/>, but also does not require the values in
/// the <see cref="IDataReader"/> to be a defined field for the table this class represents.
/// Because of this, you need to be careful when using this method because values
/// can easily be skipped without any indication.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataReader"/> to read the values from. Must already be ready to be read from.</param>
public static void TryReadValues(this CharacterStatusEffectTable source, System.Data.IDataRecord dataRecord)
{
for (int i = 0; i < dataRecord.FieldCount; i++)
{
switch (dataRecord.GetName(i))
{
case "character_id":
source.CharacterID = (DemoGame.CharacterID)(DemoGame.CharacterID)dataRecord.GetInt32(i);
break;


case "id":
source.ID = (DemoGame.ActiveStatusEffectID)(DemoGame.ActiveStatusEffectID)dataRecord.GetInt32(i);
break;


case "power":
source.Power = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
break;


case "status_effect_id":
source.StatusEffect = (DemoGame.StatusEffectType)(DemoGame.StatusEffectType)dataRecord.GetByte(i);
break;


case "time_left_secs":
source.TimeLeftSecs = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
break;


}

}
}

/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The key must already exist in the DbParameterValues
/// for the value to be copied over. If any of the keys in the DbParameterValues do not
/// match one of the column names, or if there is no field for a key, then it will be
/// ignored. Because of this, it is important to be careful when using this method
/// since columns or keys can be skipped without any indication.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void TryCopyValues(this ICharacterStatusEffectTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "character_id":
paramValues[i] = (System.Int32)source.CharacterID;
break;


case "id":
paramValues[i] = (System.Int32)source.ID;
break;


case "power":
paramValues[i] = (System.UInt16)source.Power;
break;


case "status_effect_id":
paramValues[i] = (System.Byte)source.StatusEffect;
break;


case "time_left_secs":
paramValues[i] = (System.UInt16)source.TimeLeftSecs;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="ICharacterStatusEffectTable"/> contains the same values as another <see cref="ICharacterStatusEffectTable"/>.
/// </summary>
/// <param name="source">The source <see cref="ICharacterStatusEffectTable"/>.</param>
/// <param name="otherItem">The <see cref="ICharacterStatusEffectTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="ICharacterStatusEffectTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
/// </returns>
public static System.Boolean HasSameValues(this ICharacterStatusEffectTable source, ICharacterStatusEffectTable otherItem)
{
return Equals(source.CharacterID, otherItem.CharacterID) && 
Equals(source.ID, otherItem.ID) && 
Equals(source.Power, otherItem.Power) && 
Equals(source.StatusEffect, otherItem.StatusEffect) && 
Equals(source.TimeLeftSecs, otherItem.TimeLeftSecs);
}

}

}
