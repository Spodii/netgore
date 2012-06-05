using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the different results for an attempt to log into an account.
    /// </summary>
    public enum AccountLoginResult : byte
    {
        /// <summary>
        /// Login was successful.
        /// </summary>
        Successful,

        /// <summary>
        /// An invalid account name was supplied or no account exists with that name.
        /// </summary>
        InvalidName,

        /// <summary>
        /// An invalid account password was supplied or the password that was supplied does not match
        /// the password for the given account name.
        /// </summary>
        InvalidPassword,

        /// <summary>
        /// The account is already in use.
        /// </summary>
        AccountInUse
    }
}