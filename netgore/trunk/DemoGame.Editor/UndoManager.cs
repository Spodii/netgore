using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NetGore;
using NetGore.IO;
using NetGore.World;

namespace DemoGame.Editor
{
    /// <summary>
    /// Takes care of the undo/redo support on the map.
    /// This is just a simple, straight-forward implementation that manages states by serializing the whole map.
    /// </summary>
    public class MapUndoManager
    {
        /// <summary>
        /// Maximum number of states. Once we hit this count, we will crop out old states by the _cropAmount.
        /// </summary>
        const int _maxStates = 50;

        /// <summary>
        /// The number of old states to crop at once.
        /// </summary>
        const int _cropAmount = 5;

        /// <summary>
        /// A list of the current states. Newest states are at the end of the list, older at the front.
        /// </summary>
        readonly List<byte[]> _states = new List<byte[]>();

        /// <summary>
        /// Our current index in the states list. Will always be pointing to the last element in the collection until
        /// undos are made.
        /// </summary>
        int _stateIndex = -1;

        public MapUndoManager(EditorMap map, IDynamicEntityFactory dynamicEntityFactory)
        {
            Map = map;
            DynamicEntityFactory = dynamicEntityFactory;
        }

        /// <summary>
        /// Gets the map this UndoManager is for.
        /// </summary>
        public EditorMap Map { get; private set; }

        /// <summary>
        /// Gets the DynamicEntityFactory to use on this map.
        /// </summary>
        public IDynamicEntityFactory DynamicEntityFactory { get; private set; }

        /// <summary>
        /// Takes a state snapshot of the map. Only records it as a new state if the state has changed since the last snapshot.
        /// If the state has changed, all forward states are destroyed. So if you undo a few times then call this, you cannot redo those undos anymore.
        /// </summary>
        /// <returns>True if a new state snapshot was taken; false if the state has not changed.</returns>
        public bool Snapshot()
        {
            // Serilize the whole map as binary into memory
            byte[] serialized = BinaryValueWriter.CreateAndWrite(w => Map.Save(w, DynamicEntityFactory, MapBase.MapSaveFlags.DoNotSort), useEnumNames: false);

            // Check if the state changed since the last time
            if (_stateIndex >= 0 && ByteArrayEqualityComparer.AreEqual(_states[_stateIndex], serialized))
                return false;

            // Remove all forward (redo) states
            if (_states.Count > _stateIndex + 1)
            {
                _states.RemoveRange(_stateIndex + 1, _states.Count - _stateIndex - 1);
            }

            // Remove old states
            if (_states.Count > _maxStates)
            {
                _states.RemoveRange(0, _cropAmount);
            }

            // Push in new state
            _states.Add(serialized);
            _stateIndex = _states.Count - 1;

            return true;
        }

        /// <summary>
        /// Undos to the previous snapshot if there are any states to undo to.
        /// </summary>
        /// <returns>True if moved to the previous state; false if there are no previous states to move to.</returns>
        public bool Undo()
        {
            if (_stateIndex <= 0)
                return false;

            _stateIndex--;
            LoadMapFromState(_stateIndex);

            return true;
        }

        /// <summary>
        /// Redos to the next state (undos an undo).
        /// </summary>
        /// <returns>True if moved to the next state; false if there was no state to redo to.</returns>
        public bool Redo()
        {
            if (_stateIndex >= _states.Count - 1)
                return false;

            _stateIndex++;
            LoadMapFromState(_stateIndex);

            return true;
        }

        void LoadMapFromState(int stateIndex)
        {
            Map.Load(BinaryValueReader.Create(_states[stateIndex], useEnumNames: false), true, DynamicEntityFactory);
        }
    }
}
