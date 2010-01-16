using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    public sealed class ClientPacketIDHelper : EnumIOHelper<ClientPacketID>
    {
        static readonly ClientPacketIDHelper _instance;

        /// <summary>
        /// Initializes the <see cref="ClientPacketIDHelper"/> class.
        /// </summary>
        static ClientPacketIDHelper()
        {
            _instance = new ClientPacketIDHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPacketIDHelper"/> class.
        /// </summary>
        ClientPacketIDHelper()
        {
        }

        /// <summary>
        /// Gets the <see cref="ClientPacketIDHelper"/> instance.
        /// </summary>
        public static ClientPacketIDHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="ClientPacketID"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="ClientPacketID"/>.</returns>
        public override ClientPacketID FromInt(int value)
        {
            return (ClientPacketID)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="ClientPacketID"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        public override int ToInt(ClientPacketID value)
        {
            return (int)value;
        }
    }
}