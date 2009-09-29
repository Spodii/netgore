using System.Linq;
using DemoGame;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.RPGComponents;

namespace DemoGame
{
    /// <summary>
    /// Defines the basics of a single item.
    /// </summary>
    public abstract class ItemEntityBase : ItemEntityBase<ItemType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEntityBase&lt;TItemType&gt;"/> class.
        /// </summary>
        /// <param name="pos">Position to place the item</param>
        /// <param name="size">Size of the item's CollisionBox</param>
        protected ItemEntityBase(Vector2 pos, Vector2 size) : base(pos, size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEntityBase&lt;TItemType&gt;"/> class.
        /// </summary>
        protected ItemEntityBase()
        {
        }
    }
}