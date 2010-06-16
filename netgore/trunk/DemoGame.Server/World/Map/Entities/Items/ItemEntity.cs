using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Features.DisplayAction;
using NetGore.Stats;
using SFML.Graphics;

namespace DemoGame.Server
{
    // TODO: I REALLY need to make it so that copying values with an ItemEntity is as non-redundant and automated by the IItemTable as possible

    /// <summary>
    /// A single item instance on the server. Can be either a single item, or a stack of the exact same kind
    /// of item combined into one (<see cref="ItemEntity.Amount"/> greater than 1).
    /// </summary>
    public class ItemEntity : ItemEntityBase, IItemTable
    {
        /// <summary>
        /// Delegate for handling <see cref="ItemEntity"/> events.
        /// </summary>
        /// <param name="itemEntity"><see cref="ItemEntity"/> this event came from.</param>
        public delegate void ItemEntityEventHandler(ItemEntity itemEntity);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The <see cref="DeleteItemQuery"/> instance to use.
        /// </summary>
        static readonly DeleteItemQuery _queryDeleteItem;

        /// <summary>
        /// The <see cref="ItemIDCreator"/> instance to use.
        /// </summary>
        static readonly ItemIDCreator _queryIDCreator;

        /// <summary>
        /// The <see cref="ReplaceItemQuery"/> instance to use.
        /// </summary>
        static readonly ReplaceItemQuery _queryReplaceItem;

        /// <summary>
        /// The <see cref="UpdateItemFieldQuery"/> instance to use.
        /// </summary>
        static readonly UpdateItemFieldQuery _queryUpdateItemField;

        readonly StatCollection<StatType> _baseStats;
        readonly ItemID _id;
        readonly StatCollection<StatType> _reqStats;

        byte _amount = 1;
        string _description;
        string _equippedBody;
        GrhIndex _graphicIndex;
        SPValueType _hp;
        SPValueType _mp;
        string _name;
        ushort _range;
        ItemTemplateID? _templateID;
        ItemType _type;
        int _value;
        WeaponType _weaponType;

