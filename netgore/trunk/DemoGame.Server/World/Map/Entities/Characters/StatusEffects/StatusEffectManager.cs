using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Collections;
using NetGore.Features.StatusEffects;

namespace DemoGame.Server
{
    public sealed class StatusEffectManager : StatusEffectManager<StatType, StatusEffectType>
    {
        /// <summary>
        /// Gets the <see cref="StatusEffectManager"/> instance.
        /// </summary>
        public static StatusEffectManager Instance { get { return _instance; } }

        /// <summary>
        /// The <see cref="StatusEffectManager"/> instance.
        /// </summary>
        static readonly StatusEffectManager _instance;

        /// <summary>
        /// Initializes the <see cref="StatusEffectManager"/> class.
        /// </summary>
        static StatusEffectManager()
        {
            _instance = new StatusEffectManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectManager"/> class.
        /// </summary>
        StatusEffectManager()
        {
        }
    }
}