using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.EditorTools;

namespace DemoGame.Editor
{
    /// <summary>
    /// Contains the <see cref="EditorCursor{TContainer}"/>s for the <see cref="EditMapForm"/>.
    /// There may be only one instance of this class.
    /// </summary>
    public sealed partial class EditMapCursorsForm : ChildWindowForm
    {
        readonly EditorCursorManager<MapScreenControl> _cursorManager;

        /// <summary>
        /// Gets the <see cref="EditMapCursorsForm"/> instance.
        /// </summary>
        public static EditMapCursorsForm Instance { get { return _instance; } }

        static readonly EditMapCursorsForm _instance;

        /// <summary>
        /// Initializes the <see cref="EditMapCursorsForm"/> class.
        /// </summary>
        static EditMapCursorsForm()
        {
            _instance = new EditMapCursorsForm();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditMapCursorsForm"/> class.
        /// </summary>
        EditMapCursorsForm()
        {
            InitializeComponent();

            // TODO: !! _cursorManager  = new EditorCursorManager<MapScreenControl>(null, null, this, 
        }
    }
}
