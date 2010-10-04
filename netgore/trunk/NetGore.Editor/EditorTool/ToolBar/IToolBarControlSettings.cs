using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Contains the settings common for all <see cref="ToolBarControlType"/>s.
    /// </summary>
    public interface IToolBarControlSettings
    {
        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarControlSettings.BackColor"/> property changes.
        /// </summary>
        event EventHandler BackColorChanged;

        /// <summary>
        /// Notifies listeners when this control was clicked.
        /// </summary>
        event EventHandler Click;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarControlSettings.DisplayStyle"/> property changes.
        /// </summary>
        event EventHandler DisplayStyleChanged;

        /// <summary>
        /// Notifies listeners when this control was double-clicked.
        /// </summary>
        event EventHandler DoubleClick;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarControlSettings.Enabled"/> property changes.
        /// </summary>
        event EventHandler EnabledChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarControlSettings.ForeColor"/> property changes.
        /// </summary>
        event EventHandler ForeColorChanged;

        /// <summary>
        /// Notifies listeners when the mouse pointer is over the control and a mouse button is pressed.
        /// </summary>
        event MouseEventHandler MouseDown;

        /// <summary>
        /// Notifies listeners when the mouse pointer enters the control.
        /// </summary>
        event EventHandler MouseEnter;

        /// <summary>
        /// Notifies listeners when the mouse pointer hovers over the control.
        /// </summary>
        event EventHandler MouseHover;

        /// <summary>
        /// Notifies listeners when the mouse pointer leaves the control.
        /// </summary>
        event EventHandler MouseLeave;

        /// <summary>
        /// Notifies listeners when the mouse pointer moves over the control.
        /// </summary>
        event MouseEventHandler MouseMove;

        /// <summary>
        /// Notifies listeners when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        event MouseEventHandler MouseUp;

        /// <summary>
        /// Notifies listeners when the control is redrawn.
        /// </summary>
        event PaintEventHandler Paint;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarControlSettings.Text"/> property changes.
        /// </summary>
        event EventHandler TextChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the control is automatically sized.
        /// </summary>
        bool AutoSize { set; get; }

        /// <summary>
        /// A <see cref="System.Drawing.Color"/> that represents the background color of the item.
        /// The default is the value of the <see cref="System.Windows.Forms.Control.DefaultBackColor"/> property.
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// Gets the size and location of the control.
        /// </summary>
        Rectangle Bounds { get; }

        /// <summary>
        /// Gets a value indicating whether the control can be selected.
        /// </summary>
        bool CanSelect { get; }

        /// <summary>
        /// Gets the area where content, such as text and icons, can be placed within the control without overwriting background borders.
        /// </summary>
        Rectangle ContentRectangle { get; }

        /// <summary>
        /// Gets or sets whether text and images are displayed.
        /// The default is <see cref="System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText"/>.
        /// </summary>
        ToolStripItemDisplayStyle DisplayStyle { set; get; }

        /// <summary>
        /// Gets or sets if the control is enabled.
        /// The default value is true.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Font"/> of the control's text.
        /// The default is the value of the <see cref="System.Windows.Forms.Control.DefaultFont"/> property.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// The foreground <see cref="System.Drawing.Color"/> of the control.
        /// The default is the value of the <see cref="System.Windows.Forms.Control.DefaultForeColor"/> property.
        /// </summary>
        Color ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the height of the control in pixels
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Image"/> to display on the control.
        /// </summary>
        Image Image { get; set; }

        /// <summary>
        /// Gets or sets how the <see cref="IToolBarControlSettings.Image"/> should be aligned on the control
        /// The default value is <see cref="System.Drawing.ContentAlignment.MiddleLeft"/>.
        /// </summary>
        ContentAlignment ImageAlign { get; set; }

        /// <summary>
        /// Gets or sets how the <see cref="IToolBarControlSettings.Image"/> is sized on the control.
        /// The default is <see cref="System.Windows.Forms.ToolStripItemImageScaling.SizeToFit"/>.
        /// </summary>
        ToolStripItemImageScaling ImageScaling { set; get; }

        /// <summary>
        /// Gets or sets the color to treat as transparent in the control's image.
        /// </summary>
        Color ImageTransparentColor { set; get; }

        /// <summary>
        /// Gets or sets the internal spacing, in pixels, between the item's contents and its edges.
        /// </summary>
        Padding Padding { set; get; }

        /// <summary>
        /// Gets if this control is currently pressed.
        /// </summary>
        bool Pressed { get; }

        /// <summary>
        /// Gets if this control is currently selected.
        /// </summary>
        bool Selected { get; }

        /// <summary>
        /// Gets or sets the size of the control.
        /// </summary>
        Size Size { set; get; }

        /// <summary>
        /// Gets or sets the text that is to be displayed on the control.
        /// </summary>
        string Text { set; get; }

        /// <summary>
        /// Gets or sets the alignment of the text.
        /// The default is <see cref="System.Drawing.ContentAlignment.MiddleRight"/>.
        /// </summary>
        ContentAlignment TextAlign { set; get; }

        /// <summary>
        /// Gets or sets the direction of the text.
        /// </summary>
        ToolStripTextDirection TextDirection { set; get; }

        /// <summary>
        /// Gets or sets the position of the text and image on the control relative to one another.
        /// The default is <see cref="System.Windows.Forms.TextImageRelation.ImageBeforeText"/>.
        /// </summary>
        TextImageRelation TextImageRelation { set; get; }

        /// <summary>
        /// Gets or sets the tooltip text to display.
        /// </summary>
        string ToolTipText { get; set; }

        /// <summary>
        /// Gets or sets the width of the control in pixels.
        /// </summary>
        int Width { get; set; }
    }
}