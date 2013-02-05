using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using NetGore;

namespace DemoGame.Client.Say
{
    /// <summary>
    /// Proccesses what a given user has said client-side
    /// </summary>
    public class SayHandler : SayHandlerBase<Character, SayCommandAttribute>
    {

        /// <summary>
        /// The gameplay screen that is affected by this <see cref="SayHandler"/>.
        /// Used for things like chat output
        /// </summary>
        public GameplayScreen GameplayScreen { get; set; }

        public SayHandler(GameplayScreen gameplayScreen) : base(new SayHandlerCommands(gameplayScreen))
        {         
            GameplayScreen = gameplayScreen;
        
           
        }

        /// <summary>
        /// When overridden in the derived class, handles the output from a command.
        /// </summary>
        /// <param name="user">The user that the command came from.</param>
        /// <param name="text">The output text from the command. Will not be null or empty.</param>
        /// <param name="orginalMessage">The orginal message that was inititally input.</param>
        protected override void HandleCommandOutput(Character user, string text, string orginalMessage)
        {
            // The server will handle sending down invalid comand if it's not present here, so there's no need to worry about it

            // If the command is unknown, send out to the server for parsing, otherwise output the error to the client
            if (text == StringCommandParser.UnknownCommandSpecified)
            {
                using (var pw = ClientPacket.Say(orginalMessage))
                {
                    GameplayScreen.Socket.Send(pw, ClientMessageType.Chat);
                }
            }
            else
            {
                GameplayScreen.AppendToChatOutput(text);
            }

        }

        /// <summary>
        /// When overridden in the derived class, handles text that was not a command.
        /// </summary>
        /// <param name="user">The user the <paramref name="text"/> came from.</param>
        /// <param name="text">The text that wasn't a command.</param>
        /// <param name="orginalMessage">The orginal message that was inititally input.</param>
        protected override void HandleNonCommand(Character user, string text)
        {
           // Send it as a normal message for the server to be parsed
            
            using (var pw = ClientPacket.Say(text))
            {
                GameplayScreen.Socket.Send(pw, ClientMessageType.Chat);
            }


        }

        /// <summary>
        /// There is no real permissions to use anything on the client-side; so return nothing for now. This may change with time
        /// so support is still here. 
        /// </summary>
        /// <param name="user">The user invoking the command.</param>
        /// <param name="commandData">The information about the command to be invoked.</param>
        /// <returns>The message to display to the <paramref name="user"/>, or null or empty to display nothing.</returns>
        protected override string GetCommandNotAllowedMessage(Character user, StringCommandParserCommandData<SayCommandAttribute> commandData)
        {
            return null;
        }

        /// <summary>
        /// Gets if the given <paramref name="user"/> is allowed to invoke the given command.
        /// For now, all commands can be executed by the client. (No restrictions)
        /// </summary>
        /// <param name="user">The user invoking the command.</param>
        /// <param name="commandData">The information about the command to be invoked.</param>
        /// <returns>True if the command can be invoked; otherwise false.</returns>
        protected override bool AllowInvokeCommand(Character user, StringCommandParserCommandData<SayCommandAttribute> commandData)
        {
            return true;
        }





    }
}
