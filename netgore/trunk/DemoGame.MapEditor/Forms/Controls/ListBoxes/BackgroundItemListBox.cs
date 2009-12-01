using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.Client;
using NetGore.EditorTools;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    public class BackgroundItemListBox : MapItemListBox<Map, BackgroundImage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundItemListBox"/> class.
        /// </summary>
        public BackgroundItemListBox()
            : base(true, false, false)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates a clone of the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to clone.</param>
        protected override void Clone(BackgroundImage item)
        {
            // TODO: Clone BackgroundImage
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in the derived class, deletes the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to delete.</param>
        protected override void Delete(BackgroundImage item)
        {
            // TODO: Remove BackgroundImage
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of objects to be used in this MapItemListBox.
        /// </summary>
        /// <returns>
        /// An IEnumerable of objects to be used in this MapItemListBox.
        /// </returns>
        protected override IEnumerable<BackgroundImage> GetItems()
        {
            return Map.BackgroundImages;
        }

        /// <summary>
        /// When overridden in the derived class, centers the camera on the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to locate.</param>
        protected override void Locate(BackgroundImage item)
        {
        }
    }
}