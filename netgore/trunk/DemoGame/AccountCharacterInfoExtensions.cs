using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    public static class AccountCharacterInfoExtensions
    {
        public static void Write(this IValueWriter writer, AccountCharacterInfo accountCharacterInfo)
        {
            accountCharacterInfo.Write(writer);
        }

        public static AccountCharacterInfo ReadAccountCharacterInfo(this IValueReader reader)
        {
            return new AccountCharacterInfo(reader);
        }
    }
}