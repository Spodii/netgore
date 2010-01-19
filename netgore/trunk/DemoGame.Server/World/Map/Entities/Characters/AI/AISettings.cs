using System.Linq;

namespace DemoGame.AI
{
    public static class AISettings
    {
        static bool _aiDisabled;

        ///<summary>
        /// Sets and gets whether the AI is switched on or not.
        /// </summary>
        public static bool AIDisabled
        {
            get { return _aiDisabled; }
            set { _aiDisabled = value; }
        }
    }
}