using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.Collections;
using NetGore.Graphics;

namespace NetGore.Editor
{
    // TODO: Remove when done with the new editor

    /// <summary>
    /// Manages multiple <see cref="EditorCursor{TContainer}"/>.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container the cursors are in.</typeparam>
    public class EditorCursorManager<TContainer>
    {
        /// <summary>
        /// Delegate for handling events from the <see cref="EditorCursorManager{TScreen}"/>.
        /// </summary>
        /// <param name="sender">The object the event came from.</param>
        public delegate void EditorCursorManagerEventHandler(EditorCursorManager<TContainer> sender);

        readonly Control _cursorContainer;
        readonly List<PictureBox> _cursorControls = new List<PictureBox>();
        readonly List<EditorCursor<TContainer>> _cursors = new List<EditorCursor<TContainer>>();
        readonly Control _gameScreen;
        readonly ToolTip _toolTip;

        TContainer _container;
        EditorCursor<TContainer> _lastCurrentCursor;
        EditorCursor<TContainer> _selectedAltCursor;
        EditorCursor<TContainer> _selectedCursor;
        bool _useAlternateCursor;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorCursorManager{TScreen}"/> class.
        /// </summary>
        /// <param name="container">The container the cursors are in. Recommended that you use the Form
        /// the cursors are on or the screen the cursors are for.</param>
        /// <param name="toolTip">The tool tip. Can be null.</param>
        /// <param name="cursorContainer">The <see cref="Control"/> that the cursors are to be added to.</param>
        /// <param name="gameScreen">The <see cref="Control"/> that contains the actual game screen. Mouse
        /// events for cursors will be added to this.</param>
        public EditorCursorManager(TContainer container, ToolTip toolTip, Control cursorContainer, Control gameScreen)
        {
            if (Equals(container, default(TContainer)))
                throw new ArgumentNullException("container");
            if (cursorContainer == null)
                throw new ArgumentNullException("cursorContainer");
            if (gameScreen == null)
                throw new ArgumentNullException("gameScreen");

            _container = container;
            _toolTip = toolTip;
            _cursorContainer = cursorContainer;
            _gameScreen = gameScreen;

            LoadTypeInstances();

            _selectedCursor = Cursors.FirstOrDefault();
            _selectedAltCursor = Cursors.Where(x => x != _selectedCursor).FirstOrDefault();

            _gameScreen.MouseDown += _gameScreen_MouseDown;
            _gameScreen.MouseMove += _gameScreen_MouseMove;
            _gameScreen.MouseUp += _gameScreen_MouseUp;
        }

        /// <summary>
        /// Notifies listeners when the currently active cursor changes.
        /// </summary>
        public event EditorCursorManagerEventHandler CurrentCursorChanged;

        /// <summary>
        /// Gets or sets the container used by the cursors in this manager.
        /// </summary>
        public TContainer Container
        {
            get { return _container; }
            set { _container = value; }
        }

        /// <summary>
        /// Gets the cursors in this collection.
        /// </summary>
        public IEnumerable<EditorCursor<TContainer>> Cursors
        {
            get { return _cursors; }
        }

        /// <summary>
        /// Gets or sets the alternate selected cursor.
        /// </summary>
        public EditorCursor<TContainer> SelectedAltCursor
        {
            get { return _selectedAltCursor; }
            set
            {
                if (SelectedCursor == value || SelectedAltCursor == value)
                    return;

                _selectedAltCursor = value;

                if (_useAlternateCursor)
                    OnCurrentCursorChanged();

                ApplyCursorControlColoring();
            }
        }

        /// <summary>
        /// Gets or sets the selected cursor.
        /// </summary>
        public EditorCursor<TContainer> SelectedCursor
        {
            get { return _selectedCursor; }
            set
            {
                if (SelectedCursor == value)
                    return;

                _selectedCursor = value;

                if (!_useAlternateCursor)
                    OnCurrentCursorChanged();

                ApplyCursorControlColoring();
            }
        }

        /// <summary>
        /// Gets the <see cref="ToolTip"/> used by this object.
        /// </summary>
        public ToolTip ToolTip
        {
            get { return _toolTip; }
        }

        /// <summary>
        /// Gets or sets whether to use the alternate cursor. This also applies to what cursor is selected when
        /// selecting by clicking the cursor's <see cref="Control"/>.
        /// </summary>
        public bool UseAlternateCursor
        {
            get { return _useAlternateCursor; }
            set
            {
                if (_useAlternateCursor == value)
                    return;

                _useAlternateCursor = value;

                OnCurrentCursorChanged();
            }
        }

        /// <summary>
        /// Sets the coloring for the cursor <see cref="PictureBox"/>es.
        /// </summary>
        void ApplyCursorControlColoring()
        {
            PictureBox primary = null;
            if (_selectedCursor != null)
                primary = _cursorControls.FirstOrDefault(x => x.Tag == _selectedCursor);

            PictureBox secondary = null;
            if (_selectedAltCursor != null)
                secondary = _cursorControls.FirstOrDefault(x => x.Tag == _selectedAltCursor);

            foreach (var control in _cursorControls)
            {
                if (control == primary)
                    SetControlBackColor(control, Color.Lime);
                else if (control == secondary)
                    SetControlBackColor(control, Color.Aqua);
                else
                    SetControlBackColor(control, Color.White);
            }
        }

