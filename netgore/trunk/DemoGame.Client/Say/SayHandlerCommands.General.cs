using System.Linq;
using NetGore;

namespace DemoGame.Client.Say
{
    /// <summary>
    /// <see cref="SayHandlerCommands"/> for any permission level and no specific feature that can be parsed client side..
    /// </summary>
    public partial class SayHandlerCommands
    {
        
        /// <summary>
        /// Logs the user out from the game
        /// </summary>
        [SayCommand("Quit")]
        public void ExitGame()
        {
            // Terminate the game
           GameplayScreen.Logout();
        }

        /// <summary>
        /// Echos something out to the screen
        /// </summary>
        /// <param name="echoMessage">The message the user has decided to echo out</param>
        [SayCommand("Echo")]
        public void Echo(string echoMessage)
        {
           GameplayScreen.AppendToChatOutput(echoMessage);       
        }

        [SayCommand("FPS")]
        public void DisplayFPS()
        {
            GameplayScreen.AppendToChatOutput("FPS: " + GameplayScreen.ScreenManager.FPS);
        }


    }
}