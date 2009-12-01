using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.Client;
using NetGore;
using NetGore.EditorTools;

namespace DemoGame.MapEditor
{
    public class WallListBox : MapItemListBox<Map, WallEntityBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WallListBox"/> class.
        /// </summary>
        public WallListBox() : base(true, false, false)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates a clone of the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to clone.</param>
        protected override void Clone(WallEntityBase item)
        {
            // TODO: Clone WallEntity
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in the derived class, deletes the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to delete.</param>
        protected override void Delete(WallEntityBase item)
        {
            // TODO: Delete WallEntity
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of objects to be used in this MapItemListBox.
        /// </summary>
        /// <returns>
        /// An IEnumerable of objects to be used in this MapItemListBox.
        /// </returns>
        protected override IEnumerable<WallEntityBase> GetItems()
        {
            return Map.Entities.OfType<WallEntityBase>();
        }

        /// <summary>
        /// When overridden in the derived class, centers the camera on the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to locate.</param>
        protected override void Locate(WallEntityBase item)
        {
            Camera.CenterOn(item);
        }
    }
}