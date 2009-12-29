using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    /// <summary>
    /// States how a transformation box can modify the target
    /// </summary>
    [Flags]
    public enum TransBoxType
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

    /// <summary>
    /// Describes an entity transformation box
    /// </summary>
    public class TransBox
    {
        /// <summary>
        /// Color for a box connected to multiple walls
        /// </summary>
        static readonly Color _colorGroup = new Color(0, 255, 0, 175);

        /// <summary>
        /// Color for a normal box connected to a single wall
        /// </summary>
        static readonly Color _colorNormal = new Color(255, 255, 255, 175);

        static GrhData _move;
        static Vector2 _moveSize;
        static GrhData _scale;
        static Vector2 _scaleSize;

        /// <summary>
        /// Area the box occupies
        /// </summary>
        public readonly Rectangle Area;

        /// <summary>
        /// Entity the box is attached to
        /// </summary>
        public readonly Entity Entity;

        /// <summary>
        /// Position (top-left corner) of the box
        /// </summary>
        public readonly Vector2 Position;

        /// <summary>
        /// Type of transformation box
        /// </summary>
        public readonly TransBoxType TransType;

        /// <summary>
        /// Grh used to draw the TransBox
        /// </summary>
        readonly Grh _grh;

        /// <summary>
        /// Creates a transformation box
        /// </summary>
        /// <param name="transType">Type of transformation box</param>
        /// <param name="entity">Entity to attach the box to</param>
        /// <param name="position">Position to create the transformation box</param>
        public TransBox(TransBoxType transType, Entity entity, Vector2 position)
        {
            TransType = transType;
            Position = position;
            Entity = entity;

            GrhData sourceGrhData;

            if (transType == TransBoxType.Move)
                sourceGrhData = Move;
            else
                sourceGrhData = Scale;

            Vector2 size = sourceGrhData.Size;
            Area = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            _grh = new Grh(sourceGrhData);
        }

        /// <summary>
        /// Gets the GrhData for the moving icon
        /// </summary>
        public static GrhData Move
        {
            get { return _move; }
        }

        /// <summary>
        /// Gets the size of the moving icon
        /// </summary>
        public static Vector2 MoveSize
        {
            get { return _moveSize; }
        }

        /// <summary>
        /// Gets the GrhData for the scaling icon
        /// </summary>
        public static GrhData Scale
        {
            get { return _scale; }
        }

        /// <summary>
        /// Gets the size of the scaling icon
        /// </summary>
        public static Vector2 ScaleSize
        {
            get { return _scaleSize; }
        }

        /// <summary>
        /// Draws the transformation box
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        public void Draw(SpriteBatch sb)
        {
            if (TransType == TransBoxType.Move)
            {
                // Movement box
                Color drawColor;
                if (Entity == null)
                    drawColor = _colorGroup;
                else
                    drawColor = _colorNormal;

                _grh.Draw(sb, Position, drawColor);
            }
            else
            {
                // Selection box
                _grh.Draw(sb, Position, _colorNormal);
            }
        }

        /// <summary>
        /// Initializes the TransBoxes
        /// </summary>
        /// <param name="moveIconData">GrhData for the move icon</param>
        /// <param name="sizeIconData">GrhData for the resize icon</param>
        public static void Initialize(GrhData moveIconData, GrhData sizeIconData)
        {
            _move = moveIconData;
            _scale = sizeIconData;

            _moveSize = moveIconData.Size;
            _scaleSize = _scale.Size;
        }

        /// <summary>
        /// Creates a series of transformation boxes around an entity
        /// </summary>
        /// <param name="entity">Entity to create the transformation boxes for</param>
        /// <param name="destList">Pre-existing list to add the results to</param>
        public static void SurroundEntity(Entity entity, ICollection<TransBox> destList)
        {
            var ret = SurroundEntity(entity);
            foreach (TransBox box in ret)
            {
                destList.Add(box);
            }
        }

        /// <summary>
        /// Creates a series of transformation boxes around an entity
        /// </summary>
        /// <param name="entity">Entity to create the transformation boxes for</param>
        public static List<TransBox> SurroundEntity(Entity entity)
        {
            var ret = new List<TransBox>(9);

            Vector2 min = entity.CB.Min;
            Vector2 max = entity.CB.Max;

            Vector2 halfScaleSize = ScaleSize / 2f;

            // Find the center of the sides for the resize and move icons
            Vector2 sizeCenter = min + (entity.CB.Size / 2f) - halfScaleSize;
            sizeCenter = sizeCenter.Round();

            float moveCenterX = min.X + (entity.CB.Size.X / 2f) - (MoveSize.X / 2f);
            moveCenterX = (float)Math.Round(moveCenterX);

            // Move box
            ret.Add(new TransBox(TransBoxType.Move, entity, new Vector2(moveCenterX, min.Y - Move.Height - 8)));

            // Four corners
            ret.Add(new TransBox(TransBoxType.TopLeft, entity, new Vector2(min.X - ScaleSize.X, min.Y - Scale.Height)));
            ret.Add(new TransBox(TransBoxType.TopRight, entity, new Vector2(max.X, min.Y - ScaleSize.Y)));
            ret.Add(new TransBox(TransBoxType.BottomLeft, entity, new Vector2(min.X - ScaleSize.X, max.Y)));
            ret.Add(new TransBox(TransBoxType.BottomRight, entity, max));

            // Horizontal sides
            if (entity.CB.Size.X > ScaleSize.X + 4)
            {
                ret.Add(new TransBox(TransBoxType.Top, entity, new Vector2(sizeCenter.X, min.Y - ScaleSize.Y)));
                ret.Add(new TransBox(TransBoxType.Bottom, entity, new Vector2(sizeCenter.X, max.Y)));
            }

            // Veritcal sides
            if (entity.CB.Size.Y > ScaleSize.Y + 4)
            {
                ret.Add(new TransBox(TransBoxType.Left, entity, new Vector2(min.X - ScaleSize.X, sizeCenter.Y)));
                ret.Add(new TransBox(TransBoxType.Right, entity, new Vector2(max.X, sizeCenter.Y)));
            }

            return ret;
        }
    }
}