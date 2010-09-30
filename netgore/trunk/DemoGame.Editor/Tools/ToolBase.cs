using System;
using System.Drawing;
using System.Linq;

namespace DemoGame.Editor.AddOns
{
    /// <summary>
    /// The base class for tools in the editor.
    /// </summary>
    public abstract class ToolBase : IDisposable
    {
        readonly string _name;
        readonly ToolManager _toolManager;
        bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBase"/> class.
        /// </summary>
        /// <param name="name">The name of the tool.</param>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="toolManager"/> is null.</exception>
        protected ToolBase(string name, ToolManager toolManager)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (toolManager == null)
                throw new ArgumentNullException("toolManager");

            _name = name;
            _toolManager = toolManager;
        }

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the <see cref="ToolManager"/> that this tool is in.
        /// </summary>
        public ToolManager ToolManager
        {
            get { return _toolManager; }
        }

        /// <summary>
        /// When overridden in the derived class, handles disposing this object.
        /// </summary>
        /// <param name="disposeManaged">When true, <see cref="IDisposable.Dispose"/> was called directly and managed resources need to be
        /// disposed. When false, this object was garbage collected and managed resources should not be disposed.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ToolBase"/> is reclaimed by garbage collection.
        /// </summary>
        ~ToolBase()
        {
            _isDisposed = true;
            Dispose(false);
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="Image"/> to display for this <see cref="ToolBase"/> in
        /// the toolbar.
        /// </summary>
        /// <returns>The <see cref="Image"/> to display in the toolbar, or null if it cannot be added to the toolbar.</returns>
        protected virtual Image GetToolbarIcon()
        {
            return null;
        }

        /// <summary>
        /// When overridden in the derived class, handles when the tool is clicked in the toolbar.
        /// </summary>
        protected virtual void OnToolbarClicked()
        {
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            GC.SuppressFinalize(this);
            _isDisposed = true;

            Dispose(true);
        }

        #endregion
    }
}