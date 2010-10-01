namespace DemoGame.Editor
{
    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.DropDownButton"/>.
    /// </summary>
    public interface IToolBarSplitButtonSettings : IToolBarDropDownItemSettings
    {
        /// <summary>
        /// Retrieves the size of a rectangular area into which a <see cref="System.Windows.Forms.ToolStripSplitButton"/> can be fitted.
        /// </summary>
        /// <param name="constrainingSize">The custom-sized area for a control.</param>
        /// <returns>An ordered pair of type <see cref="System.Drawing.Size"/>, representing the width and height of a rectangle.</returns>
        System.Drawing.Size GetPreferredSize(System.Drawing.Size constrainingSize);

        /// <summary>
        /// If the <see cref="System.Windows.Forms.ToolStripItem.Enabled"/> property is true,
        /// calls the <see cref="System.Windows.Forms.ToolStripSplitButton.OnButtonClick(System.EventArgs)"/> method.
        /// </summary>
        void PerformButtonClick();

        /// <summary>
        /// Gets or sets a value indicating whether default or custom <see cref="System.Windows.Forms.ToolTip"/> text
        /// is displayed on the <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        bool AutoToolTip { set; get; }

        /// <summary>
        /// Gets the size and location of the standard button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        System.Drawing.Rectangle ButtonBounds { get; }

        /// <summary>
        /// Gets a value indicating whether the button portion of the <see cref="System.Windows.Forms.ToolStripSplitButton"/>
        /// is in the pressed state.
        /// </summary>
        bool ButtonPressed { get; }

        /// <summary>
        /// Gets a value indicating whether the standard button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/>
        /// is selected or the <see cref="System.Windows.Forms.ToolStripSplitButton.DropDownButtonPressed"/> property is true.
        /// </summary>
        bool ButtonSelected { get; }

        /// <summary>
        /// Gets or sets the portion of the <see cref="System.Windows.Forms.ToolStripSplitButton"/> that is activated when the
        /// control is first selected.
        /// </summary>
        System.Windows.Forms.ToolStripItem DefaultItem { set; get; }

        /// <summary>
        /// Gets the size and location, in screen coordinates, of the drop-down button portion of a
        /// <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        System.Drawing.Rectangle DropDownButtonBounds { get; }

        /// <summary>
        /// Gets a value indicating whether the drop-down portion of the <see cref="System.Windows.Forms.ToolStripSplitButton"/>
        /// is in the pressed state.
        /// </summary>
        bool DropDownButtonPressed { get; }

        /// <summary>
        /// Gets a value indicating whether the drop-down button portion of a
        /// <see cref="System.Windows.Forms.ToolStripSplitButton"/> is selected.
        /// </summary>
        bool DropDownButtonSelected { get; }

        /// <summary>
        /// The width, in pixels, of the drop-down button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        int DropDownButtonWidth { set; get; }

        /// <summary>
        /// Gets the boundaries of the separator between the standard and drop-down button portions of a
        /// <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        System.Drawing.Rectangle SplitterBounds { get; }

        /// <summary>
        /// Occurs when the standard button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/> is clicked.
        /// </summary>
        event System.EventHandler ButtonClick;

        /// <summary>
        /// Occurs when the standard button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/> is double-clicked.
        /// </summary>
        event System.EventHandler ButtonDoubleClick;

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripSplitButton.DefaultItem"/> has changed.
        /// </summary>
        event System.EventHandler DefaultItemChanged;
    }
}