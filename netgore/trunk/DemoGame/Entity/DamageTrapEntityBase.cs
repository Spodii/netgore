using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Base class for an Entity which damages those who touch it.
    /// </summary>
    public abstract class DamageTrapEntityBase : DynamicEntity
    {
        int _damage;

        /// <summary>
        /// Gets or sets the amount of damage that is done to the Entity that touches this DamageTrapEntityBase.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        public int Damage { get { return _damage; } set { _damage = value; } }
    }
}
