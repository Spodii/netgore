using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A specialization of a <see cref="MapGrh"/> that is used to display a <see cref="MapGrh"/> for only a short period of time
    /// as a map-based effect (such as for spells).
    /// </summary>
    public abstract class MapGrhEffect : MapGrh, ITemporaryMapEffect
    {
        bool _isAlive = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrhEffect"/> class.
        /// </summary>
        /// <param name="grh">Grh to draw.</param>
        /// <param name="position">Position to draw on the map.</param>
        protected MapGrhEffect(Grh grh, Vector2 position) : base(grh, position)
        {
        }

        /// <summary>
        /// When overridden in the derived class, performs the additional updating that this <see cref="MapGrhEffect"/>
        /// needs to do such as checking if it is time to kill the effect. This method should be overridden instead of
        /// <see cref="MapGrh.Update"/>. This method will not be called after the effect has been killed.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        protected abstract void UpdateEffect(TickCount currentTime);

        #region ITemporaryMapEffect Members

        /// <summary>
        /// Notifies listeners when this <see cref="ITemporaryMapEffect"/> has died. This is only raised once per
        /// <see cref="ITemporaryMapEffect"/>, and is raised when <see cref="ITemporaryMapEffect.IsAlive"/> is set to false.
        /// </summary>
        public event TypedEventHandler<ITemporaryMapEffect> Died;

        /// <summary>
        /// Gets if this map effect is still alive. When false, it will be removed from the map. Once set to false, this
        /// value will remain false.
        /// </summary>
        public bool IsAlive
        {
            get { return _isAlive; }
        }

        /// <summary>
        /// Forcibly kills the effect.
        /// </summary>
        /// <param name="immediate">If true, the effect will be killed immediately and <see cref="IsAlive"/> will be
        /// false by the time the method returns. If false, the effect is allowed to enter itself into a "terminating" state,
        /// allowing it to cleanly transition the effect out.</param>
        public virtual void Kill(bool immediate)
        {
            if (!IsAlive)
                return;

            _isAlive = false;

            if (Died != null)
                Died.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the <see cref="MapGrh"/>.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        public override void Update(TickCount currentTime)
        {
            base.Update(currentTime);

            if (IsAlive)
                UpdateEffect(currentTime);
        }

        #endregion
    }
}