using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.Client;
using NetGore.EditorTools;

namespace DemoGame.Editor
{
    /// <summary>
    /// A form that displays a <see cref="Map"/> and provides interactive editing of it.
    /// </summary>
    public sealed partial class EditMapForm : ChildWindowForm
    {
        readonly List<TransBox> _transBoxes = new List<TransBox>(9);
        EditorCursorManager<EditMapForm> _cursorManager;
        TransBox _selTransBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditMapForm"/> class.
        /// </summary>
        public EditMapForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the <see cref="EditorCursorManager{T}"/> used by this form.
        /// </summary>
        public EditorCursorManager<EditMapForm> CursorManager
        {
            get { return _cursorManager; }
        }

        /// <summary>
        /// Gets the <see cref="MapScreenControl"/> for this EditMapForm.
        /// </summary>
        public MapScreenControl MapScreenControl
        {
            get { return mapScreen; }
        }

        /// <summary>
        /// Gets or sets the selected transformation box.
        /// </summary>
        public TransBox SelectedTransBox
        {
            get { return _selTransBox; }
            set { _selTransBox = value; }
        }

        /// <summary>
        /// Gets the list of the current <see cref="TransBox"/>es.
        /// </summary>
        public List<TransBox> TransBoxes
        {
            get { return _transBoxes; }
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

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _cursorManager = new EditorCursorManager<EditMapForm>(this, null, EditMapCursorsForm.Instance, mapScreen,
                                                                  AllowEditorCursorChange);

            _cursorManager.CurrentCursorChanged += _cursorManager_CurrentCursorChanged;
        }

        void _cursorManager_CurrentCursorChanged(EditorCursorManager<EditMapForm> sender)
        {
            SelectedTransBox = null;
            TransBoxes.Clear();
        }
    }
}