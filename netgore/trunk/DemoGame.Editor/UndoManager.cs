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
    public class MapUndoManager
    {
        /// <summary>
        /// Maximum number of states. Once we hit this count, we will crop out old states.
        /// </summary>
        const int _maxStates = 50;

        /// <summary>
        /// The number of old states to crop at once.
        /// </summary>
        const int _cropAmount = 5;

        readonly List<byte[]> _states = new List<byte[]>();

        int _stateIndex = -1;

        public MapUndoManager(EditorMap map, IDynamicEntityFactory dynamicEntityFactory)
        {
            Map = map;
            DynamicEntityFactory = dynamicEntityFactory;
        }

        public EditorMap Map { get; private set; }

        public IDynamicEntityFactory DynamicEntityFactory { get; private set; }

        public void Snapshot()
        {
            byte[] serialized = BinaryValueWriter.CreateAndWrite(w => Map.Save(w, DynamicEntityFactory));

            // Check if the state changed
            if (_stateIndex >= 0 && ByteArrayEqualityComparer.AreEqual(_states[_stateIndex], serialized))
                return;

            // Remove all forward states
            if (_states.Count > _stateIndex + 1)
            {
                _states.RemoveRange(_stateIndex + 1, _states.Count - _stateIndex - 1);
            }

            // Remove old states
            if (_states.Count > 50)
            {
                _states.RemoveRange(0, 10);
                _stateIndex -= 10;
            }

            // Push in new state
            _states.Add(serialized);
            _stateIndex++;
        }

        public bool Undo()
        {
            if (_stateIndex <= 0)
                return false;

            _stateIndex--;
            Map.Load(BinaryValueReader.Create(_states[_stateIndex]), true, DynamicEntityFactory);

            return true;
        }

        public bool Redo()
        {
            if (_stateIndex >= _states.Count - 1)
                return false;

            _stateIndex++;
            Map.Load(BinaryValueReader.Create(_states[_stateIndex]), true, DynamicEntityFactory);

            return true;
        }
    }
}
