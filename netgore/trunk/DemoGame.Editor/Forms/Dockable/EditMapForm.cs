using System.Collections.Generic;
using System.Linq;
using DemoGame.Client;
using NetGore.Editor.Docking;

namespace DemoGame.Editor
{
    /// <summary>
    /// A form that displays a <see cref="Map"/> and provides interactive editing of it.
    /// </summary>
    public sealed partial class EditMapForm : ToolTargetFormBase
    {
        readonly List<TransBox> _transBoxes = new List<TransBox>(9);

        TransBox _selTransBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditMapForm"/> class.
        /// </summary>
        public EditMapForm()
        {
            InitializeComponent();
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
    }
}