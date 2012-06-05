using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Features.ActionDisplays
{
    public class ActionDisplayCollection : IEnumerable<ActionDisplay>, IPersistable
    {
        const string _fileRootName = "ActionDisplayCollection";
        const string _rootNodeName = "ActionDisplays";

        readonly DArray<ActionDisplay> _items = new DArray<ActionDisplay>(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDisplayCollection"/> class.
        /// </summary>
        public ActionDisplayCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDisplayCollection"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public ActionDisplayCollection(IValueReader reader)
        {
            ((IPersistable)this).ReadState(reader);
        }

        /// <summary>
        /// Gets the <see cref="ActionDisplay"/> with the given <see cref="ActionDisplayID"/>.
        /// </summary>
        /// <param name="id">The <see cref="ActionDisplayID"/> of the <see cref="ActionDisplay"/> to get.</param>
        /// <returns>The <see cref="ActionDisplay"/> at the given <paramref name="id"/>, or null if the <paramref name="id"/>
        /// was invalid or no <see cref="ActionDisplay"/> exists at the given ID.</returns>
        public ActionDisplay this[ActionDisplayID id]
        {
            get
            {
                if (!_items.CanGet((int)id))
                    return null;

                return _items[(int)id];
            }
        }

        /// <summary>
        /// Creates a new <see cref="ActionDisplay"/> in this collection.
        /// </summary>
        /// <returns>The new <see cref="ActionDisplay"/>.</returns>
        public ActionDisplay CreateAction()
        {
            var id = _items.NextFreeIndex();
            var item = new ActionDisplay(new ActionDisplayID(id));
            _items.Insert(id, item);
            return item;
        }

        /// <summary>
        /// Gets the path to the default <see cref="ActionDisplayCollection"/> file.
        /// </summary>
        /// <param name="path">The <see cref="ContentPaths"/> to use.</param>
        /// <returns>The path to the default <see cref="ActionDisplayCollection"/> file.</returns>
        public static string DefaultFilePath(ContentPaths path)
        {
            return path.Data.Join("actiondisplays" + EngineSettings.DataFileSuffix);
        }

        /// <summary>
        /// Reads a <see cref="ActionDisplayCollection"/> from file.
        /// </summary>
        /// <param name="path">The <see cref="ContentPaths"/> to use.</param>
        /// <returns>
        /// The loaded <see cref="ActionDisplayCollection"/>.
        /// </returns>
        public static ActionDisplayCollection Read(ContentPaths path)
        {
            var filePath = DefaultFilePath(path);
            return Read(filePath);
        }

        /// <summary>
        /// Reads a <see cref="ActionDisplayCollection"/> from file. If the file at the given <paramref name="path"/> does not
        /// exist, then an empty <see cref="ActionDisplayCollection"/> will be used.
        /// </summary>
        /// <param name="path">The path of the file to read from.</param>
        /// <returns>The loaded <see cref="ActionDisplayCollection"/>.</returns>
        public static ActionDisplayCollection Read(string path)
        {
            if (!File.Exists(path))
                return new ActionDisplayCollection();

            var reader = XmlValueReader.CreateFromFile(path, _fileRootName);
            return new ActionDisplayCollection(reader);
        }

        /// <summary>
        /// Removes the given <see cref="ActionDisplay"/>.
        /// </summary>
        /// <param name="item">The <see cref="ActionDisplay"/> to remove.</param>
        public void Remove(ActionDisplay item)
        {
            if (item == null)
                return;

            RemoveAt(item.ID);
        }

        /// <summary>
        /// Removes the item at the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the item to remove.</param>
        public void RemoveAt(ActionDisplayID id)
        {
            _items.RemoveAt((int)id);
        }

        /// <summary>
        /// Writes the <see cref="ActionDisplayCollection"/> to file.
        /// </summary>
        /// <param name="path">The path of the file to write to.</param>
        public void Save(string path)
        {
            using (var writer = XmlValueWriter.Create(path, _fileRootName))
            {
                ((IPersistable)this).WriteState(writer);
            }
        }

        /// <summary>
        /// Writes the <see cref="ActionDisplayCollection"/> to the default file path.
        /// </summary>
        /// <param name="path">The <see cref="ContentPaths"/> to use.</param>
        public void Save(ContentPaths path)
        {
            var filePath = DefaultFilePath(path);
            Save(filePath);
        }

        #region IEnumerable<ActionDisplay> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ActionDisplay> GetEnumerator()
        {
            return ((ICollection<ActionDisplay>)_items).GetEnumerator();
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

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        void IPersistable.ReadState(IValueReader reader)
        {
            var items = reader.ReadManyNodes(_rootNodeName, r => new ActionDisplay(r));

            _items.Clear();

            foreach (var item in items)
            {
                Debug.Assert(!_items.CanGet((int)item.ID) || _items[(int)item.ID] == null, "This ID is already in use!");
                _items.Insert((int)item.ID, item);
            }
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        void IPersistable.WriteState(IValueWriter writer)
        {
            var items = _items.Where(x => x != null).ToArray();
            writer.WriteManyNodes(_rootNodeName, items, (w, item) => ((IPersistable)item).WriteState(w));
        }

        #endregion
    }
}