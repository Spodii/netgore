using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.Client.Controls
{
    // TODO: $$ Shop form
    /*
    class ShopForm : Form, IRestorableSettings
    {

        #region IRestorableSettings Members

        /// <summary>
        /// Loads the values supplied by the <paramref name="items"/> to reconstruct the settings.
        /// </summary>
        /// <param name="items">NodeItems containing the values to restore.</param>
        public void Load(IDictionary<string, string> items)
        {
            Position = new Vector2(items.AsFloat("X", Position.X), items.AsFloat("Y", Position.Y));
        }

        /// <summary>
        /// Returns the key and value pairs needed to restore the settings.
        /// </summary>
        /// <returns>The key and value pairs needed to restore the settings.</returns>
        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[] { new NodeItem("X", Position.X), new NodeItem("Y", Position.Y)};
        }

        #endregion
    }
    */
}
