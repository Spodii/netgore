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
/// Contains extension methods for class QuestTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class QuestTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IQuestTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["id"] = (System.UInt16)source.ID;
paramValues["repeatable"] = (System.Boolean)source.Repeatable;
paramValues["reward_cash"] = (System.Int32)source.RewardCash;
paramValues["reward_exp"] = (System.Int32)source.RewardExp;
}

/// <summary>
/// Reads the values from an <see cref="IDataRecord"/> and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataRecord"/> to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this QuestTable source, System.Data.IDataRecord dataRecord)
{
System.Int32 i;

i = dataRecord.GetOrdinal("id");

source.ID = (NetGore.Features.Quests.QuestID)(NetGore.Features.Quests.QuestID)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("repeatable");

source.Repeatable = (System.Boolean)(System.Boolean)dataRecord.GetBoolean(i);

i = dataRecord.GetOrdinal("reward_cash");

source.RewardCash = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("reward_exp");

source.RewardExp = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
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
public static void TryReadValues(this QuestTable source, System.Data.IDataRecord dataRecord)
{
for (int i = 0; i < dataRecord.FieldCount; i++)
{
switch (dataRecord.GetName(i))
{
case "id":
source.ID = (NetGore.Features.Quests.QuestID)(NetGore.Features.Quests.QuestID)dataRecord.GetUInt16(i);
break;


case "repeatable":
source.Repeatable = (System.Boolean)(System.Boolean)dataRecord.GetBoolean(i);
break;


case "reward_cash":
source.RewardCash = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "reward_exp":
source.RewardExp = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
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
public static void TryCopyValues(this IQuestTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "id":
paramValues[i] = (System.UInt16)source.ID;
break;


case "repeatable":
paramValues[i] = (System.Boolean)source.Repeatable;
break;


case "reward_cash":
paramValues[i] = (System.Int32)source.RewardCash;
break;


case "reward_exp":
paramValues[i] = (System.Int32)source.RewardExp;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="IQuestTable"/> contains the same values as another <see cref="IQuestTable"/>.
/// </summary>
/// <param name="source">The source <see cref="IQuestTable"/>.</param>
/// <param name="otherItem">The <see cref="IQuestTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="IQuestTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
/// </returns>
public static System.Boolean HasSameValues(this IQuestTable source, IQuestTable otherItem)
{
return Equals(source.ID, otherItem.ID) && 
Equals(source.Repeatable, otherItem.Repeatable) && 
Equals(source.RewardCash, otherItem.RewardCash) && 
Equals(source.RewardExp, otherItem.RewardExp);
}

}

}
