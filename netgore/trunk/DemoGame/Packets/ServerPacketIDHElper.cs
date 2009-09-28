using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore;

namespace DemoGame
{
    public class ServerPacketIDHelper : EnumHelper<ServerPacketID>
    {
        static readonly ServerPacketIDHelper _instance;

        /// <summary>
        /// Gets the <see cref="ServerPacketIDHelper"/> instance.
        /// </summary>
        public static ServerPacketIDHelper Instance { get { return _instance; } }

        /// <summary>
        /// Initializes the <see cref="ServerPacketIDHelper"/> class.
        /// </summary>
        static ServerPacketIDHelper()
        {
            _instance = new ServerPacketIDHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerPacketIDHelper"/> class.
        /// </summary>
        ServerPacketIDHelper()
        {
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="ServerPacketID"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="ServerPacketID"/>.</returns>
        protected override ServerPacketID FromInt(int value)
        {
            return (ServerPacketID)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="ServerPacketID"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected override int ToInt(ServerPacketID value)
        {
            return (int)value;
        }
    }
}
