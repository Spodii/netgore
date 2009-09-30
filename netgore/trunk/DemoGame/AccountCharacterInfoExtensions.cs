using System.Linq;
using DemoGame;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    public static class AccountCharacterInfoExtensions
    {
        public static AccountCharacterInfo ReadAccountCharacterInfo(this IValueReader reader)
        {
            return new AccountCharacterInfo(reader);
        }

        public static void Write(this IValueWriter writer, AccountCharacterInfo accountCharacterInfo)
        {
            accountCharacterInfo.Write(writer);
        }
    }
}