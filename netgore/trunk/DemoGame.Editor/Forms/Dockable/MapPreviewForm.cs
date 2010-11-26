using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Editor.Docking;

namespace DemoGame.Editor
{
    public partial class MapPreviewForm : DockContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPreviewForm"/> class.
        /// </summary>
        public MapPreviewForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the <see cref="MapPreviewScreenControl"/> for this <see cref="MapPreviewForm"/>.
        /// </summary>
        public MapPreviewScreenControl MapScreenControl
        {
            get { return mapScreen; }
        }
    }
}
