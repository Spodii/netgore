using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platyform.Extensions;
using Platyform.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// A Grh instance bound to the map. Although it inherits the IDrawableEntity interface,
    /// it is not actually an Entity. It is simply a container for a map-bound Grh with no behavior
    /// besides rendering and updating.
    /// </summary>
    public class MapGrh : IDrawableEntity
    {
        readonly Grh _grh;
        Vector2 _destination;
        bool _isForeground;

        /// <summary>
        /// Gets or sets the destination to draw the MapGrh
        /// </summary>
        public Vector2 Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        /// <summary>
        /// Gets the Grh for the MapGrh
        /// </summary>
        public Grh Grh
        {
            get { return _grh; }
        }

        /// <summary>
        /// Gets or sets if the MapGrh is in the foreground
        /// </summary>
        public bool IsForeground
        {
            get { return _isForeground; }
            set
            {
                if (_isForeground != value)
                {
                    MapRenderLayer oldLayer = MapRenderLayer;
                    _isForeground = value;
                    OnChangeRenderLayer(this, oldLayer);
                }
            }
        }

        /// <summary>
        /// MapGrh constructor
        /// </summary>
        /// <param name="grh">Grh to draw</param>
        /// <param name="dest">Position to draw on the map</param>
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
        /// Checks if the given point is over the MapGrh
        /// </summary>
        /// <param name="p">World point to check</param>
        /// <returns>True if the point is at the MapGrh, else false</returns>
        public bool HitTest(Vector2 p)
        {
            return (Destination.X <= p.X && Destination.X + Grh.Width >= p.X && Destination.Y <= p.Y &&
                    Destination.Y + Grh.Height >= p.Y);
        }

        public static MapGrh Load(XmlReader r, int currentTime)
        {
            r.MoveToContent();

            r.MoveToAttribute("X");
            float x = r.ReadContentAsFloat();

            r.MoveToAttribute("Y");
            float y = r.ReadContentAsFloat();

            r.MoveToAttribute("I");
            int index = r.ReadContentAsInt();

            r.MoveToAttribute("FG");
            bool isForeground = r.ReadContentAsBoolean();

            r.Read();

            Grh grh = new Grh((ushort)index, AnimType.Loop, currentTime);
            return new MapGrh(grh, new Vector2(x, y), isForeground);
        }

        public void Save(XmlWriter w)
        {
            w.WriteStartElement("MapGrh");
            w.WriteAttributeString("X", Destination.X.ToString());
            w.WriteAttributeString("Y", Destination.Y.ToString());
            w.WriteAttributeString("I", Grh.GrhData.GrhIndex.ToString());
            w.WriteAttributeString("FG", IsForeground.ToString().ToLower());
            w.WriteEndElement();
        }

        public void Update(int currentTime)
        {
            _grh.Update(currentTime);
        }

        #region IDrawableEntity Members

        /// <summary>
        /// Checks if in view of the specified camera
        /// </summary>
        /// <param name="camera">Camera to check if in view of</param>
        /// <returns>True if in view of the camera, else false</returns>
        public bool InView(Camera2D camera)
        {
            return camera.InView(_grh, _destination);
        }

        /// <summary>
        /// Gets the MapRenderLayer for the MapGrh
        /// </summary>
        public MapRenderLayer MapRenderLayer
        {
            get
            {
                // MapGrhs can be either foreground or background
                if (IsForeground)
                    return MapRenderLayer.Foreground;
                else
                    return MapRenderLayer.Background;
            }
        }

        /// <summary>
        /// Notifies when the MapGrh's render layer changes
        /// </summary>
        public event MapRenderLayerChange OnChangeRenderLayer;

        public void Draw(SpriteBatch sb)
        {
            _grh.Draw(sb, _destination, Color.White);
        }

        #endregion
    }
}