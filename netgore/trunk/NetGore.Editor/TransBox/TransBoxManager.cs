using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Editor
{
    /// <summary>
    /// Provides support for on-screen transformation boxes (Trans Boxes for short) which allow you to edit an <see cref="ISpatial"/>
    /// through clicking and dragging.
    /// </summary>
    public partial class TransBoxManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly List<Type> _typesToIgnore = new List<Type>();

        readonly List<ISpatial> _items = new List<ISpatial>();
        readonly List<ITransBox> _transBoxes = new List<ITransBox>();

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s that are currently in this <see cref="TransBoxManager"/> and are part of the
        /// on-screen transformation boxes.
        /// </summary>
        public IEnumerable<ISpatial> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the <see cref="Type"/>s that are being ignored. These <see cref="Type"/>s will never be able to have the
        /// transformation boxes added for them. Use this if you ever want to force a <see cref="ISpatial"/> to not have
        /// a transformation box even when <see cref="ISpatial.SupportsMove"/> and <see cref="ISpatial.SupportsResize"/> are true.
        /// </summary>
        public static IEnumerable<Type> TypesToIgnore
        {
            get { return _typesToIgnore; }
        }

        /// <summary>
        /// Clears all transformation boxes.
        /// </summary>
        public void Clear()
        {
            if (_items.Count == 0)
                return;

            _items.Clear();

            UpdateTransBoxes();
        }

        /// <summary>
        /// Draws the <see cref="TransBoxManager"/> and all transformation boxes in it.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> describing the current view.</param>
        public void Draw(ISpriteBatch spriteBatch, ICamera2D camera)
        {
            foreach (var tb in _transBoxes)
            {
                tb.Draw(spriteBatch, camera);
            }
        }

        /// <summary>
        /// Gets the <see cref="Cursor"/> for a <see cref="ITransBox"/>.
        /// </summary>
        /// <param name="type">The <see cref="TransBoxType"/>.</param>
        /// <returns>The <see cref="Cursor"/> of a <see cref="ITransBox"/> for the given <paramref name="type"/>.</returns>
        static Cursor GetCursor(TransBoxType type)
        {
            switch (type)
            {
                case TransBoxType.Bottom:
                case TransBoxType.Top:
                    return Cursors.SizeNS;

                case TransBoxType.Left:
                case TransBoxType.Right:
                    return Cursors.SizeWE;

                case TransBoxType.TopLeft:
                case TransBoxType.BottomRight:
                    return Cursors.SizeNESW;

                case TransBoxType.TopRight:
                case TransBoxType.BottomLeft:
                    return Cursors.SizeNWSE;

                case TransBoxType.Move:
                    return Cursors.SizeAll;

                default:
                    const string errmsg = "Unsupported TransBoxType `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, type);
                    Debug.Fail(string.Format(errmsg, type));
                    return Cursors.Default;
            }
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for a <see cref="ITransBox"/>.
        /// </summary>
        /// <param name="type">The <see cref="TransBoxType"/>.</param>
        /// <returns>The <see cref="ISprite"/> of a <see cref="ITransBox"/> for the given <paramref name="type"/>.</returns>
        static ISprite GetSprite(TransBoxType type)
        {
            switch (type)
            {
                case TransBoxType.Move:
                    return SystemSprites.Move;

                default:
                    return SystemSprites.Resize;
            }
        }

        /// <summary>
        /// Gets the size for a <see cref="ITransBox"/>.
        /// </summary>
        /// <param name="type">The <see cref="TransBoxType"/>.</param>
        /// <returns>The size of a <see cref="ITransBox"/> for the given <paramref name="type"/>.</returns>
        static Vector2 GetTransBoxSize(TransBoxType type)
        {
            return GetSprite(type).Size;
        }

        /// <summary>
        /// Adds a <see cref="Type"/> to be ignored by the <see cref="TransBoxManager"/>. Any object that is attempted
        /// to be added to a <see cref="TransBoxManager"/> that contains this <see cref="Type"/> will skip being added.
        /// This allows you to completely skip certain <see cref="Type"/>s.
        /// </summary>
        /// <param name="types">The <see cref="Type"/>s to add to the ignore list.</param>
        public static void IgnoreType(IEnumerable<Type> types)
        {
            if (types == null)
                return;

            foreach (var type in types)
            {
                IgnoreType(type);
            }
        }

        /// <summary>
        /// Adds a <see cref="Type"/> to be ignored by the <see cref="TransBoxManager"/>. Any object that is attempted
        /// to be added to a <see cref="TransBoxManager"/> that contains this <see cref="Type"/> will skip being added.
        /// This allows you to completely skip certain <see cref="Type"/>s.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to add to the ignore list.</param>
        public static void IgnoreType(Type type)
        {
            if (!_typesToIgnore.Contains(type))
                _typesToIgnore.Add(type);
        }

        /// <summary>
        /// Sets the <see cref="ISpatial"/>s to place a transformation boxes over. Items that are the
        /// same <see cref="Type"/> as the <see cref="Type"/>s define din <see cref="TypesToIgnore"/> will not be added,
        /// and neither will <see cref="ISpatial"/>s where <see cref="ISpatial.SupportsMove"/> and
        /// <see cref="ISpatial.SupportsResize"/> is false.
        /// </summary>
        /// <param name="items">The <see cref="ISpatial"/>s to place transformation boxes over. Using a null or
        /// empty collection is synonymous with just using <see cref="Clear"/>.</param>
        public void SetItems(IEnumerable<ISpatial> items)
        {
            // Clear
            Clear();

            if (items == null)
                return;

            // Get the items to add
            // If it doesn't support moving, don't add it, even if it supports resizing since a lot of our resizing requires moving
            var toAdd = items.Where(x => x.SupportsMove && !TypesToIgnore.Any(y => x.GetType() == y));

            if (toAdd.IsEmpty())
                return;

            // Add the items
            _items.AddRange(toAdd);

            // Update
            UpdateTransBoxes();
        }

        /// <summary>
        /// Removes an <see cref="Type"/> that was previously added to the <see cref="Type"/> ignore list.
        /// </summary>
        /// <param name="types">The <see cref="Type"/>s to remove from the ignore list.</param>
        public static void UnignoreType(IEnumerable<Type> types)
        {
            if (types == null)
                return;

            foreach (var type in types)
            {
                UnignoreType(type);
            }
        }

        /// <summary>
        /// Removes an <see cref="Type"/> that was previously added to the <see cref="Type"/> ignore list.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to remove from the ignore list.</param>
        public static void UnignoreType(Type type)
        {
            _typesToIgnore.Remove(type);
        }

        /// <summary>
        /// Recreates the <see cref="ITransBox"/>es for the current selection.
        /// </summary>
        void UpdateTransBoxes()
        {
            // Clear the old boxes
            _transBoxes.Clear();

            if (_items.Count <= 0)
            {
                // Nothing selected
                return;
            }
            else if (_items.Count == 1)
            {
                // Only one selected
                var item = _items.FirstOrDefault();
                if (item == null)
                {
                    Debug.Fail("How did this happen?");
                    Clear();
                    return;
                }

                var transBoxes = TransBox.SurroundEntity(item);
                _transBoxes.AddRange(transBoxes);
            }
            else
            {
                // Multiple selected
                var min = new Vector2(Items.Min(x => x.Position.X), Items.Min(x => x.Position.Y));
                var max = new Vector2(Items.Max(x => x.Max.X), Items.Max(x => x.Max.Y));
                var center = min + ((max - min) / 2f).Round();

                var tb = new MoveManyTransBox(_items, center);
                _transBoxes.Add(tb);
            }
        }
    }
}