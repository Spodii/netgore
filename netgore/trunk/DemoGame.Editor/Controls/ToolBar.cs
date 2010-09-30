using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="ToolStrip"/> for displaying the <see cref="ToolBase"/>s in the <see cref="ToolManager"/>.
    /// </summary>
    public class ToolBar : ToolStrip
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly List<ToolBase> _tools = new List<ToolBase>();

        ToolManager _toolManager;

        /// <summary>
        /// Gets or sets the <see cref="ToolManager"/> to use to get the tools to display on this control.
        /// </summary>
        public ToolManager ToolManager
        {
            get { return _toolManager; }
            set
            {
                if (_toolManager == value)
                    return;

                var oldValue = ToolManager;
                _toolManager = value;

                OnToolManagerChanged(oldValue, value);
            }
        }

        /// <summary>
        /// Creates the <see cref="IToolBarControl"/> for a <see cref="ToolBase"/>.
        /// </summary>
        /// <param name="tool">The <see cref="ToolBase"/> to create the control for.</param>
        /// <param name="controlType">The type of control.</param>
        /// <returns>The <see cref="IToolBarControl"/> for the <paramref name="tool"/> using the given
        /// <paramref name="controlType"/>, or null if the <paramref name="controlType"/> is
        /// <see cref="ToolBarControlType.None"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="controlType"/> does not contain a defined value of the
        /// <see cref="ToolBarControlType"/> enum.</exception>
        public static IToolBarControl CreateToolControl(ToolBase tool, ToolBarControlType controlType)
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

            // Set up the common event hooks
            IToolBarControlManagement.SetEventHooks(tool, true);

            return (IToolBarControl)c;
        }

        protected virtual void OnToolManagerChanged(ToolManager oldValue, ToolManager newValue)
        {
            // Remove the old tools
            _tools.Clear();
            Items.Clear();

            if (oldValue != null)
            {
                foreach (var tool in oldValue.Tools)
                {
                    tool.CanShowInToolbarChanged -= allToolManagerTools_CanShowInToolbarChanged;
                    tool.Disposed -= allToolManagerTools_Disposed;
                    tool.ToolBarPriorityChanged -= allToolManagerTools_ToolBarPriorityChanged;
                }
            }

            // Add the tools for the new ToolManager
            if (newValue != null)
            {
                foreach (var tool in newValue.Tools)
                {
                    UpdateToolBarStatus(tool);

                    tool.CanShowInToolbarChanged += allToolManagerTools_CanShowInToolbarChanged;
                    tool.Disposed += allToolManagerTools_Disposed;
                    tool.ToolBarPriorityChanged += allToolManagerTools_ToolBarPriorityChanged;
                }
            }
        }

        /// <summary>
        /// Re-organizes the <see cref="ToolBar"/>'s items to obey the priority order.
        /// </summary>
        void OrganizeToolBar()
        {
            var isOrganized = true;

            // Sort the list
            _tools.Sort(ToolBarItemSorter);

            // Check if already organized
            if (_tools.Count != Items.Count)
                isOrganized = false;

            if (isOrganized)
            {
                for (var i = 0; i < _tools.Count; i++)
                {
                    if (_tools[i] != ((IToolBarControl)Items[i]).Tool)
                    {
                        isOrganized = false;
                        break;
                    }
                }
            }

            if (isOrganized)
                return;

            // Clear the items and re-add them in the proper order
            Items.Clear();
            var sortedItems = _tools.Select(x => x.ToolBarControl).Cast<ToolStripItem>().ToArray();
            Items.AddRange(sortedItems);
        }

        /// <summary>
        /// Gets if a <see cref="ToolBase"/> should be in the <see cref="ToolBar"/>.
        /// </summary>
        /// <param name="tool">The <see cref="ToolBase"/> to check.</param>
        /// <returns>True if it should be in the <see cref="ToolBar"/>; otherwise false.</returns>
        bool ShouldToolBeInToolBar(ToolBase tool)
        {
            if (tool == null)
                return false;

            if (tool.IsDisposed)
                return false;

            if (!tool.CanShowInToolbar)
                return false;

            if (tool.ToolBarControl == null)
                return false;

            if (tool.ToolManager != ToolManager)
                return false;

            return true;
        }

        /// <summary>
        /// Compares two <see cref="ToolBase"/>s so they can be sorted.
        /// </summary>
        /// <param name="l">The left argument.</param>
        /// <param name="r">The right argument.</param>
        /// <returns>The comparison result.</returns>
        static int ToolBarItemSorter(ToolBase l, ToolBase r)
        {
            return l.ToolBarPriority.CompareTo(r.ToolBarPriority);
        }

        /// <summary>
        /// Tries to get the <see cref="ToolStripItem"/> for a <see cref="ToolBase"/>.
        /// </summary>
        /// <param name="tool">The tool to get the <see cref="ToolStripItem"/> for.</param>
        /// <returns>The <see cref="ToolStripItem"/> for the <paramref name="tool"/>, or null if unable to get it.</returns>
        static ToolStripItem TryGetToolStripItem(ToolBase tool)
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

        void UpdateToolBarStatus(ToolBase tool)
        {
            var add = ShouldToolBeInToolBar(tool);
            var mustReorganize = false;

            if (add)
            {
                // Make sure it is in the toolbar
                if (!_tools.Contains(tool))
                {
                    _tools.Add(tool);
                    mustReorganize = true;
                }
            }
            else
            {
                // Make sure its not in the toolbar
                if (_tools.Contains(tool))
                {
                    _tools.Remove(tool);
                    mustReorganize = true;
                }
            }

            // Re-organize
            if (mustReorganize)
                OrganizeToolBar();
        }

        void allToolManagerTools_CanShowInToolbarChanged(ToolBase sender, bool oldValue, bool newValue)
        {
            UpdateToolBarStatus(sender);
        }

        void allToolManagerTools_Disposed(ToolBase sender)
        {
            UpdateToolBarStatus(sender);
        }

        void allToolManagerTools_ToolBarPriorityChanged(ToolBase sender, int oldValue, int newValue)
        {
            if (_tools.Contains(sender))
                UpdateToolBarStatus(sender);
        }

        /// <summary>
        /// Logical grouping of stuff related to the <see cref="IToolBarControl"/> management and updating a <see cref="ToolBase"/>'s
        /// <see cref="ToolBar"/> control when the <see cref="ToolBase"/> raises certain events.
        /// </summary>
        static class IToolBarControlManagement
        {
            /// <summary>
            /// Sets the event hooks on a <see cref="ToolBase"/>.
            /// </summary>
            /// <param name="tool">The <see cref="ToolBase"/> to set the event hooks on.</param>
            /// <param name="add">True to add the event hooks, or false to remove them.</param>
            internal static void SetEventHooks(ToolBase tool, bool add)
            {
                if (add)
                {
                    // When adding the events, always remove them first just to make sure that we do not add the event listeners
                    // twice. There is no harm in removing an event that is not there, but this is harm if we get invoked twice for
                    // the same event.
                    SetEventHooks(tool, false);

                    // Add the event hooks
                    tool.ToolBarIconChanged += tool_ToolBarIconChanged;
                }
                else
                {
                    // Remove the event hooks
                    tool.ToolBarIconChanged -= tool_ToolBarIconChanged;
                }
            }

            /// <summary>
            /// Handles the <see cref="ToolBase.ToolBarIconChanged"/> event.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="oldValue">The old value.</param>
            /// <param name="newValue">The new value.</param>
            static void tool_ToolBarIconChanged(ToolBase sender, Image oldValue, Image newValue)
            {
                var c = TryGetToolStripItem(sender);
                if (c == null)
                    return;

                c.Image = sender.ToolBarIcon;
            }
        }

        /// <summary>
        /// A <see cref="ToolStripItem"/> for a <see cref="IToolBarControl"/> of type <see cref="ToolBarControlType.Button"/>.
        /// </summary>
        internal sealed class ToolBarItemButton : ToolStripButton, IToolBarControl
        {
            readonly ToolBase _tool;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarItemButton"/> class.
            /// </summary>
            /// <param name="tool">The <see cref="ToolBase"/> the control is for.</param>
            /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
            public ToolBarItemButton(ToolBase tool)
            {
                if (tool == null)
                    throw new ArgumentNullException("tool");

                _tool = tool;

                Initialize();

                _tool.ToolBarIconChanged += _tool_ToolbarIconChanged;
            }

            /// <summary>
            /// Initializes the control with the initial values.
            /// </summary>
            void Initialize()
            {
                Text = Tool.Name;
                Image = Tool.ToolBarIcon;
            }

            void _tool_ToolbarIconChanged(ToolBase sender, Image oldValue, Image newValue)
            {
                Image = newValue;
            }

            #region IToolBarControl Members

            /// <summary>
            /// Gets the <see cref="ToolBarControlType"/> that describes the type of this control.
            /// </summary>
            public ToolBarControlType ControlType
            {
                get { return ToolBarControlType.Button; }
            }

            /// <summary>
            /// Gets the <see cref="ToolBase"/> for this item.
            /// </summary>
            public ToolBase Tool
            {
                get { return _tool; }
            }

            #endregion
        }

        /// <summary>
        /// A <see cref="ToolStripItem"/> for a <see cref="IToolBarControl"/> of type <see cref="ToolBarControlType.Label"/>.
        /// </summary>
        internal sealed class ToolBarItemLabel : ToolStripLabel, IToolBarControl
        {
            readonly ToolBase _tool;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarItemLabel"/> class.
            /// </summary>
            /// <param name="tool">The <see cref="ToolBase"/> the control is for.</param>
            /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
            public ToolBarItemLabel(ToolBase tool)
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
            }

            #region IToolBarControl Members

            /// <summary>
            /// Gets the <see cref="ToolBarControlType"/> that describes the type of this control.
            /// </summary>
            public ToolBarControlType ControlType
            {
                get { return ToolBarControlType.Button; }
            }

            /// <summary>
            /// Gets the <see cref="ToolBase"/> for this item.
            /// </summary>
            public ToolBase Tool
            {
                get { return _tool; }
            }

            #endregion
        }
    }
}