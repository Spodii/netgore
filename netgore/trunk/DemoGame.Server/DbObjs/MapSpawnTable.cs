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
/// Provides a strongly-typed structure for the database table `map_spawn`.
/// </summary>
public class MapSpawnTable : IMapSpawnTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"amount", "character_template_id", "direction_id", "height", "id", "map_id", "respawn", "width", "x", "y" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumns;
}
}
/// <summary>
/// Array of the database column names for columns that are primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsKeys = new string[] {"id" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsKeys;
}
}
/// <summary>
/// Array of the database column names for columns that are not primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"amount", "character_template_id", "direction_id", "height", "map_id", "respawn", "width", "x", "y" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbNonKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsNonKey;
}
}
/// <summary>
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "map_spawn";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 10;
/// <summary>
/// The field that maps onto the database column `amount`.
/// </summary>
System.Byte _amount;
/// <summary>
/// The field that maps onto the database column `character_template_id`.
/// </summary>
System.UInt16 _characterTemplateID;
/// <summary>
/// The field that maps onto the database column `direction_id`.
/// </summary>
System.Int16 _directionId;
/// <summary>
/// The field that maps onto the database column `height`.
/// </summary>
System.Nullable<System.UInt16> _height;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.Int32 _iD;
/// <summary>
/// The field that maps onto the database column `map_id`.
/// </summary>
System.UInt16 _mapID;
/// <summary>
/// The field that maps onto the database column `respawn`.
/// </summary>
System.UInt16 _respawn;
/// <summary>
/// The field that maps onto the database column `width`.
/// </summary>
System.Nullable<System.UInt16> _width;
/// <summary>
/// The field that maps onto the database column `x`.
/// </summary>
System.Nullable<System.UInt16> _x;
/// <summary>
/// The field that maps onto the database column `y`.
/// </summary>
System.Nullable<System.UInt16> _y;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `amount`.
/// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
/// "The total number of NPCs this spawner will spawn.".
/// </summary>
[System.ComponentModel.Description("The total number of NPCs this spawner will spawn.")]
[NetGore.SyncValueAttribute()]
public System.Byte Amount
{
get
{
return (System.Byte)_amount;
}
set
{
this._amount = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_template_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The character template used to instantiate the spawned NPCs.".
/// </summary>
[System.ComponentModel.Description("The character template used to instantiate the spawned NPCs.")]
[NetGore.SyncValueAttribute()]
public DemoGame.CharacterTemplateID CharacterTemplateID
{
get
{
return (DemoGame.CharacterTemplateID)_characterTemplateID;
}
set
{
this._characterTemplateID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `direction_id`.
/// The underlying database type is `smallint(5)` with the default value of `0`.The database column contains the comment: 
/// "The direction of this spawn; None if randonm".
/// </summary>
[System.ComponentModel.Description("The direction of this spawn; None if randonm")]
[NetGore.SyncValueAttribute()]
public NetGore.Direction DirectionId
{
get
{
return (NetGore.Direction)_directionId;
}
set
{
this._directionId = (System.Int16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `height`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The height of the spawner (NULL indicates the bottom- side of the map).".
/// </summary>
[System.ComponentModel.Description("The height of the spawner (NULL indicates the bottom- side of the map).")]
[NetGore.SyncValueAttribute()]
public System.Nullable<System.UInt16> Height
{
get
{
return (System.Nullable<System.UInt16>)_height;
}
set
{
this._height = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The unique ID of this NPC spawn.".
/// </summary>
[System.ComponentModel.Description("The unique ID of this NPC spawn.")]
[NetGore.SyncValueAttribute()]
public DemoGame.MapSpawnValuesID ID
{
get
{
return (DemoGame.MapSpawnValuesID)_iD;
}
set
{
this._iD = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `map_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The map that this spawn takes place on.".
/// </summary>
[System.ComponentModel.Description("The map that this spawn takes place on.")]
[NetGore.SyncValueAttribute()]
public NetGore.World.MapID MapID
{
get
{
return (NetGore.World.MapID)_mapID;
}
set
{
this._mapID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `respawn`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `5`.The database column contains the comment: 
/// "How long in seconds to wait after death to be respawned (intended for NPCs only).".
/// </summary>
[System.ComponentModel.Description("How long in seconds to wait after death to be respawned (intended for NPCs only).")]
[NetGore.SyncValueAttribute()]
public System.UInt16 Respawn
{
get
{
return (System.UInt16)_respawn;
}
set
{
this._respawn = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `width`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The width of the spawner (NULL indicates the right-most side of the map).".
/// </summary>
[System.ComponentModel.Description("The width of the spawner (NULL indicates the right-most side of the map).")]
[NetGore.SyncValueAttribute()]
public System.Nullable<System.UInt16> Width
{
get
{
return (System.Nullable<System.UInt16>)_width;
}
set
{
this._width = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `x`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The x coordinate of the spawner (NULL indicates the left-most side of the map). Example: All x/y/width/height set to NULL spawns NPCs anywhere on the map.".
/// </summary>
[System.ComponentModel.Description("The x coordinate of the spawner (NULL indicates the left-most side of the map). Example: All x/y/width/height set to NULL spawns NPCs anywhere on the map.")]
[NetGore.SyncValueAttribute()]
public System.Nullable<System.UInt16> X
{
get
{
return (System.Nullable<System.UInt16>)_x;
}
set
{
this._x = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `y`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The y coordinate of the spawner (NULL indicates the top-most side of the map).".
/// </summary>
[System.ComponentModel.Description("The y coordinate of the spawner (NULL indicates the top-most side of the map).")]
[NetGore.SyncValueAttribute()]
public System.Nullable<System.UInt16> Y
{
get
{
return (System.Nullable<System.UInt16>)_y;
}
set
{
this._y = (System.Nullable<System.UInt16>)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IMapSpawnTable DeepCopy()
{
return new MapSpawnTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="MapSpawnTable"/> class.
/// </summary>
public MapSpawnTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="MapSpawnTable"/> class.
/// </summary>
/// <param name="amount">The initial value for the corresponding property.</param>
/// <param name="characterTemplateID">The initial value for the corresponding property.</param>
/// <param name="directionId">The initial value for the corresponding property.</param>
/// <param name="height">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="mapID">The initial value for the corresponding property.</param>
/// <param name="respawn">The initial value for the corresponding property.</param>
/// <param name="width">The initial value for the corresponding property.</param>
/// <param name="x">The initial value for the corresponding property.</param>
/// <param name="y">The initial value for the corresponding property.</param>
public MapSpawnTable(System.Byte @amount, DemoGame.CharacterTemplateID @characterTemplateID, NetGore.Direction @directionId, System.Nullable<System.UInt16> @height, DemoGame.MapSpawnValuesID @iD, NetGore.World.MapID @mapID, System.UInt16 @respawn, System.Nullable<System.UInt16> @width, System.Nullable<System.UInt16> @x, System.Nullable<System.UInt16> @y)
{
this.Amount = (System.Byte)@amount;
this.CharacterTemplateID = (DemoGame.CharacterTemplateID)@characterTemplateID;
this.DirectionId = (NetGore.Direction)@directionId;
this.Height = (System.Nullable<System.UInt16>)@height;
this.ID = (DemoGame.MapSpawnValuesID)@iD;
this.MapID = (NetGore.World.MapID)@mapID;
this.Respawn = (System.UInt16)@respawn;
this.Width = (System.Nullable<System.UInt16>)@width;
this.X = (System.Nullable<System.UInt16>)@x;
this.Y = (System.Nullable<System.UInt16>)@y;
}
/// <summary>
/// Initializes a new instance of the <see cref="MapSpawnTable"/> class.
/// </summary>
/// <param name="source">IMapSpawnTable to copy the initial values from.</param>
public MapSpawnTable(IMapSpawnTable source)
{
CopyValuesFrom(source);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="dic">The Dictionary to copy the values into.</param>
public void CopyValues(System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
CopyValues(this, dic);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="dic">The Dictionary to copy the values into.</param>
public static void CopyValues(IMapSpawnTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["amount"] = (System.Byte)source.Amount;
dic["character_template_id"] = (DemoGame.CharacterTemplateID)source.CharacterTemplateID;
dic["direction_id"] = (NetGore.Direction)source.DirectionId;
dic["height"] = (System.Nullable<System.UInt16>)source.Height;
dic["id"] = (DemoGame.MapSpawnValuesID)source.ID;
dic["map_id"] = (NetGore.World.MapID)source.MapID;
dic["respawn"] = (System.UInt16)source.Respawn;
dic["width"] = (System.Nullable<System.UInt16>)source.Width;
dic["x"] = (System.Nullable<System.UInt16>)source.X;
dic["y"] = (System.Nullable<System.UInt16>)source.Y;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this MapSpawnTable.
/// </summary>
/// <param name="source">The IMapSpawnTable to copy the values from.</param>
public void CopyValuesFrom(IMapSpawnTable source)
{
this.Amount = (System.Byte)source.Amount;
this.CharacterTemplateID = (DemoGame.CharacterTemplateID)source.CharacterTemplateID;
this.DirectionId = (NetGore.Direction)source.DirectionId;
this.Height = (System.Nullable<System.UInt16>)source.Height;
this.ID = (DemoGame.MapSpawnValuesID)source.ID;
this.MapID = (NetGore.World.MapID)source.MapID;
this.Respawn = (System.UInt16)source.Respawn;
this.Width = (System.Nullable<System.UInt16>)source.Width;
this.X = (System.Nullable<System.UInt16>)source.X;
this.Y = (System.Nullable<System.UInt16>)source.Y;
}

/// <summary>
/// Gets the value of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the value for.</param>
/// <returns>
/// The value of the column with the name <paramref name="columnName"/>.
/// </returns>
public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "amount":
return Amount;

case "character_template_id":
return CharacterTemplateID;

case "direction_id":
return DirectionId;

case "height":
return Height;

case "id":
return ID;

case "map_id":
return MapID;

case "respawn":
return Respawn;

case "width":
return Width;

case "x":
return X;

case "y":
return Y;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Sets the <paramref name="value"/> of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
/// <param name="value">Value to assign to the column.</param>
public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "amount":
this.Amount = (System.Byte)value;
break;

case "character_template_id":
this.CharacterTemplateID = (DemoGame.CharacterTemplateID)value;
break;

case "direction_id":
this.DirectionId = (NetGore.Direction)value;
break;

case "height":
this.Height = (System.Nullable<System.UInt16>)value;
break;

case "id":
this.ID = (DemoGame.MapSpawnValuesID)value;
break;

case "map_id":
this.MapID = (NetGore.World.MapID)value;
break;

case "respawn":
this.Respawn = (System.UInt16)value;
break;

case "width":
this.Width = (System.Nullable<System.UInt16>)value;
break;

case "x":
this.X = (System.Nullable<System.UInt16>)value;
break;

case "y":
this.Y = (System.Nullable<System.UInt16>)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Gets the data for the database column that this table represents.
/// </summary>
/// <param name="columnName">The database name of the column to get the data for.</param>
/// <returns>
/// The data for the database column with the name <paramref name="columnName"/>.
/// </returns>
public static ColumnMetadata GetColumnData(System.String columnName)
{
switch (columnName)
{
case "amount":
return new ColumnMetadata("amount", "The total number of NPCs this spawner will spawn.", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

case "character_template_id":
return new ColumnMetadata("character_template_id", "The character template used to instantiate the spawned NPCs.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "direction_id":
return new ColumnMetadata("direction_id", "The direction of this spawn; None if randonm", "smallint(5)", "0", typeof(System.Int16), false, false, false);

case "height":
return new ColumnMetadata("height", "The height of the spawner (NULL indicates the bottom- side of the map).", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "id":
return new ColumnMetadata("id", "The unique ID of this NPC spawn.", "int(11)", null, typeof(System.Int32), false, true, false);

case "map_id":
return new ColumnMetadata("map_id", "The map that this spawn takes place on.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "respawn":
return new ColumnMetadata("respawn", "How long in seconds to wait after death to be respawned (intended for NPCs only).", "smallint(5) unsigned", "5", typeof(System.UInt16), false, false, false);

case "width":
return new ColumnMetadata("width", "The width of the spawner (NULL indicates the right-most side of the map).", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "x":
return new ColumnMetadata("x", "The x coordinate of the spawner (NULL indicates the left-most side of the map). Example: All x/y/width/height set to NULL spawns NPCs anywhere on the map.", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "y":
return new ColumnMetadata("y", "The y coordinate of the spawner (NULL indicates the top-most side of the map).", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Reads the state of the object from an <see cref="IValueReader"/>.
/// </summary>
/// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
public virtual void ReadState(NetGore.IO.IValueReader reader)
{
NetGore.IO.PersistableHelper.Read(this, reader);
}

/// <summary>
/// Writes the state of the object to an <see cref="IValueWriter"/>.
/// </summary>
/// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
public virtual void WriteState(NetGore.IO.IValueWriter writer)
{
NetGore.IO.PersistableHelper.Write(this, writer);
}

}

}
