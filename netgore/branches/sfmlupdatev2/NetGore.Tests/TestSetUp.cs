using System;
using System.Linq;
using NetGore.Features.GameTime;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;
using NetGore.Features.PeerTrading;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Features.StatusEffects;
using NUnit.Framework;
using SFML.Graphics;

namespace NetGore.Tests
{
    /// <summary>
    /// Provides calls to initialization methods that are run before any tests.
    /// </summary>
    [SetUpFixture]
    public class TestSetUp
    {
        /// <summary>
        /// Initializes the <see cref="EngineSettings"/> for tests that require it.
        /// </summary>
        [SetUp]
        public void SetUpEngineSettings()
        {
            EngineSettings.Initialize(new EngineSettings(new Vector2(0f, 0.0009f), new Vector2(0.5f, 0.5f)));

            ShopSettings.Initialize(new ShopSettings(6 * 6));
            StatusEffectsSettings.Initialize(new StatusEffectsSettings(500));
            GroupSettings.Initialize(new GroupSettings(10, 1000 * 60, (x, y) => true));
            QuestSettings.Initialize(new QuestSettings(20));
            GameTimeSettings.Initialize(new GameTimeSettings(19, 6, -0.5f, 50, new DateTime(2010, 1, 1), 1000f));
            PeerTradingSettings.Initialize(new PeerTradingSettings(20, 128));

            var rankNames = new string[] { "Recruit", "Member", "VIP", "Founder" };
            var nameRules = new StringRules(3, 50, CharType.All);
            var tagRules = new StringRules(1, 4, CharType.All);
            var maxRank = (GuildRank)(rankNames.Length - 1);
            GuildSettings.Initialize(new GuildSettings(1000 * 60, maxRank, rankNames, nameRules, tagRules, 3, 2, 1, 1, 2, 2));
        }
    }
}