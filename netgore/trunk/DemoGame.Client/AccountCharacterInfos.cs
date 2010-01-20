using System.Diagnostics;
using System.Linq;

namespace DemoGame.Client
{
    class AccountCharacterInfos
    {
        AccountCharacterInfo[] _charInfos;
        bool _isLoaded;

        public delegate void AccountCharactersLoadedHandler();

        public event AccountCharactersLoadedHandler OnAccountCharactersLoaded;

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

            if (OnAccountCharactersLoaded != null)
                OnAccountCharactersLoaded();
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