using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.Features.Skills;
using NetGore.World;

namespace DemoGame.Client
{
    public partial class ClientPacketHandler
    {
        /// <summary>
        /// Sub-class for the <see cref="ClientPacketHandler"/> that handles fishing data out of stuff while logging and
        /// handling errors when any come up. This is only provided as a sub-class to help keep the logic for the
        /// main <see cref="ClientPacketHandler"/> more focused on the actual packets.
        /// </summary>
        class ObjGrabber
        {
            /// <summary>
            /// How long to wait after the first failure to acquiring a <see cref="DynamicEntity"/> by its <see cref="MapEntityIndex"/>
            /// before sending the request to the server for information on the <see cref="DynamicEntity"/>.
            /// </summary>
            const int _maxGetDynamicEntityFailTimeout = 1200;

            /// <summary>
            /// Contains the <see cref="MapEntityIndex"/> of DynamicEntities that have failed to be acquired properly, and the
            /// time that the first failure occured.
            /// </summary>
            readonly IDictionary<MapEntityIndex, TickCount> _getDynamicEntityFailTimers =
                new Dictionary<MapEntityIndex, TickCount>();

            readonly ClientPacketHandler _parent;

            /// <summary>
            /// Initializes a new instance of the <see cref="ObjGrabber"/> class.
            /// </summary>
            /// <param name="cph">The <see cref="ClientPacketHandler"/>.</param>
            public ObjGrabber(ClientPacketHandler cph)
            {
                _parent = cph;
            }

            /// <summary>
            /// Gets a dynamic entity.
            /// </summary>
            /// <typeparam name="T">The type of <see cref="DynamicEntity"/> to get.</typeparam>
            /// <param name="index">The <see cref="MapEntityIndex"/>.</param>
            /// <returns>The <see cref="DynamicEntity"/> for the given <paramref name="index"/>, or null if no <see cref="DynamicEntity"/>
            /// exists at that index or it was an invalid type.</returns>
            public T GetDynamicEntity<T>(MapEntityIndex index) where T : class
            {
                var de = _parent.Map.GetDynamicEntity(index);
                var casted = de as T;

                if (casted != null)
                {
                    // Successfully got the DynamicEntity
                    Debug.Assert(de.MapEntityIndex == index);
                    _getDynamicEntityFailTimers.Remove(index);
                    return casted;
                }

                // Failed to get the DynamicEntity in the right type (or at all)

                // Log
                if (de == null)
                {
                    // No DynamicEntity found
                    const string errmsg = "No DynamicEntity found at index `{0}`.";
                    if (log.IsInfoEnabled)
                        log.InfoFormat(errmsg, index);
                }
                else
                {
                    // DynamicEntity found, but of the wrong type
                    const string errmsg = "DynamicEntity `{0}` found at index `{1}`, but it was of type `{2}` and not `{3}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, de, index, de.GetType(), typeof(T));
                }

                var now = TickCount.Now;

                // Check if we have failed with this index before, and if it is time to send a request to the server. Or, if we haven't
                // had a failure yet, add it.
                TickCount firstFailTime;
                if (!_getDynamicEntityFailTimers.TryGetValue(index, out firstFailTime))
                    _getDynamicEntityFailTimers.Add(index, now);
                else
                {
                    var deltaTime = now - firstFailTime;
                    if (deltaTime > _maxGetDynamicEntityFailTimeout)
                    {
                        const string msg = "Sending request for DynamicEntity with index `{0}`.";
                        if (log.IsInfoEnabled)
                            log.InfoFormat(msg, index);

                        // Enough time has elapsed to send the request
                        using (var pw = ClientPacket.RequestDynamicEntity(index))
                        {
                            _parent.NetworkSender.Send(pw, ClientMessageType.System);
                        }

                        _getDynamicEntityFailTimers.Remove(index);
                    }
                }

                return null;
            }

            /// <summary>
            /// Gets the <see cref="SkillInfo{T}"/> for a skill.
            /// </summary>
            /// <param name="skillType">The type of skill.</param>
            /// <returns>The <see cref="SkillInfo{T}"/> for the <paramref name="skillType"/>, or null if not found.</returns>
            public SkillInfo<SkillType> GetSkillInfo(SkillType skillType)
            {
                var ret = SkillInfoManager.Instance[skillType];
                if (ret == null)
                {
                    const string errmsg = "No SkillInfo<> could be found for SkillType `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, skillType);
                    Debug.Fail(string.Format(errmsg, skillType));
                }

                return ret;
            }
        }
    }
}