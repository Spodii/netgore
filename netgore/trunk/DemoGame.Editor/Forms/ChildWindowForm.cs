using System;
using System.ComponentModel;
using System.Linq;
using NetGore.Editor;
using NetGore.Editor.Docking;
using NetGore.Editor.EditorTool;

namespace DemoGame.Editor
{
    public partial class ChildWindowForm : DockContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChildWindowForm"/> class.
        /// </summary>
        public ChildWindowForm()
        {
            HideOnClose = true;

            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the <see cref="NetGore.Editor.EditorTool.ToolBarVisibility"/> for this form. Use <see cref="NetGore.Editor.EditorTool.ToolBarVisibility.Global"/>
        /// to not set any special visibility.
        /// Default value is <see cref="NetGore.Editor.EditorTool.ToolBarVisibility.Global"/>.
        /// </summary>
        [Description("The ToolBarVisibility for this form. Use Global to not set any special visibility.")]
        [DefaultValue(ToolBarVisibility.Global)]
        [Browsable(true)]
        public ToolBarVisibility ToolBarVisibility { get; set; }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            ToolBar.CurrentToolBarVisibility = ToolBarVisibility;
        }
    }
}