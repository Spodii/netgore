using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using IDrawable=NetGore.Graphics.IDrawable;

// LATER: When editing Grhs and changing texture, change the preview
// LATER: When updating a Grh, update the preview image
// LATER: The client doesn't seem to ever use the item's name, desc, or value

namespace DemoGame.Client
{
    /// <summary>
    /// An item on the client. Used for representing any item, whether it is actually an Entity
    /// on the map, or just an item in an inventory.
    /// </summary>
    public class ItemEntity : ItemEntityBase, IDrawable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly Grh _grh;

        byte _amount = 1;
        string _description = string.Empty;
        string _name = string.Empty;
        ItemType _type;
        int _value = 0;

        public ItemEntity() : base(Vector2.Zero, Vector2.Zero)
        {
            _grh = new Grh(null);
        }

        public ItemEntity(GrhIndex graphicIndex, byte amount, int currentTime) : base(Vector2.Zero, Vector2.Zero)
        {
            _amount = amount;
            _grh = new Grh(GrhInfo.GetData(graphicIndex), AnimType.Loop, currentTime);
        }

        public ItemEntity(MapEntityIndex mapEntityIndex, Vector2 pos, Vector2 size, GrhIndex graphicIndex, int currentTime)
            : base(pos, size)
        {
            MapEntityIndex = mapEntityIndex;
            _amount = 0;
            _grh = new Grh(GrhInfo.GetData(graphicIndex), AnimType.Loop, currentTime);
        }

        /// <summary>
        /// Notifies listeners that this <see cref="Entity"/> was picked up
        /// </summary>
        public override event EntityEventHandler<CharacterEntity> OnPickup
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Gets or sets the size of this item cluster (1 for a single item)
        /// </summary>
        public override byte Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        /// <summary>
        /// Gets or sets the description of the item
        /// </summary>
        public override string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the index of the graphic that is used for this item
        /// </summary>
        public override GrhIndex GraphicIndex
        {
            get { return _grh.GrhData.GrhIndex; }
            set { _grh.SetGrh(value); }
        }

        /// <summary>
        /// Gets the Grh used to draw this ItemEntity
        /// </summary>
        public Grh Grh
        {
            get { return _grh; }
        }

        /// <summary>
        /// Gets or sets the name of the item
        /// </summary>
        public override string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the type of item this is.
        /// </summary>
        public override ItemType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Gets or sets the value of the item
        /// </summary>
        public override int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Checks if this <see cref="Entity"/> can be picked up by the specified <paramref name="charEntity"/>, but does
        /// not actually pick up this <see cref="Entity"/>
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="Entity"/></param>
        /// <returns>True if this <see cref="Entity"/> can be picked up, else false</returns>
        public override bool CanPickup(CharacterEntity charEntity)
        {
            // Every character can try to pick up an item
            return true;
        }

        public override bool CanStack(ItemEntityBase source)
        {
            throw new MethodAccessException("Client has no way to know if two ItemEntities can stack since it doesn't" +
                                            " always know everything about two items.");
        }

        /// <summary>
        /// Creates a deep copy of the inheritor, which is a new class with the same values, and returns
        /// the copy as an ItemEntityBase.
        /// </summary>
        /// <returns>A deep copy of the object</returns>
        public override ItemEntityBase DeepCopy()
        {
            return new ItemEntity(MapEntityIndex, Position, Size, GraphicIndex, _grh.LastUpdated);
        }

        /// <summary>
        /// Draws the ItemEntity
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="pos">Position to draw at</param>
        public void Draw(SpriteBatch sb, Vector2 pos)
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            if (BeforeDraw != null)
                BeforeDraw(this, sb);

            if (IsVisible)
            {
                if (_grh != null)
                    _grh.Draw(sb, pos);
            }

            if (AfterDraw != null)
                AfterDraw(this, sb);
        }

        /// <summary>
        /// Picks up this <see cref="Entity"/>
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to pick up this <see cref="Entity"/></param>
        /// <returns>True if this <see cref="Entity"/> was successfully picked up, else false</returns>
        public override bool Pickup(CharacterEntity charEntity)
        {
            const string errmsg = "Client is not allowed to pick up items.";

            Debug.Fail(errmsg);
            if (log.IsErrorEnabled)
                log.Error(errmsg);

            return false;
        }

        #region IDrawable Members

        /// <summary>
        /// Unused by the <see cref="ItemEntity"/> since the layer never changes.
        /// </summary>
        event MapRenderLayerChange IDrawable.RenderLayerChanged
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Gets the depth of the object for the <see cref="IDrawable.MapRenderLayer"/> the object is on. A higher
        /// layer depth results in the object being drawn on top of (in front of) objects with a lower value.
        /// </summary>
        [Browsable(false)]
        public int LayerDepth
        {
            get { return 0; }
        }

        bool _isVisible;

        /// <summary>
        /// Gets or sets if this <see cref="IDrawable"/> will be drawn. All <see cref="IDrawable"/>s are initially
        /// visible.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible == value)
                    return;

                _isVisible = value;

                if (VisibleChanged != null)
                    VisibleChanged(this);
            }
        }

        /// <summary>
        /// Gets the MapRenderLayer for the ItemEntity
        /// </summary>
        public MapRenderLayer MapRenderLayer
        {
            get
            {
                // Items are always on the Item layer
                return MapRenderLayer.Item;
            }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.IsVisible"/> property has changed.
        /// </summary>
        public event IDrawableEventHandler VisibleChanged;

        /// <summary>
        /// Notifies listeners immediately before this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        public event IDrawableDrawEventHandler BeforeDraw;

        /// <summary>
        /// Notifies listeners immediately after this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        public event IDrawableDrawEventHandler AfterDraw;

        /// <summary>
        /// Draws the ItemEntity
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        public void Draw(SpriteBatch sb)
        {
            Draw(sb, Position);
        }

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>
        /// True if the object is in view of the camera, else False.
        /// </returns>
        public bool InView(ICamera2D camera)
        {
            return camera.InView(this);
        }

        #endregion
    }
}