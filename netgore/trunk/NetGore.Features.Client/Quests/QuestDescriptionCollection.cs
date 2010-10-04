using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// A collection of <see cref="IQuestDescription"/>s.
    /// </summary>
    public sealed class QuestDescriptionCollection : IQuestDescriptionCollection
    {
        const string _questDescriptionsNodeName = "QuestDescriptions";
        const string _rootFileNodeName = "QuestData";

        static readonly ThreadSafeHashCache<ContentPaths, QuestDescriptionCollection> _instanceCache =
            new ThreadSafeHashCache<ContentPaths, QuestDescriptionCollection>(x => new QuestDescriptionCollection(x));

        readonly ContentPaths _contentPath;
        readonly DArray<IQuestDescription> _questDescriptions = new DArray<IQuestDescription>(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestDescriptionCollection"/> class.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to use to get the file path.</param>
        QuestDescriptionCollection(ContentPaths contentPath)
        {
            _contentPath = contentPath;
            Load(contentPath);
        }

        /// <summary>
        /// Creates a <see cref="QuestDescriptionCollection"/>.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to use to get the file path.</param>
        /// <returns>The <see cref="QuestDescriptionCollection"/> for the given <paramref name="contentPath"/>.</returns>
        public static IQuestDescriptionCollection Create(ContentPaths contentPath)
        {
            return _instanceCache[contentPath];
        }

        /// <summary>
        /// Gets the file path for the quest descriptions file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to use to get the file path.</param>
        /// <returns>The file path for the quest descriptions file.</returns>
        public static string GetFilePath(ContentPaths contentPath)
        {
            return contentPath.Data.Join("questdata" + EngineSettings.DataFileSuffix);
        }

        /// <summary>
        /// Loads the quest descriptions from file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to use to get the file path.</param>
        void Load(ContentPaths contentPath)
        {
            var filePath = GetFilePath(contentPath);
            var reader = XmlValueReader.CreateFromFile(filePath, _rootFileNodeName);
            ReadState(reader);
        }

        /// <summary>
        /// Saves the quest descriptions to file.
        /// </summary>
        public void Save()
        {
            Save(_contentPath);
        }

        /// <summary>
        /// Saves the quest descriptions to file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to use to get the file path.</param>
        public void Save(ContentPaths contentPath)
        {
            var filePath = GetFilePath(contentPath);
            using (var w = XmlValueWriter.Create(filePath, _rootFileNodeName))
            {
                WriteState(w);
            }
        }

        #region IQuestDescriptionCollection Members

        /// <summary>
        /// Gets the <see cref="IQuestDescription"/> for a quest.
        /// </summary>
        /// <param name="questID">The ID of the quest to get the <see cref="IQuestDescription"/> for.</param>
        /// <returns>The <see cref="IQuestDescription"/> for the given <see cref="QuestID"/>.</returns>
        public IQuestDescription this[QuestID questID]
        {
            get
            {
                if (!_questDescriptions.CanGet(questID.GetRawValue()))
                    return null;

                return _questDescriptions[questID.GetRawValue()];
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return _questDescriptions.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        bool ICollection<IQuestDescription>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only.</exception>
        public void Add(IQuestDescription item)
        {
            if (_questDescriptions.CanGet(item.QuestID.GetRawValue()))
                _questDescriptions[item.QuestID.GetRawValue()] = item;
            else
                _questDescriptions.Insert(item.QuestID.GetRawValue(), item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only.</exception>
        public void Clear()
        {
            _questDescriptions.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>;
        /// otherwise, false.
        /// </returns>
        public bool Contains(IQuestDescription item)
        {
            return _questDescriptions.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an
        /// <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of
        /// the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The
        /// <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-
        /// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-
        /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than
        /// the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-
        /// Type T cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(IQuestDescription[] array, int arrayIndex)
        {
            _questDescriptions.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IQuestDescription> GetEnumerator()
        {
            return _questDescriptions.Where(x => x != null).GetEnumerator();
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

        /// <summary>
        /// Gets the <see cref="IQuestDescription"/> for a quest, or an empty description if the quest
        /// description could not be found for the specified <paramref name="questID"/>.
        /// </summary>
        /// <param name="questID">The ID of the quest to get the <see cref="IQuestDescription"/> for.</param>
        /// <returns>The <see cref="IQuestDescription"/> for a quest, or an empty description if the quest
        /// description could not be found for the specified <paramref name="questID"/>.</returns>
        public IQuestDescription GetOrDefault(QuestID questID)
        {
            var ret = this[questID];

            if (ret == null)
            {
                return new QuestDescription
                {
                    Name = "[Unknown Quest: " + questID + "]",
                    Description = "No description for this quest could be found.",
                    QuestID = questID
                };
            }
            else
                return ret;
        }

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            _questDescriptions.Clear();
            var nodes = reader.ReadManyNodes(_questDescriptionsNodeName, r => new QuestDescription(r));
            foreach (var n in nodes)
            {
                _questDescriptions.Insert(n.QuestID.GetRawValue(), n);
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if
        /// <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is
        /// read-only.</exception>
        public bool Remove(IQuestDescription item)
        {
            return _questDescriptions.Remove(item);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            var toWrite = _questDescriptions.Where(x => x != null).ToArray();
            writer.WriteManyNodes(_questDescriptionsNodeName, toWrite, (w, v) => v.WriteState(w));
        }

        #endregion
    }
}