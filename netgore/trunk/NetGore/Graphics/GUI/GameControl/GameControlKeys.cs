using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Contains the key states needed to invoke a <see cref="GameControl"/>.
    /// </summary>
    public class GameControlKeys
    {
        /// <summary>
        /// An empty IEnumerable of <see cref="SFML.Window.KeyCode"/>.
        /// </summary>
        static readonly IEnumerable<KeyCode> _emptyKeys = Enumerable.Empty<KeyCode>();

        IEnumerable<KeyCode> _keysDown;
        IEnumerable<KeyCode> _keysUp;
        string _name;
        IEnumerable<KeyCode> _newKeysDown;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the control. Cannot be null.</param>
        /// <param name="keysDown">The keys required to be down.</param>
        public GameControlKeys(string name, IEnumerable<KeyCode> keysDown) : this(name, keysDown, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keysDown">The keys required to be down.</param>
        /// <param name="keysUp">The keys required to be up.</param>
        public GameControlKeys(string name, IEnumerable<KeyCode> keysDown, IEnumerable<KeyCode> keysUp)
            : this(name, keysDown, keysUp, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keysDown">The keys required to be down.</param>
        /// <param name="keysUp">The keys required to be up.</param>
        /// <param name="newKeysDown">The keys required to be down this frame, and up last frame.</param>
        public GameControlKeys(string name, IEnumerable<KeyCode> keysDown, IEnumerable<KeyCode> keysUp,
                               IEnumerable<KeyCode> newKeysDown)
        {
            Name = name;
            NewKeysDown = newKeysDown;
            KeysUp = keysUp;
            KeysDown = keysDown;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keyDown">The key required to be down.</param>
        public GameControlKeys(string name, KeyCode? keyDown) : this(name, keyDown, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keyDown">The key required to be down.</param>
        /// <param name="keyUp">The key required to be up.</param>
        public GameControlKeys(string name, KeyCode? keyDown, KeyCode? keyUp) : this(name, keyDown, keyUp, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keyDown">The key required to be down.</param>
        /// <param name="keyUp">The key required to be up.</param>
        /// <param name="newKeyDown">The key required to be down this frame, and up last frame.</param>
        public GameControlKeys(string name, KeyCode? keyDown, KeyCode? keyUp, KeyCode? newKeyDown)
        {
            Name = name;
            KeysDown = keyDown.HasValue ? new KeyCode[] { keyDown.Value } : _emptyKeys;
            KeysUp = keyUp.HasValue ? new KeyCode[] { keyUp.Value } : _emptyKeys;
            NewKeysDown = newKeyDown.HasValue ? new KeyCode[] { newKeyDown.Value } : _emptyKeys;
        }

        /// <summary>
        /// Gets or sets an IEnumerable of the <see cref="KeyCode"/> required to be down for this
        /// <see cref="GameControlKeys"/> to be invoked.
        /// The returned <see cref="IEnumerable{KeyCode}"/> can be empty, but will not be null. If this property
        /// is set to null, an empty <see cref="IEnumerable{KeyCode}"/> will be used instead.
        /// </summary>
        public IEnumerable<KeyCode> KeysDown
        {
            get { return _keysDown; }
            set { _keysDown = value ?? _emptyKeys; }
        }

        /// <summary>
        /// Gets or sets an IEnumerable of the <see cref="KeyCode"/> required to be up for this
        /// <see cref="GameControlKeys"/> to be invoked.
        /// The returned <see cref="IEnumerable{KeyCode}"/> can be empty, but will not be null. If this property
        /// is set to null, an empty <see cref="IEnumerable{KeyCode}"/> will be used instead.
        /// </summary>
        public IEnumerable<KeyCode> KeysUp
        {
            get { return _keysUp; }
            set { _keysUp = value ?? _emptyKeys; }
        }

        /// <summary>
        /// Gets or sets the optional name of this <see cref="GameControl"/>. This name is intended primarily for
        /// debugging purposes. Cannot be null.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IEnumerable{T}"/> of <see cref="KeyCode"/>s that are required to be down
        /// for this action. The action will only take place once all of these keys are down, and will only be invoked
        /// once until all keys are raised and pressed again.
        /// The returned <see cref="IEnumerable{KeyCode}"/> can be empty, but will not be null. If this property
        /// is set to null, an empty <see cref="IEnumerable{KeyCode}"/> will be used instead.
        /// </summary>
        public IEnumerable<KeyCode> NewKeysDown
        {
            get { return _newKeysDown; }
            set { _newKeysDown = value ?? _emptyKeys; }
        }
    }
}