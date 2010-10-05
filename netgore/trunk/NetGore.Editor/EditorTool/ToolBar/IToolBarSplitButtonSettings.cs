using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.DropDownButton"/>.
    /// </summary>
    public interface IToolBarSplitButtonSettings : IToolBarDropDownItemSettings
    {
        /// <summary>
        /// Occurs when the standard button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/> is clicked.
        /// </summary>
        event EventHandler ButtonClick;

        /// <summary>
        /// Occurs when the standard button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/> is double-clicked.
        /// </summary>
        event EventHandler ButtonDoubleClick;

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripSplitButton.DefaultItem"/> has changed.
        /// </summary>
        event EventHandler DefaultItemChanged;

        /// <summary>
        /// Gets or sets a value indicating whether default or custom <see cref="System.Windows.Forms.ToolTip"/> text
        /// is displayed on the <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        bool AutoToolTip { set; get; }

        /// <summary>
        /// Gets the size and location of the standard button portion of a <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        Rectangle ButtonBounds { get; }

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
        /// Gets or sets if the <see cref="Tool"/>'s <see cref="NetGore.Editor.EditorTool.Tool.IsEnabled"/> state
        /// will be toggle by clicking this control.
        /// </summary>
        bool ClickToEnable { set; get; }

        /// <summary>
        /// Gets or sets the portion of the <see cref="System.Windows.Forms.ToolStripSplitButton"/> that is activated when the
        /// control is first selected.
        /// </summary>
        ToolStripItem DefaultItem { set; get; }

        /// <summary>
        /// Gets the size and location, in screen coordinates, of the drop-down button portion of a
        /// <see cref="System.Windows.Forms.ToolStripSplitButton"/>.
        /// </summary>
        Rectangle DropDownButtonBounds { get; }

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
        Rectangle SplitterBounds { get; }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a <see cref="System.Windows.Forms.ToolStripSplitButton"/> can be fitted.
        /// </summary>
        /// <param name="constrainingSize">The custom-sized area for a control.</param>
        /// <returns>An ordered pair of type <see cref="System.Drawing.Size"/>, representing the width and height of a rectangle.</returns>
        Size GetPreferredSize(Size constrainingSize);

        /// <summary>
        /// If the <see cref="System.Windows.Forms.ToolStripItem.Enabled"/> property is true,
        /// calls the <see cref="System.Windows.Forms.ToolStripSplitButton.OnButtonClick(System.EventArgs)"/> method.
        /// </summary>
        void PerformButtonClick();
    }
}