using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;
using NetGore.IO.PropertySync;
using SFML.Graphics;

/* NOTE ON THE POSITION AND VELOCITY SYNCING:
 * --------------------------------------------------------------------------------------------------------------
 * Position and Velocity have to be treated special because they can change so quickly and having them
 * not be synchronized correctly is probably the most visually obvious (and annoying) issue in any game.
 * We need to make sure the Position and Velocity are kept up-to-date, but not updated at an insanely
 * fast rate. Right now, what I have is the following:
 *  - When ForceUpdatePositionAndVelocity() is called, synchronization will happen immediately
 *  - When Teleport() is called, if the Position change, synchronization will happen immediately
 *  - When Velocity changes direction (ie from jumping, falling, stopping, moving), synchronization will happen immediately
 *  - Every ForceUpdatePositionAndVelocity() results in _syncPnVDupeCounter = _syncPnVDupeTimes
 *  - The _syncPnVDupeCounter is to ensure the changed data reaches the destination since it will be on
 *    a lossy network channel
 *  - While moving, every DynamicEntity will always update at a minimum rate of _syncPnVMoveRate
 * --------------------------------------------------------------------------------------------------------------
 */

namespace NetGore.World
{
    /// <summary>
    /// Base class for any Entity that must be synchronized between both the Client and Server. This
    /// base class supplies the synchronization of the supplied Properties, and synchronizing of additional
    /// properties through the SyncValue attribute.
    /// </summary>
    public abstract class DynamicEntity : Entity, IDynamicEntitySetMapEntityIndex
    {
        const string _positionValueKey = "Position";
        const string _propertyIndexValueKey = "PropertyIndex";

        /// <summary>
        /// How frequently, in milliseconds, the Position and Velocity are synchronized when _syncPnVCount is greater
        /// than zero. This is to ensure the synchronization message is received and starts off correctly.
        /// </summary>
        const int _syncPnVDupeDelay = 100;

        /// <summary>
        /// How many times the Position and Velocity will be synchronized if the value does not change.
        /// </summary>
        const byte _syncPnVDupeTimes = 3;

        /// <summary>
        /// How frequently, in milliseconds, the Position and Velocity are synchronized when the DynamicEntity is moving.
        /// This will cause synchronization to happen even when the _syncPnVCount is 0 since it is used to ensure the
        /// simulation doesn't get too out of sync between the client and server.
        /// </summary>
        const int _syncPnVMoveRate = 800;

        const string _velocityValueKey = "Velocity";

        /// <summary>
        /// Index of the last PropertySync where SkipNetworkSync is false. See <see cref="_propertySyncs"/>
        /// comments for more details.
        /// </summary>
        readonly byte _lastNetworkSyncIndex;

        /// <summary>
        /// The PropertySyncs used by this DynamicEntity. Index 0 to <see cref="_lastNetworkSyncIndex"/>, inclusive,
        /// is guarenteed to have SkipNetworkSync equal false. Any PropertySync after the index at
        /// <see cref="_lastNetworkSyncIndex"/>, if any, are guarenteed to have SkipNetworkSync equal true.
        /// </summary>
        readonly IPropertySync[] _propertySyncs;

        /// <summary>
        /// If we know if one of the PropertySync's value have changed.
        /// </summary>
        bool _isSynchronized;

        /// <summary>
        /// Last sent Position.
        /// </summary>
        Vector2 _lastSentPosition;

        /// <summary>
        /// Last sent Velocity.
        /// </summary>
        Vector2 _lastSentVelocity;

        /// <summary>
        /// Index of the map this DynamicEntity is on.
        /// </summary>
        MapEntityIndex _mapEntityIndex;

        /// <summary>
        /// How many more times the Position and Velocity will be synchronized without the values changing.
        /// </summary>
        byte _syncPnVDupeCounter;

        /// <summary>
        /// Game time that the Position and Velocity were last synchronized.
        /// </summary>
        TickCount _syncPnVLastTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntity"/> class.
        /// </summary>
        /// <param name="position">The initial world position.</param>
        /// <param name="size">The initial size.</param>
        protected DynamicEntity(Vector2 position, Vector2 size) : base(position, size)
        {
            // Get the PropertySyncBases for this DynamicEntity instance
            // OrderBy() will make sure every PropertySync where SkipNetworkSync is true is at the end of the array
            _propertySyncs = PropertySyncHelper.GetPropertySyncs(GetType()).OrderBy(x => x.SkipNetworkSync).ToArray();

            // Store the index of the last PropertySync that needs to be synchronized over the network
            _lastNetworkSyncIndex = (byte)(_propertySyncs.Count(x => !x.SkipNetworkSync) - 1);

            AssertValidPropertySyncs();
        }

