using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace NetGore.Graphics.GUI
{
    public class GameControlKeys
    {
        /// <summary>
        /// An empty IEnumerable of <see cref="Keys"/>.
        /// </summary>
        static readonly IEnumerable<Keys> _emptyKeys = Enumerable.Empty<Keys>();

        IEnumerable<Keys> _keysDown;
        IEnumerable<Keys> _keysUp;
        string _name;
        IEnumerable<Keys> _newKeysDown;
        IEnumerable<Keys> _newKeysUp;

        /// <summary>
        /// Gets or sets an IEnumerable of the <see cref="Keys"/> required to be down for this
        /// <see cref="GameControlKeys"/> to be invoked.
        /// </summary>
        public IEnumerable<Keys> KeysDown
        {
            get { return _keysDown; }
            set { _keysDown = value ?? _emptyKeys; }
        }

        /// <summary>
        /// Gets or sets an IEnumerable of the <see cref="Keys"/> required to be up for this
        /// <see cref="GameControlKeys"/> to be invoked.
        /// </summary>
        public IEnumerable<Keys> KeysUp
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
        /// Gets or sets an IEnumerable of the <see cref="Keys"/> required to be pressed for this
        /// <see cref="GameControlKeys"/> to be invoked. These keys must have been up the last frame, but
        /// pressed this frame. If they were already pressed, they do not count.
        /// </summary>
        public IEnumerable<Keys> NewKeysDown
        {
            get { return _newKeysDown; }
            set { _newKeysDown = value ?? _emptyKeys; }
        }

        /// <summary>
        /// Gets or sets an IEnumerable of the <see cref="Keys"/> required to be raised for this
        /// <see cref="GameControlKeys"/> to be invoked. These keys must have been pressed the last
        /// frame, but up this frame. If they were already up, they do not count.
        /// </summary>
        public IEnumerable<Keys> NewKeysUp
        {
            get { return _newKeysUp; }
            set { _newKeysUp = value ?? _emptyKeys; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the control. Cannot be null.</param>
        /// <param name="keysDown">The keys required to be down.</param>
        public GameControlKeys(string name, IEnumerable<Keys> keysDown) : this(name, keysDown, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keysDown">The keys required to be down.</param>
        /// <param name="keysUp">The keys required to be up.</param>
        public GameControlKeys(string name, IEnumerable<Keys> keysDown, IEnumerable<Keys> keysUp)
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
        public GameControlKeys(string name, IEnumerable<Keys> keysDown, IEnumerable<Keys> keysUp, IEnumerable<Keys> newKeysDown)
            : this(name, keysDown, keysUp, newKeysDown, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keysDown">The keys required to be down.</param>
        /// <param name="keysUp">The keys required to be up.</param>
        /// <param name="newKeysDown">The keys required to be down this frame, and up last frame.</param>
        /// <param name="newKeysUp">The keys required to be up this frame, and down last frame.</param>
        public GameControlKeys(string name, IEnumerable<Keys> keysDown, IEnumerable<Keys> keysUp, IEnumerable<Keys> newKeysDown,
                               IEnumerable<Keys> newKeysUp)
        {
            Name = name;
            NewKeysDown = newKeysDown;
            NewKeysUp = newKeysUp;
            KeysUp = keysUp;
            KeysDown = keysDown;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keyDown">The key required to be down.</param>
        public GameControlKeys(string name, Keys? keyDown) : this(name, keyDown, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keyDown">The key required to be down.</param>
        /// <param name="keyUp">The key required to be up.</param>
        public GameControlKeys(string name, Keys? keyDown, Keys? keyUp) : this(name, keyDown, keyUp, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keyDown">The key required to be down.</param>
        /// <param name="keyUp">The key required to be up.</param>
        /// <param name="newKeyDown">The key required to be down this frame, and up last frame.</param>
        public GameControlKeys(string name, Keys? keyDown, Keys? keyUp, Keys? newKeyDown)
            : this(name, keyDown, keyUp, newKeyDown, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControlKeys"/> class.
        /// </summary>
        /// <param name="name">The optional name of the <see cref="GameControlKeys"/>. Cannot be null.</param>
        /// <param name="keyDown">The key required to be down.</param>
        /// <param name="keyUp">The key required to be up.</param>
        /// <param name="newKeyDown">The key required to be down this frame, and up last frame.</param>
        /// <param name="newKeyUp">The key required to be up this frame, and down last frame.</param>
        public GameControlKeys(string name, Keys? keyDown, Keys? keyUp, Keys? newKeyDown, Keys? newKeyUp)
        {
            Name = name;
            KeysDown = keyDown.HasValue ? new Keys[] { keyDown.Value } : _emptyKeys;
            KeysUp = keyUp.HasValue ? new Keys[] { keyUp.Value } : _emptyKeys;
            NewKeysDown = newKeyDown.HasValue ? new Keys[] { newKeyDown.Value } : _emptyKeys;
            NewKeysUp = newKeyUp.HasValue ? new Keys[] { newKeyUp.Value } : _emptyKeys;
        }
    }
}