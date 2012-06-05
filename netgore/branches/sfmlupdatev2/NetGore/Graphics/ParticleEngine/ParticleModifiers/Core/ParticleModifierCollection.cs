using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A collection of <see cref="ParticleModifier"/>s.
    /// </summary>
    public sealed class ParticleModifierCollection : VirtualList<ParticleModifier>
    {
        /// <summary>
        /// ParticleModifiers that process released <see cref="Particle"/>s.
        /// </summary>
        readonly List<ParticleModifier> _releaseModifiers = new List<ParticleModifier>(2);

        /// <summary>
        /// ParticleModifiers that process updated <see cref="Particle"/>s.
        /// </summary>
        readonly List<ParticleModifier> _updateModifiers = new List<ParticleModifier>(2);

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
        public override ParticleModifier this[int index]
        {
            get { return base[index]; }
            set
            {
                if (value == null)
                    return;

                var current = base[index];
                if (current == null || current == value)
                    return;

                if (current.ProcessOnRelease)
                    _releaseModifiers.Remove(current);

                if (current.ProcessOnUpdate)
                    _updateModifiers.Remove(current);

                if (value.ProcessOnRelease)
                    _releaseModifiers.Add(value);

                if (value.ProcessOnUpdate)
                    _updateModifiers.Add(value);

                base[index] = value;
            }
        }

        /// <summary>
        /// Gets if this <see cref="ParticleModifierCollection"/> has any release modifiers.
        /// </summary>
        public bool HasReleaseModifiers
        {
            get { return _releaseModifiers.Count > 0; }
        }

        /// <summary>
        /// Gets if this <see cref="ParticleModifierCollection"/> has any update modifiers.
        /// </summary>
        public bool HasUpdateModifiers
        {
            get { return _updateModifiers.Count > 0; }
        }

        /// <summary>
        /// Gets an IEnumerable of all the <see cref="ParticleModifier"/>s that process <see cref="Particle"/>s
        /// when they are released.
        /// </summary>
        public IEnumerable<ParticleModifier> ReleaseModifiers
        {
            get { return _releaseModifiers; }
        }

        /// <summary>
        /// Gets an IEnumerable of all the <see cref="ParticleModifier"/>s that process <see cref="Particle"/>s
        /// when they are updated.
        /// </summary>
        public IEnumerable<ParticleModifier> UpdateModifiers
        {
            get { return _updateModifiers; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public override void Add(ParticleModifier item)
        {
            if (item == null)
                return;

            if (item.ProcessOnRelease)
                _releaseModifiers.Add(item);

            if (item.ProcessOnUpdate)
                _updateModifiers.Add(item);

            base.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public override void Clear()
        {
            _releaseModifiers.Clear();
            _updateModifiers.Clear();

            base.Clear();
        }

        /// <summary>
        /// Creates a deep copy of the <see cref="ParticleModifierCollection"/>.
        /// </summary>
        /// <returns>A deep copy of the <see cref="ParticleModifierCollection"/>.</returns>
        public ParticleModifierCollection DeepCopy()
        {
            var ret = new ParticleModifierCollection();
            ret.GetUnderlyingList.AddRange(GetUnderlyingList);
            ret._updateModifiers.AddRange(_updateModifiers);
            ret._releaseModifiers.AddRange(_releaseModifiers);
            return ret;
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
        public override void Insert(int index, ParticleModifier item)
        {
            if (item == null)
                return;

            if (item.ProcessOnRelease)
                _releaseModifiers.Add(item);

            if (item.ProcessOnUpdate)
                _updateModifiers.Add(item);

            base.Insert(index, item);
        }

        /// <summary>
        /// Calls all modifiers that process released <see cref="Particle"/>s on the given <paramref name="particle"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> that created the <paramref name="particle"/>.</param>
        /// <param name="particle">The <see cref="Particle"/> to process.</param>
        public void ProcessReleasedParticle(ParticleEmitter emitter, Particle particle)
        {
            foreach (var modifier in _releaseModifiers)
            {
                modifier.ProcessReleased(emitter, particle);
            }
        }

        /// <summary>
        /// Calls all modifiers that process updated <see cref="Particle"/>s on the given <paramref name="particle"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> that created the <paramref name="particle"/>.</param>
        /// <param name="particle">The <see cref="Particle"/> to process.</param>
        /// <param name="elapsedTime">The amount of time that has elapsed since the <paramref name="emitter"/>
        /// was last updated.</param>
        public void ProcessUpdatedParticle(ParticleEmitter emitter, Particle particle, int elapsedTime)
        {
            foreach (var modifier in _updateModifiers)
            {
                modifier.ProcessUpdated(emitter, particle, elapsedTime);
            }
        }

        /// <summary>
        /// Reads the <see cref="ParticleModifierCollection"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="nodeName">The name of the collection node.</param>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        public void Read(string nodeName, IValueReader reader)
        {
            // Read the modifiers
            var modifiers = reader.ReadManyNodes(nodeName, ParticleModifier.Read);

            // Clear the collection and add the created modifiers
            Clear();
            AddRange(modifiers);
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
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/>is read-only.</exception>
        public override bool Remove(ParticleModifier item)
        {
            var removed = base.Remove(item);

            if (removed)
            {
                if (item.ProcessOnRelease)
                    _releaseModifiers.Remove(item);

                if (item.ProcessOnUpdate)
                    _updateModifiers.Remove(item);
            }

            return removed;
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a
        /// valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public override void RemoveAt(int index)
        {
            var item = this[index];
            if (item != null)
            {
                if (item.ProcessOnRelease)
                    _releaseModifiers.Remove(item);

                if (item.ProcessOnUpdate)
                    _updateModifiers.Remove(item);
            }

            base.RemoveAt(index);
        }

        /// <summary>
        /// Updates the current time on all processors.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        internal void UpdateCurrentTime(TickCount currentTime)
        {
            foreach (var modifier in this)
            {
                modifier.UpdateCurrentTime(currentTime);
            }
        }

        /// <summary>
        /// Writes the <see cref="ParticleModifierCollection"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="nodeName">The name to give the collection node.</param>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public void Write(string nodeName, IValueWriter writer)
        {
            writer.WriteManyNodes(nodeName, this.ToArray(), (w, mod) => mod.Write(w));
        }
    }
}