using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Properties;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Cryptography;
using NetGore.Db;
using NetGore.Network;

namespace DemoGame.Server
{
    public class UserAccountManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly string _passwordSalt = ServerSettings.Default.PasswordSalt;

        readonly IDictionary<string, UserAccount> _accounts = new Dictionary<string, UserAccount>(StringComparer.OrdinalIgnoreCase);
        readonly object _accountsSync = new object();
        readonly IDbController _dbController;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountManager"/> class.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dbController" /> is <c>null</c>.</exception>
        public UserAccountManager(IDbController dbController)
        {
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _dbController = dbController;
        }

        /// <summary>
        /// Gets the <see cref="IDbController"/> to use.
        /// </summary>
        public IDbController DbController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Generates a salted hash for a password.
        /// </summary>
        /// <param name="originalPassword">The original (raw text) password.</param>
        /// <returns>The salted and hashed password.</returns>
        public static string EncodePassword(string originalPassword)
        {
            // Apply the salt (if one exists)
            string saltedPassword;
            if (!string.IsNullOrEmpty(_passwordSalt))
                saltedPassword = _passwordSalt + originalPassword;
            else
                saltedPassword = originalPassword;

            // Hash the salted password
            return Hasher.GetHashAsBase16String(saltedPassword);
        }

        /// <summary>
        /// Finds the <see cref="IUserAccount"/> for a given user. Can only find the <see cref="IUserAccount"/> instance for
        /// a user that is logged into this server.
        /// </summary>
        /// <param name="userName">The name of the user to find the <see cref="IUserAccount"/> for.</param>
        /// <returns>The <see cref="IUserAccount"/> for the <paramref name="userName"/>, or null if not found.</returns>
        public IUserAccount FindUserAccount(string userName)
        {
            lock (_accountsSync)
            {
                UserAccount ret;
                if (!_accounts.TryGetValue(userName, out ret))
                    return null;

                return ret;
            }
        }

        /// <summary>
        /// Tries to log in an account.
        /// </summary>
        /// <param name="socket">The socket used to communicate with the client.</param>
        /// <param name="name">The name of the account.</param>
        /// <param name="password">The account password.</param>
        /// <param name="userAccount">When this method returns <see cref="AccountLoginResult.Successful"/>,
        /// contains the <see cref="IUserAccount"/> that was logged in to. Otherwise, this value will be null.</param>
        /// <returns>If <see cref="AccountLoginResult.Successful"/>, the login was successful. Otherwise, contains
        /// the reason why the login failed.</returns>
        public AccountLoginResult Login(IIPSocket socket, string name, string password, out IUserAccount userAccount)
        {
            // Try to load the account data
            var accountTable = DbController.GetQuery<SelectAccountQuery>().TryExecute(name);
            if (accountTable == null)
            {
                userAccount = null;
                return AccountLoginResult.InvalidName;
            }

            // Check the password
            var encodedPass = EncodePassword(password);
            if (!StringComparer.OrdinalIgnoreCase.Equals(encodedPass, accountTable.Password))
            {
                userAccount = null;
                return AccountLoginResult.InvalidPassword;
            }

            // Check if the account is already logged in to
            if (accountTable.CurrentIp.HasValue)
            {
                if (ServerSettings.Default.AccountDropExistingConnectionWhenInUse)
                {
                    // Kick existing user so the new connection can enter the account
                    UserAccount existingAccount;
                    lock (_accountsSync)
                    {
                        if (!_accounts.TryGetValue(name, out existingAccount))
                            existingAccount = null;
                    }

                    if (existingAccount != null)
                        existingAccount.Dispose();
                }
                else
                {
                    // Let the existing user stay connected and reject the new connection to the account
                    userAccount = null;
                    return AccountLoginResult.AccountInUse;
                }
            }

            // Try to mark the account as in use
            if (!DbController.GetQuery<TrySetAccountIPIfNullQuery>().Execute(accountTable.ID, socket.IP))
            {
                userAccount = null;
                return AccountLoginResult.AccountInUse;
            }

            // Try to add the new account to the collection
            lock (_accountsSync)
            {
                // If for some reason an account instance already exists, close it
                UserAccount existingAccount;
                if (_accounts.TryGetValue(name, out existingAccount))
                {
                    const string errmsg = "UserAccount for `{0}` already existing in _accounts collection somehow.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, name);
                    Debug.Fail(string.Format(errmsg, name));

                    userAccount = null;
                    return AccountLoginResult.AccountInUse;
                }

                // Create the account instance
                userAccount = new UserAccount(accountTable, socket, this);

                // Add
                _accounts.Add(name, (UserAccount)userAccount);
            }

            // Load the characters in this account
            userAccount.LoadCharacterIDs();

            // Record the login IP
            DbController.GetQuery<InsertAccountIPQuery>().Execute(userAccount.ID, socket.IP);

            return AccountLoginResult.Successful;
        }

        /// <summary>
        /// Handles when a <see cref="UserAccount"/> is disposed.
        /// </summary>
        /// <param name="account">The <see cref="UserAccount"/> that was disposed.</param>
        void NotifyAccountDisposed(UserAccount account)
        {
            lock (_accountsSync)
            {
                _accounts.Remove(account.Name);
            }
        }

