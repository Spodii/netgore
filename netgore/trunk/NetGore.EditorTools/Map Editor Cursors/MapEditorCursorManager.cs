using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.Collections;

namespace NetGore.EditorTools
{
    public delegate void MapEditorCursorManagerEventHandler<TScreen>(MapEditorCursorManager<TScreen> sender) where TScreen : Form;

    public class MapEditorCursorManager<TScreen> where TScreen : Form
    {
        readonly Func<MapEditorCursorBase<TScreen>, bool> _allowCursorEventChecker;
        readonly Control _cursorContainer;
        readonly List<PictureBox> _cursorControls = new List<PictureBox>();
        readonly List<MapEditorCursorBase<TScreen>> _cursors = new List<MapEditorCursorBase<TScreen>>();
        readonly Control _gameScreen;
        readonly TScreen _screen;
        MapEditorCursorBase<TScreen> _selectedCursor;

        /// <summary>
        /// Notifies listeners when the selected cursor changes.
        /// </summary>
        public event MapEditorCursorManagerEventHandler<TScreen> OnChangeSelectedCursor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapEditorCursorManager&lt;TScreen&gt;"/> class.
        /// </summary>
        /// <param name="screen">The primary form the cursors are on.</param>
        /// <param name="cursorContainer">The <see cref="Control"/> that the cursors are to be added to.</param>
        /// <param name="gameScreen">The <see cref="Control"/> that contains the actual game screen. Mouse
        /// events for cursors will be added to this.</param>
        /// <param name="allowCursorEventChecker">Func that checks if a cursor event is allowed to be executed. This
        /// way you can prevent cursor events at certain times.</param>
        public MapEditorCursorManager(TScreen screen, Control cursorContainer, Control gameScreen,
                                      Func<MapEditorCursorBase<TScreen>, bool> allowCursorEventChecker)
        {
            if (screen == null)
                throw new ArgumentNullException("screen");
            if (cursorContainer == null)
                throw new ArgumentNullException("cursorContainer");
            if (gameScreen == null)
                throw new ArgumentNullException("gameScreen");

            _screen = screen;
            _cursorContainer = cursorContainer;
            _gameScreen = gameScreen;
            _allowCursorEventChecker = allowCursorEventChecker;

            LoadTypeInstances();

            _selectedCursor = Cursors.FirstOrDefault();

            _gameScreen.MouseDown += _gameScreen_MouseDown;
            _gameScreen.MouseMove += _gameScreen_MouseMove;
            _gameScreen.MouseUp += _gameScreen_MouseUp;
        }

        /// <summary>
        /// Gets the cursors in this collection.
        /// </summary>
        public IEnumerable<MapEditorCursorBase<TScreen>> Cursors
        {
            get { return _cursors; }
        }

        /// <summary>
        /// Gets the screen used by the cursors in this manager.
        /// </summary>
        public TScreen Screen
        {
            get { return _screen; }
        }

        /// <summary>
        /// Gets or sets the selected cursor.
        /// </summary>
        public MapEditorCursorBase<TScreen> SelectedCursor
        {
            get { return _selectedCursor; }
            set
            {
                if (_selectedCursor == value)
                    return;

                foreach (var control in _cursorControls)
                    control.BackColor = Color.White;

                _selectedCursor = value;

                if (_selectedCursor != null)
                {
                    var selectedControl = _cursorControls.FirstOrDefault(x => x.Tag == _selectedCursor);
                    if (selectedControl != null)
                        selectedControl.BackColor = Color.Lime;
                }

                if (OnChangeSelectedCursor != null)
                    OnChangeSelectedCursor(this);
            }
        }

        void _gameScreen_MouseDown(object sender, MouseEventArgs e)
        {
            if (SelectedCursor != null && _allowCursorEventChecker(SelectedCursor))
                SelectedCursor.MouseDown(Screen, e);
        }

        void _gameScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectedCursor != null && _allowCursorEventChecker(SelectedCursor))
                SelectedCursor.MouseMove(Screen, e);
        }

        void _gameScreen_MouseUp(object sender, MouseEventArgs e)
        {
            if (SelectedCursor != null && _allowCursorEventChecker(SelectedCursor))
                SelectedCursor.MouseUp(Screen, e);
        }

        public void DrawInterface()
        {
            if (SelectedCursor != null)
                SelectedCursor.DrawInterface(Screen);
        }

        public void DrawSelection()
        {
            if (SelectedCursor != null)
                SelectedCursor.DrawSelection(Screen);
        }

        void LoadTypeInstances()
        {
            TypeFilterCreator filterCreator = new TypeFilterCreator
            {
                IsAbstract = false,
                IsClass = true,
                ConstructorParameters = Type.EmptyTypes,
                RequireConstructor = true,
                Subclass = typeof(MapEditorCursorBase<TScreen>)
            };

            var typeFactory = new TypeFactory(filterCreator.GetFilter());

            foreach (var type in typeFactory)
            {
                var instance = (MapEditorCursorBase<TScreen>)TypeFactory.GetTypeInstance(type);
                _cursors.Add(instance);
            }

            foreach (var cursor in Cursors.OrderByDescending(x => x.ToolbarPriority))
            {
                var cursorControl = new PictureBox
                {
                    Size = new Size(24, 24),
                    Dock = DockStyle.Left,
                    Image = cursor.CursorImage,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.White,
                    Tag = cursor
                };

                cursorControl.Click +=
                    delegate(object sender, EventArgs e) { SelectedCursor = ((Control)sender).Tag as MapEditorCursorBase<TScreen>; };

                _cursorContainer.Controls.Add(cursorControl);
                _cursorControls.Add(cursorControl);
            }
        }

        public void PressDelete()
        {
            if (SelectedCursor != null)
                SelectedCursor.PressDelete(Screen);
        }

        /// <summary>
        /// Gets the cursor by the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type of cursor to get.</typeparam>
        /// <returns>The cursor of the specified type, or null if no cursor was found.</returns>
        public MapEditorCursorBase<TScreen> TryGetCursor<T>()
        {
            return Cursors.FirstOrDefault(x => x.GetType() == typeof(T));
        }

        public void Update()
        {
            if (SelectedCursor != null)
                SelectedCursor.UpdateCursor(Screen);
        }
    }
}