using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Guilds
{
    /// <summary>
    /// Extension methods for the <see cref="IGuildMember"/> interface.
    /// </summary>
    public static class IGuildMemberExtensions
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Attempts to cast a <see cref="IGuildMember"/> to <see cref="User"/>. This method expects the
        /// <paramref name="guildMember"/> to be of the target type. If its not, it will be treated as an error.
        /// </summary>
        /// <param name="guildMember">The <see cref="IGuildMember"/> to cast.</param>
        /// <returns>The <paramref name="guildMember"/> casted to <see cref="User"/>, or null if invalid.</returns>
        public static User AsUser(this IGuildMember guildMember)
        {
            var ret = guildMember as User;
            if (ret == null)
            {
                const string errmsg = "Guild member `{0}` is not a User.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, guildMember);
                Debug.Fail(string.Format(errmsg, guildMember));
            }

            return ret;
        }

        /// <summary>
        /// Attempts to cast a <see cref="IGuildMember"/> to <see cref="Character"/>. This method expects the
        /// <paramref name="guildMember"/> to be of the target type. If its not, it will be treated as an error.
        /// </summary>
        /// <param name="guildMember">The <see cref="IGuildMember"/> to cast.</param>
        /// <returns>The <paramref name="guildMember"/> casted to <see cref="Character"/>, or null if invalid.</returns>
        public static Character AsCharacter(this IGuildMember guildMember)
        {
            var ret = guildMember as Character;
            if (ret == null)
            {
                const string errmsg = "Guild member `{0}` is not a Character.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, guildMember);
                Debug.Fail(string.Format(errmsg, guildMember));
            }

            return ret;
        }
    }
}