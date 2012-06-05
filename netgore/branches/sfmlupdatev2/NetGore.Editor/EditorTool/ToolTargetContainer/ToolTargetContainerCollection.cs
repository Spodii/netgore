using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// A collection of <see cref="IToolTargetContainer"/>s.
    /// </summary>
    public class ToolTargetContainerCollection : IEnumerable<IToolTargetContainer>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly List<IToolTargetContainer> _buffer = new List<IToolTargetContainer>();

        /// <summary>
        /// Notifies listeners when a <see cref="IToolTargetContainer"/> is successfully added to this collection.
        /// </summary>
        public event TypedEventHandler<ToolTargetContainerCollection, EventArgs<IToolTargetContainer>> Added;

        /// <summary>
        /// Notifies listeners when a <see cref="IToolTargetContainer"/> is successfully removed from this collection.
        /// </summary>
        public event TypedEventHandler<ToolTargetContainerCollection, EventArgs<IToolTargetContainer>> Removed;

        /// <summary>
        /// Adds a <see cref="IToolTargetContainer"/> to this collection.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to add.</param>
        /// <returns>True if <paramref name="c"/> was added; false if <paramref name="c"/> was invalid or already added.</returns>
        public bool Add(IToolTargetContainer c)
        {
            // Check for a valid value
            if (c == null)
            {
                const string errmsg = "Tried to add null IToolTargetContainer to ToolTargetContainerCollection `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return false;
            }

            // Do not add if already in the collection
            if (_buffer.Contains(c))
                return false;

            // Add
            _buffer.Add(c);

            // Raise the event
            if (Added != null)
                Added.Raise(this, EventArgsHelper.Create(c));

            return true;
        }

        /// <summary>
        /// Removes a <see cref="IToolTargetContainer"/> from this collection.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to add.</param>
        /// <returns>True if <paramref name="c"/> was removed; false if <paramref name="c"/> was null or not in the collection.</returns>
        public bool Remove(IToolTargetContainer c)
        {
            // Check for a valid value
            if (c == null)
            {
                const string errmsg = "Tried to remove null IToolTargetContainer from ToolTargetContainerCollection `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return false;
            }

            // Remove from the collection
            var ret = _buffer.Remove(c);

            // Check if removed successfully
            if (ret)
            {
                // Raise the event
                if (Removed != null)
                    Removed.Raise(this, EventArgsHelper.Create(c));
            }

            return ret;
        }

        #region IEnumerable<IToolTargetContainer> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IToolTargetContainer> GetEnumerator()
        {
            return _buffer.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}