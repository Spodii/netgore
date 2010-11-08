using System;
using System.Linq;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// A specialized <see cref="SayCommandAttribute"/> for the <see cref="SayHandler"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class SayHandlerCommandAttribute : SayCommandAttribute
    {
        readonly UserPermissions _permissions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SayHandlerCommandAttribute"/> class.
        /// </summary>
        /// <param name="command">The name of the command.</param>
        /// <param name="permissions">The permission level required to invoke this command.</param>
        public SayHandlerCommandAttribute(string command, UserPermissions permissions = UserPermissions.None) : base(command)
        {
            _permissions = permissions;
        }

        /// <summary>
        /// Gets the permission levels required to invoke this command.
        /// </summary>
        public UserPermissions Permissions
        {
            get { return _permissions; }
        }
    }
}