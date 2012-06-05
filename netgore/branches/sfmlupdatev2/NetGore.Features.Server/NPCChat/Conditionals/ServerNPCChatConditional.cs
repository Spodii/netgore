using System.Diagnostics;
using System.Linq;

namespace NetGore.Features.NPCChat.Conditionals
{
    /// <summary>
    /// The base class for a conditional used in the NPC chatting. Each instanceable derived class
    /// must include a parameterless constructor (preferably private).
    /// </summary>
    /// <typeparam name="TUser">The type of User.</typeparam>
    /// <typeparam name="TNPC">The type of NPC.</typeparam>
    public abstract class ServerNPCChatConditional<TUser, TNPC> : NPCChatConditionalBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerNPCChatConditional{TUser,TNPC}"/> class.
        /// </summary>
        /// <param name="name">The unique display name of this NPCChatConditionalBase. This name must be unique
        /// for each derived class type. This string is case-sensitive.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        protected ServerNPCChatConditional(string name, params NPCChatConditionalParameterType[] parameterTypes)
            : base(name, parameterTypes)
        {
        }

        /// <summary>
        /// When overridden in the derived class, performs the actual conditional evaluation.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <param name="parameters">The parameters to use.</param>
        /// <returns>True if the conditional returns true for the given <paramref name="user"/>,
        /// <paramref name="npc"/>, and <paramref name="parameters"/>; otherwise false.</returns>
        protected abstract bool DoEvaluate(TUser user, TNPC npc, NPCChatConditionalParameter[] parameters);

        /// <summary>
        /// When overridden in the derived class, performs the actual conditional evaluation.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <param name="parameters">The parameters to use. </param>
        /// <returns>True if the conditional returns true for the given <paramref name="user"/>,
        /// <paramref name="npc"/>, and <paramref name="parameters"/>; otherwise false.</returns>
        protected override bool DoEvaluate(object user, object npc, NPCChatConditionalParameter[] parameters)
        {
            return DoEvaluate((TUser)user, (TNPC)npc, parameters);
        }

        /// <summary>
        /// Safely gets the percent of two values.
        /// </summary>
        /// <param name="num">The numerator.</param>
        /// <param name="denom">The denominator.</param>
        /// <returns>The percent.</returns>
        protected static float GetPercent(uint num, uint denom)
        {
            Debug.Assert(num >= float.MinValue && num <= float.MaxValue);
            Debug.Assert(denom >= float.MinValue && denom <= float.MaxValue);
            return GetPercent((float)num, denom);
        }

        /// <summary>
        /// Safely gets the percent of two values.
        /// </summary>
        /// <param name="num">The numerator.</param>
        /// <param name="denom">The denominator.</param>
        /// <returns>The percent.</returns>
        protected static float GetPercent(int num, int denom)
        {
            Debug.Assert(num >= float.MinValue && num <= float.MaxValue);
            Debug.Assert(denom >= float.MinValue && denom <= float.MaxValue);
            return GetPercent((float)num, denom);
        }

        /// <summary>
        /// Safely gets the percent of two values.
        /// </summary>
        /// <param name="num">The numerator.</param>
        /// <param name="denom">The denominator.</param>
        /// <returns>The percent.</returns>
        protected static float GetPercent(float num, float denom)
        {
            // Ensure we get absolute values for 0% and 100%
            if (num == 0 || denom == 0)
                return 0;
            if (num == denom)
                return 100;

            var p = (num / denom) * 100.0f;

            Debug.Assert(p >= 0f);
            Debug.Assert(p <= 100f);

            return p;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}