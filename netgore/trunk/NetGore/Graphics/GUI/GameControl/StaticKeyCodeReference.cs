using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Implementation of <see cref="IKeyCodeReference"/> that references a key directly.
    /// </summary>
    public class StaticKeyCodeReference : IKeyCodeReference
    {
        KeyCode _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticKeyCodeReference"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public StaticKeyCodeReference(KeyCode key)
        {
            _key = key;
        }

        /// <summary>
        /// Changes the referened key.
        /// </summary>
        /// <param name="newValue">The new <see cref="KeyCode"/>.</param>
        public void ChangeKey(KeyCode newValue)
        {
            _key = newValue;
        }

        /// <summary>
        /// Gets the referenced <see cref="KeyCode"/>.
        /// </summary>
        public KeyCode Key
        {
            get { return _key; }
        }
    }
}