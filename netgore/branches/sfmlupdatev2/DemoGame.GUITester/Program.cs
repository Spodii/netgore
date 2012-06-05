using System.Linq;
using NetGore;
using SFML.Graphics;

namespace DemoGame.GUITester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            EngineSettings.Initialize(new EngineSettings(Vector2.Zero, Vector2.One));

            using (new Game1())
            {
            }
        }
    }
}