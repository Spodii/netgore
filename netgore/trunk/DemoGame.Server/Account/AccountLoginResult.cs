using System.Linq;

namespace DemoGame.Server
{
    public enum AccountLoginResult : byte
    {
        Successful,
        InvalidName,
        InvalidPassword,
        AccountInUse
    }
}