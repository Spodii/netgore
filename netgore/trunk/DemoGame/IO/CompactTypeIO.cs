using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Features.ActionDisplays;
using NetGore.IO;
using SFML.Graphics;

namespace DemoGame
{
    /// <summary>
    /// Provides compacted reading and writing of types. This IO is intended to only ever be used in non-persistent streams, and never
    /// when writing to or from files or caches. These IO calls require being called explicitly to make it very clear when it is
    /// being used, and that the compact IO is used only when intended. For every read and corresponding write operation, the same
    /// input parameters should always provide the same number of bits being used. Similarly, methods should never take parameters
    /// that are prone to being different between the server and client.
    /// </summary>
    /// <remarks>
    /// This class is purely just for optimization, and nothing more. Using it is never required.
    /// Do not add to this class unless you know very well what you are doing.
    /// Make sure to never call these methods on IO to persistant storage.
    /// Overhead for all calls should be relatively low.
    /// Make sure to follow the same naming and signature style for all methods.
    /// Placeholder methods for values that need to be optimized in the future are perfectly acceptable
    ///     - just be sure to denote it with a NOTE comment.
    /// </remarks>
    public static class CompactTypeIO
    {
        #region ActionDisplayID

        public static void WriteActionDisplayID(BitStream bs, ActionDisplayID value)
        {
            // NOTE: Placeholder
            bs.Write(value);
        }

        public static ActionDisplayID ReadActionDisplayID(BitStream bs)
        {
            // NOTE: Placeholder
            return bs.ReadActionDisplayID();
        }

        #endregion

        #region NullableActionDisplayID

        public static void WriteNullableActionDisplayID(BitStream bs, ActionDisplayID? value)
        {
            bs.Write(value.HasValue);
            if (value.HasValue)
                WriteActionDisplayID(bs, value.Value);
        }

        public static ActionDisplayID? ReadNullableActionDisplayID(BitStream bs)
        {
            bool hasValue = bs.ReadBool();
            if (!hasValue)
                return null;

            return ReadActionDisplayID(bs);
        }

        #endregion

        #region Damage

        public static void WriteDamage(BitStream bs, int value)
        {
            // NOTE: Placeholder
            bs.Write(value);
        }

        public static int ReadDamage(BitStream bs)
        {
            // NOTE: Placeholder
            return bs.ReadInt();
        }

        #endregion

        #region DynamicEntityType

        public static void WriteDynamicEntityType(BitStream bs, string value)
        {
            // NOTE: Placeholder
            bs.Write(value);
        }

        public static string ReadDynamicEntityType(BitStream bs)
        {
            // NOTE: Placeholder
            return bs.ReadString();
        }

        #endregion

        #region DynamicEntityPosition

        public static void WriteDynamicEntityPosition(BitStream bs, Vector2 value)
        {
            // NOTE: Placeholder
            bs.Write(value);
        }

        public static Vector2 ReadDynamicEntityPosition(BitStream bs)
        {
            // NOTE: Placeholder
            return bs.ReadVector2();
        }

        #endregion

        #region DynamicEntityVelocity

        public static void WriteDynamicEntityVelocity(BitStream bs, Vector2 value)
        {
            // NOTE: Placeholder
            bs.Write(value);
        }

        public static Vector2 ReadDynamicEntityVelocity(BitStream bs)
        {
            // NOTE: Placeholder
            return bs.ReadVector2();
        }

        #endregion
    }
}
