using System.Linq;
using NetGore;

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