        /// <summary>
        /// Gets if the DynamicEntity's values are already synchronized.
        /// </summary>
        [Browsable(false)]
        public bool IsSynchronized
        {
            get
            {
                // If we already know we're not synchronized, just return false
                if (!_isSynchronized)
                    return false;

                // Check each property, stopping at the first non-synchronized one
                for (var i = 0; i <= _lastNetworkSyncIndex; i++)
                {
                    if (_propertySyncs[i].HasValueChanged(this))
                    {
                        _isSynchronized = false;
                        return false;
                    }
                }

                // All values are synchronized
                return true;
            }
        }

        /// <summary>
        /// Gets the unique index of this DynamicEntity on the map. This is used to distinguish each individual
        /// DynamicEntity from one another.
        /// </summary>
        [Browsable(false)]
        public MapEntityIndex MapEntityIndex
        {
            get { return _mapEntityIndex; }
        }

        /// <summary>
        /// Synchronizes the Position for the base Entity.
        /// </summary>
        [SyncValue("Position", SkipNetworkSync = true)]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected internal Vector2 syncPosition
        {
            get { return Position; }
            set { SetPositionRaw(value); }
        }

        /// <summary>
        /// Synchronizes the Size for the base Entity.
        /// </summary>
        [SyncValue("Size")]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected internal Vector2 syncSize
        {
            get { return Size; }
            set { SetSizeRaw(value); }
        }

        /// <summary>
        /// Synchronizes the Velocity for the base Entity.
        /// </summary>
        [SyncValue("Velocity", SkipNetworkSync = true)]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected internal Vector2 syncVelocity
        {
            get { return Velocity; }
            set { SetVelocityRaw(value); }
        }

        /// <summary>
        /// Synchronizes the Weight for the base Entity.
        /// </summary>
        [SyncValue("Weight")]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected internal float syncWeight
        {
            get { return Weight; }
            set { SetWeightRaw(value); }
        }

        /// <summary>
        /// When overridden in the derived class, handles post-creation processing. This method is invoked immediately
        /// after the <see cref="DynamicEntity"/> has been created and all of the serialized values have been read.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> that was used to deserialize the values.</param>
        protected virtual void AfterCreated(IValueReader reader)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles post creation-serialization processing. This method is invoked
        /// immediately after the <see cref="DynamicEntity"/>'s creation values have been serialized.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> that was used to serialize the values.</param>
        protected virtual void AfterSendCreated(IValueWriter writer)
        {
        }

        /// <summary>
        /// Asserts that the <see cref="_lastNetworkSyncIndex"/> is valid, that every index [0, _lastNetworkSyncIndex]
        /// is set to false, and [_lastNetworkSyncIndex + 1, end] is true for SkipNetworkSync.
        /// </summary>
        [Conditional("DEBUG")]
        void AssertValidPropertySyncs()
        {
            for (var i = 0; i < _lastNetworkSyncIndex; i++)
            {
                Debug.Assert(!_propertySyncs[i].SkipNetworkSync);
            }

            for (var i = _lastNetworkSyncIndex + 1; i < _propertySyncs.Length; i++)
            {
                Debug.Assert(_propertySyncs[i].SkipNetworkSync);
            }
        }

        /// <summary>
        /// Sets the Position and Velocity to not needing be synchronized anymore without actually performing
        /// the serialization. This is intended to be used when NeedSyncPositionAndVelocity() returns true,
        /// but the DynamicEntity is not in range of anyone to serialize the changes to.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        public void BypassPositionAndVelocitySync(TickCount currentTime)
        {
            _lastSentPosition = Position;
            _lastSentVelocity = Velocity;
            _syncPnVLastTime = currentTime;
        }

        /// <summary>
        /// Reads all of the changed property values from the specified IValueReader. Use in conjunction with Serialize().
        /// </summary>
        /// <param name="reader">IValueReader to read the changed property values from.</param>
        public void Deserialize(IValueReader reader)
        {
            // Highest possible PropertySync index that we will be writing
            uint highestPropertyIndex = _lastNetworkSyncIndex;

            // Grab the count for the number of properties to read
            var count = reader.ReadUInt("Count", 0, highestPropertyIndex);

            // Read the properties, which will be in ascending order
            // See the Serialize() function to see why this is, and why we do it this way
            uint lastIndex = 0;
            for (var i = 0; i < count; i++)
            {
                // Get the PropertySync to be deserialized
                var propIndex = reader.ReadUInt(_propertyIndexValueKey, lastIndex, highestPropertyIndex);
                var propertySync = _propertySyncs[propIndex];

                // Read the value into the property
                propertySync.ReadValue(this, reader);

                // Allow for additional post-deserializtion processing
                OnDeserializeProprety(reader, propertySync);

                // Store this property index as the last written index
                lastIndex = propIndex;
            }
        }

