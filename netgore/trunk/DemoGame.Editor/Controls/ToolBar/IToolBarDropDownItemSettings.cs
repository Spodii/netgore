namespace DemoGame.Editor
{
    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.DropDownButton"/>.
    /// </summary>
    public interface IToolBarDropDownItemSettings : IToolBarControlSettings
    {
        /// <summary>
        /// Makes a visible <see cref="System.Windows.Forms.ToolStripDropDown"/> hidden.
        /// </summary>
        void HideDropDown();

        /// <summary>
        /// Displays the <see cref="System.Windows.Forms.ToolStripDropDownItem"/> control associated with this
        /// <see cref="System.Windows.Forms.ToolStripDropDownItem"/>.
        /// </summary>
        void ShowDropDown();

        /// <summary>
        /// Gets or sets the <see cref="System.Windows.Forms.ToolStripDropDown"/> that will be displayed when
        /// this <see cref="System.Windows.Forms.ToolStripDropDownItem"/> is clicked.
        /// </summary>
        System.Windows.Forms.ToolStripDropDown DropDown { set; get; }

        /// <summary>
        /// Gets or sets a value indicating the direction in which the <see cref="System.Windows.Forms.ToolStripDropDownItem"/>
        /// emerges from its parent container.
        /// </summary>
        System.Windows.Forms.ToolStripDropDownDirection DropDownDirection { set; get; }

        /// <summary>
        /// Gets the collection of items in the <see cref="System.Windows.Forms.ToolStripDropDown"/> that is associated
        /// with this <see cref="System.Windows.Forms.ToolStripDropDownItem"/>.
        /// </summary>
        System.Windows.Forms.ToolStripItemCollection DropDownItems { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="System.Windows.Forms.ToolStripDropDownItem"/>
        /// has <see cref="System.Windows.Forms.ToolStripDropDown"/> controls associated with it.
        /// </summary>
        bool HasDropDownItems { get; }

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripDropDown"/> closes.
        /// </summary>
        event System.EventHandler DropDownClosed;

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripDropDown"/> is clicked.
        /// </summary>
        event System.Windows.Forms.ToolStripItemClickedEventHandler DropDownItemClicked;

        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.ToolStripDropDown"/> has opened.
        /// </summary>
        event System.EventHandler DropDownOpened;

        /// <summary>
        /// Occurs as the <see cref="System.Windows.Forms.ToolStripDropDown"/> is opening.
        /// </summary>
        event System.EventHandler DropDownOpening;
    }
}