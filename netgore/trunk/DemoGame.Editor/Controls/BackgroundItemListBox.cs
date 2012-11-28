using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NetGore.Editor.WinForms;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    public class BackgroundItemListBox : MapItemListBox<EditorMap, BackgroundImage>
    {
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
    }
}