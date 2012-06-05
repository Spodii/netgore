using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A collection of <see cref="GameControl"/>s.
    /// </summary>
    public class GameControlCollection : ICollection<GameControl>
    {
        readonly List<GameControl> _gameControls = new List<GameControl>();

        /// <summary>
        /// Creates a new <see cref="GameControl"/> and adds it to this <see cref="GameControlCollection"/>.
        /// </summary>
        /// <param name="keys">The <see cref="GameControlKeys"/>.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="additionalRequirements">The additional requirements.</param>
        /// <param name="invokeHandler">The invoke handler.</param>
        /// <returns>
        /// The instance of the created <see cref="GameControl"/> that was added to this
        /// <see cref="GameControlCollection"/>.
        /// </returns>
        public GameControl CreateAndAdd(GameControlKeys keys, int delay, Func<bool> additionalRequirements,
                                        TypedEventHandler<GameControl> invokeHandler)
        {
            var c = new GameControl(keys) { Delay = delay, AdditionalRequirements = additionalRequirements };
            c.Invoked += invokeHandler;

            Add(c);
            return c;
        }

        /// <summary>
        /// Updates all of the <see cref="GameControl"/>s in this <see cref="GameControlCollection"/>.
        /// </summary>
        /// <param name="guiManager">The <see cref="IGUIManager"/> used to update the <see cref="GameControl"/>s.</param>
        /// <param name="currentTime">The current time in milliseconds.</param>
        public void Update(IGUIManager guiManager, TickCount currentTime)
        {
            foreach (var gc in this)
            {
                gc.Update(guiManager, currentTime);
            }
        }

        #region ICollection<GameControl> Members

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return _gameControls.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        bool ICollection<GameControl>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///                 </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        ///                 </exception>
        public void Add(GameControl item)
        {
            _gameControls.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. 
        ///                 </exception>
        public void Clear()
        {
            _gameControls.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///                 </param>
        public bool Contains(GameControl item)
        {
            return _gameControls.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="array"/> is multidimensional.
        /// -or-
        /// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// -or-
        /// Type <see cref="GameControl"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        void ICollection<GameControl>.CopyTo(GameControl[] array, int arrayIndex)
        {
            _gameControls.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<GameControl> GetEnumerator()
        {
            return _gameControls.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///                 </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        ///                 </exception>
        public bool Remove(GameControl item)
        {
            return _gameControls.Remove(item);
        }

        #endregion
    }
}