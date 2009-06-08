using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.IO;

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

namespace NetGore
{
    /// <summary>
    /// Base class for any Entity that must be synchronized between both the Client and Server. This
    /// base class supplies the synchronization of the supplied Properties, and synchronizing of additional
    /// properties through the SyncValue attribute.
    /// </summary>
    public abstract class DynamicEntity : Entity
    {
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
        const int _syncPnVMoveRate = 1000;

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
        readonly PropertySyncBase[] _propertySyncs;

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
        int _syncPnVLastTime;

        /// <summary>
        /// Synchronizes the CollisionType for the base Entity.
        /// </summary>
        [SyncValue("CollisionType")]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
        [Browsable(false)]
        // ReSharper disable UnusedMember.Local
            protected internal CollisionType CollisionTypeSync // ReSharper restore UnusedMember.Local
        {
            get { return CollisionType; }
            set { SetCollisionTypeRaw(value); }
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
                for (int i = 0; i <= _lastNetworkSyncIndex; i++)
                {
                    if (_propertySyncs[i].HasValueChanged())
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
            set
            {
                // TODO: Setter needs to eventually be hidden from alteration outside of the map
                _mapEntityIndex = value;
            }
        }

        /// <summary>
        /// Synchronizes the Position for the base Entity.
        /// </summary>
        [SyncValue("Position", SkipNetworkSync = true)]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
        [Browsable(false)]
        // ReSharper disable UnusedMember.Local
            protected internal Vector2 PositionSync // ReSharper restore UnusedMember.Local
        {
            get { return Position; }
            set { SetPositionRaw(value); }
        }

        /// <summary>
        /// Synchronizes the Size for the base Entity.
        /// </summary>
        [SyncValue("Size")]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
        [Browsable(false)]
        // ReSharper disable UnusedMember.Local
            protected internal Vector2 SizeSync // ReSharper restore UnusedMember.Local
        {
            get { return Size; }
            set { SetSizeRaw(value); }
        }

        /// <summary>
        /// Synchronizes the Velocity for the base Entity.
        /// </summary>
        [SyncValue("Velocity", SkipNetworkSync = true)]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
        [Browsable(false)]
        // ReSharper disable UnusedMember.Local
            protected internal Vector2 VelocitySync // ReSharper restore UnusedMember.Local
        {
            get { return Velocity; }
            set { SetVelocityRaw(value); }
        }

        /// <summary>
        /// Synchronizes the Weight for the base Entity.
        /// </summary>
        [SyncValue("Weight")]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
        [Browsable(false)]
        // ReSharper disable UnusedMember.Local
            protected internal float WeightSync // ReSharper restore UnusedMember.Local
        {
            get { return Weight; }
            set { SetWeightRaw(value); }
        }

        /// <summary>
        /// DynamicEntity constructor.
        /// </summary>
        protected DynamicEntity()
        {
            // Get the PropertySyncBases for this DynamicEntity instance
            // OrderBy() will make sure every PropertySync where SkipNetworkSync is true is at the end of the array
            _propertySyncs = PropertySyncBase.GetPropertySyncs(this).OrderBy(x => x.SkipNetworkSync).ToArray();

            // Store the index of the last PropertySync that needs to be synchronized over the network
            _lastNetworkSyncIndex = (byte)(_propertySyncs.Count(x => !x.SkipNetworkSync) - 1);

#if DEBUG
            // Ensure the _lastNetworkSyncIndex valid, and that every index [0, _lastNetworkSyncIndex] is
            // set to false, and [_lastNetworkSyncIndex+1, end] is true for SkipNetworkSync.
            for (int i = 0; i < _lastNetworkSyncIndex; i++)
            {
                Debug.Assert(!_propertySyncs[i].SkipNetworkSync);
            }
            for (int i = _lastNetworkSyncIndex + 1; i < _propertySyncs.Length; i++)
            {
                Debug.Assert(_propertySyncs[i].SkipNetworkSync);
            }
#endif
        }

        /// <summary>
        /// When overridden in the derived class, handles post-creation processing. This is required if you want to make use
        /// of any of the Entity's values, which are not available at the constructor.
        /// </summary>
        protected virtual void AfterCreation()
        {
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
            uint count = reader.ReadUInt("Count", 0, highestPropertyIndex);

            // Read the properties, which will be in ascending order
            // See the Serialize() function to see why this is, and why we do it this way
            uint lastIndex = 0;
            for (int i = 0; i < count; i++)
            {
                // Get the PropertySync to be deserialized
                uint propIndex = reader.ReadUInt("PropertyIndex", lastIndex, highestPropertyIndex);
                PropertySyncBase propertySync = _propertySyncs[propIndex];

                // Read the value into the property
                propertySync.ReadValue(reader);

                // Allow for additional post-deserializtion processing
                DeserializeProprety(reader, propertySync);

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
            Vector2 position, velocity;
            DeserializePositionAndVelocity(reader, out position, out velocity);

            SetPositionRaw(position);
            SetVelocityRaw(velocity);
        }

        /// <summary>
        /// Reads the Position and Velocity from the specified IValueReader. Use in conjunction with
        /// SerializePositionAndVelocity();
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <param name="position">Position read from the IValueReader.</param>
        /// <param name="velocity">Position read from the IValueReader.</param>
        static void DeserializePositionAndVelocity(IValueReader reader, out Vector2 position, out Vector2 velocity)
        {
            position = reader.ReadVector2("Position");
            velocity = reader.ReadVector2("velocity");
        }

        /// <summary>
        /// Reads the Position and Velocity from the specified IValueReader without the need of a valid DynamicEntity.
        /// The data is not actually used in any way, it just progresses the reader like the values were read.
        /// </summary>
        /// <param name="reader">IValueReader ot read the values from.</param>
        public static void FlushPositionAndVelocity(IValueReader reader)
        {
            // Do nothing with the values, just read them to progress the reader
            Vector2 position, velocity;
            DeserializePositionAndVelocity(reader, out position, out velocity);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of a property that is
        /// being deserialized, immediately after the value has been read from the <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">IValueReader that the property value is being deserialized from.</param>
        /// <param name="propertySync">PropertySyncBase for the property that is being deserialized.</param>
        // ReSharper disable UnusedParameter.Global
        protected virtual void DeserializeProprety(IValueReader reader, PropertySyncBase propertySync)
            // ReSharper restore UnusedParameter.Global
        {
        }

        /// <summary>
        /// Forces the Position and Velocity to be synchronized.
        /// </summary>
        protected void ForceUpdatePositionAndVelocity()
        {
            _syncPnVDupeCounter = _syncPnVDupeTimes;
        }

        /// <summary>
        /// Checks if the Position and Velocity need to be synchronized.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        /// <returns>True if the Position and Velocity need to be synchronized, else false.</returns>
        public bool NeedSyncPositionAndVelocity(int currentTime)
        {
            // Sync instantly when _syncPnVCount == _dupeSyncTimes
            if (_syncPnVDupeCounter >= _syncPnVDupeTimes)
                return true;

            // Check for repeated syncs
            if (_syncPnVDupeCounter > 0 && _syncPnVLastTime + _syncPnVDupeDelay < currentTime)
                return true;

            // Update no matter what when moving and enough time has elapsed since last send
            if (Velocity != Vector2.Zero && _syncPnVLastTime + _syncPnVMoveRate < currentTime)
                return true;

            return false;
        }

        /// <summary>
        /// Reads all of the synchronized properties from an IValueReader. Use in conjunction with WriteAll().
        /// </summary>
        /// <param name="reader">IValueReader to read the property values from.</param>
        internal void ReadAll(IValueReader reader)
        {
            for (int i = 0; i < _propertySyncs.Length; i++)
            {
                _propertySyncs[i].ReadValue(reader);
            }

            // Perform post-creation tasks
            AfterCreation();
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
            for (int i = 0; i <= _lastNetworkSyncIndex; i++)
            {
                PropertySyncBase propertySync = _propertySyncs[i];
                if (!propertySync.SkipNetworkSync && propertySync.HasValueChanged())
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
                uint propIndex = (uint)writeIndices.Dequeue();
                writer.Write("PropertyIndex", propIndex, lastIndex, highestPropertyIndex);

                // Write the actual property value
                PropertySyncBase propertySync = _propertySyncs[propIndex];
                propertySync.WriteValue(writer);

                // Allow for additonal handling
                SerializeProperty(writer, propertySync);

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
        public void SerializePositionAndVelocity(IValueWriter writer, int currentTime)
        {
            if (_syncPnVDupeCounter > 0)
                --_syncPnVDupeCounter;

            writer.Write("Position", Position);
            writer.Write("Velocity", Velocity);

            _lastSentPosition = Position;
            _lastSentVelocity = Velocity;
            _syncPnVLastTime = currentTime;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of a property that is
        /// being serialized, immediately after the value has been written to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">IValueWriter that the property value is being serialized to.</param>
        /// <param name="propertySync">PropertySyncBase for the property that is being serialized.</param>
        // ReSharper disable UnusedParameter.Global
        protected virtual void SerializeProperty(IValueWriter writer, PropertySyncBase propertySync)
            // ReSharper restore UnusedParameter.Global
        {
        }

        /// <summary>
        /// Moves the Entity to a new location instantly.
        /// </summary>
        /// <param name="newPosition">New position for the Entity.</param>
        public override void Teleport(Vector2 newPosition)
        {
            base.Teleport(newPosition);

            // If the position has changed, force update
            if (newPosition != _lastSentPosition)
                ForceUpdatePositionAndVelocity();
        }

        /// <summary>
        /// Updates the Entity
        /// </summary>
        /// <param name="imap">Map that the Entity is on</param>
        /// <param name="deltaTime">Time elapsed (in milliseconds) since the last update</param>
        public override void Update(IMap imap, float deltaTime)
        {
            base.Update(imap, deltaTime);

            // If the velocity has changed direction, force update
            if (VelocityChangedDirection())
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
        /// Writes all of the synchrnized properties to an IValueWriter. Use in conjunction with ReadAll().
        /// </summary>
        /// <param name="writer">IValueWriter to write th property values to.</param>
        internal void WriteAll(IValueWriter writer)
        {
            for (int i = 0; i < _propertySyncs.Length; i++)
            {
                _propertySyncs[i].WriteValue(writer);
            }

            // Obviously synchronized if we write all the values
            _isSynchronized = true;
        }
    }
}