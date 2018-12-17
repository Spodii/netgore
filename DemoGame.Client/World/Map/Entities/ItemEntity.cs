using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

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
        Color _color = Color.White;
        bool _isVisible = true;

        public ItemEntity() : base(Vector2.Zero, Vector2.Zero)
        {
            Amount = 1;

            _grh = new Grh(null);
        }

        public ItemEntity(GrhIndex graphicIndex, byte amount, TickCount currentTime) : base(Vector2.Zero, Vector2.Zero)
        {
            Amount = amount;

            _grh = new Grh(GrhInfo.GetData(graphicIndex), AnimType.Loop, currentTime);
        }

        public ItemEntity(MapEntityIndex mapEntityIndex, Vector2 pos, Vector2 size, GrhIndex graphicIndex, TickCount currentTime)
            : base(pos, size)
        {
            Amount = 0;

            ((IDynamicEntitySetMapEntityIndex)this).SetMapEntityIndex(mapEntityIndex);
            _grh = new Grh(GrhInfo.GetData(graphicIndex), AnimType.Loop, currentTime);
        }

        /// <summary>
        /// Not used by the <see cref="ItemEntity"/> in the client.
        /// </summary>
        public override event TypedEventHandler<Entity, EventArgs<CharacterEntity>> PickedUp
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Gets or sets the size of this item cluster (1 for a single item)
        /// </summary>
        public override byte Amount { get; set; }

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
        /// Checks if this <see cref="Entity"/> can be picked up by the specified <paramref name="charEntity"/>, but does
        /// not actually pick up this <see cref="Entity"/>.
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="Entity"/></param>
        /// <returns>True if this <see cref="Entity"/> can be picked up, else false.</returns>
        public override bool CanPickup(CharacterEntity charEntity)
        {
            // Every character can try to pick up an item
            return true;
        }

        /// <summary>
        /// Checks if this item can be stacked with another item. To stack, both items must contain the same
        /// stat modifiers, name, description, value, and graphic index.
        /// </summary>
        /// <param name="source">Item to check if can stack on this item</param>
        /// <returns>
        /// True if the two items can stack on each other, else false
        /// </returns>
        /// <exception cref="MethodAccessException">The client has no way to know if two ItemEntities can stack since
        /// it doesn't always know everything about two items.</exception>
        public override bool CanStack(ItemEntityBase source)
        {
            throw new MethodAccessException("The client has no way to know if two ItemEntities can stack since it doesn't" +
                                            " always know everything about two items.");
        }

        /// <summary>
        /// Creates a deep copy of the inheritor, which is a new class with the same values, and returns
        /// the copy as an ItemEntityBase.
        /// </summary>
        /// <returns>A deep copy of the object.</returns>
        public override ItemEntityBase DeepCopy()
        {
            return new ItemEntity(MapEntityIndex, Position, Size, GraphicIndex, _grh.LastUpdated);
        }

        /// <summary>
        /// Draws the ItemEntity.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="pos">Position to draw at.</param>
        /// <param name="color">The color to draw the item.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sb" /> is <c>null</c>.</exception>
        public void Draw(ISpriteBatch sb, Vector2 pos, Color color)
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            if (BeforeDraw != null)
                BeforeDraw.Raise(this, EventArgsHelper.Create(sb));

            if (IsVisible)
            {
                if (_grh != null)
                    _grh.Draw(sb, pos, color);
            }

            if (AfterDraw != null)
                AfterDraw.Raise(this, EventArgsHelper.Create(sb));
        }

        /// <summary>
        /// Draws the ItemEntity.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="pos">Position to draw at.</param>
        public void Draw(ISpriteBatch sb, Vector2 pos)
        {
            Draw(sb, pos, Color);
        }

        /// <summary>
        /// When overridden in the derived class, handles destroying the <see cref="ItemEntityBase"/>.
        /// </summary>
        protected override void HandleDestroy()
        {
            Dispose();
        }

        /// <summary>
        /// Picks up this <see cref="Entity"/>.
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to pick up this <see cref="Entity"/>.</param>
        /// <returns>True if this <see cref="Entity"/> was successfully picked up, else false.</returns>
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
        /// Notifies listeners immediately after this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        public event TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> AfterDraw;

        /// <summary>
        /// Notifies listeners immediately before this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        public event TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> BeforeDraw;

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.Color"/> property has changed.
        /// </summary>
        public event TypedEventHandler<IDrawable> ColorChanged;

        /// <summary>
        /// Unused by the <see cref="ItemEntity"/> since the layer never changes.
        /// </summary>
        event TypedEventHandler<IDrawable, ValueChangedEventArgs<MapRenderLayer>> IDrawable.RenderLayerChanged
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.IsVisible"/> property has changed.
        /// </summary>
        public event TypedEventHandler<IDrawable> VisibleChanged;

        /// <summary>
        /// Gets or sets the <see cref="IDrawable.Color"/> to use when drawing this <see cref="IDrawable"/>. By default, this
        /// value will be equal to white (ARGB: 255,255,255,255).
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color == value)
                    return;

                _color = value;

                if (ColorChanged != null)
                    ColorChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets if this <see cref="IDrawable"/> will be drawn. All <see cref="IDrawable"/>s are initially
        /// visible.
        /// </summary>
        [Browsable(false)]
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible == value)
                    return;

                _isVisible = value;

                if (VisibleChanged != null)
                    VisibleChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the depth of the object for the <see cref="IDrawable.MapRenderLayer"/> the object is on. A higher
        /// layer depth results in the object being drawn on top of (in front of) objects with a lower value.
        /// </summary>
        [Browsable(false)]
        public int LayerDepth
        {
            get { return -100; }
        }

        /// <summary>
        /// Gets the MapRenderLayer for the ItemEntity
        /// </summary>
        public MapRenderLayer MapRenderLayer
        {
            get
            {
                // Items are always on the Dynamic layer
                return MapRenderLayer.Dynamic;
            }
        }

        /// <summary>
        /// Draws the ItemEntity.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        public void Draw(ISpriteBatch sb)
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