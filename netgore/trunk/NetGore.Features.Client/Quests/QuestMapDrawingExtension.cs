using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Extends the drawing of the map to provide visual indicators for quests.
    /// </summary>
    /// <typeparam name="TCharacter">The type of character.</typeparam>
    public class QuestMapDrawingExtension<TCharacter> : MapDrawingExtension where TCharacter : ISpatial
    {
        readonly Func<IDrawableMap, IEnumerable<TCharacter>> _getQuestProviders;
        readonly Func<TCharacter, IEnumerable<QuestID>> _getQuests;
        readonly UserQuestInformation _questInfo;

        Func<QuestID, bool> _hasFinishQuestReqs;
        Func<QuestID, bool> _hasStartQuestReqs;
        Action<Grh, TCharacter, ISpriteBatch> _indicatorDrawer;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestMapDrawingExtension{TCharacter}"/> class.
        /// </summary>
        /// <param name="questInfo">The quest info.</param>
        /// <param name="hasStartQuestReqs">The <see cref="Func{T,U}"/> used to determine if the user has the
        /// requirements to start a quest.</param>
        /// <param name="hasFinishQuestReqs">The <see cref="Func{T,U}"/> used to determine if the user has the
        /// requirements to finish a quest.</param>
        /// <param name="getQuestProviders">The <see cref="Func{T,U}"/> used to get the quest providers from
        /// a map.</param>
        /// <param name="getQuests">The <see cref="Func{T,U}"/> used to get the quests provided by
        /// a quest provider.</param>
        public QuestMapDrawingExtension(UserQuestInformation questInfo, Func<QuestID, bool> hasStartQuestReqs,
                                        Func<QuestID, bool> hasFinishQuestReqs,
                                        Func<IDrawableMap, IEnumerable<TCharacter>> getQuestProviders,
                                        Func<TCharacter, IEnumerable<QuestID>> getQuests)
        {
            _questInfo = questInfo;
            _hasStartQuestReqs = hasStartQuestReqs;
            _hasFinishQuestReqs = hasFinishQuestReqs;
            _getQuestProviders = getQuestProviders;
            _getQuests = getQuests;

            _indicatorDrawer = DefaultIndicatorDrawer;
        }

        /// <summary>
        /// Gets or sets the <see cref="Func{T,U}"/> used to determine if the user has the requirements to
        /// finish a quest.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public Func<QuestID, bool> HasFinishQuestReqs
        {
            get { return _hasFinishQuestReqs; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _hasFinishQuestReqs = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Func{T,U}"/> used to determine if the user has the requirements to
        /// start a quest.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public Func<QuestID, bool> HasStartQuestReqs
        {
            get { return _hasStartQuestReqs; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _hasStartQuestReqs = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Action{T,U,V}"/> used to draw the quest indicator.
        /// If set to null, the default drawing indicator will be used.
        /// </summary>
        public Action<Grh, TCharacter, ISpriteBatch> IndicatorDrawer
        {
            get { return _indicatorDrawer; }
            set { _indicatorDrawer = value ?? DefaultIndicatorDrawer; }
        }

        /// <summary>
        /// Gets or sets the indicator for a quest provider that has quests that the user can start.
        /// If null, the indicator will not be drawn.
        /// </summary>
        public Grh QuestAvailableCanStartIndicator { get; set; }

        /// <summary>
        /// Gets or sets the indicator for a quest provider that has quests but the user does not meet
        /// the requirements to start any of them.
        /// If null, the indicator will not be drawn.
        /// </summary>
        public Grh QuestAvailableCannotStartIndicator { get; set; }

        /// <summary>
        /// Gets the <see cref="UserQuestInformation"/> for the user the indicators are being drawn for.
        /// </summary>
        public UserQuestInformation QuestInfo
        {
            get { return _questInfo; }
        }

        /// <summary>
        /// Gets or sets the indicator for a quest provider that has quests that the user has started.
        /// If null, the indicator will not be drawn.
        /// </summary>
        public Grh QuestStartedIndicator { get; set; }

        /// <summary>
        /// Gets or sets the indicator for a quest provider that has quests that the user has active and
        /// can turn in.
        /// If null, the indicator will not be drawn.
        /// </summary>
        public Grh QuestTurnInIndicator { get; set; }

        /// <summary>
        /// Gets if the any of the provided quests has the status of
        /// being able to be started.
        /// </summary>
        /// <param name="incomplete">The incomplete quests.</param>
        /// <returns>The state of the respective quest-providing status.</returns>
        bool CheckStatusCanStart(IEnumerable<QuestID> incomplete)
        {
            return incomplete.Any(x => !QuestInfo.ActiveQuests.Contains(x) && HasStartQuestReqs(x));
        }

        /// <summary>
        /// Gets if the any of the provided quests has the status of
        /// not being started.
        /// </summary>
        /// <param name="incomplete">The incomplete quests.</param>
        /// <returns>The state of the respective quest-providing status.</returns>
        bool CheckStatusCannotStart(IEnumerable<QuestID> incomplete)
        {
            return incomplete.Any(x => !QuestInfo.ActiveQuests.Contains(x));
        }

        /// <summary>
        /// Gets if the any of the provided quests has the status of
        /// being started.
        /// </summary>
        /// <param name="incomplete">The incomplete quests.</param>
        /// <returns>The state of the respective quest-providing status.</returns>
        bool CheckStatusStarted(IEnumerable<QuestID> incomplete)
        {
            return incomplete.Any(x => QuestInfo.ActiveQuests.Contains(x));
        }

        /// <summary>
        /// Gets if the any of the provided quests has the status of
        /// being able to be turned in.
        /// </summary>
        /// <param name="incomplete">The incomplete quests.</param>
        /// <returns>The state of the respective quest-providing status.</returns>
        bool CheckStatusTurnIn(IEnumerable<QuestID> incomplete)
        {
            return incomplete.Any(x => QuestInfo.ActiveQuests.Contains(x) && HasFinishQuestReqs(x));
        }

        /// <summary>
        /// The default quest indicator drawer.
        /// </summary>
        /// <param name="grh">The <see cref="Grh"/> to draw.</param>
        /// <param name="c">The character the indicator is for.</param>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        protected virtual void DefaultIndicatorDrawer(Grh grh, TCharacter c, ISpriteBatch sb)
        {
            var pos = new Vector2(c.Center.X, c.Position.Y) - new Vector2(grh.Size.X / 2f, grh.Size.Y + 12);
            grh.Draw(sb, pos);
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the map after the given <see cref="MapRenderLayer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> instance containing the event data.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, DrawableMapDrawLayerEventArgs e)
        {
            // Draw after dynamic layer finishes
            if (e.Layer != MapRenderLayer.Dynamic)
                return;

            // Update the valid sprites
            var currentTime = map.GetTime();
            if (QuestStartedIndicator != null)
                QuestStartedIndicator.Update(currentTime);

            if (QuestTurnInIndicator != null)
                QuestTurnInIndicator.Update(currentTime);

            if (QuestAvailableCannotStartIndicator != null)
                QuestAvailableCannotStartIndicator.Update(currentTime);

            if (QuestAvailableCanStartIndicator != null)
                QuestAvailableCanStartIndicator.Update(currentTime);

            // Get all the quest providers in the area of interest (visible area)
            foreach (var c in _getQuestProviders(map))
            {
                // Get the provided quests we have not completed or if it's repeatable
                var incomplete = _getQuests(c).Where(x => !QuestInfo.CompletedQuests.Contains(x) ||
                    QuestInfo.RepeatableQuests.Contains(x)).ToImmutable();

                // If they don't have any quests, continue
                if (incomplete.IsEmpty())
                    continue;

                // For each of the four indicators, start at the "highest priority" one and stop when we find
                // the first valid status that we can use
                if (CheckStatusTurnIn(incomplete))
                {
                    IndicatorDrawer(QuestTurnInIndicator, c, e.SpriteBatch);
                    continue;
                }
                else if (CheckStatusCanStart(incomplete))
                {
                    IndicatorDrawer(QuestAvailableCanStartIndicator, c, e.SpriteBatch);
                    continue;
                }
                else if (CheckStatusStarted(incomplete))
                {
                    IndicatorDrawer(QuestStartedIndicator, c, e.SpriteBatch);
                    continue;
                }
                else if (CheckStatusCannotStart(incomplete))
                {
                    IndicatorDrawer(QuestAvailableCannotStartIndicator, c, e.SpriteBatch);
                    continue;
                }
            }
        }
    }
}