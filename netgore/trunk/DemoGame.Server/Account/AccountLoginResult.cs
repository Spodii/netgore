using System.Linq;

namespace DemoGame.Server
{
    public enum AccountLoginResult
    {
        Successful,
        InvalidName,
        InvalidPassword,
        AccountInUse
    }
}