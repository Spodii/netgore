using System;
using System.Linq;

namespace DemoGame.Server.Queries
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbControllerQueryAttribute : Attribute
    {
    }
}