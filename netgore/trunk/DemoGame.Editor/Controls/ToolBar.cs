using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        protected virtual void OnToolManagerChanged(ToolManager oldValue, ToolManager newValue)
        {
            // TODO: !!
            // Remove tools from the old ToolManager
            if (oldValue != null)
            {
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

                default :
                    const string errmsg = "ToolBarControlType `{0}` is not supported - could not create control for tool `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, controlType, tool);
                    Debug.Fail(string.Format(errmsg, controlType, tool));
                    return null;
            }

            // Set up the common event hooks
            SetEventHooks(tool, true);

            return (IToolBarControl)c;
        }

        /// <summary>
        /// Sets the event hooks on a <see cref="ToolBase"/>.
        /// </summary>
        /// <param name="tool">The <see cref="ToolBase"/> to set the event hooks on.</param>
        /// <param name="add">True to add the event hooks, or false to remove them.</param>
        static void SetEventHooks(ToolBase tool, bool add)
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

        /// <summary>
        /// Handles the <see cref="ToolBase.ToolBarIconChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void tool_ToolBarIconChanged(ToolBase sender, System.Drawing.Image oldValue, System.Drawing.Image newValue)
        {
            var c = TryGetToolStripItem(sender);
            if (c == null)
                return;

            c.Image = sender.ToolBarIcon;
        }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A <see cref="ToolStripItem"/> for a <see cref="IToolBarControl"/> of type <see cref="ToolBarControlType.Label"/>.
        /// </summary>
        sealed class ToolBarItemLabel : ToolStripLabel, IToolBarControl
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
            /// Gets the <see cref="ToolBase"/> for this item.
            /// </summary>
            public ToolBase Tool
            {
                get { return _tool; }
            }

            /// <summary>
            /// Gets the <see cref="ToolBarControlType"/> that describes the type of this control.
            /// </summary>
            public ToolBarControlType ControlType
            {
                get { return ToolBarControlType.Button; }
            }

            /// <summary>
            /// Initializes the control with the initial values.
            /// </summary>
            void Initialize()
            {
                Text = Tool.Name;
            }
        }

        /// <summary>
        /// A <see cref="ToolStripItem"/> for a <see cref="IToolBarControl"/> of type <see cref="ToolBarControlType.Button"/>.
        /// </summary>
        sealed class ToolBarItemButton : ToolStripButton, IToolBarControl
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

            void _tool_ToolbarIconChanged(ToolBase sender, System.Drawing.Image oldValue, System.Drawing.Image newValue)
            {
                Image = newValue;
            }

            /// <summary>
            /// Gets the <see cref="ToolBase"/> for this item.
            /// </summary>
            public ToolBase Tool
            {
                get { return _tool; }
            }

            /// <summary>
            /// Gets the <see cref="ToolBarControlType"/> that describes the type of this control.
            /// </summary>
            public ToolBarControlType ControlType
            {
                get { return ToolBarControlType.Button; }
            }

            /// <summary>
            /// Initializes the control with the initial values.
            /// </summary>
            void Initialize()
            {
                Text = Tool.Name;
                Image = Tool.ToolBarIcon;
            }
        }
    }
}