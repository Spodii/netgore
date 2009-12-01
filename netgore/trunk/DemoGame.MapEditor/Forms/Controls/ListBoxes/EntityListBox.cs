using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.Client;
using NetGore;
using NetGore.EditorTools;

namespace DemoGame.MapEditor
{
    public class EntityListBox : MapItemListBox<Map, Entity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityListBox"/> class.
        /// </summary>
        public EntityListBox() : base(true, false, true)
    {
    }

        /// <summary>
        /// When overridden in the derived class, creates a clone of the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to clone.</param>
        protected override void Clone(Entity item)
        {
            // TODO: Add support for this
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in the derived class, deletes the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to delete.</param>
        protected override void Delete(Entity item)
        {
            Map.RemoveEntity(item);
           this.RemoveItemAndReselect(item);
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of objects to be used in this MapItemListBox.
        /// </summary>
        /// <returns>
        /// An IEnumerable of objects to be used in this MapItemListBox.
        /// </returns>
        protected override IEnumerable<Entity> GetItems()
        {
            return Map.DynamicEntities.OfType<Entity>();
        }

        /// <summary>
        /// When overridden in the derived class, centers the camera on the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to locate.</param>
        protected override void Locate(Entity item)
        {
            Camera.CenterOn(item);
        }

    }
}