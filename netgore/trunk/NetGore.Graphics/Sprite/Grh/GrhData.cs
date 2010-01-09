using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Base class for a <see cref="GrhData"/>, which is what describes how a <see cref="Grh"/> functions.
    /// </summary>
    public abstract class GrhData
    {
        const string _categorizationValueKey = "Categorization";
        const string _indexValueKey = "Index";

        readonly GrhIndex _grhIndex;
        SpriteCategorization _categorization;

        /// <summary>
        /// Notifies listeners when the <see cref="GrhData"/>'s categorization has changed.
        /// </summary>
        public event GrhDataChangeCategorizationHandler OnChangeCategorization;

        protected GrhData(GrhIndex grhIndex, SpriteCategorization cat)
        {
            _categorization = cat;
            _grhIndex = grhIndex;
        }

        /// <summary>
        /// Gets the <see cref="SpriteCategorization"/> for this <see cref="GrhData"/>.
        /// </summary>
        public SpriteCategorization Categorization
        {
            get { return _categorization; }
        }

        /// <summary>
        /// Gets the index of the <see cref="GrhData"/>.
        /// </summary>
        public GrhIndex GrhIndex
        {
            get { return _grhIndex; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the size of the <see cref="GrhData"/>'s sprite in pixels.
        /// </summary>
        public abstract Vector2 Size { get; }

        /// <summary>
        /// When overridden in the derived class, creates a new <see cref="GrhData"/> equal to this <see cref="GrhData"/>
        /// except for the specified parameters.
        /// </summary>
        /// <param name="newCategorization">The <see cref="SpriteCategorization"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <param name="newGrhIndex">The <see cref="GrhIndex"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <returns>A deep copy of this <see cref="GrhData"/>.</returns>
        protected abstract GrhData DeepCopy(SpriteCategorization newCategorization, GrhIndex newGrhIndex);

        /// <summary>
        /// Creates a deep copy of the <see cref="GrhData"/>.
        /// </summary>
        /// <param name="newCategorization">Categorization for the duplicated GrhData. Must be unique.</param>
        /// <returns>Deep copy of the <see cref="GrhData"/> with the new categorization and its own
        /// unique <see cref="GrhIndex"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="newCategorization"/> is already in use.</exception>
        public GrhData Duplicate(SpriteCategorization newCategorization)
        {
            if (GrhInfo.GetData(newCategorization) != null)
                throw new ArgumentException("Category already in use.", "newCategorization");

            GrhIndex index = GrhInfo.NextFreeIndex();
            Debug.Assert(GrhInfo.GetData(index) == null,
                         "Slot to use is already in use! How the hell did this happen!? GrhInfo.NextFreeIndex() must be broken.");

            var dc = DeepCopy(newCategorization, index);

            GrhInfo.AddGrhData(dc);

            return dc;
        }

        protected internal static void ReadHeader(IValueReader r, out GrhIndex grhIndex, out SpriteCategorization cat)
        {
            grhIndex = r.ReadGrhIndex(_indexValueKey);
            cat = r.ReadSpriteCategorization(_categorizationValueKey);
        }

        /// <summary>
        /// Sets the categorization for the <see cref="GrhData"/>.
        /// </summary>
        /// <param name="categorization">The new categorization.</param>
        public void SetCategorization(SpriteCategorization categorization)
        {
            if (categorization == null)
                throw new ArgumentNullException("categorization");

            // Check that either of the values are different
            if (_categorization == categorization)
                return;

            var oldCategorization = _categorization;
            _categorization = categorization;

            if (OnChangeCategorization != null)
                OnChangeCategorization(this, oldCategorization);
        }

        /// <summary>
        /// Returns a System.String that represents the <see cref="GrhData"/>.
        /// </summary>
        /// <returns>A System.String that represents the <see cref="GrhData"/>.</returns>
        public override string ToString()
        {
            return string.Format("[{0}] {1}", GrhIndex, Categorization);
        }

        /// <summary>
        /// Writes the <see cref="GrhData"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="w"><see cref="IValueWriter"/> to write to.</param>
        public void Write(IValueWriter w)
        {
            // Check for valid data
            if (GrhIndex <= 0)
                throw new Exception("GrhIndex invalid.");
            if (w == null)
                throw new ArgumentNullException("w");

            Debug.Assert(Categorization != null);

            // Write the index and category
            w.Write(_indexValueKey, GrhIndex);
            w.Write(_categorizationValueKey, Categorization);

            // Write the custom values
            WriteCustomValues(w);
        }

        /// <summary>
        /// When overridden in the derived class, writes the values unique to this derived type to the
        /// <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        protected abstract void WriteCustomValues(IValueWriter writer);
    }
}