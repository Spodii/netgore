using System.Linq;
using DemoGame;
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