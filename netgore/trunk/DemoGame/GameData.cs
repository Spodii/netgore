using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Extensions;

namespace DemoGame
{
    /// <summary>
    /// Contains static data for the game.
    /// </summary>
    public static class GameData
    {
        /// <summary>
        /// Gets the number of milliseconds between each World update step. This only applies to the synchronized
        /// physics, not client-side visuals.
        /// </summary>
        public static int WorldPhysicsUpdateRate { get { return 20; } }

        /// <summary>
        /// Gets the maximum delta time between draws for any kind of drawable component. If the delta time between
        /// draw calls on the component exceeds this value, the delta time should then be reduced to be equal to this value.
        /// </summary>
        public static int MaxDrawDeltaTime { get { return 100; } }

        /// <summary>
        /// Maximum length of a Say packet's string from the client to the server.
        /// </summary>
        public const int MaxClientSayLength = 255;

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
        /// Gets the IP address of the server.
        /// </summary>
        public static string ServerIP { get { return "127.0.0.1"; } }

        /// <summary>
        /// Gets the port used by the server for handling pings.
        /// </summary>
        public static int ServerPingPort { get { return 44446; } }

        /// <summary>
        /// Gets the port used by the server for TCP connections.
        /// </summary>
        public static int ServerTCPPort { get { return 44445; } }

        /// <summary>
        /// Size of the screen (ScreenWidth / ScreenHeight) represented in a Vector2
        /// </summary>
        public static Vector2 ScreenSize = new Vector2(800, 600);

        /// <summary>
        /// RegEx for name and password validation
        /// </summary>
        static readonly Regex _namePassRegex = new Regex("^[a-zA-Z0-9]{3,15}$", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Array of all the body information
        /// </summary>
        static BodyInfo[] _bodyInfo;

        /// <summary>
        /// Retreives the information of a body by a given index.
        /// </summary>
        /// <param name="index">Index of the body.</param>
        /// <returns>Body information for the index.</returns>
        public static BodyInfo Body(int index)
        {
            // TODO: Move this crap out of GameData. Body data should be with the BodyInfo class.
            if (index < _bodyInfo.Length)
                return _bodyInfo[index];
            else
                return null;
        }

        /// <summary>
        /// Checks if a character's name is valid
        /// </summary>
        /// <param name="name">Name of the character</param>
        /// <returns>True if valid, else false</returns>
        public static bool IsValidCharName(string name)
        {
            return _namePassRegex.IsMatch(name);
        }

        /// <summary>
        /// Checks if a password is valid
        /// </summary>
        /// <param name="name">Name of the character</param>
        /// <returns>True if valid, else false</returns>
        public static bool IsValidPassword(string name)
        {
            return _namePassRegex.IsMatch(name);
        }

        /// <summary>
        /// Gets the experience required for a given level
        /// </summary>
        /// <param name="x">Level to check (current level)</param>
        /// <returns>Experience required for the given level</returns>
        public static int LevelCost(int x)
        {
            float z = x + 1;
            float y = 25f * z + 75f * (float)Math.Pow(z, 2.1f + z * 0.003f);
            return (int)y;
        }

        /// <summary>
        /// Loads the game data
        /// </summary>
        public static void Load()
        {
            PathString path = ContentPaths.Build.Data.Join("bodies.xml");
            _bodyInfo = BodyInfo.Load(path);
        }

        /// <summary>
        /// Gets the experience required for a given stat level
        /// </summary>
        /// <param name="x">Stat level to check (current stat level)</param>
        /// <returns>Experience required for the given stat level</returns>
        public static int StatCost(int x)
        {
            float y = 5f + 3.5f * (float)Math.Pow(x, 2f + x * 0.001f);
            return (int)y;
        }

        /// <summary>
        /// Gets if the distance between two points is short enough to allow picking-up.
        /// </summary>
        /// <param name="source">The Entity doing the picking-up.</param>
        /// <param name="target">The Entity to be picked-up.</param>
        /// <returns>True if the <paramref name="source"/> is close enough to the <paramref name="target"/> to
        /// pick it up, otherwise false.</returns>
        public static bool ValidServerPickupDistance(Entity source, Entity target)
        {
            // TODO: Make use of this!
            const float maxDistance = 200.0f;
            float dist = source.Position.QuickDistance(target.Position);
            return dist < maxDistance;
        }
    }
}