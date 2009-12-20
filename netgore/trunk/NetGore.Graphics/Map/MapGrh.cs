using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// A Grh instance bound to the map. Although it inherits the <see cref="IDrawable"/> interface,
    /// it is not actually an <see cref="Entity"/>. It is simply a container for a map-bound Grh with no behavior
    /// besides rendering and updating, and resides completely on the Client.
    /// </summary>
    public class MapGrh : IDrawable
    {
        readonly Grh _grh;
        Vector2 _destination;
        bool _isForeground;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrh"/> class.
        /// </summary>
        /// <param name="grh">Grh to draw.</param>
        /// <param name="dest">Position to draw on the map.</param>
        /// <param name="isForeground">If true, this will be drawn in the foreground layer. If false,
        /// it will be drawn in the background layer.</param>
        public MapGrh(Grh grh, Vector2 dest, bool isForeground)
        {
            if (grh == null)
            {
                Debug.Fail("grh is null.");
                return;
            }

            _grh = grh;
            _destination = dest;
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

            _destination = reader.ReadVector2("Position");
            GrhIndex grhIndex = reader.ReadGrhIndex("GrhIndex");
            _isForeground = reader.ReadBool("IsForeground");

            _grh = new Grh(grhIndex, AnimType.Loop, currentTime);
        }

        /// <summary>
        /// Gets or sets the destination to draw the <see cref="MapGrh"/>.
        /// </summary>
        public Vector2 Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        /// <summary>
        /// Gets the <see cref="Grh"/> for the <see cref="MapGrh"/>.
        /// </summary>
        public Grh Grh
        {
            get { return _grh; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="MapGrh"/> is in the foreground.
        /// </summary>
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
        /// Checks if the given point is over the <see cref="MapGrh"/>.
        /// </summary>
        /// <param name="p">World point to check.</param>
        /// <returns>True if the point is at the <see cref="MapGrh"/>, else false.</returns>
        public bool HitTest(Vector2 p)
        {
            return (Destination.X <= p.X && Destination.X + Grh.Width >= p.X && Destination.Y <= p.Y &&
                    Destination.Y + Grh.Height >= p.Y);
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
            writer.Write("Position", Destination);
            writer.Write("GrhIndex", Grh.GrhData.GrhIndex);
            writer.Write("IsForeground", IsForeground);
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
            return camera.InView(_grh, _destination);
        }

        /// <summary>
        /// Gets the <see cref="MapRenderLayer"/> that this object is rendered on.
        /// </summary>
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
        /// Notifies listeners that the object's <see cref="MapRenderLayer"/> has changed.
        /// </summary>
        public event MapRenderLayerChange OnChangeRenderLayer;

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(SpriteBatch sb)
        {
            _grh.Draw(sb, _destination, Color.White);
        }

        #endregion
    }
}