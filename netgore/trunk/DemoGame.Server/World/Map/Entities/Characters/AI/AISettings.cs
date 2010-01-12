
namespace DemoGame.AI
{
    public static class AISettings
    {
        private static bool _aiDisabled;

        ///<summary>
        /// Sets and gets whether the AI is on or not.
        /// </summary>
        public static bool AIDisabled
        {
            get { return _aiDisabled; }
            set { _aiDisabled = value; }
        }

    }
}
