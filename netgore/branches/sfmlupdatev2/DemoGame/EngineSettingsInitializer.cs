using System;
using System.Linq;
using NetGore;
using NetGore.Features.GameTime;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;
using NetGore.Features.PeerTrading;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Features.StatusEffects;
using SFML.Graphics;

namespace DemoGame
{
    /// <summary>
    /// Provides a clean way to initialize the <see cref="EngineSettings"/>, along with any of the other game
    /// settings that need to be initialized.
    /// </summary>
    public sealed class EngineSettingsInitializer : EngineSettings
    {
        /// <summary>
        /// Initializes the <see cref="EngineSettingsInitializer"/> class.
        /// </summary>
        static EngineSettingsInitializer()
        {
            Initialize(new EngineSettingsInitializer());

            ShopSettings.Initialize(new ShopSettings(6 * 6));
            StatusEffectsSettings.Initialize(new StatusEffectsSettings(500));
            GroupSettings.Initialize(new GroupSettings(10, 1000 * 60, CanJoinGroupHandler));
            QuestSettings.Initialize(new QuestSettings(20));
            GameTimeSettings.Initialize(new GameTimeSettings(19, 6, -0.5f, 50, new DateTime(2010, 1, 1), 1000f));
            PeerTradingSettings.Initialize(new PeerTradingSettings(GameData.MaxInventorySize, 128));

            var rankNames = new string[] { "Recruit", "Member", "VIP", "Founder" };
            var nameRules = new StringRules(3, 50, CharType.All);
            var tagRules = new StringRules(1, 4, CharType.All);
            var maxRank = (GuildRank)(rankNames.Length - 1);
            GuildSettings.Initialize(new GuildSettings(1000 * 60, maxRank, rankNames, nameRules, tagRules, 3, 2, 1, 1, 2, 2));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineSettingsInitializer"/> class.
        /// </summary>
        EngineSettingsInitializer() : base(new Vector2(0f, 0.0009f), new Vector2(0.5f, 0.5f))
        {
        }

        /// <summary>
        /// Method used to determine if an <see cref="IGroupable"/> can join an existing <see cref="IGroup"/>.
        /// </summary>
        /// <param name="member">The <see cref="IGroupable"/> to check if can join the <paramref name="group"/>.</param>
        /// <param name="group">The <see cref="IGroup"/> the <paramref name="member"/> is trying to join.</param>
        /// <returns>True if the <paramref name="member"/> can join the <paramref name="group"/>; otherwise false.</returns>
        static bool CanJoinGroupHandler(IGroupable member, IGroup group)
        {
            // Put your additional group restrictions here, such as level range restrictions. No need to check for stuff
            // that is already checked for such as if the member is already in a group, if the group is full, etc.
            return true;
        }

        /// <summary>
        /// Initializes the <see cref="EngineSettings"/>. This can be called any number of times, but must
        /// be called at least once before the engine components are utilized.
        /// </summary>
        public static void Initialize()
        {
            // This method is empty, but it still initializes the GameSettings. When this method is called,
            // it will force the static GameSettings constructor to be called, which will handle the actual
            // initialization. We just need this method to perform the initial invoke to get things rolling.
        }
    }
}