        /// <summary>
        /// Tries to add a <see cref="Character"/> to an account.
        /// </summary>
        /// <param name="accountName">Name of the account.</param>
        /// <param name="characterName">Name of the character.</param>
        /// <param name="errorMsg">If this method returns false, contains the error message.</param>
        /// <returns>True if the character was successfully added to the account; otherwise false.</returns>
        public bool TryAddCharacter(string accountName, string characterName, out string errorMsg)
        {
            // Try to execute the query
            var createUserQuery = DbController.GetQuery<CreateUserOnAccountQuery>();
            if (!createUserQuery.TryExecute(accountName, characterName, out errorMsg))
                return false;

            errorMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// Tries to create a new user account.
        /// </summary>
        /// <param name="socket">The socket containing the connection trying to create the account. Can be null.</param>
        /// <param name="name">The account name.</param>
        /// <param name="password">The account password.</param>
        /// <param name="email">The account email address.</param>
        /// <param name="failReason">When this method returns false, contains a <see cref="GameMessage"/> that will be
        /// used to describe why the account failed to be created.</param>
        /// <returns>True if the account was successfully created; otherwise false.</returns>
        public bool TryCreateAccount(IIPSocket socket, string name, string password, string email, out GameMessage failReason)
        {
            // Check for valid values
            if (!GameData.AccountName.IsValid(name))
            {
                failReason = GameMessage.CreateAccountInvalidName;
                return false;
            }

            if (!GameData.AccountPassword.IsValid(password))
            {
                failReason = GameMessage.CreateAccountInvalidPassword;
                return false;
            }

            if (!GameData.AccountEmail.IsValid(email))
            {
                failReason = GameMessage.CreateAccountInvalidEmail;
                return false;
            }

            // Get the IP to use
            var ip = socket != null ? socket.IP : GameData.DefaultCreateAccountIP;

            // Check if too many accounts have been created from this IP
            if (ip != GameData.DefaultCreateAccountIP)
            {
                var recentCreatedCount = DbController.GetQuery<CountRecentlyCreatedAccounts>().Execute(ip);
                if (recentCreatedCount >= ServerSettings.Default.MaxRecentlyCreatedAccounts)
                {
                    failReason = GameMessage.CreateAccountTooManyCreated;
                    return false;
                }
            }

            // Check if the account exists
            var existingAccountID = DbController.GetQuery<SelectAccountIDFromNameQuery>().Execute(name);
            if (existingAccountID.HasValue)
            {
                failReason = GameMessage.CreateAccountAlreadyExists;
                return false;
            }

            // Try to execute the query
            var success = DbController.GetQuery<CreateAccountQuery>().TryExecute(name, password, email, ip);

            failReason = GameMessage.CreateAccountUnknownError;
            return success;
        }

        /// <summary>
        /// Tries to get the AccountID for the account with the given name.
        /// </summary>
        /// <param name="accountName">The name of the account.</param>
        /// <param name="accountID">When the method returns true, contains the ID of the account with the given
        /// <paramref name="accountName"/>.</param>
        /// <returns>True if the <paramref name="accountID"/> was found; otherwise false.</returns>
        public bool TryGetAccountID(string accountName, out AccountID accountID)
        {
            var value = DbController.GetQuery<SelectAccountIDFromNameQuery>().Execute(accountName);

            if (!value.HasValue)
            {
                accountID = new AccountID(0);
                return false;
            }
            else
            {
                accountID = value.Value;
                return true;
            }
        }

        /// <summary>
        /// Contains the information for a <see cref="DemoGame.Server.User"/>'s account.
        /// </summary>
        class UserAccount : AccountTable, IUserAccount
        {
            readonly List<CharacterID> _characterIDs = new List<CharacterID>();
            readonly UserAccountManager _parent;
            readonly IIPSocket _socket;

            User _user;

            /// <summary>
            /// Initializes a new instance of the <see cref="UserAccount"/> class.
            /// </summary>
            /// <param name="accountTable">The account table.</param>
            /// <param name="socket">The socket.</param>
            /// <param name="parent">The <see cref="UserAccountManager"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="socket" /> is <c>null</c>.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="parent" /> is <c>null</c>.</exception>
            internal UserAccount(IAccountTable accountTable, IIPSocket socket, UserAccountManager parent) : base(accountTable)
            {
                if (socket == null)
                    throw new ArgumentNullException("socket");
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _socket = socket;
                _parent = parent;
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override string ToString()
            {
                return string.Format("{0} [{1}]", Name, ID);
            }

            #region IUserAccount Members

            /// <summary>
            /// Gets the number of Characters in this UserAccount.
            /// </summary>
            public byte CharacterCount
            {
                get { return (byte)_characterIDs.Count; }
            }

            /// <summary>
            /// Gets an IEnumerable of the <see cref="CharacterID"/>s for the <see cref="Character"/>s that are in
            /// this <see cref="IUserAccount"/>.
            /// </summary>
            public IEnumerable<CharacterID> CharacterIDs
            {
                get { return _characterIDs; }
            }

            public IDbController DbController
            {
                get { return _parent.DbController; }
            }

            /// <summary>
            /// Gets the <see cref="IIPSocket"/> that is used to communicate with the client connected to this <see cref="IUserAccount"/>.
            /// </summary>
            public IIPSocket Socket
            {
                get { return _socket; }
            }

            /// <summary>
            /// Gets the <see cref="DemoGame.Server.User"/> currently logged in on this <see cref="IUserAccount"/>.
            /// </summary>
            public User User
            {
                get { return _user; }
            }

            /// <summary>
            /// Logs out the User from this UserAccount. If the <see cref="User"/> is not null and has not been disposed,
            /// this method will dispose of the User, too.
            /// </summary>
            public void CloseUser()
            {
                ThreadAsserts.IsMainThread();

                var u = _user;
                if (u == null)
                    return;

                _user = null;

                if (log.IsInfoEnabled)
                    log.InfoFormat("Closed User `{0}` on account `{1}`.", u, this);

                // Make sure the user was disposed
                if (!u.IsDisposed)
                    u.Dispose();
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <param name="disconnectMessage">The message to use for when disconnecting the socket. When disposing an active connection,
            /// this can provide the client a reason why they were disconnected. The default is
            /// <see cref="GameMessage.DisconnectUserDisposed"/>.</param>
            /// <param name="p">The arguments for the <paramref name="disconnectMessage"/>.</param>
            public void Dispose(GameMessage disconnectMessage, params object[] p)
            {
                ThreadAsserts.IsMainThread();

                try
                {
                    // Make sure the User is closed
                    CloseUser();

                    // Break the connection, if connected
                    if (Socket != null)
                        Socket.Disconnect(disconnectMessage, p);

                    // Log the account out in the database
                    DbController.GetQuery<SetAccountCurrentIPNullQuery>().Execute(ID);

                    if (log.IsInfoEnabled)
                        log.InfoFormat("Disposed account `{0}`.", this);
                }
                finally
                {
                    _parent.NotifyAccountDisposed(this);
                }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                Dispose(GameMessage.DisconnectUserDisposed);
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
            /// Loads the <see cref="CharacterID"/>s for the Characters in this account.
            /// </summary>
            public void LoadCharacterIDs()
            {
                var ids = DbController.GetQuery<SelectAccountCharacterIDsQuery>().Execute(ID);
                _characterIDs.Clear();
                _characterIDs.AddRange(ids);
            }

            /// <summary>
            /// Sends the <see cref="AccountCharacterInfo"/>s for the <see cref="Character"/>s in this account to the
            /// client.
            /// </summary>
            public void SendAccountCharacterInfos()
            {
                var charInfos = new AccountCharacterInfo[CharacterCount];
                for (var i = 0; i < charInfos.Length; i++)
                {
                    var characterID = _characterIDs[i];

                    var v = DbController.GetQuery<SelectAccountCharacterInfoQuery>().Execute(characterID, (byte)i);
                    if (v != null)
                    {
                        var eqBodies = DbController.GetQuery<SelectCharacterEquippedBodiesQuery>().Execute(characterID);
                        if (eqBodies != null)
                            v.SetEquippedBodies(eqBodies);

                        charInfos[i] = v;
                    }
                }

                using (var pw = ServerPacket.SendAccountCharacters(charInfos))
                {
                    Socket.Send(pw, ServerMessageType.System);
                }
            }

            /// <summary>
            /// Sets the permission level for this account.
            /// </summary>
            /// <param name="newPermissions">The new <see cref="UserPermissions"/> level.</param>
            public void SetPermissions(UserPermissions newPermissions)
            {
                if (newPermissions == Permissions)
                    return;

                // Set the new value
                Permissions = newPermissions;

                // Update the database
                DbController.GetQuery<UpdateAccountPermissionsQuery>().Execute(ID, Permissions);
            }

            /// <summary>
            /// Sets the friends list for this account.
            /// </summary>
            /// <param name="friends">The friends list.</param>
            public void SetFriends(string friends)
            {
                if (String.IsNullOrEmpty(friends))
                    return;

                // Set the new value
                Friends = friends;

                // Update the database
                DbController.GetQuery<UpdateAccountFriendsQuery>().Execute(ID, Friends);
            }

            /// <summary>
            /// Loads and sets the User being used by this account.
            /// </summary>
            /// <param name="world">The World that the User will be part of.</param>
            /// <param name="characterID">The CharacterID of the user to use.</param>
            public void SetUser(World world, CharacterID characterID)
            {
                ThreadAsserts.IsMainThread();

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
                try
                {
                    _user = new User(this, world, characterID);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to create user with ID `{0}`. Exception: {1}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, characterID, ex);
                    Debug.Fail(string.Format(errmsg, characterID, ex));

                    Dispose();
                    return;
                }

                if (log.IsInfoEnabled)
                    log.InfoFormat("Set User `{0}` on account `{1}`.", _user, this);
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

            #endregion
        }
    }
}