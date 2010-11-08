using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Collection of points to generate a polygon.
    /// </summary>
    /// <remarks>By implementing the IList interface explicitly, we can effectively override certain methods of the
    /// base class without them being declared as virtual.</remarks>
    public class PolygonPointCollection : List<Vector2>, IList
    {
        const string _categoryName = "Polygon Points";

        /// <summary>
        /// The translation <see cref="Matrix"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "Allows us to pass this field by reference, which can increase performance due to less copying.")]
        public Matrix TranslationMatrix = Matrix.Identity;

        PolygonOrigin _origin = PolygonEmitter.DefaultPolygonOrigin;

        /// <summary>
        /// Gets or sets the origin mode of the polygon for the given points.
        /// </summary>
        [Category(_categoryName)]
        [Description("The origin mode of the polygon for the given points.")]
        [DisplayName("Polygon Origin")]
        [DefaultValue(PolygonEmitter.DefaultPolygonOrigin)]
        public PolygonOrigin Origin
        {
            get { return _origin; }
            set
            {
                if (_origin == value)
                    return;

                _origin = value;
                RecalculateTranslation();
            }
        }

        /// <summary>
        /// Gets the translation <see cref="Matrix"/> for the average center point.
        /// </summary>
        /// <param name="matrix">The translation <see cref="Matrix"/> for the average center point.</param>
        void GetCenterTranslation(out Matrix matrix)
        {
            // Creates a rectangle around the polygon and use its center as the polygon center
            Vector2 v0;
            if (Count > 0)
                v0 = base[0];
            else
                v0 = Vector2.Zero;

            var left = v0.X;
            var right = v0.X;
            var top = v0.Y;
            var bottom = v0.Y;

            // Check all the points to make the rectangle surround the entire polygon
            for (var i = 1; i < Count; i++)
            {
                var v = base[i];

                if (v.X < left)
                    left = v.X;

                if (v.X > right)
                    right = v.X;

                if (v.Y < top)
                    top = v.Y;

                if (v.Y > bottom)
                    bottom = v.Y;
            }

            // Apply the translation offset
            var x = -((right - left) / 2 + left);
            var y = -((bottom - top) / 2 + top);
            matrix = Matrix.CreateTranslation(x, y, 0f);
        }

        /// <summary>
        /// Gets the default translation <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The default translation <see cref="Matrix"/>.</param>
        static void GetDefaultTranslation(out Matrix matrix)
        {
            matrix = Matrix.Identity;
        }

        /// <summary>
        /// Gets the translation <see cref="Matrix"/> for the origin.
        /// </summary>
        /// <param name="matrix">The translation <see cref="Matrix"/> for the origin.</param>
        void GetOriginTranslation(out Matrix matrix)
        {
            var point = Vector2.Zero;

            if (Count > 0)
                point = base[0];

            matrix = Matrix.CreateTranslation(-point.X, -point.Y, 0f);
        }

        /// <summary>
        /// Reads the <see cref="PolygonPointCollection"/> values from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="nodeName">The name of the collection node.</param>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        public void Read(string nodeName, IValueReader reader)
        {
            // Read the values
            var values = reader.ReadMany(nodeName, (r, name) => r.ReadVector2(name));

            // Clear the list and repopulate with the read values
            Clear();
            AddRange(values);
        }

        /// <summary>
        /// Calculates the translation based on the <see cref="PolygonOrigin"/> used.
        /// </summary>
        void RecalculateTranslation()
        {
            switch (Origin)
            {
                case PolygonOrigin.Default:
                    GetDefaultTranslation(out TranslationMatrix);
                    break;

                case PolygonOrigin.Center:
                    GetCenterTranslation(out TranslationMatrix);
                    break;

                case PolygonOrigin.Origin:
                    GetOriginTranslation(out TranslationMatrix);
                    break;
            }
        }

        /// <summary>
        /// Writes the <see cref="PolygonPointCollection"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="nodeName">The name of the collection node.</param>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public void Write(string nodeName, IValueWriter writer)
        {
            writer.WriteMany(nodeName, ToArray(), writer.Write);
        }

        #region IList Members

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        object IList.this[int index]
        {
            get { return base[index]; }
            set
            {
                var v = (Vector2)value;

                base[index] = v;

                RecalculateTranslation();
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to add to the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The position into which the new element was inserted.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.IList"/> is read-only.
        /// -or-
        /// The <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </exception>
        int IList.Add(object value)
        {
            var v = (Vector2)value;

            Add(v);

            RecalculateTranslation();

            return IndexOf(v);
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        void IList.Clear()
        {
            Clear();

            TranslationMatrix = Matrix.Identity;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to remove from the
        /// <see cref="T:System.Collections.IList"/>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.IList"/> is read-only.
        /// -or-
        /// The <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </exception>
        void IList.Remove(object value)
        {
            var v = (Vector2)value;

            if (Contains(v))
                Remove(v);

            RecalculateTranslation();
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than 0.
        /// -or-
        /// <paramref name="index"/> is equal to or greater than <see cref="P:System.Collections.Generic.List`1.Count"/>.
        /// </exception>
        void IList.RemoveAt(int index)
        {
            RemoveAt(index);

            RecalculateTranslation();
        }

        #endregion
    }
}