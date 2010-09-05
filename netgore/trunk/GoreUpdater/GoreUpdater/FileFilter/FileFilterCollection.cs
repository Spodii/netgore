using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;

namespace GoreUpdater
{
    /// <summary>
    /// Implementation of the <see cref="IFileFilterCollection"/> that checks if a file path matches
    /// any of multiple <see cref="IFileFilter"/>s.
    /// This class is not thread-safe.
    /// </summary>
    public class FileFilterCollection : IFileFilterCollection
    {
        readonly List<IFileFilter> _filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFilterCollection"/> class.
        /// </summary>
        public FileFilterCollection()
        {
            _filters = new List<IFileFilter>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFilterCollection"/> class.
        /// </summary>
        /// <param name="filters">The initial <see cref="IFileFilter"/>s.</param>
        public FileFilterCollection(IEnumerable<IFileFilter> filters)
        {
            _filters = new List<IFileFilter>(filters);
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IFileFilter> GetEnumerator()
        {
            return _filters.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<IFileFilter>

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return _filters.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        bool ICollection<IFileFilter>.IsReadOnly
        {
            get { return false; }
        }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(IFileFilter item)
        {
            if (!_filters.Contains(item))
            {
                if (log.IsDebugEnabled)
                    log.DebugFormat("Adding IFileFilter `{0}` to `{1}` failed: object already in collection.", item, this);

                _filters.Add(item);
            }
            else
            {
                if (log.IsDebugEnabled)
                    log.DebugFormat("Added IFileFilter `{0}` to `{1}`.", item, this);
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Clearing `{0}`.", this);

            _filters.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(IFileFilter item)
        {
            return _filters.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>,
        /// starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/>
        /// to the end of the destination <paramref name="array"/>.-or-Type <see cref="IFileFilter"/> cannot be cast automatically to the type
        /// of the destination <paramref name="array"/>.</exception>
        public void CopyTo(IFileFilter[] array, int arrayIndex)
        {
            _filters.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Checks if any of the filters in this collection match the given file path.
        /// </summary>
        /// <param name="filePath">The file path to test.</param>
        /// <returns>
        /// True if any of the filters match the <paramref name="filePath"/>; otherwise false.
        /// </returns>
        public bool IsMatch(string filePath)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Performing IsMatch on `{0}` for filePath `{1}`.", this, filePath);

            return _filters.Any(x => x.IsMatch(filePath));
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>;
        /// otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(IFileFilter item)
        {
            return _filters.Remove(item);
        }

        #endregion
    }
}