using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A collection of <see cref="ParticleModifierBase"/>s.
    /// </summary>
    public class ParticleModifierCollection : IList<ParticleModifierBase>
    {
        /// <summary>
        /// Creates a deep copy of the <see cref="ParticleModifierCollection"/>.
        /// </summary>
        /// <returns>A deep copy of the <see cref="ParticleModifierCollection"/>.</returns>
        public ParticleModifierCollection DeepCopy()
        {
            var ret = new ParticleModifierCollection();
            ret._allModifiers.AddRange(_allModifiers);
            ret._updateModifiers.AddRange(_updateModifiers);
            ret._releaseModifiers.AddRange(_releaseModifiers);
            return ret;
        }

        /// <summary>
        /// Gets if this <see cref="ParticleModifierCollection"/> has any update modifiers.
        /// </summary>
        public bool HasUpdateModifiers { get { return _updateModifiers.Count > 0; } }

        /// <summary>
        /// Gets if this <see cref="ParticleModifierCollection"/> has any release modifiers.
        /// </summary>
        public bool HasReleaseModifiers { get { return _releaseModifiers.Count > 0; } }

        /// <summary>
        /// Modifiers that process released <see cref="Particle"/>s.
        /// </summary>
        readonly List<ParticleModifierBase> _releaseModifiers = new List<ParticleModifierBase>(2);

        /// <summary>
        /// Modifiers that process updated <see cref="Particle"/>s.
        /// </summary>
        readonly List<ParticleModifierBase> _updateModifiers = new List<ParticleModifierBase>(2);

        /// <summary>
        /// All of the modifiers.
        /// </summary>
        readonly List<ParticleModifierBase> _allModifiers = new List<ParticleModifierBase>(2);

        public IEnumerable<ParticleModifierBase> ReleaseModifiers { get { return _releaseModifiers; } }

        public IEnumerable<ParticleModifierBase> UpdateModifiers { get { return _updateModifiers; } }

        public void UpdateCurrentTime(int currentTime)
        {
            foreach (var modifier in this)
                modifier.UpdateCurrentTime(currentTime);
        }

        public void ProcessReleasedParticle(ParticleEmitter emitter, Particle particle)
        {
            foreach (var modifier in _releaseModifiers)
                modifier.ProcessReleased(emitter, particle);
        }

        public void ProcessUpdatedParticle(ParticleEmitter emitter, Particle particle)
        {
            foreach (var modifier in _updateModifiers)
                modifier.ProcessReleased(emitter, particle);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<ParticleModifierBase> GetEnumerator()
        {
            return _allModifiers.GetEnumerator();
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

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(ParticleModifierBase item)
        {
            if (item == null)
                return;

            _allModifiers.Add(item);

            if (item.ProcessOnRelease)
                _releaseModifiers.Add(item);

            if (item.ProcessOnUpdate)
                _updateModifiers.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Clear()
        {
            _allModifiers.Clear();
            _releaseModifiers.Clear();
            _updateModifiers.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>;
        /// otherwise, false.
        /// </returns>
        public bool Contains(ParticleModifierBase item)
        {
            return _allModifiers.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>,
        /// starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the
        /// elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.
        ///                     -or-
        ///                 <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        ///                     -or-
        ///                     The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/>
        ///                     is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination
        ///                     <paramref name="array"/>.
        ///                     -or-
        ///                     Type <see cref="ParticleModifierBase"/> cannot be cast automatically to the type of the destination
        ///                     <paramref name="array"/>.
        /// </exception>
        public void CopyTo(ParticleModifierBase[] array, int arrayIndex)
        {
            _allModifiers.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/>is read-only.</exception>
        public bool Remove(ParticleModifierBase item)
        {
            if (!_allModifiers.Remove(item))
                return false;

            _releaseModifiers.Remove(item);
            _updateModifiers.Remove(item);
            return true;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return _allModifiers.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(ParticleModifierBase item)
        {
            return _allModifiers.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid
        /// index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void Insert(int index, ParticleModifierBase item)
        {
            if (item == null)
                return;

            _allModifiers.Insert(index, item);

            if (item.ProcessOnRelease)
                _releaseModifiers.Add(item);

            if (item.ProcessOnUpdate)
                _updateModifiers.Add(item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a
        /// valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void RemoveAt(int index)
        {
            var item = _allModifiers[index];
            if (item != null)
            {
                if (item.ProcessOnRelease)
                    _releaseModifiers.Remove(item);

                if (item.ProcessOnUpdate)
                    _updateModifiers.Remove(item);
            }

            _allModifiers.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the
        /// <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the
        /// <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public ParticleModifierBase this[int index]
        {
            get { return _allModifiers[index]; }
            set
            {
                if (value == null)
                    return;

                var current = _allModifiers[index];
                if (current == null || current == value)
                    return;

                if (current.ProcessOnRelease)
                    _releaseModifiers.Remove(current);

                if (current.ProcessOnUpdate)
                    _updateModifiers.Remove(current);

                _allModifiers[index] = value;

                if (value.ProcessOnRelease)
                    _releaseModifiers.Add(value);

                if (value.ProcessOnUpdate)
                    _updateModifiers.Add(value);
            }
        }
    }
}
