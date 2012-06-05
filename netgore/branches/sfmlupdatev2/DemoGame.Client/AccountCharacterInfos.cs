using System;
using System.Diagnostics;
using System.Linq;
using NetGore;

namespace DemoGame.Client
{
    public class AccountCharacterInfos
    {
        AccountCharacterInfo[] _charInfos;
        bool _isLoaded;

        /// <summary>
        /// Notifies listeners when the account characters are loaded.
        /// </summary>
        public event TypedEventHandler<AccountCharacterInfos> AccountCharactersLoaded;

        public AccountCharacterInfo this[byte index]
        {
            get { return _charInfos[index]; }
        }

        public byte Count
        {
            get { return (byte)_charInfos.Length; }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the event with the corresponding name.
        /// </summary>
        protected virtual void OnAccountCharactersLoaded()
        {
        }

        public void SetInfos(AccountCharacterInfo[] charInfos)
        {
            if (charInfos == null)
            {
                // Shouldn't be null, but we can recover
                Debug.Fail("charInfos parameter is null.");
                charInfos = new AccountCharacterInfo[0];
            }

            _charInfos = charInfos;
            _isLoaded = true;

            if (AccountCharactersLoaded != null)
                AccountCharactersLoaded(this, EventArgs.Empty);
        }

        public bool TryGetInfo(byte index, out AccountCharacterInfo charInfo)
        {
            if (index < 0 || index >= _charInfos.Length)
            {
                charInfo = null;
                return false;
            }

            charInfo = _charInfos[index];
            return true;
        }
    }
}