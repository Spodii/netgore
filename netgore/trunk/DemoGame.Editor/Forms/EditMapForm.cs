using System;
using System.Linq;
using DemoGame.Client;
using NetGore.EditorTools;

namespace DemoGame.Editor
{
    /// <summary>
    /// A form that displays a <see cref="Map"/> and provides interactive editing of it.
    /// There may be only one instance of this class.
    /// </summary>
    public sealed partial class EditMapForm : ChildWindowForm
    {
        static readonly EditMapForm _instance;
        EditorCursorManager<EditMapForm> _cursorManager;

        /// <summary>
        /// Initializes the <see cref="EditMapForm"/> class.
        /// </summary>
        static EditMapForm()
        {
            _instance = new EditMapForm();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditMapForm"/> class.
        /// </summary>
        EditMapForm()
        {
            InitializeComponent();
        }

        public EditorCursorManager<EditMapForm> CursorManager
        {
            get { return _cursorManager; }
        }

        /// <summary>
        /// Gets the <see cref="EditMapForm"/> instance.
        /// </summary>
        public static EditMapForm Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the <see cref="MapScreenControl"/> for this EditMapForm.
        /// </summary>
        public MapScreenControl MapScreenControl
        {
            get { return mapScreen; }
        }

        /// <summary>
        /// Gets if the cursor is allowed to be changed to a new cursor.
        /// </summary>
        /// <param name="newCursor">The new cursor to change true.</param>
        /// <returns>True if we can change to the <paramref name="newCursor"/>; otherwise false.</returns>
        bool AllowEditorCursorChange(EditorCursor<EditMapForm> newCursor)
        {
            return true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _cursorManager = new EditorCursorManager<EditMapForm>(this, null, EditMapCursorsForm.Instance, mapScreen,
                                                                  AllowEditorCursorChange);
        }
    }
}