using System.ComponentModel;
using System.Drawing;

namespace DemoGame.Editor
{
    /// <summary>
    /// Contains the settings common for all <see cref="ToolBarControlType"/>s.
    /// </summary>
    public interface IToolBarControlSettings
    {
        /// <summary>
        /// Gets or sets the <see cref="Font"/> of the control's text.
        /// The default is the value of the <see cref="System.Windows.Forms.Control.DefaultFont"/> property.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Image"/> to display on the control.
        /// </summary>
        Image Image { get; set; }

        /// <summary>
        /// Gets or sets how the <see cref="IToolBarControlSettings.Image"/> should be aligned on the control
        /// The default value is <see cref="System.Drawing.ContentAlignment.MiddleLeft"/>.
        /// </summary>
        ContentAlignment ImageAlign {get;set;}

        /// <summary>
        /// Gets or sets the alignment of the text.
        /// The default is <see cref="System.Drawing.ContentAlignment.MiddleRight"/>.
        /// </summary>
        System.Drawing.ContentAlignment TextAlign { set; get; }

        /// <summary>
        /// Gets or sets how the <see cref="IToolBarControlSettings.Image"/> is sized on the control.
        /// The default is <see cref="System.Windows.Forms.ToolStripItemImageScaling.SizeToFit"/>.
        /// </summary>
        System.Windows.Forms.ToolStripItemImageScaling ImageScaling { set; get; }

        /// <summary>
        /// Gets or sets the direction of the text.
        /// </summary>
        System.Windows.Forms.ToolStripTextDirection TextDirection { set; get; }

        /// <summary>
        /// Gets or sets the position of the text and image on the control relative to one another.
        /// The default is <see cref="System.Windows.Forms.TextImageRelation.ImageBeforeText"/>.
        /// </summary>
        System.Windows.Forms.TextImageRelation TextImageRelation { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control is automatically sized.
        /// </summary>
        bool AutoSize { set; get; }

        /// <summary>
        /// Gets the area where content, such as text and icons, can be placed within the control without overwriting background borders.
        /// </summary>
        System.Drawing.Rectangle ContentRectangle { get; }

        /// <summary>
        /// Gets the size and location of the control.
        /// </summary>
        System.Drawing.Rectangle Bounds { get; }

        /// <summary>
        /// Gets if this control is currently selected.
        /// </summary>
        bool Selected { get; }

        /// <summary>
        /// Gets or sets the internal spacing, in pixels, between the item's contents and its edges.
        /// </summary>
        System.Windows.Forms.Padding Padding { set; get; }

        /// <summary>
        /// Gets or sets the size of the control.
        /// </summary>
        System.Drawing.Size Size { set; get; }

        /// <summary>
        /// Gets if this control is currently pressed.
        /// </summary>
        bool Pressed { get; }

        /// <summary>
        /// Gets or sets the tooltip text to display.
        /// </summary>
        string ToolTipText { get; set; }

        /// <summary>
        /// Gets a value indicating whether the control can be selected.
        /// </summary>
        bool CanSelect { get; }

        /// <summary>
        /// Gets or sets whether text and images are displayed.
        /// The default is <see cref="System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText"/>.
        /// </summary>
        System.Windows.Forms.ToolStripItemDisplayStyle DisplayStyle { set; get; }

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
        /// Gets or sets the width of the control in pixels.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Gets or sets if the control is enabled.
        /// The default value is true.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// A <see cref="System.Drawing.Color"/> that represents the background color of the item.
        /// The default is the value of the <see cref="System.Windows.Forms.Control.DefaultBackColor"/> property.
        /// </summary>
        Color BackColor { get; set; }
    }
}