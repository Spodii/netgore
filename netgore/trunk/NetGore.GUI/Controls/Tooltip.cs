using System;
using System.Linq;

namespace NetGore.Graphics.GUI
{
    public class Tooltip
    {
        readonly GUIManagerBase _guiManager;

        int _delay;
        int _timeout;

        public int Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        public GUIManagerBase GUIManager
        {
            get { return _guiManager; }
        }

        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public Tooltip(GUIManagerBase guiManager)
        {
            if (guiManager == null)
                throw new ArgumentNullException("guiManager");

            _guiManager = guiManager;
        }

        Control _lastUnderCursor;

        public void Update(int currentTime)
        {
            var currentUnderCursor = GUIManager.UnderCursor;

            // TODO: Unfinished
        }
    }
}