using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using IDrawable = NetGore.Graphics.IDrawable;

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
        string _description;
        string _name;
        ItemType _type;
        int _value;

        /// <summary>
        /// Notifies listeners that this <see cref="Entity"/> was picked up
        /// </summary>
        public override event EntityEventHandler<CharacterEntity> OnPickup
        {
            add { }
            remove { }
        }

        public ItemEntity()
        {
            _grh = new Grh(null);
        }

        public ItemEntity(GrhIndex graphicIndex, byte amount, int currentTime) : base(Vector2.Zero, Vector2.Zero)
        {
            // NOTE: Can I get rid of this constructor?
            _amount = amount;

            _name = string.Empty;
            _description = string.Empty;
            _value = 0;

            _grh = new Grh(GrhInfo.GetData(graphicIndex), AnimType.Loop, currentTime);
        }

        public ItemEntity(MapEntityIndex mapEntityIndex, Vector2 pos, Vector2 size, GrhIndex graphicIndex, int currentTime)
            : base(pos, size)
        {
            // NOTE: Can I get rid of this constructor?
            MapEntityIndex = mapEntityIndex;

            _amount = 0;
            _name = string.Empty;
            _description = string.Empty;
            _value = 0;

            _grh = new Grh(GrhInfo.GetData(graphicIndex), AnimType.Loop, currentTime);
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
            return new ItemEntity(MapEntityIndex, Position, CB.Size, GraphicIndex, _grh.LastUpdated);
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

            if (_grh != null)
                _grh.Draw(sb, pos);
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
        /// Draws the ItemEntity
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        public void Draw(SpriteBatch sb)
        {
            Draw(sb, Position);
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
        /// Notifies when the ItemEntity's render layer changes (which is never for ItemEntity)
        /// </summary>
        public event MapRenderLayerChange OnChangeRenderLayer
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Checks if in view of the specified camera
        /// </summary>
        /// <param name="camera">Camera to check if in view of</param>
        /// <returns>True if in view of the camera, else false</returns>
        public bool InView(Camera2D camera)
        {
            return camera.InView(this);
        }

        #endregion
    }
}