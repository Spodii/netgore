using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.Graphics;

namespace NetGore.Features.Quests
{
    public class QuestMapDrawingExtension<TCharacter> : MapDrawingExtension where TCharacter : ISpatial
    {
        readonly Func<IDrawableMap, IEnumerable<TCharacter>> _getQuestProviders;
        readonly Func<TCharacter, IEnumerable<QuestID>> _getQuests;
        readonly HasQuestRequirementsTracker _hasStartQuestReqs;
        readonly UserQuestInformation _questInfo;

        public QuestMapDrawingExtension(UserQuestInformation questInfo, HasQuestRequirementsTracker hasStartQuestReqs,
                                        Func<IDrawableMap, IEnumerable<TCharacter>> getQuestProviders,
                                        Func<TCharacter, IEnumerable<QuestID>> getQuests)
        {
            _questInfo = questInfo;
            _hasStartQuestReqs = hasStartQuestReqs;
            _getQuestProviders = getQuestProviders;
            _getQuests = getQuests;
        }

        public HasQuestRequirementsTracker HasStartQuestReqs
        {
            get { return _hasStartQuestReqs; }
        }

        public UserQuestInformation QuestInfo
        {
            get { return _questInfo; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the map after the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that was just drawn.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, MapRenderLayer layer, ISpriteBatch spriteBatch)
        {
            if (layer != MapRenderLayer.Chararacter)
                return;

            foreach (var c in _getQuestProviders(map))
            {
                // Get the provided quests we have not completed
                var incomplete = _getQuests(c).Where(x => !QuestInfo.CompletedQuests.Contains(x)).ToImmutable();
                if (incomplete.IsEmpty())
                    continue;

                if (incomplete.Any(x => QuestInfo.ActiveQuests.Contains(x)))
                {
                    // Contains a quest we are working on
                    Grh g = new Grh(GrhInfo.GetData("Quest", "started"));
                    g.Draw(spriteBatch, new Vector2(c.Center.X, c.Position.Y) - new Vector2(g.Size.X / 2f, g.Size.Y));
                }
                else
                {
                    // Does not contain a quest we are working on
                    if (incomplete.Any(x => HasStartQuestReqs.HasRequirements(x) == true))
                    {
                        // Contains an available quest
                        Grh g = new Grh(GrhInfo.GetData("Quest", "available"));
                        g.Draw(spriteBatch, new Vector2(c.Center.X, c.Position.Y) - new Vector2(g.Size.X / 2f, g.Size.Y));
                    }
                    else if (incomplete.Any(x => HasStartQuestReqs.HasRequirements(x) == false))
                    {
                        // Contains an unavailable quest
                        Grh g = new Grh(GrhInfo.GetData("Quest", "unavailable"));
                        g.Draw(spriteBatch, new Vector2(c.Center.X, c.Position.Y) - new Vector2(g.Size.X / 2f, g.Size.Y));
                    }
                }
            }
        }
    }
}