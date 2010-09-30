using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="ToolStrip"/> for displaying the <see cref="Tool"/>s in the <see cref="ToolManager"/>.
    /// </summary>
    public class ToolBar : ToolStrip
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets or sets the global <see cref="ToolBar"/>.
        /// </summary>
        public static ToolBar GlobalToolBar { get; set; }

        /// <summary>
        /// Gets or sets the non-global <see cref="ToolBar"/>.
        /// </summary>
        public static ToolBar NonGlobalToolBar { get; set; }

        public static void AddToToolBar(Tool tool)
        {
            if (!tool.CanShowInToolbar)
                return;

            if (tool.ToolBarControl.IsOnToolBar)
                return;

            var c = TryGetToolStripItem(tool);
            if (c == null)
                return;

            var tb = GetToolBar(tool.ToolBarVisibility);

            Debug.Assert(!tb.Items.Contains(c));

            tb.Items.Add(c);
        }

        public static ToolBar GetToolBar(ToolBarVisibility visibility)
        {
            switch (visibility)
            {
                case ToolBarVisibility.Global:
                    return GlobalToolBar;

                default:
                    return NonGlobalToolBar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBar"/> class.
        /// </summary>
        public ToolBar()
        {
            AllowItemReorder = true;
        }

        /// <summary>
        /// Creates the <see cref="IToolBarControl"/> for a <see cref="Tool"/>.
        /// </summary>
        /// <param name="tool">The <see cref="Tool"/> to create the control for.</param>
        /// <param name="controlType">The type of control.</param>
        /// <returns>The <see cref="IToolBarControl"/> for the <paramref name="tool"/> using the given
        /// <paramref name="controlType"/>, or null if the <paramref name="controlType"/> is
        /// <see cref="ToolBarControlType.None"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="controlType"/> does not contain a defined value of the
        /// <see cref="ToolBarControlType"/> enum.</exception>
        public static IToolBarControl CreateToolControl(Tool tool, ToolBarControlType controlType)
        {
            if (tool == null)
                throw new ArgumentNullException("tool");
            if (!EnumHelper<ToolBarControlType>.IsDefined(controlType))
                throw new ArgumentOutOfRangeException("controlType");

            // Create the control
            ToolStripItem c;
            switch (controlType)
            {
                case ToolBarControlType.None:
                    return null;

                case ToolBarControlType.Button:
                    c = new ToolBarItemButton(tool);
                    break;

                case ToolBarControlType.Label:
                    c = new ToolBarItemLabel(tool);
                    break;

                default:
                    const string errmsg = "ToolBarControlType `{0}` is not supported - could not create control for tool `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, controlType, tool);
                    Debug.Fail(string.Format(errmsg, controlType, tool));
                    return null;
            }

            return (IToolBarControl)c;
        }

        /// <summary>
        /// Tries to get the <see cref="ToolStripItem"/> for a <see cref="Tool"/>.
        /// </summary>
        /// <param name="tool">The tool to get the <see cref="ToolStripItem"/> for.</param>
        /// <returns>The <see cref="ToolStripItem"/> for the <paramref name="tool"/>, or null if unable to get it.</returns>
        protected static ToolStripItem TryGetToolStripItem(Tool tool)
        {
            if (tool == null)
            {
                const string errmsg = "Tool is null.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg);
                Debug.Fail(errmsg);
                return null;
            }

            if (tool.ToolBarControl == null)
            {
                const string errmsg = "Tool `{0}` has null ToolBarControl.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, tool);
                Debug.Fail(string.Format(errmsg, tool));
                return null;
            }

            if (!(tool.ToolBarControl is ToolStripItem))
            {
                const string errmsg = "Tool `{0}` ToolBarControl `{1}` is not of the expected type ToolStripItem (is type: `{2}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, tool, tool.ToolBarControl, tool.ToolBarControl.GetType());
                Debug.Fail(string.Format(errmsg, tool, tool.ToolBarControl, tool.ToolBarControl.GetType()));
                return null;
            }

            return tool.ToolBarControl as ToolStripItem;
        }

        /// <summary>
        /// A <see cref="ToolStripItem"/> for a <see cref="IToolBarControl"/> of type <see cref="ToolBarControlType.Button"/>.
        /// </summary>
        internal sealed class ToolBarItemButton : ToolStripButton, IToolBarControl, IToolBarButtonSettings
        {
            readonly Tool _tool;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarItemButton"/> class.
            /// </summary>
            /// <param name="tool">The <see cref="Editor.Tool"/> the control is for.</param>
            /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
            public ToolBarItemButton(Tool tool)
            {
                if (tool == null)
                    throw new ArgumentNullException("tool");

                _tool = tool;

                Initialize();
            }

            /// <summary>
            /// Initializes the control with the initial values.
            /// </summary>
            void Initialize()
            {
                Text = Tool.Name;
                Name = Tool.Name;
            }

            /// <summary>
            /// Raises the <see cref="E:System.Windows.Forms.ToolStripItem.OwnerChanged"/> event.
            /// </summary>
            /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
            protected override void OnOwnerChanged(EventArgs e)
            {
                base.OnOwnerChanged(e);

                // If we changed to something that is not a ToolBar, get it out of there!
                if (!(Owner is ToolBar))
                {
                    const string errmsg = "Attempted to add ToolBar item `{0}` to regular ToolStrip `{1}`!";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, Owner);
                    Debug.Fail(string.Format(errmsg, this, Owner));

                    Owner.Items.Remove(this);
                }
            }

            #region IToolBarControl Members

            /// <summary>
            /// Gets the <see cref="IToolBarControlSettings"/> for this control. Can be safely up-casted to the appropriate
            /// interface for a more specific type using the <see cref="IToolBarControl.ControlType"/> property.
            /// </summary>
            /// <example>
            /// When <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.Button"/>, the
            /// <see cref="IToolBarControl.ControlSettings"/> can be up-casted to <see cref="IToolBarButtonSettings"/>.
            /// </example>
            public IToolBarControlSettings ControlSettings
            {
                get { return this; }
            }

            /// <summary>
            /// Gets if this control is currently on a <see cref="ToolBar"/>.
            /// </summary>
            public bool IsOnToolBar
            {
                get {
                    Debug.Assert(Owner == null || Owner is ToolBar);
                    return (Owner as ToolBar) != null; }
            }

            /// <summary>
            /// Gets the <see cref="ToolBarControlType"/> that describes the type of this control.
            /// </summary>
            public ToolBarControlType ControlType
            {
                get { return ToolBarControlType.Button; }
            }

            /// <summary>
            /// Gets the <see cref="Editor.Tool"/> for this item.
            /// </summary>
            public Tool Tool
            {
                get { return _tool; }
            }

            #endregion
        }

        /// <summary>
        /// A <see cref="ToolStripItem"/> for a <see cref="IToolBarControl"/> of type <see cref="ToolBarControlType.Label"/>.
        /// </summary>
        internal sealed class ToolBarItemLabel : ToolStripLabel, IToolBarControl, IToolBarLabelSettings
        {
            readonly Tool _tool;

            /// <summary>
            /// Gets if this control is currently on a <see cref="ToolBar"/>.
            /// </summary>
            public bool IsOnToolBar
            {
                get
                {
                    Debug.Assert(Owner == null || Owner is ToolBar);
                    return (Owner as ToolBar) != null;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarItemLabel"/> class.
            /// </summary>
            /// <param name="tool">The <see cref="Editor.Tool"/> the control is for.</param>
            /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
            public ToolBarItemLabel(Tool tool)
            {
                if (tool == null)
                    throw new ArgumentNullException("tool");

                _tool = tool;

                Initialize();
            }

            /// <summary>
            /// Initializes the control with the initial values.
            /// </summary>
            void Initialize()
            {
                Text = Tool.Name;
                Name = Tool.Name;
            }

            #region IToolBarControl Members

            /// <summary>
            /// Gets the <see cref="IToolBarControlSettings"/> for this control. Can be safely up-casted to the appropriate
            /// interface for a more specific type using the <see cref="IToolBarControl.ControlType"/> property.
            /// </summary>
            /// <example>
            /// When <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.Button"/>, the
            /// <see cref="IToolBarControl.ControlSettings"/> can be up-casted to <see cref="IToolBarButtonSettings"/>.
            /// </example>
            public IToolBarControlSettings ControlSettings
            {
                get { return this; }
            }

            /// <summary>
            /// Gets the <see cref="ToolBarControlType"/> that describes the type of this control.
            /// </summary>
            public ToolBarControlType ControlType
            {
                get { return ToolBarControlType.Button; }
            }

            /// <summary>
            /// Gets the <see cref="Editor.Tool"/> for this item.
            /// </summary>
            public Tool Tool
            {
                get { return _tool; }
            }

            #endregion
        }
    }
}