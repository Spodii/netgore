using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Base class for any Entity that must be synchronized between both the Client and Server.
    /// </summary>
    public abstract class DynamicEntity : Entity
    {
        readonly PropertySyncBase[] _propertySyncs;
        bool _isSynchronized;
        ushort _mapIndex;

        /// <summary>
        /// Number of bits used for I/O on the Property index.
        /// </summary>
        byte _indexBitLength;

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
            _propertySyncs = PropertySyncBase.GetPropertySyncs(this).ToArray();

            // Cache the number of bits needed for the index
            _indexBitLength = (byte)BitOps.RequiredBits((uint)_propertySyncs.Length - 1);
        }

        /// <summary>
        /// Reads all of the changed property values from the specified IValueReader. Use in conjunction with Serialize().
        /// </summary>
        /// <param name="reader">IValueReader to read the changed property values from.</param>
        public void Deserialize(IValueReader reader)
        {
            // Grab the count for the number of properties to read
            byte count = reader.ReadByte(null);

            // Read the properties
            for (int i = 0; i < count; i++)
            {
                byte propIndex = reader.ReadByte(null);
                _propertySyncs[propIndex].ReadValue(reader);
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
        /// When overridden in the derived class, handles post-creation processing. This is required if you want to make use
        /// of any of the Entity's values, which are not available at the constructor.
        /// </summary>
        protected virtual void AfterCreation()
        {
        }

        /// <summary>
        /// Writes all of the changed property values to the specified IValueWriter. Use in conjunction with Deserialize().
        /// </summary>
        /// <param name="writer">IValueWriter to write the changed property values to.</param>
        public void Serialize(IValueWriter writer)
        {
            // TODO: See: http://netgore.com/bugs/view.php?id=50

            // Find the indicies that need to be synchronized
            var writeIndices = new Queue<byte>(_propertySyncs.Length);
            for (int i = 0; i < _propertySyncs.Length; i++)
            {
                if (_propertySyncs[i].HasValueChanged())
                    writeIndices.Enqueue((byte)i);
            }

            // Write the count
            byte count = (byte)writeIndices.Count;
            writer.Write(null, count);

            // Write the properties
            while (writeIndices.Count > 0)
            {
                byte propIndex = writeIndices.Dequeue();
                writer.Write("PropertyIndex", propIndex);
                _propertySyncs[propIndex].WriteValue(writer);
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