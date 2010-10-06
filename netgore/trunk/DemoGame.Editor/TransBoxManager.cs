using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor
{
    public class TransBoxManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly List<Type> _typesToIgnore = new List<Type>();

        readonly List<ISpatial> _items = new List<ISpatial>();
        readonly List<ITransBox> _transBoxes = new List<ITransBox>();

        public IEnumerable<ISpatial> Items
        {
            get { return _items; }
        }

        public static IEnumerable<Type> TypesToIgnore
        {
            get { return _typesToIgnore; }
        }

        public void Clear()
        {
            if (_items.Count == 0)
                return;

            _items.Clear();

            UpdateTransBoxes();
        }

        public void Draw(ISpriteBatch spriteBatch, ICamera2D camera)
        {
            foreach (var tb in _transBoxes)
            {
                tb.Draw(spriteBatch, camera);
            }
        }

        static Cursor GetCursor(TransBoxType t)
        {
            switch (t)
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
                        log.ErrorFormat(errmsg, t);
                    Debug.Fail(string.Format(errmsg, t));
                    return Cursors.Default;
            }
        }

        static ISprite GetSprite(TransBoxType t)
        {
            switch (t)
            {
                case TransBoxType.Move:
                    return SystemSprites.Move;

                default:
                    return SystemSprites.Resize;
            }
        }

        static Vector2 GetTransBoxSize(TransBoxType type)
        {
            return GetSprite(type).Size;
        }

        public static void IgnoreType(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                IgnoreType(type);
            }
        }

        public static void IgnoreType(Type type)
        {
            if (!_typesToIgnore.Contains(type))
                _typesToIgnore.Add(type);
        }

        public void SetItems(IEnumerable<ISpatial> items)
        {
            Clear();

            var toAdd = items.Where(x => !TypesToIgnore.Any(y => x.GetType().IsSubclassOf(y)));
            _items.AddRange(toAdd);

            UpdateTransBoxes();
        }

        public static void UnignoreType(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                UnignoreType(type);
            }
        }

        public static void UnignoreType(Type type)
        {
            _typesToIgnore.Remove(type);
        }

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

        interface ITransBox
        {
            /// <summary>
            /// Gets the <see cref="Cursor"/> to display when this <see cref="ITransBox"/> is selected or the mouse is over it.
            /// </summary>
            Cursor MouseCursor { get; }

            /// <summary>
            /// Checks if this <see cref="ITransBox"/> contains the given world point.
            /// </summary>
            /// <param name="worldPos">The world point.</param>
            /// <returns>True if this <see cref="ITransBox"/> contains the <paramref name="worldPos"/>; otherwise false.</returns>
            bool ContainsPoint(Vector2 worldPos);

            /// <summary>
            /// Handles when the mouse cursor moves while this <see cref="ITransBox"/> is selected.
            /// </summary>
            /// <param name="offset">The amount the cursor has moved.</param>
            void CursorMoved(Vector2 offset);

            /// <summary>
            /// Draws the <see cref="ITransBox"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="camera">The <see cref="ICamera2D"/>.</param>
            void Draw(ISpriteBatch spriteBatch, ICamera2D camera);
        }

        sealed class MoveManyTransBox : ITransBox
        {
            static readonly Vector2 _size = GetTransBoxSize(TransBoxType.Move);

            readonly IEnumerable<ISpatial> _spatials;

            Vector2 _position;

            public MoveManyTransBox(IEnumerable<ISpatial> spatials, Vector2 position)
            {
                _position = position;
                _spatials = spatials.ToImmutable();
            }

            Vector2 Max
            {
                get { return Position + Size; }
            }

            Vector2 Position
            {
                get { return _position; }
            }

            static Vector2 Size
            {
                get { return _size; }
            }

            #region ITransBox Members

            /// <summary>
            /// Gets the <see cref="Cursor"/> to display when this <see cref="ITransBox"/> is selected or the mouse is over it.
            /// </summary>
            public Cursor MouseCursor
            {
                get { return Cursors.SizeAll; }
            }

            /// <summary>
            /// Checks if this <see cref="ITransBox"/> contains the given world point.
            /// </summary>
            /// <param name="worldPos">The world point.</param>
            /// <returns>True if this <see cref="ITransBox"/> contains the <paramref name="worldPos"/>; otherwise false.</returns>
            public bool ContainsPoint(Vector2 worldPos)
            {
                var w = worldPos;
                var lo = Position;
                var hi = Max;
                return (lo.X >= w.X && lo.Y >= w.Y && hi.X <= w.X && hi.Y <= w.Y);
            }

            /// <summary>
            /// Handles when the mouse cursor moves while this <see cref="ITransBox"/> is selected.
            /// </summary>
            /// <param name="offset">The amount the cursor has moved.</param>
            public void CursorMoved(Vector2 offset)
            {
                foreach (var s in _spatials)
                {
                    s.TryMove(offset);
                }

                _position += offset;
            }

            /// <summary>
            /// Draws the <see cref="ITransBox"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="camera">The <see cref="ICamera2D"/>.</param>
            public void Draw(ISpriteBatch spriteBatch, ICamera2D camera)
            {
                var p = camera.ToScreen(Position).Round();
                var s = Size.Round();
                var r = new Rectangle((int)p.X, (int)p.Y, (int)s.X, (int)s.Y);
                SystemSprites.Move.Draw(spriteBatch, r, Color.White);
            }

            #endregion
        }

        sealed class TransBox : ITransBox
        {
            readonly Vector2 _size;
            readonly ISpatial _spatial;
            readonly TransBoxType _type;

            Vector2 _position;

            TransBox(TransBoxType type, ISpatial spatial, Vector2 position)
            {
                _type = type;
                _position = position;
                _spatial = spatial;

                _size = GetTransBoxSize(type);
            }

            Vector2 Max
            {
                get { return Position + Size; }
            }

            Vector2 Position
            {
                get { return _position; }
            }

            Vector2 Size
            {
                get { return _size; }
            }

            /// <summary>
            /// Creates a series of transformation boxes around an entity.
            /// </summary>
            /// <param name="entity">Entity to create the transformation boxes for.</param>
            public static IEnumerable<ITransBox> SurroundEntity(ISpatial entity)
            {
                var ret = new List<ITransBox>(9);

                var min = entity.Position;
                var max = entity.Max;

                var moveSize = GetTransBoxSize(TransBoxType.Move);
                var moveHalfSize = moveSize / 2f;

                var scaleSize = GetTransBoxSize(TransBoxType.BottomLeft);
                var scaleHalfSize = scaleSize / 2f;

                // Find the center of the sides for the resize and move icons
                var sizeCenter = min + (entity.Size / 2f) - scaleHalfSize;
                sizeCenter = sizeCenter.Round();

                var moveCenterX = min.X + (entity.Size.X / 2f) - moveHalfSize.X;
                moveCenterX = (float)Math.Round(moveCenterX);

                // Move box
                ret.Add(new TransBox(TransBoxType.Move, entity, new Vector2(moveCenterX, min.Y - moveSize.Y - scaleSize.Y)));

                // Four corners
                ret.Add(new TransBox(TransBoxType.TopLeft, entity, new Vector2(min.X - scaleSize.X, min.Y - scaleSize.Y)));
                ret.Add(new TransBox(TransBoxType.TopRight, entity, new Vector2(max.X, min.Y - scaleSize.Y)));
                ret.Add(new TransBox(TransBoxType.BottomLeft, entity, new Vector2(min.X - scaleSize.X, max.Y)));
                ret.Add(new TransBox(TransBoxType.BottomRight, entity, max));

                // Horizontal sides
                if (entity.Size.X > scaleSize.X + 4)
                {
                    ret.Add(new TransBox(TransBoxType.Top, entity, new Vector2(sizeCenter.X, min.Y - scaleSize.Y)));
                    ret.Add(new TransBox(TransBoxType.Bottom, entity, new Vector2(sizeCenter.X, max.Y)));
                }

                // Veritcal sides
                if (entity.Size.Y > scaleSize.Y + 4)
                {
                    ret.Add(new TransBox(TransBoxType.Left, entity, new Vector2(min.X - scaleSize.X, sizeCenter.Y)));
                    ret.Add(new TransBox(TransBoxType.Right, entity, new Vector2(max.X, sizeCenter.Y)));
                }

                return ret;
            }

            #region ITransBox Members

            /// <summary>
            /// Gets the <see cref="Cursor"/> to display when this <see cref="ITransBox"/> is selected or the mouse is over it.
            /// </summary>
            public Cursor MouseCursor
            {
                get { return GetCursor(_type); }
            }

            /// <summary>
            /// Checks if this <see cref="ITransBox"/> contains the given world point.
            /// </summary>
            /// <param name="worldPos">The world point.</param>
            /// <returns>True if this <see cref="ITransBox"/> contains the <paramref name="worldPos"/>; otherwise false.</returns>
            public bool ContainsPoint(Vector2 worldPos)
            {
                var w = worldPos;
                var lo = Position;
                var hi = Max;
                return (lo.X >= w.X && lo.Y >= w.Y && hi.X <= w.X && hi.Y <= w.Y);
            }

            /// <summary>
            /// Handles when the mouse cursor moves while this <see cref="ITransBox"/> is selected.
            /// </summary>
            /// <param name="offset">The amount the cursor has moved.</param>
            public void CursorMoved(Vector2 offset)
            {
                switch (_type)
                {
                    case TransBoxType.Bottom:
                        _spatial.TryResize(new Vector2(0, offset.Y));
                        break;

                    case TransBoxType.BottomLeft:
                        if (_spatial.TryResize(new Vector2(-offset.X, offset.Y)))
                            _spatial.TryMove(new Vector2(-offset.X, 0));
                        break;

                    case TransBoxType.BottomRight:
                        _spatial.TryResize(offset);
                        break;

                    case TransBoxType.Left:
                        if (_spatial.TryResize(new Vector2(-offset.X, 0)))
                            _spatial.TryMove(new Vector2(-offset.X, 0));
                        break;

                    case TransBoxType.Move:
                        _spatial.TryMove(offset);
                        break;

                    case TransBoxType.Right:
                        _spatial.TryResize(new Vector2(offset.X, 0));
                        break;

                    case TransBoxType.Top:
                        if (_spatial.TryResize(new Vector2(0, -offset.Y)))
                            _spatial.TryMove(new Vector2(0, -offset.Y));
                        break;

                    case TransBoxType.TopLeft:
                        if (_spatial.TryResize(-offset))
                            _spatial.TryMove(-offset);
                        break;

                    case TransBoxType.TopRight:
                        if (_spatial.TryResize(new Vector2(offset.X, -offset.Y)))
                            _spatial.TryResize(new Vector2(offset.X, -offset.Y));
                        break;

                    default:
                        const string errmsg = "Unsupported TransBoxType `{0}`.";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, _type);
                        Debug.Fail(string.Format(errmsg, _type));
                        break;
                }

                _position += offset;
            }

            /// <summary>
            /// Draws the <see cref="ITransBox"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="camera">The <see cref="ICamera2D"/>.</param>
            public void Draw(ISpriteBatch spriteBatch, ICamera2D camera)
            {
                var sprite = GetSprite(_type);
                if (sprite == null)
                    return;

                var p = camera.ToScreen(Position).Round();
                var s = Size.Round();
                var r = new Rectangle((int)p.X, (int)p.Y, (int)s.X, (int)s.Y);

                sprite.Draw(spriteBatch, r, Color.White);
            }

            #endregion
        }

        /// <summary>
        /// States how a transformation box can modify the target
        /// </summary>
        [Flags]
        enum TransBoxType : byte
        {
            /// <summary>
            /// Move box (no transformation)
            /// </summary>
            Move = 0,

            /// <summary>
            /// Top side transform
            /// </summary>
            Top = 1,

            /// <summary>
            /// Left side transform
            /// </summary>
            Left = 2,

            /// <summary>
            /// Bottom side transform
            /// </summary>
            Bottom = 4,

            /// <summary>
            /// Right side transform
            /// </summary>
            Right = 8,

            /// <summary>
            /// Top-left side transform
            /// </summary>
            TopLeft = Top | Left,

            /// <summary>
            /// Top-right side transform
            /// </summary>
            TopRight = Top | Right,

            /// <summary>
            /// Bottom-left side transform
            /// </summary>
            BottomLeft = Bottom | Left,

            /// <summary>
            /// Bottom-right side transform
            /// </summary>
            BottomRight = Bottom | Right
        }
    }
}