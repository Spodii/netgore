using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Delegate for handling ItemEntity events.
    /// </summary>
    /// <param name="itemEntity">ItemEntity this event came from.</param>
    public delegate void ItemEntityEventHandler(ItemEntity itemEntity);

    /// <summary>
    /// An item on the server.
    /// </summary>
    public class ItemEntity : ItemEntityBase, IItemTable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static DBController _dbController;

        readonly ItemStats _baseStats;
        readonly ItemID _id;
        readonly ItemStats _reqStats;

        byte _amount = 1;
        string _description;
        GrhIndex _graphicIndex;
        SPValueType _hp;
        SPValueType _mp;
        string _name;
        ItemType _type;
        int _value;

        /// <summary>
        /// Notifies listeners that the item's Amount or GraphicIndex have changed.
        /// </summary>
        public event ItemEntityEventHandler OnChangeGraphicOrAmount;

        /// <summary>
        /// Notifies listeners that this <see cref="Entity"/> was picked up.
        /// </summary>
        public override event EntityEventHandler<CharacterEntity> OnPickup;

        static DeleteItemQuery DeleteItem
        {
            get { return _dbController.GetQuery<DeleteItemQuery>(); }
        }

        static ItemIDCreator IDCreator
        {
            get { return _dbController.GetQuery<ItemIDCreator>(); }
        }

        static ReplaceItemQuery ReplaceItem
        {
            get { return _dbController.GetQuery<ReplaceItemQuery>(); }
        }

        static UpdateItemFieldQuery UpdateItemField
        {
            get { return _dbController.GetQuery<UpdateItemFieldQuery>(); }
        }

        public ItemStats BaseStats
        {
            get { return _baseStats; }
        }

        /// <summary>
        /// Gets or sets the index of the graphic that is used for this item.
        /// </summary>
        public override GrhIndex GraphicIndex
        {
            get { return _graphicIndex; }
            set
            {
                if (_graphicIndex == value)
                    return;

                _graphicIndex = value;

                if (OnChangeGraphicOrAmount != null)
                    OnChangeGraphicOrAmount(this);

                SynchronizeField("graphic", _graphicIndex);
            }
        }

        public ItemStats ReqStats
        {
            get { return _reqStats; }
        }

        public ItemEntity(ItemTemplate t, byte amount) : this(t, Vector2.Zero, amount)
        {
        }

        public ItemEntity(ItemTemplate t, Vector2 pos, byte amount, Map map) : this(t, pos, amount)
        {
            map.AddEntity(this);
        }

        public ItemEntity(ItemTemplate t, Vector2 pos, byte amount)
            : this(pos, t.Size, t.Name, t.Description, t.Type, t.Graphic, t.Value, amount, t.HP, t.MP, t.Stats, t.ReqStats)
        {
        }

        public ItemEntity()
        {
            _id = new ItemID(IDCreator.GetNext());
        }

        public ItemEntity(IItemTable iv) : base(Vector2.Zero, new Vector2(iv.Width, iv.Height))
        {
            _id = iv.ID;

            _name = iv.Name;
            _description = iv.Description;
            _graphicIndex = iv.Graphic;
            _value = iv.Value;
            _amount = iv.Amount;
            _type = iv.Type;

            _baseStats = NewItemStats(iv.Stats, StatCollectionType.Base);
            _reqStats = NewItemStats(iv.ReqStats, StatCollectionType.Requirement);

            OnResize += ItemEntity_OnResize;
        }

        ItemEntity(Vector2 pos, Vector2 size, string name, string desc, ItemType type, GrhIndex graphic, int value, byte amount,
                   SPValueType hp, SPValueType mp, IEnumerable<KeyValuePair<StatType, int>> baseStats,
                   IEnumerable<KeyValuePair<StatType, int>> reqStats) : base(pos, size)
        {
            _id = new ItemID(IDCreator.GetNext());

            _name = name;
            _description = desc;
            _graphicIndex = graphic;
            _value = value;
            _amount = amount;
            _type = type;
            _hp = hp;
            _mp = mp;

            _baseStats = NewItemStats(baseStats, StatCollectionType.Base);
            _reqStats = NewItemStats(reqStats, StatCollectionType.Requirement);

            var itemValues = DeepCopyValues();
            ReplaceItem.Execute(itemValues);

            OnResize += ItemEntity_OnResize;
        }

        /// <summary>
        /// Gets a deep copy of the ItemEntity's values, providing a "snapshot" of the values of the ItemEntity.
        /// </summary>
        /// <returns>A deep copy of the ItemEntity's values.</returns>
        public IItemTable DeepCopyValues()
        {
            return ((IItemTable)this).DeepCopy();
        }

        ItemEntity(ItemEntity s)
            : this(
                s.Position, s.CB.Size, s.Name, s.Description, s.Type, s.GraphicIndex, s.Value, s.Amount, s.HP, s.MP,
                s.BaseStats.ToKeyValuePairs(), s.ReqStats.ToKeyValuePairs())
        {
        }

        void BaseStatChangeReceiver(IStat stat)
        {
            string field = stat.StatType.GetDatabaseField(StatCollectionType.Base);
            SynchronizeField(field, stat.Value);
        }

        /// <summary>
        /// Checks if this <see cref="Entity"/> can be picked up by the specified <paramref name="charEntity"/>, but does
        /// not actually pick up this <see cref="Entity"/>.
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="Entity"/>.</param>
        /// <returns>True if this <see cref="Entity"/> can be picked up, else false.</returns>
        public override bool CanPickup(CharacterEntity charEntity)
        {
            // Every character can pick up an item
            return true;
        }

        /// <summary>
        /// Checks if this item can be stacked with another item. To stack, both items must contain the same
        /// stat modifiers, name, description, value, and graphic index.
        /// </summary>
        /// <param name="source">Item to check if can stack on this item</param>
        /// <returns>True if the two items can stack on each other, else false</returns>
        public override bool CanStack(ItemEntityBase source)
        {
            // Check for equal reference
            if (ReferenceEquals(this, source))
            {
                // Although it makes sense for an ItemEntity to be able to stack onto itself,
                // there is no reason this should ever happen intentionally
                const string errmsg =
                    "Trying to stack an item `{0}` onto itself. Although this is not an error, " +
                    "it makes no sense why it would be attempted.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return true;
            }

            // Check for non-equal values
            if (Value != source.Value || GraphicIndex != source.GraphicIndex || Type != source.Type || Name != source.Name ||
                Description != source.Description)
                return false;

            // Check for non-equal stats
            ItemEntity itemEntity = (ItemEntity)source;
            if (!BaseStats.HasEqualValues(itemEntity.BaseStats) || !ReqStats.HasEqualValues(itemEntity.ReqStats))
                return false;

            // Everything important is equal, so they can be stacked
            return true;
        }

        /// <summary>
        /// Creates a deep copy of the inheritor, which is a new class with the same values, and returns
        /// the copy as an ItemEntityBase.
        /// </summary>
        /// <returns>A deep copy of the object</returns>
        public override ItemEntityBase DeepCopy()
        {
            return new ItemEntity(this);
        }

        /// <summary>
        /// Disposes of the ItemEntity, freeing its ID and existance in the database. Once disposed, an ItemEntity
        /// should never be used again.
        /// </summary>
        protected override void HandleDispose()
        {
            // Delete the item from the database
            DeleteItem.Execute(ID);

            // Free the item's ID
            IDCreator.FreeID((int)ID);

            base.HandleDispose();
        }

        /// <summary>
        /// Initializes the objects needed for assisting the ItemEntity creation and destruction. Must be called
        /// before any ItemEntity is created.
        /// </summary>
        /// <param name="dbController">DBController used to communicate with the database for item data.</param>
        public static void Initialize(DBController dbController)
        {
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _dbController = dbController;
        }

        /// <summary>
        /// Handles when an ItemEntity is resized.
        /// </summary>
        /// <param name="entity">ItemEntity that was resized.</param>
        /// <param name="oldSize">Old ItemEntity size.</param>
        void ItemEntity_OnResize(Entity entity, Vector2 oldSize)
        {
            Debug.Assert(entity == this, "Why did we receive an ItemEntity_OnResize for another Entity?");

            // Get the sizes as a byte
            byte oldWidth = (byte)oldSize.X;
            byte oldHeight = (byte)oldSize.Y;
            byte width = (byte)entity.CB.Width;
            byte height = (byte)entity.CB.Height;

            // Update the changed sizes
            if (oldWidth != width)
                SynchronizeField("width", width);

            if (oldHeight != height)
                SynchronizeField("height", height);
        }

        /// <summary>
        /// Creates an ItemStats from the given collection of IStats.
        /// </summary>
        ItemStats NewItemStats(IEnumerable<KeyValuePair<StatType, int>> statValues, StatCollectionType statCollectionType)
        {
            ItemStats ret = new ItemStats(statValues, statCollectionType);

            switch (statCollectionType)
            {
                case StatCollectionType.Base:
                    ret.OnStatChange += BaseStatChangeReceiver;
                    break;
                case StatCollectionType.Requirement:
                    ret.OnStatChange += ReqStatChangeReceiver;
                    break;
                case StatCollectionType.Modified:
                    throw new ArgumentException("ItemEntity does not use StatCollectionType.Modified.", "statCollectionType");
                default:
                    throw new ArgumentOutOfRangeException("statCollectionType");
            }

            return ret;
        }

        /// <summary>
        /// Picks up this <see cref="Entity"/>.
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to pick up this <see cref="Entity"/>.</param>
        /// <returns>True if this <see cref="Entity"/> was successfully picked up, else false.</returns>
        public override bool Pickup(CharacterEntity charEntity)
        {
            // Check for invalid character
            if (charEntity == null)
            {
                const string errmsg = "Null charEntity specified.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                Debug.Fail(errmsg);
                return false;
            }

            // Check if the item can be picked up
            if (!CanPickup(charEntity))
                return false;

            // Convert to a character
            Character character = charEntity as Character;
            if (character == null)
            {
                const string errmsg =
                    "Unable to convert CharacterEntity `{0}` to Character for some reason. " +
                    "Is there another type, besides Character, inheriting CharacterEntity?";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, charEntity);
                Debug.Fail(string.Format(errmsg, charEntity));
                return false;
            }

            // Give the item to the character
            if (character.GiveItem(this) == null)
            {
                // The item was all added to the inventory, so dispose of it
                // The map automatically removes disposed Entities
                Dispose();
            }

            // Notify listeners
            if (OnPickup != null)
                OnPickup(this, charEntity);

            return true;
        }

        void ReqStatChangeReceiver(IStat stat)
        {
            string field = stat.StatType.GetDatabaseField(StatCollectionType.Requirement);
            SynchronizeField(field, stat.Value);
        }

        /// <summary>
        /// Splits the ItemEntity into two parts. This ItemEntity's amount will be decreased, and a new
        /// ItemEntity will be constructed as the product of the method. The original ItemEntity must still have
        /// an amount of at least one for the split to succeed.
        /// </summary>
        /// <param name="amount">Amount of the ItemEntity for the new part to contain. This must be less than
        /// the Amount of the existing ItemEntity, since both resulting ItemEntities must have an amount of
        /// at least 1.</param>
        /// <returns>New ItemEntity of the specified <paramref name="amount"/>, or null if the specified
        /// amount of the ItemEntity could not be acquired.</returns>
        public ItemEntity Split(byte amount)
        {
            // Check for a valid amount
            if (Amount <= 0)
            {
                Debug.Fail("Tried to Split() an ItemEntity with an Amount <= 0.");
                return null;
            }

            // Check if we can't perform a full split
            if (amount >= Amount)
                return null;

            // Create the new ItemEntity
            ItemEntity child = new ItemEntity(this) { Amount = amount };

            // Lower the amount of this ItemEntity
            Amount -= amount;

            return child;
        }

        /// <summary>
        /// Updates a single field for the ItemEntity in the database.
        /// </summary>
        /// <param name="field">Name of the field to update.</param>
        /// <param name="value">New value for the field.</param>
        void SynchronizeField(string field, object value)
        {
            UpdateItemField.Execute(_id, field, value);
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, ID);
        }

        #region IItemTable Members

        /// <summary>
        /// Gets or sets the size of this item cluster (1 for a single item).
        /// </summary>
        public override byte Amount
        {
            get { return _amount; }
            set
            {
                if (_amount == value)
                    return;

                _amount = value;

                if (OnChangeGraphicOrAmount != null)
                    OnChangeGraphicOrAmount(this);

                SynchronizeField("amount", _amount);
            }
        }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public override string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                    return;

                _description = value;

                SynchronizeField("description", _description);
            }
        }

        /// <summary>
        /// Gets the value of the database column `graphic`.
        /// </summary>
        GrhIndex IItemTable.Graphic
        {
            get
            {
                // TODO: Remove this by renaming GraphicIndex overload
                return GraphicIndex;
            }
        }

        /// <summary>
        /// Gets the value of the database column `height`.
        /// </summary>
        byte IItemTable.Height
        {
            get { return (byte)CB.Height; }
        }

        public SPValueType HP
        {
            get { return _hp; }
            set
            {
                if (_hp == value)
                    return;

                _hp = value;

                SynchronizeField("hp", _hp);
            }
        }

        /// <summary>
        /// Gets the unique ID for this ItemEntity.
        /// </summary>
        public ItemID ID
        {
            get { return _id; }
        }

        public SPValueType MP
        {
            get { return _mp; }
            set
            {
                if (_mp == value)
                    return;

                _mp = value;

                SynchronizeField("mp", _mp);
            }
        }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public override string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                _name = value;

                SynchronizeField("name", _name);
            }
        }

        /// <summary>
        /// Gets an IEnumerable of KeyValuePairs containing the values in the `ReqStat` collection. The
        /// key is the collection's key and the value is the value for that corresponding key.
        /// </summary>
        IEnumerable<KeyValuePair<StatType, int>> IItemTable.ReqStats
        {
            get { return ReqStats.ToKeyValuePairs(); }
        }

        /// <summary>
        /// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
        /// key is the collection's key and the value is the value for that corresponding key.
        /// </summary>
        IEnumerable<KeyValuePair<StatType, int>> IItemTable.Stats
        {
            get { return BaseStats.ToKeyValuePairs(); }
        }

        /// <summary>
        /// Gets or sets the type of item this is.
        /// </summary>
        public override ItemType Type
        {
            get { return _type; }
            set
            {
                if (_type == value)
                    return;

                _type = value;

                SynchronizeField("type", (byte)_type);
            }
        }

        /// <summary>
        /// Gets or sets the value of the item.
        /// </summary>
        public override int Value
        {
            get { return _value; }
            set
            {
                if (_value == value)
                    return;

                _value = value;

                SynchronizeField("value", _value);
            }
        }

        /// <summary>
        /// Gets the value of the database column `width`.
        /// </summary>
        byte IItemTable.Width
        {
            get { return (byte)CB.Width; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IItemTable IItemTable.DeepCopy()
        {
            return new ItemTable(this);
        }

        /// <summary>
        /// Gets the value of the database column in the column collection `ReqStat`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        int IItemTable.GetReqStat(StatType key)
        {
            return _reqStats[key];
        }

        /// <summary>
        /// Gets the value of the database column in the column collection `Stat`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        int IItemTable.GetStat(StatType key)
        {
            return _baseStats[key];
        }

        #endregion
    }
}