        /// <summary>
        /// Handles when a cursor <see cref="PictureBox"/> is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void CursorControlClickHandler(object sender, EventArgs e)
        {
            var c = ((Control)sender).Tag as EditorCursor<TContainer>;
            if (c == null)
                return;

            if (UseAlternateCursor)
                SelectedAltCursor = c;
            else
                SelectedCursor = c;
        }

        /// <summary>
        /// Draws the interface for the current cursor.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use for drawing.</param>
        public void DrawInterface(ISpriteBatch spriteBatch)
        {
            var cursor = GetCurrentCursor();

            if (cursor != null)
                cursor.DrawInterface(spriteBatch);
        }

        /// <summary>
        /// Draws the selection for the current cursor.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use for drawing.</param>
        public void DrawSelection(ISpriteBatch spriteBatch)
        {
            var cursor = GetCurrentCursor();

            if (cursor != null)
                cursor.DrawSelection(spriteBatch);
        }

        /// <summary>
        /// Gets the current cursor with respect to whether or not the alternate cursor is set to be used.
        /// </summary>
        /// <returns>The current cursor with respect to whether or not the alternate cursor is set to be used.</returns>
        public EditorCursor<TContainer> GetCurrentCursor()
        {
            if (UseAlternateCursor && SelectedAltCursor != null)
                return SelectedAltCursor;
            else
                return SelectedCursor;
        }

        void LoadTypeInstances()
        {
            // Get the type filter and set up the factory
            var filterCreator = new TypeFilterCreator
            {
                IsAbstract = false,
                IsClass = true,
                ConstructorParameters = Type.EmptyTypes,
                RequireConstructor = true,
                Subclass = typeof(EditorCursor<TContainer>)
            };

            var typeFactory = new TypeFactory(filterCreator.GetFilter());

            // Create the instances
            foreach (var type in typeFactory)
            {
                var instance = (EditorCursor<TContainer>)TypeFactory.GetTypeInstance(type);
                _cursors.Add(instance);
            }

            // Invoke the initialize method on each cursor instance. We do this now instead of in the prior loop to allow
            // all the constructors to be called first
            foreach (var cursor in _cursors)
            {
                cursor.InvokeInitialize(this);
            }

            // Now that we have called initialize on them all, set up all the cursors
            EventHandler clickHandler = CursorControlClickHandler;

            foreach (var cursor in Cursors.OrderByDescending(x => x.ToolbarPriority))
            {
                var cursorControl = new PictureBox
                {
                    Size = new Size(24, 24),
                    Dock = DockStyle.Left,
                    Image = cursor.CursorImage,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.White,
                    Tag = cursor,
                    ContextMenu = cursor.GetContextMenu()
                };

                if (ToolTip != null)
                {
                    var s = cursor.Name;
                    if (cursorControl.ContextMenu != null)
                        s += " (right-click for cursor settings)";

                    ToolTip.SetToolTip(cursorControl, s);
                }

                cursorControl.Click += clickHandler;

                _cursorContainer.Controls.Add(cursorControl);
                _cursorControls.Add(cursorControl);
            }
        }

        /// <summary>
        /// Tells the active cursor to handle the mouse wheel scrolling.
        /// </summary>
        /// <param name="amount">How much the mouse wheel has scrolled, and which direction.</param>
        public void MoveMouseWheel(int amount)
        {
            if (amount == 0)
                return;

            var cursor = GetCurrentCursor();

            if (cursor != null)
                cursor.MoveMouseWheel(amount);
        }

        /// <summary>
        /// Handles what happens when the current cursor changes.
        /// </summary>
        protected virtual void OnCurrentCursorChanged()
        {
            var cursor = GetCurrentCursor();

            if (_lastCurrentCursor != null)
                _lastCurrentCursor.Deactivate();

            _lastCurrentCursor = cursor;

            if (cursor != null)
                cursor.Activate();

            if (CurrentCursorChanged != null)
                CurrentCursorChanged(this);
        }

        /// <summary>
        /// Tells the active cursor to handle the Delete button being pressed.
        /// </summary>
        public void PressDelete()
        {
            var cursor = GetCurrentCursor();

            if (cursor != null)
                cursor.PressDelete();
        }

        static void SetControlBackColor(Control control, Color color)
        {
            if (control == null)
                return;

            control.BackColor = color;
        }

        /// <summary>
        /// Gets the cursor by the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type of cursor to get.</typeparam>
        /// <returns>The cursor of the specified type, or null if no cursor was found.</returns>
        public T TryGetCursor<T>() where T : EditorCursor<TContainer>
        {
            return Cursors.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Updates the current cursor.
        /// </summary>
        public void Update()
        {
            var c = GetCurrentCursor();

            if (c != null)
                c.UpdateCursor();
        }

        void _gameScreen_MouseDown(object sender, MouseEventArgs e)
        {
            var c = GetCurrentCursor();

            if (c != null)
                c.MouseDown(e);
        }

        void _gameScreen_MouseMove(object sender, MouseEventArgs e)
        {
            var c = GetCurrentCursor();

            if (c != null)
                c.MouseMove(e);
        }

        void _gameScreen_MouseUp(object sender, MouseEventArgs e)
        {
            var c = GetCurrentCursor();

            if (c != null)
                c.MouseUp(e);
        }
    }
}