using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.IO;

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
        /// Index of the map this DynamicEntity is on.
        /// </summary>
        ushort _mapIndex;

        /// <summary>
        /// Synchronizes the CollisionType for the base Entity.
        /// </summary>
        [SyncValue("CollisionType")]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
// ReSharper disable UnusedMember.Local
            protected internal CollisionType CollisionTypeSync // ReSharper restore UnusedMember.Local
        {
            get { return CollisionType; }
            set { SetCollisionTypeRaw(value); }
        }

        /// <summary>
        /// Gets if the DynamicEntity's values are already synchronized.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                // If we already know we're not synchronized, just return false
                if (!_isSynchronized)
                    return false;

                // Check each property, stopping at the first non-synchronized one
                for (int i = 0; i < _propertySyncs.Length; i++)
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
        /// Gets the unique index of this DynamicEntity on the map.
        /// </summary>
        public int MapIndex
        {
            get { return _mapIndex; }
            set
            {
                // TODO: Setter needs to eventually be hidden from alteration outside of the map
                if (value < 0 || value > ushort.MaxValue)
                    throw new ArgumentOutOfRangeException("value");

                _mapIndex = (ushort)value;
            }
        }

        /// <summary>
        /// Synchronizes the Position for the base Entity.
        /// </summary>
        [SyncValue("Position")]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
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
// ReSharper disable UnusedMember.Local
            protected internal Vector2 SizeSync // ReSharper restore UnusedMember.Local
        {
            get { return Size; }
            set { SetSizeRaw(value); }
        }

        /// <summary>
        /// Synchronizes the Velocity for the base Entity.
        /// </summary>
        [SyncValue("Velocity")]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
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
                Debug.Assert(!_propertySyncs[i].SkipNetworkSync);
            for (int i = _lastNetworkSyncIndex + 1; i < _propertySyncs.Length; i++)
                Debug.Assert(_propertySyncs[i].SkipNetworkSync);
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
                uint propIndex = reader.ReadUInt("PropertyIndex", lastIndex, highestPropertyIndex);
                _propertySyncs[propIndex].ReadValue(reader);
                lastIndex = propIndex;
            }
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
                uint propIndex = (uint)writeIndices.Dequeue();
                writer.Write("PropertyIndex", propIndex, lastIndex, highestPropertyIndex);
                _propertySyncs[propIndex].WriteValue(writer);
                lastIndex = propIndex;
            }

            // Synchronized!
            _isSynchronized = true;
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