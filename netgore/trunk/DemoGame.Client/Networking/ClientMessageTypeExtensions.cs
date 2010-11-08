using System.Linq;
using Lidgren.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Extension methods for the <see cref="ClientMessageType"/> enum.
    /// </summary>
    public static class ClientMessageTypeExtensions
    {
#if false
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
#endif

        /// <summary>
        /// Gets the <see cref="NetDeliveryMethod"/> and sequence channel to use for a given <see cref="ClientMessageType"/>.
        /// </summary>
        /// <param name="msgType">The <see cref="ClientMessageType"/>.</param>
        /// <param name="method">The <see cref="NetDeliveryMethod"/> to use for the <paramref name="msgType"/>.</param>
        /// <param name="seqChannel">The sequence channel to use for the <paramref name="msgType"/>.</param>
        public static void GetDeliveryMethod(this ClientMessageType msgType, out NetDeliveryMethod method, out int seqChannel)
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
             * If you update this comment block, please also update it in the server under:
             *      ServerMessageTypeExtensions.GetDeliveryMethod().
             */

#if true
            // NOTE: For now, we use a very simple and straight-forward approach. In the future, we will use more complex deliveries.

            method = NetDeliveryMethod.ReliableOrdered;
            seqChannel = 0;

#else
    // Listing of the used sequence numbers, grouped by delivery method
            const int chRO_General = 0;
            const int chRO_Chat = 1;

            // FUTURE: Can start dividing stuff up for the client into different channels in the future. For now, it shouldn't be important.

            switch (msgType)
            {
                case ClientMessageType.CharacterMove:
                case ClientMessageType.CharacterInteract:
                case ClientMessageType.General:
                case ClientMessageType.GUI:
                case ClientMessageType.GUIItems:
                case ClientMessageType.System:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_General;
                    break;

                case ClientMessageType.Chat:
                    method = NetDeliveryMethod.ReliableOrdered;
                    seqChannel = chRO_Chat;
                    break;

                default:
                    const string errmsg =
                        "ClientMessageType `{0}` does not explicitly define a delivery method and sequence channel." +
                        " All message types should define this explicitly. Falling back to ClientMessageType.General.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, msgType);
                    GetDeliveryMethod(ClientMessageType.General, out method, out seqChannel);
                    break;
            }
#endif
        }
    }
}