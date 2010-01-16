using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// A Grh instance bound to the map. This is simply a container for a map-bound Grh with no behavior
    /// besides rendering and updating, and resides completely on the Client.
    /// </summary>
    public class MapGrh : ISpatial, IDrawable, IPersistable
    {
        const string _grhIndexKeyName = "GrhIndex";
        const string _isForegroundKeyName = "IsForeground";
        const string _layerDepthKeyName = "LayerDepth";
        const string _mapGrhCategoryName = "MapGrh";
        const string _positionKeyName = "Position";

        readonly Grh _grh;

        bool _isForeground;
        short _layerDepth;
        Vector2 _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrh"/> class.
        /// </summary>
        /// <param name="grh">Grh to draw.</param>
        /// <param name="position">Position to draw on the map.</param>
        /// <param name="isForeground">If true, this will be drawn in the foreground layer. If false,
        /// it will be drawn in the background layer.</param>
        public MapGrh(Grh grh, Vector2 position, bool isForeground)
        {
            if (grh == null)
            {
                Debug.Fail("grh is null.");
                return;
            }

            _grh = grh;
            _position = position;
            IsForeground = isForeground;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrh"/> class.
        /// </summary>
        /// <param name="reader">The reader to read the values from.</param>
        /// <param name="currentTime">The current time.</param>
        public MapGrh(IValueReader reader, int currentTime)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            _grh = new Grh(null, AnimType.Loop, currentTime);

            ReadState(reader);
        }

        /// <summary>
        /// Gets the <see cref="Grh"/> for the <see cref="MapGrh"/>.
        /// </summary>
        [Category(_mapGrhCategoryName)]
        [Browsable(true)]
        [DisplayName("GrhData")]
        [Description("The GrhData that is drawn for this MapGrh.")]
        [Editor(EditorHelper.GrhEditorTypeName, EditorHelper.UITypeEditorTypeName)]
        public Grh Grh
        {
            get { return _grh; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="MapGrh"/> is in the foreground layer, in front of characters and items.
        /// </summary>
        [Category(_mapGrhCategoryName)]
        [Browsable(true)]
        [DisplayName("IsForeground")]
        [Description("If the MapGrh is in the foreground layer, in front of characters and items.")]
        [DefaultValue(false)]
        public bool IsForeground
        {
            get { return _isForeground; }
            set
            {
                if (_isForeground == value)
                    return;

                MapRenderLayer oldLayer = MapRenderLayer;
                _isForeground = value;

                if (OnChangeRenderLayer != null)
                    OnChangeRenderLayer(this, oldLayer);
            }
        }

        /// <summary>
        /// Updates the <see cref="MapGrh"/>.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        public void Update(int currentTime)
        {
            _grh.Update(currentTime);
        }

        /// <summary>
        /// Writes the MapGrh to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the MapGrh to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write(_positionKeyName, Position);
            writer.Write(_grhIndexKeyName, Grh.GrhData.GrhIndex);
            writer.Write(_isForegroundKeyName, IsForeground);
            writer.Write(_layerDepthKeyName, _layerDepth);
        }

        #region IDrawable Members

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>
        /// True if the object is in view of the camera, else False.
        /// </returns>
        public bool InView(ICamera2D camera)
        {
            return camera.InView(_grh, Position);
        }

        /// <summary>
        /// Gets the <see cref="MapRenderLayer"/> that this object is rendered on.
        /// </summary>
        [Browsable(false)]
        public MapRenderLayer MapRenderLayer
        {
            get
            {
                // MapGrhs can be either foreground or background
                if (IsForeground)
                    return MapRenderLayer.SpriteForeground;
                else
                    return MapRenderLayer.SpriteBackground;
            }
        }

        /// <summary>
        /// Gets the depth of the object for the <see cref="IDrawable.MapRenderLayer"/> the object is on. A higher
        /// layer depth results in the object being drawn on top of (in front of) objects with a lower value.
        /// The value must be between short.MinValue and short.MaxValue.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than short.MinValue
        /// or greater than short.MaxValue.</exception>
        [Category(_mapGrhCategoryName)]
        [Browsable(true)]
        [DisplayName("Layer Depth")]
        [Description("The drawing depth of the object. Objects with higher values are drawn above those with lower values.")]
        [DefaultValue((byte)0)]
        public int LayerDepth
        {
            get { return _layerDepth; }
            set
            {
                if (value < short.MinValue || value > short.MaxValue)
                    throw new ArgumentOutOfRangeException("value", "value must be between short.MinValue and short.MaxValue.");

                _layerDepth = (short)value;
            }
        }

        /// <summary>
        /// Notifies listeners that the object's <see cref="MapRenderLayer"/> has changed.
        /// </summary>
        public event MapRenderLayerChange OnChangeRenderLayer;

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(SpriteBatch sb)
        {
            _grh.Draw(sb, Position, Color.White);
        }

        #endregion

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            Position = reader.ReadVector2(_positionKeyName);
            GrhIndex grhIndex = reader.ReadGrhIndex(_grhIndexKeyName);
            _isForeground = reader.ReadBool(_isForegroundKeyName);
            _layerDepth = reader.ReadShort(_layerDepthKeyName);

            if (!grhIndex.IsInvalid)
                _grh.SetGrh(grhIndex);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            writer.Write(_positionKeyName, Position);
            writer.Write(_grhIndexKeyName, Grh.GrhData != null ? Grh.GrhData.GrhIndex : GrhIndex.Invalid);
            writer.Write(_isForegroundKeyName, IsForeground);
            writer.Write(_layerDepthKeyName, _layerDepth);
        }

        #endregion

        #region ISpatial Members

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/> occupies.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/>
        /// occupies.</returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }

        /// <summary>
        /// Gets or sets the position to draw the <see cref="MapGrh"/> at.
        /// </summary>
        [Category(_mapGrhCategoryName)]
        [DisplayName("Position")]
        [Description("Location of the top-left corner of the MapGrh on the map.")]
        [Browsable(true)]
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (Position == value)
                    return;

                var oldPosition = Position;
                _position = value;

                if (OnMove != null)
                    OnMove(this, oldPosition);
            }
        }

        /// <summary>
        /// Gets the size of this <see cref="ISpatial"/>.
        /// </summary>
        [Browsable(false)]
        public Vector2 Size
        {
            get { return _grh.Size; }
        }

        /// <summary>
        /// Gets the world coordinates of the bottom-right corner of this <see cref="ISpatial"/>.
        /// </summary>
        [Browsable(false)]
        public Vector2 Max
        {
            get { return Position + Size; }
        }

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        public event SpatialMoveEventHandler OnMove;

        /// <summary>
        /// Unused by the <see cref="MapGrh"/>.
        /// </summary>
        event SpatialResizeEventHandler ISpatial.OnResize
        {
            add { }
            remove { }
        }

        #endregion
    }
}