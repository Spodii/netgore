using System;
using System.Linq;

namespace NetGore.Features.PeerTrading
{
    public class ClientPeerTradeInfoHandlerTradeCanceledEventArgs : EventArgs
    {
        readonly bool _sourceCanceled;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPeerTradeInfoHandlerTradeCanceledEventArgs"/> class.
        /// </summary>
        /// <param name="sourceCanceled">If it was the source character who canceled the trade.</param>
        public ClientPeerTradeInfoHandlerTradeCanceledEventArgs(bool sourceCanceled)
        {
            _sourceCanceled = sourceCanceled;
        }

        /// <summary>
        /// Gets if it was the source character who canceled the trade.
        /// </summary>
        public bool SourceCanceled
        {
            get { return _sourceCanceled; }
        }
    }
}