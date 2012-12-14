using System.Linq;
using NetGore;
using NetGore.Features.ActionDisplays;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame
{
    /// <summary>
    /// Contains static data and methods for the game that describes how some different aspects of the general game works.
    /// </summary>
    /// <seealso cref="EngineSettingsInitializer"/>
    public static class GameData
    {
        /// <summary>
        /// If a User is allowed to move while they have a chat dialog open with a NPC.
        /// </summary>
        public const bool AllowMovementWhileChattingToNPC = false;

        /// <summary>
        /// The velocity to assume all character movement animations are made at. That is, if the character is moving
        /// with a velocity equal to this value, the animation will update at the usual speed. If it is twice as much
        /// as this value, the character's animation will update twice as fast. This is to make the rate a character
        /// moves proportionate to the rate their animation is moving.
        /// </summary>
        public const float AnimationSpeedModifier = 0.13f;

        /// <summary>
        /// The base (default) attack timeout value for all characters.
        /// </summary>
        public const int AttackTimeoutDefault = 500;

        /// <summary>
        /// The minimum attack timeout value. The attack timeout may never be less than this value.
        /// </summary>
        public const int AttackTimeoutMin = 150;

        /// <summary>
        /// The IP address to use by default when creating accounts when no IP can be specified, such as if the account
        /// is created from the console.
        /// </summary>
        public const uint DefaultCreateAccountIP = 0;

        /// <summary>
        /// Maximum number of characters allowed in a single account.
        /// If you update this value, you must also update the `max_character_count` parameter in the
        /// stored database function `create_user_on_account`.
        /// </summary>
        public const byte MaxCharactersPerAccount = 9;

        /// <summary>
        /// Maximum length of a Say packet's string from the client to the server.
        /// </summary>
        public const int MaxClientSayLength = 250;

        /// <summary>
        /// The maximum size (number of different item sets) of the Inventory. Any slot greater than or equal to
        /// the MaxInventorySize is considered invalid.
        /// </summary>
        public const int MaxInventorySize = 6 * 6;

        /// <summary>
        /// The maximum number of pixels a user may be away from a NPC to be able to talk to it.
        /// </summary>
        public const int MaxNPCChatDistance = 16;

        /// <summary>
        /// Maximum length of each parameter string in the server's SendMessage.
        /// </summary>
        public const int MaxServerMessageParameterLength = 250;

        /// <summary>
        /// Maximum length of a Say packet's string from the server to the client.
        /// </summary>
        public const int MaxServerSayLength = 500;

        /// <summary>
        /// Maximum length of the Name string used by the server's Say messages.
        /// </summary>
        public const int MaxServerSayNameLength = 60;

        /// <summary>
        /// The maximum distance a target can be from a character for them to be targeted.
        /// </summary>
        public const float MaxTargetDistance = 500f;

        static readonly StringRules _accountEmail = new StringRules(3, 30, CharType.Alpha | CharType.Numeric | CharType.Punctuation);
        static readonly StringRules _accountName = new StringRules(3, 30, CharType.Alpha | CharType.Numeric);
        static readonly StringRules _accountPassword = new StringRules(3, 30, CharType.Alpha | CharType.Numeric | CharType.Punctuation);
        static readonly StringRules _characterName = new StringRules(1, 30, CharType.Alpha | CharType.Numeric | CharType.Whitespace);
        static readonly ActionDisplayID _defaultActionDisplayID = new ActionDisplayID(0);
        static readonly Vector2 _screenSize = new Vector2(1024, 768);
        static readonly StringRules _userName = new StringRules(3, 15, CharType.Alpha);

        /// <summary>
        /// Gets the rules for the account email addresses.
        /// </summary>
        public static StringRules AccountEmail
        {
            get { return _accountEmail; }
        }

        /// <summary>
        /// Gets the rules for the account names.
        /// </summary>
        public static StringRules AccountName
        {
            get { return _accountName; }
        }

        /// <summary>
        /// Gets the rules for the account passwords.
        /// </summary>
        public static StringRules AccountPassword
        {
            get { return _accountPassword; }
        }

        /// <summary>
        /// Gets the rules for the character names.
        /// </summary>
        public static StringRules CharacterName
        {
            get { return _characterName; }
        }

        /// <summary>
        /// Gets the default <see cref="ActionDisplayID"/> to use when none is specified.
        /// </summary>
        public static ActionDisplayID DefaultActionDisplayID
        {
            get { return _defaultActionDisplayID; }
        }

        /// <summary>
        /// Gets the maximum delta time between draws for any kind of drawable component. If the delta time between
        /// draw calls on the component exceeds this value, the delta time should then be reduced to be equal to this value.
        /// </summary>
        public static int MaxDrawDeltaTime
        {
            get { return 100; }
        }

        /// <summary>
        /// Gets the size of the screen display.
        /// </summary>
        public static Vector2 ScreenSize
        {
            get { return _screenSize; }
        }

        /// <summary>
        /// Gets the rules for the user names.
        /// </summary>
        public static StringRules UserName
        {
            get { return _userName; }
        }

        /// <summary>
        /// Gets the number of milliseconds between each World update step. This only applies to the synchronized
        /// physics, not client-side visuals.
        /// </summary>
        public static int WorldPhysicsUpdateRate
        {
            get { return 20; }
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> containing the hit area for a melee attack.
        /// </summary>
        /// <param name="c">The <see cref="CharacterEntity"/> that is attacking.</param>
        /// <param name="range">The range of the attack.</param>
        /// <returns>The <see cref="Rectangle"/> that describes the hit area for a melee attack.</returns>
        public static Rectangle GetMeleeAttackArea(CharacterEntity c, ushort range)
        {
            // Start with the rect for the char's area
            var ret = c.ToRectangle();

            // Add the range to the width
            ret.Width += range;

            // If looking left, subtract the range from the X position so that the area is to the left, not right
            if (c.Heading == Direction.West || c.Heading == Direction.SouthWest || c.Heading == Direction.NorthWest)
                ret.X -= range;

            return ret;
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the valid area that items can be picked up at from
        /// the given <paramref name="spatial"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> doing the picking-up.</param>
        /// <returns>A <see cref="Rectangle"/> that represents the area region that items can be picked up at from
        /// the given <paramref name="spatial"/>.</returns>
        public static Rectangle GetPickupArea(ISpatial spatial)
        {
            const int padding = 70;
            return spatial.ToRectangle().Inflate(padding);
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that describes all of the potential area that
        /// a ranged attack can reach.
        /// </summary>
        /// <param name="c">The <see cref="Entity"/> that is attacking.</param>
        /// <param name="range">The range of the attack.</param>
        /// <returns>A <see cref="Rectangle"/> that describes all of the potential area that
        /// a ranged attack can reach.</returns>
        public static Rectangle GetRangedAttackArea(Entity c, ushort range)
        {
            return c.ToRectangle().Inflate(range);
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> containing the area that the <paramref name="shopper"/> may use to
        /// shop. Any <see cref="Entity"/> that owns a shop and intersects with this area is considered in a valid
        /// distance to shop with.
        /// </summary>
        /// <param name="shopper">The <see cref="Entity"/> doing the shopping.</param>
        /// <returns>A <see cref="Rectangle"/> containing the area that the <paramref name="shopper"/> may use to
        /// shop.</returns>
        public static Rectangle GetValidShopArea(ISpatial shopper)
        {
            return shopper.ToRectangle();
        }

        /// <summary>
        /// Gets if the <paramref name="shopper"/> is close enough to the <paramref name="shopOwner"/> to shop.
        /// </summary>
        /// <param name="shopper">The <see cref="Entity"/> doing the shopping.</param>
        /// <param name="shopOwner">The <see cref="Entity"/> that owns the shop.</param>
        /// <returns>True if the <paramref name="shopper"/> is close enough to the <paramref name="shopOwner"/> to
        /// shop; otherwise false.</returns>
        public static bool IsValidDistanceToShop(ISpatial shopper, ISpatial shopOwner)
        {
            var area = GetValidShopArea(shopper);
            return shopOwner.Intersects(area);
        }

        /// <summary>
        /// Checks if an <see cref="ISpatial"/> is close enough to another <see cref="ISpatial"/> to pick it up.
        /// </summary>
        /// <param name="grabber">The <see cref="ISpatial"/> doing the picking up.</param>
        /// <param name="toGrab">The <see cref="ISpatial"/> being picked up.</param>
        /// <returns>True if the <paramref name="grabber"/> is close enough to the <paramref name="toGrab"/>
        /// to pick it up; otherwise false.</returns>
        public static bool IsValidPickupDistance(ISpatial grabber, ISpatial toGrab)
        {
            var region = GetPickupArea(grabber);
            return toGrab.Intersects(region);
        }

        /// <summary>
        /// Gets the experience required for a given level.
        /// </summary>
        /// <param name="x">Level to check (current level).</param>
        /// <returns>Experience required for the given level.</returns>
        public static int LevelCost(int x)
        {
            return x * 30;
        }

        /// <summary>
        /// Converts the integer-based movement speed to a real velocity value.
        /// </summary>
        /// <param name="movementSpeed">The integer-based movement speed.</param>
        /// <returns>The real velocity value for the given integer-based movement speed.</returns>
        public static float MovementSpeedToVelocity(int movementSpeed)
        {
            return movementSpeed / 10000.0f;
        }

        /// <summary>
        /// Gets the points required for a given stat level.
        /// </summary>
        /// <param name="x">Stat level to check (current stat level).</param>
        /// <returns>Points required for the given stat level.</returns>
        public static int StatCost(int x)
        {
            return 1;
        }

        /// <summary>
        /// Converts a real velocity value to the integer-based movement speed.
        /// </summary>
        /// <param name="velocity">The real velocity value.</param>
        /// <returns>The integer-based movement speed for the given real velocity value.</returns>
        public static int VelocityToMovementSpeed(float velocity)
        {
            return (int)(velocity * 10000.0f);
        }
    }
}