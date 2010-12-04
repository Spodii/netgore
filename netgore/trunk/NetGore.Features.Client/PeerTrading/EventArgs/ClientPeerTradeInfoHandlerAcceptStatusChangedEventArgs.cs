using System;

namespace NetGore.Features.PeerTrading
{
    public class ClientPeerTradeInfoHandlerAcceptStatusChangedEventArgs : EventArgs
    {
        readonly bool _isSourceSide;
        readonly bool _hasAccepted;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPeerTradeInfoHandlerAcceptStatusChangedEventArgs"/> class.
        /// </summary>
        /// <param name="isSourceSide">If true, the changed was on the source character's side.
        /// Otherwise, it was on the target character's side.</param>
        /// <param name="hasAccepted">If true, the status changed to accepted. If false, the status changed to not accepted.</param>
        public ClientPeerTradeInfoHandlerAcceptStatusChangedEventArgs(bool isSourceSide, bool hasAccepted)
        {
            _isSourceSide = isSourceSide;
            _hasAccepted = hasAccepted;
        }

        /// <summary>
        /// Gets if the changed was on the source character's side. If false, it was on the target character's side.
        /// </summary>
        public bool IsSourceSide { get { return _isSourceSide; } }

        /// <summary>
        /// Gets if the status changed to accepted. If false, the status changed to not accepted.
        /// </summary>
        public bool HasAccepted { get { return _hasAccepted; } }
    }
}