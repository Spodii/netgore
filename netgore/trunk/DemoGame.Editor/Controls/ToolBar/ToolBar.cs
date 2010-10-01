using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        static readonly Dictionary<ToolBarVisibility, ToolBar> _toolBars =
            new Dictionary<ToolBarVisibility, ToolBar>(EnumComparer<ToolBarVisibility>.Instance);

        static ToolBarVisibility _currentToolBarVisibility = ToolBarVisibility.Global;

        bool _hasVisibilityBeenSet;
        ToolBarVisibility _visibility;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBar"/> class.
        /// </summary>
        public ToolBar()
        {
            AllowItemReorder = true;
        }

        /// <summary>
        /// Gets or sets the current <see cref="ToolBarVisibility"/>.
        /// </summary>
        public static ToolBarVisibility CurrentToolBarVisibility
        {
            get { return _currentToolBarVisibility; }
            set
            {
                if (_currentToolBarVisibility == value)
                    return;

                _currentToolBarVisibility = value;

                // Update the visibility of all ToolBars
                foreach (var tb in _toolBars)
                {
                    if (tb.Key == _currentToolBarVisibility)
                        tb.Value.Visible = true;
                    else
                        tb.Value.Visible = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the visibility of this <see cref="ToolBar"/>. This value should NOT be changed after it is set!
        /// </summary>
        [Browsable(true)]
        [Description("The ToolBarVisibility handled by this ToolBar.")]
        [DefaultValue(ToolBarVisibility.None)]
        public ToolBarVisibility ToolBarVisibility
        {
            get { return _visibility; }
            set
            {
                var oldValue = _visibility;
                _visibility = value;

                // Check if we already set this value
                if (_hasVisibilityBeenSet)
                    Debug.Fail("You already set the visibility for this ToolBar!");

                _hasVisibilityBeenSet = true;

                // Remove from the old key
                if (oldValue != ToolBarVisibility.None)
                {
                    if (_toolBars.ContainsKey(oldValue) && _toolBars[oldValue] == this)
                        _toolBars.Remove(oldValue);
                }

                // Add to the new key
                if (value != ToolBarVisibility.None)
                {
                    if (!_toolBars.ContainsKey(value))
                    {
                        _toolBars.Add(value, this);
                    }
                    else
                    {
                        const string errmsg = "Setting ToolBar `{0}` as the ToolBar with visibility `{1}`, though ToolBar `{2}` was already there." +
                            " Make sure you do not have multiple ToolBars with the same ToolBarVisibility.";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, this, value, _toolBars[value]);
                        Debug.Fail(string.Format(errmsg, this, value, _toolBars[value]));

                        _toolBars[value] = this;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a <see cref="Tool"/> go its <see cref="ToolBar"/>.
        /// </summary>
        /// <param name="tool">The <see cref="Tool"/> to add to its <see cref="ToolBar"/>.</param>
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
            if (tb == null)
                return;

            Debug.Assert(!tb.Items.Contains(c));

            tb.Items.Add(c);

            Debug.Assert(tb.Items.Contains(c));
        }

        /// <summary>
        /// Removes a <see cref="Tool"/> from its <see cref="ToolBar"/>.
        /// </summary>
        /// <param name="tool">The <see cref="Tool"/> to remove from its <see cref="ToolBar"/>.</param>
        public static void RemoveFromToolBar(Tool tool)
        {
            if (!tool.ToolBarControl.IsOnToolBar)
                return;

            var c = TryGetToolStripItem(tool);
            if (c == null)
                return;

            var tb = GetToolBar(tool.ToolBarVisibility);
            if (tb == null)
                return;

            Debug.Assert(tb.Items.Contains(c));

            tb.Items.Remove(c);

            Debug.Assert(!tb.Items.Contains(c));
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
        /// Gets the <see cref="ToolBar"/> for a given <see cref="ToolBarVisibility"/> level.
        /// </summary>
        /// <param name="visibility">The <see cref="ToolBarVisibility"/> of the <see cref="ToolBar"/> to get.</param>
        /// <returns>The <see cref="ToolBar"/> for the given <paramref name="visibility"/>, or null if none exists
        /// for the given <paramref name="visibility"/>.</returns>
        public static ToolBar GetToolBar(ToolBarVisibility visibility)
        {
            // Make sure its a legal value
            if (visibility == ToolBarVisibility.None || !EnumHelper<ToolBarVisibility>.IsDefined(visibility))
            {
                const string errmsg = "Invalid ToolBarVisibility value `{0}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, visibility);
                Debug.Fail(string.Format(errmsg, visibility));
                return null;
            }

            // Try to get the value
            ToolBar ret;
            if (!_toolBars.TryGetValue(visibility, out ret))
            {
                const string errmsg = "No ToolBar found for ToolBarVisibility `{0}`. Did you forget to create a ToolBar for that visibility?";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, visibility);
                Debug.Fail(string.Format(errmsg, visibility));
                return null;
            }

            return ret;
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
            /// Gets the <see cref="ToolBarControlType"/> that describes the type of this control.
            /// </summary>
            public ToolBarControlType ControlType
            {
                get { return ToolBarControlType.Button; }
            }

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