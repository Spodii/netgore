using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;
using NetGore.Editor.EditorTool;

namespace DemoGame.Editor
{
    /// <summary>
    /// Base class for a dockable <see cref="System.Windows.Forms.Form"/> in the editor that is the source of input and interaction
    /// for <see cref="Tool"/>s.
    /// </summary>
    public partial class ToolTargetFormBase : DockContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolTargetFormBase"/> class.
        /// </summary>
        protected ToolTargetFormBase()
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

        /// <summary>
        /// Handles when the form receives focus.
        /// </summary>
        void FormFocused()
        {
            if (DesignMode)
                return;

            // Set the ToolBarVisibility to that of this form
            ToolBar.CurrentToolBarVisibility = ToolBarVisibility;

            // Grab the corresponding ToolBar
            var tb = ToolBar.GetToolBar(ToolBarVisibility);
            if (tb != null)
            {
                // Set the ToolBar's object
                var toolBarObj = GetToolBarObject();
                tb.DisplayObject = toolBarObj;
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the object that represents the focus of this <see cref="ToolTargetFormBase"/>
        /// and what the <see cref="ToolBar"/> is being displayed for.
        /// </summary>
        /// <returns>The object that represents what the <see cref="ToolBar"/> is being displayed for.</returns>
        protected virtual object GetToolBarObject()
        {
            Debug.Fail("Derived classes should override this and return an object.");
            return null;
        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Form.Activated"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (DesignMode)
                return;

            FormFocused();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Enter"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (DesignMode)
                return;

            FormFocused();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            if (DesignMode)
                return;

            FormFocused();
        }
    }
}