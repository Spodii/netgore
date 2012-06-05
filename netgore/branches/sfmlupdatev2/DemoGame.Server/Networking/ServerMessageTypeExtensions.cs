using System.Linq;
using Lidgren.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Extension methods for the <see cref="ServerMessageType"/> enum.
    /// </summary>
    public static class ServerMessageTypeExtensions
    {
#if false
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
#endif

        /// <summary>
        /// Gets the <see cref="NetDeliveryMethod"/> and sequence channel to use for a given <see cref="ServerMessageType"/>.
        /// </summary>
        /// <param name="msgType">The <see cref="ServerMessageType"/>.</param>
        /// <param name="method">The <see cref="NetDeliveryMethod"/> to use for the <paramref name="msgType"/>.</param>
        /// <param name="seqChannel">The sequence channel to use for the <paramref name="msgType"/>.</param>
        public static void GetDeliveryMethod(this ServerMessageType msgType, out NetDeliveryMethod method, out int seqChannel)
        {
            /* Some important things to keep in mind:
             * 
             *  - Sequence numbers are unique per NetDeliveryMethod. That is, seqChannel=1 for ReliableOrder is not
             *      the same as seqChannel=1 for UnreliableSequenced.
             *      
             *  - Sequence numbers are not used on Unreliable and ReliableUnordered. For these channels, always use seqChannel=0.
             *  
             *  - The max sequence number is defined by NetConstants.NetChannelsPerDeliveryMethod.
             *      
             *  - All sequences are created equally. They just provide a way to, when using ordering/sequencing, not have to
             *      stall on messages that are irrelevant. For example, say Chat and MapEffect were on the same channel. Then
             *      you send a huge chat message, then a MapEffect message shortly after. For some reason, the chat message gets
             *      lost in the network and has to be retransmitted while the MapEffect arrived very quickly. Even though they are
             *      logically irrelevant, since they use the same channel, the MapEffect will not be handled until that chat message
             *      was received. Putting them on a different sequence channel resolves this issue.
             *      
             *  - Unreliable messages are just raw UDP. Not only can they be lost forever, they can arrive out-of-order and duplicate
             *      copies can arrive.
             *      
             *  - It is perfectly fine, and even recommended, to use the same method and channel for multiple message types. Even if there
             *      is no important distinction on the network layer, it keeps them logically separated in the upper layer. This makes it
             *      very easy to change them on the network layer if needed.
             *      
             *  - Never create or use a message type based on the underlying delivery method and/or sequence channel. How it arrives should
             *      be irrelevant on the upper layer.
             *      
             *  - A delivery method and (when applicable) sequence number should be specified for every message type. Never just rely
             *      on the default behavior.
             *      
             * If you update this comment block, please also update it in the client under:
             *      ClientMessageTypeExtensions.GetDeliveryMethod().
             */

#if true
            // NOTE: For now, we use a very simple and straight-forward approach. In the future, we will use more complex deliveries.

            if (msgType == ServerMessageType.MapDynamicEntitySpatialUpdate)
            {
                method = NetDeliveryMethod.ReliableSequenced;
                seqChannel = 1;
            }
            else
            {
                method = NetDeliveryMethod.ReliableOrdered;
                seqChannel = 0;
            }
#else
    // Listing of the used sequence numbers, grouped by delivery method
            const int chRO_General = 0;
            const int chRO_GUI = 1;
            const int chRO_GUIChat = 2;
            const int chRO_GUIItems = 3;
            const int chRO_GUIUserStats = 4;
            const int chRO_GUIUserStatus = 5;
            const int chRO_Map = 6;
            const int chRO_MapDynamicEntityProperty = 7;

            const int chRU_MapEffect = 1;

            const int chRS_GUIItemInfo = 1;

            const int chUS_MapDynamicEntitySpatialUpdate = 1;

            switch (msgType)
            {
                case ServerMessageType.General:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_General;
                    return;

                case ServerMessageType.GUI:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_GUI;
                    return;

                case ServerMessageType.GUIChat:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_GUIChat;
                    return;

                case ServerMessageType.GUIItemInfo:
                    method = NetDeliveryMethod.ReliableSequenced;
                    seqChannel = chRS_GUIItemInfo;
                    break;

                case ServerMessageType.GUIItems:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_GUIItems;
                    break;

                case ServerMessageType.GUIUserStats:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_GUIUserStats;
                    break;

                case ServerMessageType.GUIUserStatus:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_GUIUserStatus;
                    break;

                case ServerMessageType.Map:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_Map;
                    break;

                case ServerMessageType.MapCharacterSP:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_MapDynamicEntityProperty;
                    break;

                case ServerMessageType.MapDynamicEntityProperty:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_MapDynamicEntityProperty;
                    break;

                case ServerMessageType.MapDynamicEntitySpatialUpdate:
                    method = NetDeliveryMethod.UnreliableSequenced;
                    seqChannel = chUS_MapDynamicEntitySpatialUpdate;
                    break;

                case ServerMessageType.MapEffect:
                    method = NetDeliveryMethod.UnreliableSequenced;
                    seqChannel = chRU_MapEffect;
                    break;

                case ServerMessageType.MapEffectDependent:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_Map;
                    break;

                case ServerMessageType.System:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_General;
                    break;

                default:
                    const string errmsg =
                        "ServerMessageType `{0}` does not explicitly define a delivery method and sequence channel." +
                        " All message types should define this explicitly. Falling back to ServerMessageType.General.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, msgType);
                    GetDeliveryMethod(ServerMessageType.General, out method, out seqChannel);
                    break;
            }
#endif
        }
    }
}