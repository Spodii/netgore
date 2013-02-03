using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Db;
using NetGore.Network;

namespace DemoGame.Server
{
    public interface IUserAccount : IAccountTable, IDisposable
    {
        /// <summary>
        /// Gets the number of Characters in this UserAccount.
        /// </summary>
        byte CharacterCount { get; }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="CharacterID"/>s for the <see cref="Character"/>s that are in
        /// this <see cref="IUserAccount"/>.
        /// </summary>
        IEnumerable<CharacterID> CharacterIDs { get; }

        IDbController DbController { get; }

        /// <summary>
        /// Gets the <see cref="IIPSocket"/> that is used to communicate with the client connected to this <see cref="IUserAccount"/>.
        /// </summary>
        IIPSocket Socket { get; }

        /// <summary>
        /// Gets the <see cref="DemoGame.Server.User"/> currently logged in on this <see cref="IUserAccount"/>.
        /// </summary>
        User User { get; }

        /// <summary>
        /// Logs out the User from this account. If the <see cref="User"/> is not null and has not been disposed,
        /// this method will dispose of the User, too.
        /// </summary>
        void CloseUser();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disconnectMessage">The message to use for when disconnecting the socket. When disposing an active connection,
        /// this can provide the client a reason why they were disconnected. The default is
        /// <see cref="GameMessage.DisconnectUserDisposed"/>.</param>
        /// <param name="p">The arguments for the <paramref name="disconnectMessage"/>.</param>
        void Dispose(GameMessage disconnectMessage, params object[] p);

        /// <summary>
        /// Gets the CharacterID for a Character in the account by the given index.
        /// </summary>
        /// <param name="index">The 0-based index of the CharacterID.</param>
        /// <returns>The CharacterID for the Character at the given <paramref name="index"/>.</returns>
        CharacterID GetCharacterID(byte index);

        /// <summary>
        /// Loads the <see cref="CharacterID"/>s for the Characters in this account.
        /// </summary>
        void LoadCharacterIDs();

        /// <summary>
        /// Sends the <see cref="AccountCharacterInfo"/>s for the <see cref="Character"/>s in this account to the
        /// client.
        /// </summary>
        void SendAccountCharacterInfos();

        /// <summary>
        /// Sets the permission level for this account.
        /// </summary>
        /// <param name="newPermissions">The new <see cref="UserPermissions"/> level.</param>
        void SetPermissions(UserPermissions newPermissions);

        /// <summary>
        /// Loads and sets the User being used by this account.
        /// </summary>
        /// <param name="world">The World that the User will be part of.</param>
        /// <param name="characterID">The CharacterID of the user to use.</param>
        void SetUser(World world, CharacterID characterID);

        /// <summary>
        /// Tries to get the CharacterID for a Character in the account by the given index.
        /// </summary>
        /// <param name="index">The 0-based index of the CharacterID.</param>
        /// <param name="value">The CharacterID for the Character at the given <paramref name="index"/>.</param>
        /// <returns>True if the <paramref name="value"/> was successfully acquired; otherwise false.</returns>
        bool TryGetCharacterID(byte index, out CharacterID value);

        /// <summary>
        /// Sets the list of friends for this account.
        /// </summary>
        /// <param name="friends">The friends list.</param>
        void SetFriends(string friends);
    }
}