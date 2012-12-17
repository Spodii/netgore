using System.Linq;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Implementation of <see cref="IKeyCodeReference"/> that references a key directly.
    /// </summary>
    public class StaticKeyCodeReference : IKeyCodeReference
    {
        Keyboard.Key _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticKeyCodeReference"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public StaticKeyCodeReference(Keyboard.Key key)
        {
            _key = key;
        }

        /// <summary>
        /// Changes the referened key.
        /// </summary>
        /// <param name="newValue">The new <see cref="Keyboard.Key"/>.</param>
        public void ChangeKey(Keyboard.Key newValue)
        {
            _key = newValue;
        }

        #region IKeyCodeReference Members

        /// <summary>
        /// Gets the referenced <see cref="Keyboard.Key"/>.
        /// </summary>
        public Keyboard.Key Key
        {
            get { return _key; }
        }

        #endregion
    }
}