        /// <summary>
        /// Reads the Position and Velocity from the specified IValueReader. Use in conjunction with
        /// SerializePositionAndVelocity();
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        public void DeserializePositionAndVelocity(IValueReader reader)
        {
            Vector2 position;
            Vector2 velocity;
            DeserializePositionAndVelocity(reader, out position, out velocity);

            SetPositionRaw(position);
            SetVelocityRaw(velocity);
        }

        /// <summary>
        /// Reads the Position and Velocity from the specified IValueReader. Use in conjunction with
        /// SerializePositionAndVelocity();
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        /// <param name="position">The read position value.</param>
        /// <param name="velocity">The read velocity value.</param>
        static void DeserializePositionAndVelocity(IValueReader reader, out Vector2 position, out Vector2 velocity)
        {
            position = reader.ReadVector2(_positionValueKey);
            velocity = reader.ReadVector2(_velocityValueKey);
        }

        /// <summary>
        /// Reads the Position and Velocity from the specified IValueReader without the need of a valid DynamicEntity.
        /// The data is not actually used in any way, it just progresses the reader like the values were read.
        /// </summary>
        /// <param name="reader">IValueReader ot read the values from.</param>
        public static void FlushPositionAndVelocity(IValueReader reader)
        {
            // Do nothing with the values, just read them to progress the reader
            Vector2 position;
            Vector2 velocity;
            DeserializePositionAndVelocity(reader, out position, out velocity);
        }

        /// <summary>
        /// Forces the Position and Velocity to be synchronized.
        /// </summary>
        protected void ForceUpdatePositionAndVelocity()
        {
            _syncPnVDupeCounter = _syncPnVDupeTimes;
        }

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected override void HandleUpdate(IMap imap, int deltaTime)
        {
            base.HandleUpdate(imap, deltaTime);

            // If the velocity has changed direction, force update
            if (VelocityChangedDirection())
                ForceUpdatePositionAndVelocity();
        }

