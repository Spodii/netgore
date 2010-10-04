using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetGore;
using NetGore.EditorTools;

namespace DemoGame.Editor
{
    public abstract class Tool : ToolBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        /// <param name="name">The name of the tool.</param>
        /// <param name="toolBarControlType">The <see cref="ToolBarControlType"/> to use for displaying this <see cref="ToolBase"/>
        /// in a toolbar.</param>
        /// <param name="toolBarVisibility">The visibility of this <see cref="ToolBase"/> in a <see cref="ToolBar"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="toolManager"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="toolBarControlType"/> does not contain a defined value of the
        /// <see cref="ToolBarControlType"/> enum.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="toolBarVisibility"/> does not contain a defined value of the
        /// <see cref="ToolBarVisibility"/> enum.</exception>
        protected Tool(ToolManager toolManager, string name, ToolBarControlType toolBarControlType, ToolBarVisibility toolBarVisibility) : base(toolManager, name, toolBarControlType, toolBarVisibility)
        {
        }

        /// <summary>
        /// Sets this <see cref="ToolBase"/> up to be updated every game tick.
        /// </summary>
        protected override void AddUpdateHook()
        {
            GlobalState.Instance.Tick += tickCallback;
        }

        /// <summary>
        /// Removes this <see cref="ToolBase"/> from being updated every game tick.
        /// </summary>
        protected override void RemoveUpdateHook()
        {
            GlobalState.Instance.Tick -= tickCallback;
        }

        /// <summary>
        /// Handles the <see cref="GlobalState.Tick"/> event.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void tickCallback(TickCount currentTime)
        {
            Debug.Assert(IsEnabled);

            HandleUpdate(currentTime);
        }
    }
}
