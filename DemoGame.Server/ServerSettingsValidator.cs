using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Properties;
using log4net;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// Validates the settings of the <see cref="Server"/> to make sure they are valid and logical.
    /// </summary>
    public class ServerSettingsValidator
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Adds a message to the error list.
        /// </summary>
        /// <param name="errs">The error list.</param>
        /// <param name="message">The message.</param>
        /// <param name="p">The <paramref name="message"/> parameters.</param>
        void Add(List<string> errs, string message, params object[] p)
        {
            string m;

            if (p != null && p.Length > 0)
                m = string.Format(message, p);
            else
                m = message;

            if (log.IsWarnEnabled)
                log.WarnFormat("Suspicious setting found: {0}", m);

            errs.Add(m);
        }

        /// <summary>
        /// Checks the server's settings for being valid.
        /// </summary>
        /// <returns>The list of descriptions on potentially invalid settings. Will be empty if no settings appeared to be invalid.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="server"/> is null.</exception>
        public List<string> Check(Server server)
        {
            if (log.IsInfoEnabled)
                log.Info("Checking the server settings.");

            var errs = new List<string>();

            CheckServerSettings(server, errs);

            return errs;
        }

        void CheckLegalMapPos(List<string> errs, Server server, string settingName, MapID mapID, Vector2 pos)
        {
            var map = server.World.GetMap(mapID);
            if (map == null)
            {
                Add(errs, "`{0}` is invalid: map `{1}` does not exist", settingName, mapID);
                return;
            }

            if (pos.X < 0 || pos.Y < 0 || pos.X > map.Width || pos.Y > map.Height)
            {
                Add(errs, "`{0}` is invalid: position `{1}` is outside of the bounds of map `{2}` (size: {3})", settingName, pos,
                    map, map.Size);
                return;
            }
        }

        /// <summary>
        /// Checks the <see cref="ServerSettings"/>.
        /// </summary>
        /// <param name="server">The <see cref="Server"/>.</param>
        /// <param name="errs">The list for holding errors.</param>
        void CheckServerSettings(Server server, List<string> errs)
        {
            const int secs = 1000;
            const int mins = secs * 60;
            const int hours = mins * 60;
            const int days = hours * 24;

            var ss = ServerSettings.Default;

            /* Range checks */
            CheckValueGreaterThan(errs, ss.CharacterJumpVelocity, "ServerSettings.CharacterJumpVelocity", -10);
            CheckValueLessThan(errs, ss.CharacterJumpVelocity, "ServerSettings.CharacterJumpVelocity", float.Epsilon);

            CheckValueGreaterThan(errs, ss.DefaultMapItemLife, "ServerSettings.DefaultMapItemLife", 1 * secs);
            CheckValueLessThan(errs, ss.DefaultMapItemLife, "ServerSettings.DefaultMapItemLife", 1 * days);

            CheckValueGreaterThan(errs, ss.MaxConnections, "ServerSettings.MaxConnections", -1);
            CheckValueLessThan(errs, ss.MaxConnections, "ServerSettings.MaxConnections", 60000);

            CheckValueGreaterThan(errs, ss.MaxRecentlyCreatedAccounts, "ServerSettings.MaxRecentlyCreatedAccounts", 0);
            CheckValueLessThan(errs, ss.MaxRecentlyCreatedAccounts, "ServerSettings.MaxRecentlyCreatedAccounts", 20);

            CheckValueGreaterThan(errs, ss.RespawnablesUpdateRate, "ServerSettings.MaxRecentlyCreatedAccounts", 100);
            CheckValueLessThan(errs, ss.RespawnablesUpdateRate, "ServerSettings.RespawnablesUpdateRate", mins * 10);

            CheckValueGreaterThan(errs, ss.RoutineServerSaveRate, "ServerSettings.RoutineServerSaveRate", mins * 1);
            CheckValueLessThan(errs, ss.RoutineServerSaveRate, "ServerSettings.RoutineServerSaveRate", days * 2);

            CheckValueGreaterThan(errs, ss.ServerUpdateRate, "ServerSettings.ServerUpdateRate", 0);
            CheckValueLessThan(errs, ss.ServerUpdateRate, "ServerSettings.ServerUpdateRate", 500);

            CheckValueGreaterThan(errs, ss.SyncExtraUserInformationRate, "ServerSettings.SyncExtraUserInformationRate", 10);
            CheckValueLessThan(errs, ss.SyncExtraUserInformationRate, "ServerSettings.SyncExtraUserInformationRate", secs * 5);

            /* Map checks */
            CheckLegalMapPos(errs, server, "ServerSettings.InvalidPersistentNPCLoadMap", ss.InvalidPersistentNPCLoadMap,
                ss.InvalidPersistentNPCLoadPosition);

            CheckLegalMapPos(errs, server, "ServerSettings.InvalidUserLoadMap", ss.InvalidUserLoadMap, ss.InvalidUserLoadPosition);
        }

        void CheckValueGreaterThan(List<string> errs, long actual, string name, long min)
        {
            if (actual < min)
                Add(errs, "`{0}` is set suspiciously low. Value: {1}", name, actual);
        }

        void CheckValueGreaterThan(List<string> errs, double actual, string name, double min)
        {
            if (actual < min)
                Add(errs, "`{0}` is set suspiciously low. Value: {1}", name, actual);
        }

        void CheckValueLessThan(List<string> errs, long actual, string name, long max)
        {
            if (actual > max)
                Add(errs, "`{0}` is set suspiciously high. Value: {1}", name, actual);
        }

        void CheckValueLessThan(List<string> errs, double actual, string name, double max)
        {
            if (actual > max)
                Add(errs, "`{0}` is set suspiciously high. Value: {1}", name, actual);
        }
    }
}