        /// <summary>
        /// Checks if the Position and Velocity need to be synchronized.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        /// <returns>True if the Position and Velocity need to be synchronized, else false.</returns>
        public bool NeedSyncPositionAndVelocity(TickCount currentTime)
        {
            // Sync instantly when _syncPnVCount == _dupeSyncTimes
            if (_syncPnVDupeCounter >= _syncPnVDupeTimes)
                return true;

            // Check for repeated syncs
            if ((_syncPnVDupeCounter > 0) && (_syncPnVLastTime + _syncPnVDupeDelay < currentTime))
                return true;

            // Update no matter what when moving and enough time has elapsed since last send
            if ((Velocity != Vector2.Zero) && (_syncPnVLastTime + _syncPnVMoveRate < currentTime))
                return true;

            return false;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of a property that is
        /// being deserialized, immediately after the value has been read from the <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">IValueReader that the property value is being deserialized from.</param>
        /// <param name="propertySync">PropertySyncBase for the property that is being deserialized.</param>
        protected virtual void OnDeserializeProprety(IValueReader reader, IPropertySync propertySync)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of a property that is
        /// being serialized, immediately after the value has been written to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">IValueWriter that the property value is being serialized to.</param>
        /// <param name="propertySync">PropertySyncBase for the property that is being serialized.</param>
        protected virtual void OnSerializeProperty(IValueWriter writer, IPropertySync propertySync)
        {
        }

        /// <summary>
        /// Reads all of the synchronized properties from an IValueReader. Use in conjunction with WriteAll().
        /// </summary>
        /// <param name="reader">IValueReader to read the property values from.</param>
        internal void ReadAll(IValueReader reader)
        {
            for (var i = 0; i < _propertySyncs.Length; i++)
            {
                _propertySyncs[i].ReadValue(this, reader);
            }

            // Perform post-creation tasks
            AfterCreated(reader);
        }

        /// <summary>
        /// Writes all of the changed property values to the specified IValueWriter. Use in conjunction with Deserialize().
        /// </summary>
        /// <param name="writer">IValueWriter to write the changed property values to.</param>
        public void Serialize(IValueWriter writer)
        {
            // Highest possible PropertySync index that we will be writing
            uint highestPropertyIndex = _lastNetworkSyncIndex;
            
            // Find the indicies that need to be synchronized
            // Its important to note that we are iterating in ascending order and putting them in a queue, so they
            // will come out in ascending order, too
            var writeIndices = new Queue<int>(_lastNetworkSyncIndex + 1);
            for (var i = 0; i <= _lastNetworkSyncIndex; i++)
            {
                var propertySync = _propertySyncs[i];
                if (!propertySync.SkipNetworkSync && propertySync.HasValueChanged(this))
                    writeIndices.Enqueue(i);
            }

            // Write the count so the reader knows how many indicies there will be
            writer.Write("Count", (uint)writeIndices.Count, 0, highestPropertyIndex);

            // Write the properties - first their index (so we know what property it is), then the value
            // Since the indices are sorted, we know that each index cannot be greater than the previous index
            uint lastIndex = 0;
            while (writeIndices.Count > 0)
            {
                // Write the index of the property
                var propIndex = (uint)writeIndices.Dequeue();
                writer.Write(_propertyIndexValueKey, propIndex, lastIndex, highestPropertyIndex);

                // Write the actual property value
                var propertySync = _propertySyncs[propIndex];
                propertySync.WriteValue(this, writer);

                // Allow for additonal handling
                OnSerializeProperty(writer, propertySync);

                // Store this property index as the last written index
                lastIndex = propIndex;
            }

            // Synchronized!
            _isSynchronized = true;
        }

        /// <summary>
        /// Writes the Position and Velocity to the specified IValueWriter. Use in conjuncture with
        /// DeserializePositionAndVelocity().
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        /// <param name="currentTime">Current game time.</param>
        public void SerializePositionAndVelocity(IValueWriter writer, TickCount currentTime)
        {
            if (_syncPnVDupeCounter > 0)
                --_syncPnVDupeCounter;

            writer.Write(_positionValueKey, Position);
            writer.Write(_velocityValueKey, Velocity);

            _lastSentPosition = Position;
            _lastSentVelocity = Velocity;
            _syncPnVLastTime = currentTime;
        }

        /// <summary>
        /// Moves the Entity to a new location instantly.
        /// </summary>
        /// <param name="newPosition">New position for the Entity.</param>
        protected override void Teleport(Vector2 newPosition)
        {
            base.Teleport(newPosition);

            // If the position has changed, force update
            if (newPosition != _lastSentPosition)
                ForceUpdatePositionAndVelocity();
        }

        /// <summary>
        /// Checks if the Velocity has changed direction from the last synchronization.
        /// </summary>
        /// <returns>True if the Velocity has changed direction, else false.</returns>
        bool VelocityChangedDirection()
        {
            if ((Velocity.X == 0 && _lastSentVelocity.X != 0) || (Velocity.Y == 0 && _lastSentVelocity.Y != 0))
                return true;

            if ((Velocity.X < 0 && _lastSentVelocity.X >= 0) || (Velocity.Y < 0 && _lastSentVelocity.Y >= 0))
                return true;

            if ((Velocity.X > 0 && _lastSentVelocity.X <= 0) || (Velocity.Y > 0 && _lastSentVelocity.Y <= 0))
                return true;
            
            return false;
        }

        /// <summary>
        /// Writes all of the synchronized properties to an IValueWriter. Use in conjunction with ReadAll().
        /// </summary>
        /// <param name="writer">IValueWriter to write th property values to.</param>
        internal void WriteAll(IValueWriter writer)
        {
            for (var i = 0; i < _propertySyncs.Length; i++)
            {
                _propertySyncs[i].WriteValue(this, writer);
            }

            // Obviously synchronized if we write all the values
            _isSynchronized = true;

            // Perform post creation-serialized tasks
            AfterSendCreated(writer);
        }

        #region IDynamicEntitySetMapEntityIndex Members

        /// <summary>
        /// Sets the <see cref="MapEntityIndex"/> for this <see cref="DynamicEntity"/>. This should only ever be done by
        /// the <see cref="IMap"/> that contains this <see cref="DynamicEntity"/>.
        /// </summary>
        /// <param name="value">The new value.</param>
        void IDynamicEntitySetMapEntityIndex.SetMapEntityIndex(MapEntityIndex value)
        {
            _mapEntityIndex = value;
        }

        #endregion
    }
}