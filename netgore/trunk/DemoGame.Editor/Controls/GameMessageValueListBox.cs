using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="ListBox"/> for a <see cref="GameMessage"/> and its corresponding value string.
    /// </summary>
    public class GameMessageValueListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMessageValueListBox"/> class.
        /// </summary>
        public GameMessageValueListBox()
        {
            if (DesignMode)
                return;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            DrawMode = DrawMode.OwnerDrawFixed;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DesignMode ||
                !ControlHelper.DrawListItem<KeyValuePair<GameMessage, string>>(Items, e,
                    x => new KeyValuePair<string, string>(x.Key.ToString(), x.Value)))
                base.OnDrawItem(e);
        }
    }
}