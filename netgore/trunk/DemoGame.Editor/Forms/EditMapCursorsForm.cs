using System.Linq;
using NetGore.EditorTools;

namespace DemoGame.Editor
{
    /// <summary>
    /// Contains the <see cref="EditorCursor{TContainer}"/>s for the <see cref="EditMapForm"/>.
    /// There may be only one instance of this class.
    /// </summary>
    public sealed partial class EditMapCursorsForm : ChildWindowForm
    {
        static readonly EditMapCursorsForm _instance;
        readonly EditorCursorManager<MapScreenControl> _cursorManager;

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

        /// <summary>
        /// Gets the <see cref="EditMapCursorsForm"/> instance.
        /// </summary>
        public static EditMapCursorsForm Instance
        {
            get { return _instance; }
        }
    }
}