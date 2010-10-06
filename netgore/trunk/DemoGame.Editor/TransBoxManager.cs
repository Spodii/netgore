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
        static readonly Vector2 _transBoxSize = Vector2.Max(SystemSprites.Move.Size, SystemSprites.Resize.Size);

        readonly List<ISpatial> _items = new List<ISpatial>();
        readonly List<ITransBox> _transBoxes = new List<ITransBox>();

        public IEnumerable<ISpatial> Items
        {
            get { return _items; }
        }

        public void Clear()
        {
            if (_items.Count == 0)
                return;

            _items.Clear();

            UpdateTransBoxes();
        }

        public void SetItems(IEnumerable<ISpatial> items)
        {
            Clear();

            _items.AddRange(items);

            UpdateTransBoxes();
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
            void Draw(ISpriteBatch spriteBatch);
        }

        sealed class MoveManyTransBox : ITransBox
        {
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
                get { return _transBoxSize; }
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
            public void Draw(ISpriteBatch spriteBatch)
            {
                var r = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
                SystemSprites.Move.Draw(spriteBatch, r, Color.White);
            }

            #endregion
        }

        sealed class TransBox : ITransBox
        {
            static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            readonly ISpatial _spatial;
            readonly TransBoxType _type;

            Vector2 _position;

            TransBox(TransBoxType type, ISpatial spatial, Vector2 position)
            {
                _type = type;
                _position = position;
                _spatial = spatial;
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
                get { return _transBoxSize; }
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

                var size = _transBoxSize;
                var halfSize = size / 2f;

                // Find the center of the sides for the resize and move icons
                var sizeCenter = min + (entity.Size / 2f) - halfSize;
                sizeCenter = sizeCenter.Round();

                var moveCenterX = min.X + (entity.Size.X / 2f) - (size.X / 2f);
                moveCenterX = (float)Math.Round(moveCenterX);

                // Move box
                ret.Add(new TransBox(TransBoxType.Move, entity, new Vector2(moveCenterX, min.Y - size.Y - 8)));

                // Four corners
                ret.Add(new TransBox(TransBoxType.TopLeft, entity, new Vector2(min.X - size.X, min.Y - size.Y)));
                ret.Add(new TransBox(TransBoxType.TopRight, entity, new Vector2(max.X, min.Y - size.Y)));
                ret.Add(new TransBox(TransBoxType.BottomLeft, entity, new Vector2(min.X - size.X, max.Y)));
                ret.Add(new TransBox(TransBoxType.BottomRight, entity, max));

                // Horizontal sides
                if (entity.Size.X > size.X + 4)
                {
                    ret.Add(new TransBox(TransBoxType.Top, entity, new Vector2(sizeCenter.X, min.Y - size.Y)));
                    ret.Add(new TransBox(TransBoxType.Bottom, entity, new Vector2(sizeCenter.X, max.Y)));
                }

                // Veritcal sides
                if (entity.Size.Y > size.Y + 4)
                {
                    ret.Add(new TransBox(TransBoxType.Left, entity, new Vector2(min.X - size.X, sizeCenter.Y)));
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
            public void Draw(ISpriteBatch spriteBatch)
            {
                var r = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
                SystemSprites.Move.Draw(spriteBatch, r, Color.White);
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