        /// <summary>
        /// Initializes the <see cref="ItemEntity"/> class.
        /// </summary>
        static ItemEntity()
        {
            var dbController = DbControllerBase.GetInstance();
            _queryUpdateItemField = dbController.GetQuery<UpdateItemFieldQuery>();
            _queryReplaceItem = dbController.GetQuery<ReplaceItemQuery>();
            _queryIDCreator = dbController.GetQuery<ItemIDCreator>();
            _queryDeleteItem = dbController.GetQuery<DeleteItemQuery>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEntity"/> class.
        /// </summary>
        /// <param name="t">The item template to copy the initial values from.</param>
        /// <param name="amount">The amount of the item.</param>
        public ItemEntity(IItemTemplateTable t, byte amount) : this(t, Vector2.Zero, amount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEntity"/> class.
        /// </summary>
        /// <param name="t">The item template to copy the initial values from.</param>
        /// <param name="pos">The world position of the item.</param>
        /// <param name="amount">The amount of the item.</param>
        /// <param name="map">The map the item is to spawn on.</param>
        public ItemEntity(IItemTemplateTable t, Vector2 pos, byte amount, MapBase map) : this(t, pos, amount)
        {
            // Since the item is spawning on a map, ensure that the position is valid for the map
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Teleport(ValidatePosition(map, pos));
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            map.AddEntity(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEntity"/> class.
        /// </summary>
        /// <param name="t">The item template to copy the initial values from.</param>
        /// <param name="pos">The world position of the item.</param>
        /// <param name="amount">The amount of the item.</param>
        public ItemEntity(IItemTemplateTable t, Vector2 pos, byte amount)
            : this(
                pos, new Vector2(t.Width, t.Height), t.ID, t.Name, t.Description, t.Type, t.WeaponType, t.Range, t.Graphic,
                t.Value, amount, t.HP, t.MP, t.EquippedBody, t.ActionDisplayID, t.Stats.Select(x => (Stat<StatType>)x),
                t.ReqStats.Select(x => (Stat<StatType>)x))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEntity"/> class.
        /// </summary>
        public ItemEntity() : base(Vector2.Zero, Vector2.Zero)
        {
            _id = _queryIDCreator.GetNext();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEntity"/> class.
        /// </summary>
        /// <param name="iv">The item values.</param>
        public ItemEntity(IItemTable iv) : base(Vector2.Zero, new Vector2(iv.Width, iv.Height))
        {
            _id = iv.ID;
            _templateID = iv.ItemTemplateID;

            _name = iv.Name;
            _description = iv.Description;
            _graphicIndex = iv.Graphic;
            _value = iv.Value;
            _amount = iv.Amount;
            _type = iv.Type;
            _weaponType = iv.WeaponType;
            _range = iv.Range;
            _equippedBody = iv.EquippedBody;

            _baseStats = NewItemStats(iv.Stats.Select(x => (Stat<StatType>)x), StatCollectionType.Base);
            _reqStats = NewItemStats(iv.ReqStats.Select(x => (Stat<StatType>)x), StatCollectionType.Requirement);

            Resized += ItemEntity_Resized;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEntity"/> class.
        /// </summary>
        /// <param name="pos">The pos.</param>
        /// <param name="size">The size.</param>
        /// <param name="templateID">The template ID.</param>
        /// <param name="name">The name.</param>
        /// <param name="desc">The desc.</param>
        /// <param name="type">The type.</param>
        /// <param name="weaponType">Type of the weapon.</param>
        /// <param name="range">The range.</param>
        /// <param name="graphic">The graphic.</param>
        /// <param name="value">The value.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="hp">The hp.</param>
        /// <param name="mp">The mp.</param>
        /// <param name="equippedBody">The equipped body.</param>
        /// <param name="actionDisplayID">The action display ID.</param>
        /// <param name="baseStats">The base stats.</param>
        /// <param name="reqStats">The req stats.</param>
        ItemEntity(Vector2 pos, Vector2 size, ItemTemplateID? templateID, string name, string desc, ItemType type,
                   WeaponType weaponType, ushort range, GrhIndex graphic, int value, byte amount, SPValueType hp, SPValueType mp,
                   string equippedBody, ActionDisplayID? actionDisplayID, IEnumerable<Stat<StatType>> baseStats, IEnumerable<Stat<StatType>> reqStats)
            : base(pos, size)
        {
            _id = _queryIDCreator.GetNext();

            _templateID = templateID;
            _name = name;
            _description = desc;
            _graphicIndex = graphic;
            _value = value;
            _amount = amount;
            _type = type;
            _weaponType = weaponType;
            _range = range;
            _hp = hp;
            _mp = mp;
            _equippedBody = equippedBody;
            _actionDisplayID = actionDisplayID;

            _baseStats = NewItemStats(baseStats, StatCollectionType.Base);
            _reqStats = NewItemStats(reqStats, StatCollectionType.Requirement);

            var itemValues = DeepCopyValues();
            _queryReplaceItem.Execute(itemValues);

            Resized += ItemEntity_Resized;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEntity"/> class.
        /// </summary>
        /// <param name="s">The <see cref="ItemEntity"/> to copy the values from.</param>
        ItemEntity(ItemEntity s)
            : this(
                s.Position, s.Size, s.ItemTemplateID, s.Name, s.Description, s.Type, s.WeaponType, s.Range, s.GraphicIndex,
                s.Value, s.Amount, s.HP, s.MP, s.EquippedBody, s.ActionDisplayID, s.BaseStats, s.ReqStats)
        {
        }

        /// <summary>
        /// Notifies listeners that the ItemEntity's Amount or GraphicIndex have changed.
        /// </summary>
        public event ItemEntityEventHandler GraphicOrAmountChanged;

        /// <summary>
        /// Notifies listeners that this <see cref="Entity"/> was picked up.
        /// </summary>
        public override event EntityEventHandler<CharacterEntity> PickedUp;

        /// <summary>
        /// Gets the <see cref="ItemEntity"/>'s base stats.
        /// </summary>
        public StatCollection<StatType> BaseStats
        {
            get { return _baseStats; }
        }

        /// <summary>
        /// Gets or sets the index of the graphic that is used for this ItemEntity.
        /// </summary>
        public override GrhIndex GraphicIndex
        {
            get { return _graphicIndex; }
            set
            {
                if (_graphicIndex == value)
                    return;

                _graphicIndex = value;

                if (GraphicOrAmountChanged != null)
                    GraphicOrAmountChanged(this);

                SynchronizeField("graphic", _graphicIndex);
            }
        }

        /// <summary>
        /// Gets the <see cref="ItemEntity"/>'s required stats.
        /// </summary>
        public StatCollection<StatType> ReqStats
        {
            get { return _reqStats; }
        }

        /// <summary>
        /// Checks if this <see cref="Entity"/> can be picked up by the specified <paramref name="charEntity"/>, but does
        /// not actually pick up this <see cref="Entity"/>.
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="Entity"/>.</param>
        /// <returns>True if this <see cref="Entity"/> can be picked up, else false.</returns>
        public override bool CanPickup(CharacterEntity charEntity)
        {
            // Every character can pick up an ItemEntity
            return true;
        }

        /// <summary>
        /// Checks if this item can be stacked with another item. To stack, both items must contain the same
        /// stat modifiers, name, description, value, and graphic index.
        /// </summary>
        /// <param name="source">Item to check if can stack on this item</param>
        /// <returns>
        /// True if the two items can stack on each other, else false
        /// </returns>
        public override bool CanStack(ItemEntityBase source)
        {
            var s = source as ItemEntity;
            if (s == null)
                return false;

            return CanStack(s);
        }

        /// <summary>
        /// Checks if this ItemEntity can be stacked with another ItemEntity. To stack, both items must contain the same
        /// stat modifiers, name, description, value, and graphic index.
        /// </summary>
        /// <param name="source">Item to check if can stack on this ItemEntity.</param>
        /// <returns>True if the two items can stack on each other, else false.</returns>
        public bool CanStack(ItemEntity source)
        {
            // Check for equal reference
            if (ReferenceEquals(this, source))
            {
                // Although it makes sense for an ItemEntity to be able to stack onto itself,
                // there is no reason this should ever happen intentionally
                const string errmsg =
                    "Trying to stack an ItemEntity `{0}` onto itself. Although this is not an error, " +
                    "it makes no sense why it would be attempted.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return true;
            }

            // Check for non-equal values
            if (Value != source.Value || GraphicIndex != source.GraphicIndex || Type != source.Type || Name != source.Name ||
                Description != source.Description || Range != source.Range || WeaponType != source.WeaponType)
                return false;

            // Check for non-equal stats
            if (!BaseStats.HasSameValues(source.BaseStats) || !ReqStats.HasSameValues(source.ReqStats))
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

        ActionDisplayID? _actionDisplayID;

        /// <summary>
        /// Gets or sets the <see cref="ActionDisplayID"/> to use when using this item.
        /// </summary>
        public ActionDisplayID? ActionDisplayID
        {
            get { return _actionDisplayID; }
            set
            {
                if (_actionDisplayID == value)
                    return;

                _actionDisplayID = value;

                SynchronizeField("action_display_id", _actionDisplayID);
            }
        }

        /// <summary>
        /// Gets a deep copy of the ItemEntity's values, providing a "snapshot" of the values of the ItemEntity.
        /// </summary>
        /// <returns>A deep copy of the ItemEntity's values.</returns>
        public IItemTable DeepCopyValues()
        {
            return ((IItemTable)this).DeepCopy();
        }

        /// <summary>
        /// Disposes of the ItemEntity, freeing its ID and existance in the database. Once disposed, an ItemEntity
        /// should never be used again.
        /// </summary>
        protected override void HandleDispose()
        {
            // Delete the ItemEntity from the database
            _queryDeleteItem.Execute(ID);

            // Free the ItemEntity's ID
            _queryIDCreator.FreeID(ID);

            base.HandleDispose();
        }

        /// <summary>
        /// Handles when an ItemEntity is resized.
        /// </summary>
        /// <param name="entity">ItemEntity that was resized.</param>
        /// <param name="oldSize">Old ItemEntity size.</param>
        void ItemEntity_Resized(ISpatial entity, Vector2 oldSize)
        {
            Debug.Assert(entity == this, "Why did we receive an ItemEntity_OnResize for another Entity?");

            // Get the sizes as a byte
            var oldWidth = (byte)oldSize.X;
            var oldHeight = (byte)oldSize.Y;
            var width = (byte)entity.Size.X;
            var height = (byte)entity.Size.Y;

            // Update the changed sizes
            if (oldWidth != width)
                SynchronizeField("width", width);

            if (oldHeight != height)
                SynchronizeField("height", height);
        }

        /// <summary>
        /// Creates a <see cref="StatCollection{StatType}"/> from the given collection of stats.
        /// </summary>
        StatCollection<StatType> NewItemStats(IEnumerable<Stat<StatType>> statValues, StatCollectionType statCollectionType)
        {
            var ret = new StatCollection<StatType>(statCollectionType, statValues);

            switch (statCollectionType)
            {
                case StatCollectionType.Base:
                case StatCollectionType.Requirement:
                    ret.StatChanged += StatCollection_StatChanged;
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

            // Check if the ItemEntity can be picked up
            if (!CanPickup(charEntity))
                return false;

            // Convert to a character
            var character = charEntity as Character;
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

            // Give the ItemEntity to the character
            if (character.GiveItem(this) == null)
            {
                // The ItemEntity was all added to the inventory, so dispose of it
                // The map automatically removes disposed Entities
                Dispose();
            }

            // Notify listeners
            if (PickedUp != null)
                PickedUp(this, charEntity);

            return true;
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
            var child = new ItemEntity(this) { Amount = amount };

            // Lower the amount of this ItemEntity
            Amount -= amount;

            return child;
        }

        /// <summary>
        /// Handles the <see cref="IStatCollection{T}.StatChanged"/> event for the stat collections in this class.
        /// </summary>
        /// <param name="statCollection">The sender.</param>
        /// <param name="statType">Type of the stat that changed.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        void StatCollection_StatChanged(IStatCollection<StatType> statCollection, StatType statType, StatValueType oldValue,
                                        StatValueType newValue)
        {
            Debug.Assert(statCollection.StatCollectionType != StatCollectionType.Modified,
                         "ItemEntity does not use StatCollectionType.Modified.");

            var field = statType.GetDatabaseField(statCollection.StatCollectionType);
            SynchronizeField(field, newValue);
        }

        /// <summary>
        /// Updates a single field for the ItemEntity in the database.
        /// </summary>
        /// <param name="field">Name of the field to update.</param>
        /// <param name="value">New value for the field.</param>
        void SynchronizeField(string field, object value)
        {
            _queryUpdateItemField.Execute(_id, field, value);
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, ID);
        }

        /// <summary>
        /// Validates the given <paramref name="position"/> by attempting to make it a legal position if it is not
        /// one already.
        /// </summary>
        /// <param name="map">The map that the <see cref="ItemEntity"/> is on or will be on.</param>
        /// <param name="position">The position to validate.</param>
        /// <returns>If the <paramref name="position"/> was already valid, or no valid position was found, contains
        /// the same value as the <paramref name="position"/>; otherwise, contains the corrected valid position.</returns>
        Vector2 ValidatePosition(MapBase map, Vector2 position)
        {
            if (map == null)
                return position;

            var tempRect = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y);

            Vector2 closestLegalPosition;
            bool isClosestPositionValid;
            if (!map.IsValidPlacementPosition(tempRect, out closestLegalPosition, out isClosestPositionValid))
            {
                if (isClosestPositionValid)
                    return closestLegalPosition;
                else
                {
                    // TODO: Could not find a valid position for the ItemEntity
                }
            }

            return position;
        }

        #region IItemTable Members

        /// <summary>
        /// Gets or sets the size of this ItemEntity cluster (1 for a single ItemEntity).
        /// </summary>
        public override byte Amount
        {
            get { return _amount; }
            set
            {
                if (_amount == value)
                    return;

                _amount = value;

                if (GraphicOrAmountChanged != null)
                    GraphicOrAmountChanged(this);

                SynchronizeField("amount", _amount);
            }
        }

        /// <summary>
        /// Gets or sets the description of the ItemEntity.
        /// </summary>
        public string Description
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
        /// Gets the value of the database column `equipped_body`.
        /// </summary>
        public string EquippedBody
        {
            get { return _equippedBody; }
            set
            {
                if (_equippedBody == value)
                    return;

                _equippedBody = value;

                SynchronizeField("equipped_body", _equippedBody);
            }
        }

        /// <summary>
        /// Gets the value of the database column `graphic`.
        /// </summary>
        GrhIndex IItemTable.Graphic
        {
            get { return GraphicIndex; }
        }

        /// <summary>
        /// Gets the value of the database column `hp`.
        /// </summary>
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
        /// Gets the value of the database column `height`.
        /// </summary>
        byte IItemTable.Height
        {
            get { return (byte)Size.Y; }
        }

        /// <summary>
        /// Gets the unique ID for this ItemEntity.
        /// </summary>
        public ItemID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the value of the database column `item_template_id`.
        /// </summary>
        public ItemTemplateID? ItemTemplateID
        {
            get { return _templateID; }
            private set
            {
                if (_templateID == value)
                    return;

                _templateID = value;

                SynchronizeField("item_template_id", _mp);
            }
        }

        /// <summary>
        /// Gets the value of the database column `mp`.
        /// </summary>
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
        /// Gets or sets the name of the ItemEntity.
        /// </summary>
        public string Name
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
        /// Gets or sets the value of the database column `range`.
        /// </summary>
        public ushort Range
        {
            get { return _range; }
            set
            {
                if (_range == value)
                    return;

                _range = value;

                SynchronizeField("range", _range);
            }
        }

        /// <summary>
        /// Gets an IEnumerable of KeyValuePairs containing the values in the `ReqStat` collection. The
        /// key is the collection's key and the value is the value for that corresponding key.
        /// </summary>
        IEnumerable<KeyValuePair<StatType, int>> IItemTable.ReqStats
        {
            get { return ReqStats.Cast<KeyValuePair<StatType, int>>(); }
        }

        /// <summary>
        /// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
        /// key is the collection's key and the value is the value for that corresponding key.
        /// </summary>
        IEnumerable<KeyValuePair<StatType, int>> IItemTable.Stats
        {
            get { return BaseStats.Cast<KeyValuePair<StatType, int>>(); }
        }

        /// <summary>
        /// Gets or sets the type of ItemEntity this is.
        /// </summary>
        public ItemType Type
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
        /// Gets or sets the value of the ItemEntity.
        /// </summary>
        public int Value
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
        /// Gets or sets the value of the database column `weapon_type`.
        /// </summary>
        public WeaponType WeaponType
        {
            get { return _weaponType; }
            set
            {
                if (_weaponType == value)
                    return;

                _weaponType = value;

                SynchronizeField("weapon_type", _weaponType);
            }
        }

        /// <summary>
        /// Gets the value of the database column `width`.
        /// </summary>
        byte IItemTable.Width
        {
            get { return (byte)Size.X; }
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