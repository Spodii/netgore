using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;

namespace NetGore.Editor.EditorTool
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
                // Ensure value actually changed
                if (_currentToolBarVisibility == value)
                    return;

                // Ignore when using ToolBarVisibility.None
                if (value == ToolBarVisibility.None)
                    return;

                _currentToolBarVisibility = value;

                // Update the visibility of all ToolBars
                foreach (var tb in _toolBars)
                {
                    if (tb.Key == _currentToolBarVisibility || tb.Key == ToolBarVisibility.Global)
                        tb.Value.Visible = true;
                    else
                        tb.Value.Visible = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the object that is the reason why this <see cref="ToolBar"/> is currently visible. Can be null.
        /// Can also be of any type, though it is highly recommended that a consistent type is used for each
        /// <see cref="ToolBarVisibility"/>.
        /// </summary>
        public object DisplayObject { get; set; }

        /// <summary>
        /// Gets or sets the visibility of this <see cref="ToolBar"/>. This value should NOT be changed after it is set!
        /// If set to <see cref="NetGore.Editor.EditorTool.ToolBarVisibility.None"/>, it won't automatically show any tools,
        /// making it quite useless.
        /// </summary>
        [Browsable(true)]
        [Description("The ToolBarVisibility handled by this ToolBar.")]
        [DefaultValue(ToolBarVisibility.None)]
        public ToolBarVisibility ToolBarVisibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility == value)
                    return;

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
                        _toolBars.Add(value, this);
                    else
                    {
                        const string errmsg =
                            "Setting ToolBar `{0}` as the ToolBar with visibility `{1}`, though ToolBar `{2}` was already there." +
                            " Make sure you do not have multiple ToolBars with the same ToolBarVisibility.";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, this, value, _toolBars[value]);
                        Debug.Fail(string.Format(errmsg, this, value, _toolBars[value]));

                        _toolBars[value] = this;
                    }
                }

                // Set the visibility based on the CurrentToolBarVisibility
                if (value == CurrentToolBarVisibility || value == ToolBarVisibility.Global)
                    Visible = true;
                else
                    Visible = false;
            }
        }

        /// <summary>
        /// Gets the <see cref="ToolBar"/> instances.
        /// </summary>
        public static IEnumerable<ToolBar> ToolBars
        {
            get { return _toolBars.Values; }
        }

        /// <summary>
        /// Adds a <see cref="Tool"/> to its <see cref="ToolBar"/>.
        /// </summary>
        /// <param name="tool">The <see cref="Tool"/> to add to its <see cref="ToolBar"/>.</param>
        public static void AddToToolBar(Tool tool)
        {
            // Make sure the tool can be shown in a toolbar
            if (!tool.CanShowInToolbar)
                return;

            // Don't do anything if already on the toolbar
            if (tool.ToolBarControl.IsOnToolBar)
                return;

            // Get the tool's ToolStripItem
            var c = TryGetToolStripItem(tool);
            if (c == null)
                return;

            // Get the toolbar for the tool
            var tb = GetToolBar(tool.ToolBarVisibility);
            if (tb == null)
                return;

            Debug.Assert(!tb.Items.Contains(c));

            // Add the tool to the ToolBar
            tb.Items.Add(c);

            Debug.Assert(tb.Items.Contains(c));
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

                case ToolBarControlType.ComboBox:
                    c = new ToolBarItemComboBox(tool);
                    break;

                case ToolBarControlType.DropDownButton:
                    c = new ToolBarItemDropDownButton(tool);
                    break;

                case ToolBarControlType.Label:
                    c = new ToolBarItemLabel(tool);
                    break;

                case ToolBarControlType.ProgressBar:
                    c = new ToolBarItemProgressBar(tool);
                    break;

                case ToolBarControlType.SplitButton:
                    c = new ToolBarItemSplitButton(tool);
                    break;

                case ToolBarControlType.TextBox:
                    c = new ToolBarItemTextBox(tool);
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
        /// Gets the <see cref="IToolBarControl"/>s on the <see cref="ToolBar"/>.
        /// </summary>
        /// <returns>The <see cref="IToolBarControl"/>s on the <see cref="ToolBar"/>, ordered by the same order that they appear.
        /// Separators and other non-<see cref="IToolBarControl"/> items are denoted with a null value.</returns>
        public IEnumerable<IToolBarControl> GetItems()
        {
            var ret = new IToolBarControl[Items.Count];

            for (var i = 0; i < ret.Length; i++)
            {
                var item = Items[i];
                ret[i] = item as IToolBarControl;
            }

            return ret;
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
                const string errmsg =
                    "No ToolBar found for ToolBarVisibility `{0}`. Did you forget to create a ToolBar for that visibility?";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, visibility);
                Debug.Fail(string.Format(errmsg, visibility));
                return null;
            }

            return ret;
        }

        /// <summary>
        /// General <see cref="ToolStripItem"/> initialization.
        /// </summary>
        /// <param name="t">The <see cref="Tool"/>.</param>
        /// <param name="c">The <see cref="ToolStripItem"/>.</param>
        static void InitializeGeneral(Tool t, ToolStripItem c)
        {
            c.Text = t.Name;
            c.Name = t.Name;
            c.AutoToolTip = true;
            c.ToolTipText = t.Name;
        }

        /// <summary>
        /// Adds a separator into the <see cref="ToolBar"/>.
        /// </summary>
        /// <param name="index">The 0-based index to insert the separator into.</param>
        /// <returns>The <see cref="ToolStripSeparator"/> that was added.</returns>
        public ToolStripSeparator InsertSeparator(int index)
        {
            var c = new ToolStripSeparator();
            Items.Insert(index, c);
            return c;
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

        static class ToolBarControlHelper
        {
            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the start of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            /// <param name="c">The <see cref="IToolBarControl"/>.</param>
            public static void MoveToHead(ToolStripItem c)
            {
                if (c == null)
                    return;

                var owner = c.Owner;
                if (owner == null)
                    return;

                owner.Items.Insert(0, c);
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the end of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            /// <param name="c">The <see cref="IToolBarControl"/>.</param>
            public static void MoveToTail(ToolStripItem c)
            {
                if (c == null)
                    return;

                var owner = c.Owner;
                if (owner == null)
                    return;

                var i = Math.Max(owner.Items.Count - 1, 0);
                owner.Items.Insert(i, c);
            }
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
            /// <param name="tool">The <see cref="Tool"/> the control is for.</param>
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
                InitializeGeneral(_tool, this);
            }

            /// <summary>
            /// Raises the <see cref="E:System.Windows.Forms.Control.Click"/> event.
            /// </summary>
            /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
            protected override void OnClick(EventArgs e)
            {
                // Toggle IsEnabled when clicked
                if (ClickToEnable)
                    Tool.IsEnabled = !Tool.IsEnabled;

                base.OnClick(e);
            }

            /// <summary>
            /// Raises the <see cref="E:System.Windows.Forms.ToolStripItem.OwnerChanged"/> event.
            /// </summary>
            /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
            protected override void OnOwnerChanged(EventArgs e)
            {
                base.OnOwnerChanged(e);

                // If we changed to something that is not a ToolBar, get it out of there!
                if (Owner != null && !(Owner is ToolBar))
                {
                    const string errmsg = "Attempted to add ToolBar item `{0}` to regular ToolStrip `{1}`!";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, Owner);
                    Debug.Fail(string.Format(errmsg, this, Owner));

                    Owner.Items.Remove(this);
                }
            }

            #region IToolBarButtonSettings Members

            /// <summary>
            /// Gets or sets if the <see cref="Tool"/>'s <see cref="NetGore.Editor.EditorTool.Tool.IsEnabled"/> state
            /// will be toggle by clicking this control. Some types of controls may ignore this value when it makes no logical
            /// sense to behave this way.
            /// </summary>
            /// <value></value>
            public bool ClickToEnable { get; set; }

            #endregion

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
            /// Gets the <see cref="Tool"/> for this item.
            /// </summary>
            public Tool Tool
            {
                get { return _tool; }
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the start of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToHead()
            {
                ToolBarControlHelper.MoveToHead(this);
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the end of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToTail()
            {
                ToolBarControlHelper.MoveToTail(this);
            }

            #endregion
        }

        /// <summary>
        /// A <see cref="ToolStripItem"/> for a <see cref="IToolBarControl"/> of type <see cref="ToolBarControlType.ComboBox"/>.
        /// </summary>
        internal sealed class ToolBarItemComboBox : ToolStripComboBox, IToolBarControl, IToolBarComboBoxSettings
        {
            readonly Tool _tool;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarItemComboBox"/> class.
            /// </summary>
            /// <param name="tool">The <see cref="Tool"/> the control is for.</param>
            /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
            public ToolBarItemComboBox(Tool tool)
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
                InitializeGeneral(_tool, this);
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
            /// When <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.ComboBox"/>, the
            /// <see cref="IToolBarControl.ControlSettings"/> can be up-casted to <see cref="IToolBarComboBoxSettings"/>.
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
                get { return ToolBarControlType.ComboBox; }
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
            /// Gets the <see cref="Tool"/> for this item.
            /// </summary>
            public Tool Tool
            {
                get { return _tool; }
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the start of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToHead()
            {
                ToolBarControlHelper.MoveToHead(this);
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the end of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToTail()
            {
                ToolBarControlHelper.MoveToTail(this);
            }

            #endregion
        }

        /// <summary>
        /// A <see cref="ToolStripItem"/> for a <see cref="IToolBarControl"/> of type <see cref="ToolBarControlType.DropDownButton"/>.
        /// </summary>
        internal sealed class ToolBarItemDropDownButton : ToolStripDropDownButton, IToolBarControl, IToolBarDropDownButtonSettings
        {
            readonly Tool _tool;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarItemDropDownButton"/> class.
            /// </summary>
            /// <param name="tool">The <see cref="Tool"/> the control is for.</param>
            /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
            public ToolBarItemDropDownButton(Tool tool)
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
                InitializeGeneral(_tool, this);
            }

            /// <summary>
            /// Raises the <see cref="E:System.Windows.Forms.ToolStripItem.OwnerChanged"/> event.
            /// </summary>
            /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
            protected override void OnOwnerChanged(EventArgs e)
            {
                base.OnOwnerChanged(e);

                // If we changed to something that is not a ToolBar, get it out of there!
                if (Owner != null && !(Owner is ToolBar))
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
            /// When <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.DropDownButton"/>, the
            /// <see cref="IToolBarControl.ControlSettings"/> can be up-casted to <see cref="IToolBarDropDownButtonSettings"/>.
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
                get { return ToolBarControlType.DropDownButton; }
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
            /// Gets the <see cref="Tool"/> for this item.
            /// </summary>
            public Tool Tool
            {
                get { return _tool; }
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the start of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToHead()
            {
                ToolBarControlHelper.MoveToHead(this);
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the end of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToTail()
            {
                ToolBarControlHelper.MoveToTail(this);
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
            /// <param name="tool">The <see cref="Tool"/> the control is for.</param>
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
                InitializeGeneral(_tool, this);
            }

            /// <summary>
            /// Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click"/> event.
            /// </summary>
            /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
            protected override void OnClick(EventArgs e)
            {
                // Toggle IsEnabled when clicked
                if (ClickToEnable)
                    Tool.IsEnabled = !Tool.IsEnabled;

                base.OnClick(e);
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
            /// Gets the <see cref="Tool"/> for this item.
            /// </summary>
            public Tool Tool
            {
                get { return _tool; }
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the start of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToHead()
            {
                ToolBarControlHelper.MoveToHead(this);
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the end of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToTail()
            {
                ToolBarControlHelper.MoveToTail(this);
            }

            #endregion

            #region IToolBarLabelSettings Members

            /// <summary>
            /// Gets or sets if the <see cref="Tool"/>'s <see cref="NetGore.Editor.EditorTool.Tool.IsEnabled"/> state
            /// will be toggle by clicking this control. Some types of controls may ignore this value when it makes no logical
            /// sense to behave this way.
            /// </summary>
            public bool ClickToEnable { get; set; }

            #endregion
        }

        /// <summary>
        /// A <see cref="ToolStripItem"/> for a <see cref="IToolBarControl"/> of type <see cref="ToolBarControlType.ProgressBar"/>.
        /// </summary>
        internal sealed class ToolBarItemProgressBar : ToolStripProgressBar, IToolBarControl, IToolBarProgressBarSettings
        {
            readonly Tool _tool;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarItemProgressBar"/> class.
            /// </summary>
            /// <param name="tool">The <see cref="Tool"/> the control is for.</param>
            /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
            public ToolBarItemProgressBar(Tool tool)
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
                InitializeGeneral(_tool, this);
            }

            /// <summary>
            /// Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click"/> event.
            /// </summary>
            /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
            protected override void OnClick(EventArgs e)
            {
                // Toggle IsEnabled when clicked
                if (ClickToEnable)
                    Tool.IsEnabled = !Tool.IsEnabled;

                base.OnClick(e);
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
            /// When <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.ProgressBar"/>, the
            /// <see cref="IToolBarControl.ControlSettings"/> can be up-casted to <see cref="IToolBarProgressBarSettings"/>.
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
                get { return ToolBarControlType.ProgressBar; }
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
            /// Gets the <see cref="Tool"/> for this item.
            /// </summary>
            public Tool Tool
            {
                get { return _tool; }
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the start of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToHead()
            {
                ToolBarControlHelper.MoveToHead(this);
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the end of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToTail()
            {
                ToolBarControlHelper.MoveToTail(this);
            }

            #endregion

            #region IToolBarProgressBarSettings Members

            /// <summary>
            /// Gets or sets if the <see cref="Tool"/>'s <see cref="NetGore.Editor.EditorTool.Tool.IsEnabled"/> state
            /// will be toggle by clicking this control.
            /// </summary>
            public bool ClickToEnable { get; set; }

            #endregion
        }

        /// <summary>
        /// A <see cref="ToolStripItem"/> for a <see cref="IToolBarControl"/> of type <see cref="ToolBarControlType.SplitButton"/>.
        /// </summary>
        internal sealed class ToolBarItemSplitButton : ToolStripSplitButton, IToolBarControl, IToolBarSplitButtonSettings
        {
            readonly Tool _tool;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarItemSplitButton"/> class.
            /// </summary>
            /// <param name="tool">The <see cref="Tool"/> the control is for.</param>
            /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
            public ToolBarItemSplitButton(Tool tool)
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
                InitializeGeneral(_tool, this);
            }

            /// <summary>
            /// Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click"/> event.
            /// </summary>
            /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
            protected override void OnClick(EventArgs e)
            {
                // Toggle IsEnabled when clicked
                if (ClickToEnable && ButtonPressed)
                    Tool.IsEnabled = !Tool.IsEnabled;

                base.OnClick(e);
            }

            /// <summary>
            /// Raises the <see cref="E:System.Windows.Forms.ToolStripItem.OwnerChanged"/> event.
            /// </summary>
            /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
            protected override void OnOwnerChanged(EventArgs e)
            {
                base.OnOwnerChanged(e);

                // If we changed to something that is not a ToolBar, get it out of there!
                if (Owner != null && !(Owner is ToolBar))
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
            /// When <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.SplitButton"/>, the
            /// <see cref="IToolBarControl.ControlSettings"/> can be up-casted to <see cref="IToolBarSplitButtonSettings"/>.
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
                get { return ToolBarControlType.SplitButton; }
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
            /// Gets the <see cref="Tool"/> for this item.
            /// </summary>
            public Tool Tool
            {
                get { return _tool; }
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the start of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToHead()
            {
                ToolBarControlHelper.MoveToHead(this);
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the end of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToTail()
            {
                ToolBarControlHelper.MoveToTail(this);
            }

            #endregion

            #region IToolBarSplitButtonSettings Members

            /// <summary>
            /// Gets or sets if the <see cref="Tool"/>'s <see cref="NetGore.Editor.EditorTool.Tool.IsEnabled"/> state
            /// will be toggle by clicking this control.
            /// </summary>
            public bool ClickToEnable { get; set; }

            #endregion
        }

        /// <summary>
        /// A <see cref="ToolStripItem"/> for a <see cref="IToolBarControl"/> of type <see cref="ToolBarControlType.TextBox"/>.
        /// </summary>
        internal sealed class ToolBarItemTextBox : ToolStripTextBox, IToolBarControl, IToolBarTextBoxSettings
        {
            readonly Tool _tool;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarItemTextBox"/> class.
            /// </summary>
            /// <param name="tool">The <see cref="Tool"/> the control is for.</param>
            /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
            public ToolBarItemTextBox(Tool tool)
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
                InitializeGeneral(_tool, this);
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
            /// When <see cref="IToolBarControl.ControlType"/> equals <see cref="ToolBarControlType.TextBox"/>, the
            /// <see cref="IToolBarControl.ControlSettings"/> can be up-casted to <see cref="IToolBarTextBoxSettings"/>.
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
                get { return ToolBarControlType.TextBox; }
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
            /// Gets the <see cref="Tool"/> for this item.
            /// </summary>
            public Tool Tool
            {
                get { return _tool; }
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the start of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToHead()
            {
                ToolBarControlHelper.MoveToHead(this);
            }

            /// <summary>
            /// Moves the <see cref="IToolBarControl"/> to the end of the <see cref="ToolBar"/>. Only valid when
            /// <see cref="IToolBarControl.IsOnToolBar"/> is set.
            /// </summary>
            public void MoveToTail()
            {
                ToolBarControlHelper.MoveToTail(this);
            }

            #endregion
        }
    }
}