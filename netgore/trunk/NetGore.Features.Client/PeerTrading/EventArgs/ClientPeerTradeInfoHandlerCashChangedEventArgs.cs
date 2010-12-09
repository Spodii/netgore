using System;
using System.Linq;

namespace NetGore.Features.PeerTrading
{
    public class ClientPeerTradeInfoHandlerCashChangedEventArgs : EventArgs
    {
        readonly int _cash;
        readonly bool _isSourceSide;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPeerTradeInfoHandlerCashChangedEventArgs"/> class.
        /// </summary>
        /// <param name="cash">The new cash value.</param>
        /// <param name="isSourceSide">If true, the changed cash amount was on the source character's side.
        /// If false, it was on the target character's side.</param>
        public ClientPeerTradeInfoHandlerCashChangedEventArgs(int cash, bool isSourceSide)
        {
            _cash = cash;
            _isSourceSide = isSourceSide;
        }

        /// <summary>
        /// Gets the new cash value.
        /// </summary>
        public int Cash
        {
            get { return _cash; }
        }

        /// <summary>
        /// Gets if the changed cash amount was on the source character's side.
        /// If false, it was on the target character's side.
        /// </summary>
        public bool IsSourceSide
        {
            get { return _isSourceSide; }
        }
    }
}