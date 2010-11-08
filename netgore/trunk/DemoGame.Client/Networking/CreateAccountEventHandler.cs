using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Handles when a CreateAccount message is received.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="successful">If the account was successfully created.</param>
    /// <param name="errorMessage">If <paramref name="successful"/> is false, contains the reason
    /// why the account failed to be created.</param>
    public delegate void CreateAccountEventHandler(IIPSocket sender, bool successful, string errorMessage);
}