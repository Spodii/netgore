using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the information for a <see cref="DemoGame.Server.User"/>'s account.
    /// </summary>
    public class UserAccount : AccountTable, IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly DBController _dbController;
        readonly object _setUserLock = new object();
        readonly IIPSocket _socket;

        List<CharacterID> _characterIDs;
        User _user;

        /// <summary>
        /// Gets the number of Characters in this UserAccount.
        /// </summary>
        public byte CharacterCount
        {
            get { return (byte)_characterIDs.Count; }
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="CharacterID"/>s for the <see cref="Character"/>s that are in
        /// this <see cref="UserAccount"/>.
        /// </summary>
        public IEnumerable<CharacterID> CharacterIDs
        {
            get { return _characterIDs; }
        }

        /// <summary>
        /// Gets the IIPSocket that is used to communicate with the client connected to this UserAccount.
        /// </summary>
        public IIPSocket Socket
        {
            get { return _socket; }
        }

        /// <summary>
        /// Gets the <see cref="DemoGame.Server.User"/> currently logged in on this <see cref="UserAccount"/>.
        /// </summary>
        public User User
        {
            get { return _user; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccount"/> class.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="dbController">The db controller.</param>
        UserAccount(IIPSocket socket, DBController dbController)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _socket = socket;
            _dbController = dbController;
        }

        /// <summary>
        /// Logs out the User from this UserAccount. If the <see cref="User"/> is not null and has not been disposed,
        /// this method will dispose of the User, too.
        /// </summary>
        public void CloseUser()
        {
            User u;

            lock (_setUserLock)
            {
                u = _user;
                if (u == null)
                    return;

                _user = null;
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Closed User `{0}` on account `{1}`.", u, this);

            // Make sure the user was disposed
            if (!u.IsDisposed)
                u.Dispose();
        }

        /// <summary>
        /// Gets the CharacterID for a Character in the UserAccount by the given index.
        /// </summary>
        /// <param name="index">The 0-based index of the CharacterID.</param>
        /// <returns>The CharacterID for the Character at the given <paramref name="index"/>.</returns>
        public CharacterID GetCharacterID(byte index)
        {
            return _characterIDs[index];
        }

        /// <summary>
        /// Checks if the given string is a valid string for an account name.
        /// </summary>
        /// <param name="s">The string to test.</param>
        /// <returns>True if <paramref name="s"/> is a valid string for an account name; otherwise false.</returns>
        public static bool IsValidName(string s)
        {
            return GameData.AccountName.IsValid(s);
        }

        /// <summary>
        /// Checks if the given string is a valid string for an account password.
        /// </summary>
        /// <param name="s">The string to test.</param>
        /// <returns>True if <paramref name="s"/> is a valid string for an account password; otherwise false.</returns>
        public static bool IsValidPassword(string s)
        {
            return GameData.AccountPassword.IsValid(s);
        }

        void LoadCharacterIDs(DBController dbController)
        {
            _characterIDs = dbController.GetQuery<SelectAccountCharacterIDsQuery>().Execute(ID).ToList();
        }

        public static AccountLoginResult Login(DBController dbController, IIPSocket socket, string name, string password,
                                               out UserAccount userAccount)
        {
            userAccount = new UserAccount(socket, dbController);

            // Try to load the account data
            if (!dbController.GetQuery<SelectAccountQuery>().TryExecute(name, password, userAccount))
            {
                userAccount = null;
                return AccountLoginResult.InvalidName;
            }

            // Check for a matching password
            if (!password.Equals(userAccount.Password, StringComparison.Ordinal))
            {
                userAccount = null;
                return AccountLoginResult.InvalidPassword;
            }

            // Check for in use already
            if (userAccount.CurrentIp.HasValue)
            {
                userAccount = null;
                return AccountLoginResult.AccountInUse;
            }

            // Try to mark the account as in use
            if (!dbController.GetQuery<TrySetAccountIPIfNullQuery>().Execute(userAccount.ID, socket.IP))
            {
                userAccount = null;
                return AccountLoginResult.AccountInUse;
            }

            // Get the characters in this account
            userAccount.LoadCharacterIDs(dbController);

            return AccountLoginResult.Successful;
        }

        /// <summary>
        /// Sends the <see cref="AccountCharacterInfo"/>s for the <see cref="Character"/>s in this account to the
        /// client.
        /// </summary>
        public void SendAccountCharacterInfos()
        {
            var charInfos = new AccountCharacterInfo[CharacterCount];
            for (int i = 0; i < charInfos.Length; i++)
            {
                CharacterID characterID = _characterIDs[i];
                charInfos[i] = _dbController.GetQuery<SelectAccountCharacterInfoQuery>().Execute(characterID, (byte)i);
            }

            using (PacketWriter pw = ServerPacket.SendAccountCharacters(charInfos))
            {
                Socket.Send(pw);
            }
        }

        /// <summary>
        /// Loads and sets the User being used by this account.
        /// </summary>
        /// <param name="world">The World that the User will be part of.</param>
        /// <param name="characterID">The CharacterID of the user to use.</param>
        public void SetUser(World world, CharacterID characterID)
        {
            lock (_setUserLock)
            {
                // Make sure the user is not already set
                if (User != null)
                {
                    const string errmsg = "Cannot use SetUser when the User is not null.";
                    Debug.Fail(errmsg);
                    if (log.IsErrorEnabled)
                        log.Error(errmsg);
                    return;
                }

                // Make sure the CharacterID is an ID of a character belonging to this UserAccount
                if (!_characterIDs.Contains(characterID))
                {
                    const string errmsg = "Cannot use CharacterID `{0}` - that character does not belong to this UserAccount.";
                    Debug.Fail(string.Format(errmsg, characterID));
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, characterID);
                    return;
                }

                // Load the User
                _user = new User(_socket, world, characterID);
            }

            User u = User;
            if (log.IsInfoEnabled && u != null)
                log.InfoFormat("Set User `{0}` on account `{1}`.", u, this);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, ID);
        }

        /// <summary>
        /// Tries to get the CharacterID for a Character in the UserAccount by the given index.
        /// </summary>
        /// <param name="index">The 0-based index of the CharacterID.</param>
        /// <param name="value">The CharacterID for the Character at the given <paramref name="index"/>.</param>
        /// <returns>True if the <paramref name="value"/> was successfully acquired; otherwise false.</returns>
        public bool TryGetCharacterID(byte index, out CharacterID value)
        {
            if (index < 0 || index >= _characterIDs.Count)
            {
                value = new CharacterID(0);
                return false;
            }

            value = _characterIDs[index];
            return true;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            // Make sure the User is closed
            CloseUser();

            // Dispose of the socket
            if (Socket != null)
                Socket.Dispose();

            // Log the account out in the database
            _dbController.GetQuery<SetAccountCurrentIPNullQuery>().Execute(ID);

            if (log.IsInfoEnabled)
                log.InfoFormat("Disposed account `{0}`.", this);
        }

        #endregion
    }
}