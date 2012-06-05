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
        static readonly IEnumerable<IKeyCodeReference> _emptyKeys = Enumerable.Empty<IKeyCodeReference>();

        IEnumerable<IKeyCodeReference> _keysDown;
        IEnumerable<IKeyCodeReference> _keysUp;
        string _name;
        IEnumerable<IKeyCodeReference> _newKeysDown;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keysDown">The keys required to be down.</param>
        /// <param name="keysUp">The keys required to be up.</param>
        public GameControlKeys(string name, IEnumerable<IKeyCodeReference> keysDown, IEnumerable<IKeyCodeReference> keysUp = null)
            : this(name, keysDown, keysUp, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keysDown">The keys required to be down.</param>
        /// <param name="keysUp">The keys required to be up.</param>
        /// <param name="newKeysDown">The keys required to be down this frame, and up last frame.</param>
        public GameControlKeys(string name, IEnumerable<IKeyCodeReference> keysDown, IEnumerable<IKeyCodeReference> keysUp,
                               IEnumerable<IKeyCodeReference> newKeysDown)
        {
            Name = name;
            NewKeysDown = newKeysDown;
            KeysUp = keysUp;
            KeysDown = keysDown;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keyDown">The key required to be down.</param>
        /// <param name="keyUp">The key required to be up.</param>
        /// <param name="newKeyDown">The key required to be down this frame, and up last frame.</param>
        public GameControlKeys(string name, IKeyCodeReference keyDown, IKeyCodeReference keyUp = null,
                               IKeyCodeReference newKeyDown = null)
        {
            Name = name;
            KeysDown = keyDown != null ? new IKeyCodeReference[] { keyDown } : _emptyKeys;
            KeysUp = keyUp != null ? new IKeyCodeReference[] { keyUp } : _emptyKeys;
            NewKeysDown = newKeyDown != null ? new IKeyCodeReference[] { newKeyDown } : _emptyKeys;
        }

        /// <summary>
        /// Gets or sets an IEnumerable of the <see cref="KeyCode"/> required to be down for this
        /// <see cref="GameControlKeys"/> to be invoked.
        /// The returned <see cref="IEnumerable{KeyCode}"/> can be empty, but will not be null. If this property
        /// is set to null, an empty <see cref="IEnumerable{KeyCode}"/> will be used instead.
        /// </summary>
        public IEnumerable<IKeyCodeReference> KeysDown
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
        public IEnumerable<IKeyCodeReference> KeysUp
        {
            get { return _keysUp; }
            set { _keysUp = value ?? _emptyKeys; }
        }

        /// <summary>
        /// Gets or sets the optional name of this <see cref="GameControl"/>. This name is intended primarily for
        /// debugging purposes. Cannot be null.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
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
        public IEnumerable<IKeyCodeReference> NewKeysDown
        {
            get { return _newKeysDown; }
            set { _newKeysDown = value ?? _emptyKeys; }
        }
    }
}