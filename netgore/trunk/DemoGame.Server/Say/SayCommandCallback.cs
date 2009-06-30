using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.Server
{
    /// <summary>
    /// Callback for a SayCommandAttribute.
    /// </summary>
    /// <param name="text">Text to handle. Does not include the command itself.</param>
    /// <param name="user">User that the text came from.</param>
    delegate void SayCommandCallback(string text, User user);
}