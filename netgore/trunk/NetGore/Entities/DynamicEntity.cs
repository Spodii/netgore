using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    public abstract class DynamicEntity : Entity
    {
        readonly PropertySyncBase[] _propertySyncs;
        ushort _mapIndex;

        /// <summary>
        /// Synchronizes the CollisionType for the base Entity.
        /// </summary>
        [SyncValue("CollisionType")]
        [Obsolete("This property is not to be called directly. It is only to be used for value synchronization.")]
// ReSharper disable UnusedMember.Local
            CollisionType CollisionTypeSync // ReSharper restore UnusedMember.Local
        {
            get { return CollisionType; }
            set { SetCollisionTypeRaw(value); }
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
            Vector2 PositionSync // ReSharper restore UnusedMember.Local
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
            Vector2 SizeSync // ReSharper restore UnusedMember.Local
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
            Vector2 VelocitySync // ReSharper restore UnusedMember.Local
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
            float WeightSync // ReSharper restore UnusedMember.Local
        {
            get { return Weight; }
            set { SetWeightRaw(value); }
        }

        protected DynamicEntity()
        {
            _propertySyncs = PropertySyncBase.GetPropertySyncs(this).ToArray();
        }

        public void Read(IValueReader reader)
        {
            for (int i = 0; i < _propertySyncs.Length; i++)
            {
                _propertySyncs[i].ReadValue(reader);
            }
        }

        protected static CollisionType ReadCollisionType(BitStream writer)
        {
            return (CollisionType)writer.ReadByte();
        }

        public void Write(IValueWriter writer)
        {
            for (int i = 0; i < _propertySyncs.Length; i++)
            {
                _propertySyncs[i].WriteValue(writer);
            }
        }
    }
}