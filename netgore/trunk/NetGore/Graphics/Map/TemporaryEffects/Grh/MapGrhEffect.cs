using System.Diagnostics;
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
        /// <param name="isForeground">If true, this will be drawn in the foreground layer. If false,
        /// it will be drawn in the background layer.</param>
        protected MapGrhEffect(Grh grh, Vector2 position, bool isForeground) : base(grh, position, isForeground)
        {
        }

        /// <summary>
        /// Kills this <see cref="MapGrhEffect"/>. This should happen once and only once for every <see cref="MapGrhEffect"/>.
        /// </summary>
        protected void Kill()
        {
            if (!IsAlive)
            {
                const string errmsg = "Tried to kill dead MapGrhEffect `{0}`. This should only be called once.";
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            _isAlive = false;

            if (Died != null)
                Died(this);
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
        public event TemporaryMapEffectDiedHandler Died;

        /// <summary>
        /// Gets if this map effect is still alive. When false, it will be removed from the map. Once set to false, this
        /// value will remain false.
        /// </summary>
        public bool IsAlive
        {
            get { return _isAlive; }
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