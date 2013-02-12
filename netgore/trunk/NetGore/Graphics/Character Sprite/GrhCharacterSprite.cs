using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A very basic, primitive, and restrictive implementation of <see cref="ICharacterSprite"/> that draws the
    /// character's sprite using the Grh system. This is only provided for demonstration purposes for top-down
    /// view, and is recommended you implement your own more powerful <see cref="ICharacterSprite"/> if you are
    /// not using skeleton sprites.
    /// </summary>
    public class GrhCharacterSprite : ICharacterSprite
    {
        readonly Entity _character;
        readonly Grh _bodyGrh = new Grh(null);
        readonly SpriteCategory _rootCategory;

        /// <summary>
        /// The <see cref="Direction"/> the character was facing when the body modifier was set.
        /// </summary>
        Direction _bodyModifierDirection;

        /// <summary>
        /// The current body. This joins with the root category to create the sprite category to grab the sprites from.
        /// </summary>
        string _bodyName = string.Empty;

        /// <summary>
        /// The current modifier, which will animate once then revert back to the <see cref="_currentSet"/>.
        /// </summary>
        string _currentBodyModifier;

        /// <summary>
        /// The current direction of the character.
        /// </summary>
        Direction _currentHeading;

        /// <summary>
        /// The current set. This joins with the body name as the sprite title.
        /// </summary>
        string _currentSet = string.Empty;

        TickCount _currentTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhCharacterSprite"/> class.
        /// </summary>
        /// <param name="character">The character this <see cref="GrhCharacterSprite"/> is for.</param>
        /// <param name="rootCategory">The root category for the character sprites.</param>
        public GrhCharacterSprite(Entity character, SpriteCategory rootCategory)
        {
            _character = character;
            _rootCategory = rootCategory;
        }

        /// <summary>
        /// Gets the name used for the set for a given <paramref name="direction"/>.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>The name used for the set for a given <paramref name="direction"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="direction"/> contains a value not defined by the <see cref="Direction"/>
        /// enum.</exception>
        static string GetDirectionSetName(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                case Direction.NorthWest:
                case Direction.NorthEast:
                    return "Up";

                case Direction.None:
                case Direction.South:
                case Direction.SouthWest:
                case Direction.SouthEast:
                    return "Down";

                case Direction.East:
                    return "Right";

                case Direction.West:
                    return "Left";
            }

            throw new ArgumentOutOfRangeException("direction");
        }

        /// <summary>
        /// Gets the <see cref="GrhData"/> for a set.
        /// </summary>
        /// <param name="bodyName">The name of the body (determines the category to use).</param>
        /// <param name="setName">The name of the set (the sprite categorization title).</param>
        /// <returns>The <see cref="GrhData"/> for the given body and set, or null if not found.</returns>
        GrhData GetSetGrhData(string bodyName, string setName)
        {
            return GrhInfo.GetData(_rootCategory + SpriteCategorization.Delimiter + bodyName, setName);
        }

        /// <summary>
        /// Sets the primary set, which in this case is just the sprite's title.
        /// </summary>
        /// <param name="setName">The name of the set.</param>
        /// <param name="animType">The AnimType to use.</param>
        void InternalSetSet(string setName, AnimType animType = AnimType.Loop)
        {
            _currentSet = setName;

            var grhData = GetSetGrhData(_bodyName, setName);
            if (grhData == null)
                return;

            if (_bodyGrh.GrhData == grhData)
                return;

            _bodyGrh.SetGrh(grhData, animType, _currentTime);
        }

        #region ICharacterSprite Members

        /// <summary>
        /// Gets or sets if paperdolling is enabled.
        /// </summary>
        public bool Paperdoll { get; set; }

        /// <summary>
        /// Gets the character this <see cref="ICharacterSprite"/> is drawing the sprite for.
        /// </summary>
        public Entity Character
        {
            get { return _character; }
        }

        public Vector2 SpriteSize
        {
            get { return _bodyGrh.Size; }
        }

        /// <summary>
        /// Adds a sprite body modifier that alters some, but not all, of the body. <see cref="ICharacterSprite"/>s
        /// that do not support dynamic sprites treat this the same as <see cref="ICharacterSprite.SetBody"/>.
        /// </summary>
        /// <param name="bodyModifierName">The name of the sprite body modifier.</param>
        public void AddBodyModifier(string bodyModifierName)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(_currentBodyModifier, bodyModifierName))
                return;

            // Update the sprite
            var grhData = GetSetGrhData(_bodyName, bodyModifierName + " " + GetDirectionSetName(_currentHeading));
            if (grhData == null)
                return;

            // Set the new modifier
            _currentBodyModifier = bodyModifierName;
            _bodyModifierDirection = _currentHeading;

            // Set the animation to loop once
            InternalSetSet(bodyModifierName + " " + GetDirectionSetName(_bodyModifierDirection), AnimType.LoopOnce);
        }

        /// <summary>
        /// Draws the <see cref="ICharacterSprite"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw with.</param>
        /// <param name="position">The position to draw the sprite.</param>
        /// <param name="heading">The character's heading.</param>
        /// <param name="color">The color of the sprite.</param>
        public void Draw(ISpriteBatch spriteBatch, Vector2 position, Direction heading, Color color)
        {
            // If we have a body modifier being used, invalidate it if:
            // 1. The heading has changed.
            // 2. The animation has ended.
            //
            // If we don't have a body modifier being used, just ensure we have the correct Set being used.
            //
            // If we are moving, always use the walking animation.

            _currentHeading = heading;

            // If the body modifier is set, check if it needs to be unset
            if (_currentBodyModifier != null)
            {
                if (_bodyGrh.AnimType == AnimType.None || _bodyModifierDirection != heading)
                    _currentBodyModifier = null;
            }

            // If we are moving, the body modifier is not set, or the sprite is invalid, use the non-modifier set
            if (Character.Velocity != Vector2.Zero || _currentBodyModifier == null || _bodyGrh.GrhData == null)
            {
                var prefix = (Character.Velocity == Vector2.Zero ? string.Empty : "Walk ");
                var directionSuffix = GetDirectionSetName(heading);

                _currentBodyModifier = null;
                InternalSetSet(prefix + directionSuffix);
            }

            // Ensure the sprite is valid before trying to update and draw it
            if (_bodyGrh.GrhData == null)
                return;

            // Update
            _bodyGrh.Update(_currentTime);

            position += new Vector2(Character.Size.X / 2f, Character.Size.Y - _bodyGrh.Size.Y);

            // Get the GrhDatas to draw, along with their draw order
            List<Tuple<int, GrhData, PaperDollLayerType>> grhDatas = new List<Tuple<int, GrhData, PaperDollLayerType>>();
            grhDatas.Add(new Tuple<int, GrhData, PaperDollLayerType>(GetLayerOrder(PaperDollLayerType.Body, heading), _bodyGrh.GrhData, PaperDollLayerType.Body));

            if (Paperdoll)
            {
                string setSuffix = !string.IsNullOrEmpty(_currentSet) ? "." + _currentSet : "";
                if (_layers != null)
                {
                    foreach (var layerName in _layers)
                    {
                        GrhData gd = GrhInfo.GetData(new SpriteCategorization("Character." + layerName + setSuffix));
                        if (gd == null)
                            continue;

                        PaperDollLayerType layerType = GetPaperDollLayerType(layerName);
                        int layerOrder = GetLayerOrder(layerType, heading);
                        grhDatas.Add(new Tuple<int, GrhData, PaperDollLayerType>(layerOrder, gd, layerType));
                    }
                }
            }

            // Sort by layer order
            grhDatas = grhDatas.OrderBy(x => x.Item1).ToList();

            // Draw in order
            var drawingGrh = _bodyGrh.DeepCopy();
            for (int i = 0; i < grhDatas.Count; i++)
            {
                GrhData gd = grhDatas[i].Item2;

                // Set frame
                GrhData gdFrame = gd.GetFrame((int)Math.Floor(_bodyGrh.Frame)) ?? gd.Frames.LastOrDefault();
                if (gdFrame == null)
                    continue;

                drawingGrh.SetGrh(gdFrame);

                // Get offset
                Vector2 sizeXOffset = new Vector2(drawingGrh.Size.X / -2f, 0);
                Vector2 layerOffset = GetPaperDollLayerOffset(grhDatas[i].Item3);

                // Draw
                drawingGrh.Draw(spriteBatch, position + layerOffset + sizeXOffset, color);
            }
        }

        /// <summary>
        /// Sets the sprite's body, which describes the components to use to draw a Set.
        /// </summary>
        /// <param name="bodyName">The name of the sprite body.</param>
        public void SetBody(string bodyName)
        {
            if (_bodyName.Equals(bodyName, StringComparison.OrdinalIgnoreCase))
                return;

            _bodyName = bodyName;
            _currentSet = string.Empty;
        }

        string[] _layers;

        /// <summary>
        /// Sets the sprite's paper doll layers. This will set all of the layers at once. Layers that are not in the
        /// <paramref name="layers"/> collection should be treated as they are not used and be removed, not be treated
        /// as they are just not updating.
        /// </summary>
        /// <param name="layers">The name of the paper doll layers.</param>
        public void SetPaperDollLayers(IEnumerable<string> layers)
        {
            // Store the list of the layers to be used later when drawing
            _layers = layers == null ? null : layers.ToArray();
        }

        /// <summary>
        /// Sets the Set that describes how the sprite is laid out.
        /// </summary>
        /// <param name="setName">The name of the Set.</param>
        /// <param name="bodySize">The size of the body.</param>
        public void SetSet(string setName, Vector2 bodySize)
        {
        }

        /// <summary>
        /// Updates the <see cref="ICharacterSprite"/>.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public void Update(TickCount currentTime)
        {
            _currentTime = currentTime;
        }

        #endregion

        /// <summary>
        /// Enum of the different layer types.
        /// </summary>
        enum PaperDollLayerType : byte
        {
            Weapon,
            Hat,
            Body,
        }

        /// <summary>
        /// Gets the drawing position offset to use for a paper doll layer. The offset is relative to the original drawing position (0,0).
        /// </summary>
        /// <param name="layerType">The paper doll layer.</param>
        /// <returns>The drawing offset</returns>
        static Vector2 GetPaperDollLayerOffset(PaperDollLayerType layerType)
        {
            // No offsets defined by default
            return Vector2.Zero;
        }

        /// <summary>
        /// Uses the layer name to figure out the layer type.
        /// </summary>
        static PaperDollLayerType GetPaperDollLayerType(string layerName)
        {
            if (layerName.StartsWith("Weapon", StringComparison.OrdinalIgnoreCase))
                return PaperDollLayerType.Weapon;
            else if (layerName.StartsWith("Hat", StringComparison.OrdinalIgnoreCase))
                return PaperDollLayerType.Hat;
            else
                return PaperDollLayerType.Body;
        }

        /// <summary>
        /// Uses the layer type and direction to determine the draw order. A lower draw order gets drawn first (behind), while a higher draw
        /// order gets drawn last (on top).
        /// </summary>
        static int GetLayerOrder(string layer, Direction heading)
        {
            return GetLayerOrder(GetPaperDollLayerType(layer), heading);
        }

        /// <summary>
        /// Uses the layer type and direction to determine the draw order. A lower draw order gets drawn first (behind), while a higher draw
        /// order gets drawn last (on top).
        /// </summary>
        static int GetLayerOrder(PaperDollLayerType layerType, Direction heading)
        {
            switch (heading)
            {
                case Direction.North:
                case Direction.NorthWest:
                case Direction.NorthEast:
                    switch (layerType)
                    {
                        case PaperDollLayerType.Hat: return 30;
                        case PaperDollLayerType.Body: return 20;
                        case PaperDollLayerType.Weapon: return 10;
                    }
                    break;

                case Direction.South:
                case Direction.SouthWest:
                case Direction.SouthEast:
                    switch (layerType)
                    {
                        case PaperDollLayerType.Weapon: return 30;
                        case PaperDollLayerType.Hat: return 20;
                        case PaperDollLayerType.Body: return 10;
                    }
                    break;

                case Direction.East:
                    switch (layerType)
                    {
                        case PaperDollLayerType.Weapon: return 30;
                        case PaperDollLayerType.Body: return 20;
                        case PaperDollLayerType.Hat: return 10;
                    }
                    break;

                case Direction.West:
                    switch (layerType)
                    {
                        case PaperDollLayerType.Body: return 30;
                        case PaperDollLayerType.Weapon: return 20;
                        case PaperDollLayerType.Hat: return 10;
                    }
                    break;
            }

            return 0;
        }
    }
}