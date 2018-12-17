using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.World;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> showing scaled down version of the map the user is on.
    /// </summary>
    public class MiniMapForm : Form
    {
        private Grh _scaledMapGrh;
        private List<Tuple<Grh, Vector2>> _grhEntities = new List<Tuple<Grh, Vector2>>();
        private readonly Grh _entityCharGrh;
        private readonly Grh _entityNPCGrh;
        private readonly GameplayScreen _gamePlayScreen;
        private Vector2 _topLeftPos;

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniMapForm"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="screen">The game play screen.</param>
        public MiniMapForm(Control parent, GameplayScreen screen) : base(parent, new Vector2(50, 50), new Vector2(250, 250))
        {
            _topLeftPos = new Vector2(this.Position.X + this.Border.LeftWidth, this.Position.Y + this.Border.TopHeight);
            _scaledMapGrh = new Grh(null);
            _entityCharGrh = new Grh(GrhInfo.GetData("GUI", "MiniMapCharacter"));
            _entityNPCGrh = new Grh(GrhInfo.GetData("GUI", "MiniMapNPC"));
            _gamePlayScreen = screen;
        }

        /// <summary>
        /// Gets the <see cref="GamePlayScreen"/> control info that the <see cref="MiniMapForm"/> is displaying the information for.
        /// </summary>
        public GameplayScreen GamePlayScreen
        {
            get { return _gamePlayScreen; }
        }

        /// <summary>
        /// Gets the <see cref="ScaledMapGrh"/> grh info that the <see cref="MiniMapForm"/> is displaying the information for.
        /// </summary>
        public Grh ScaledMapGrh
        {
            get { return _scaledMapGrh; }
            set { _scaledMapGrh = value; }
        }

        /// <summary>
        /// Gets the <see cref="TopLeftPos"/> of the <see cref="MiniMapForm"/> after the borders.
        /// </summary>
        public Vector2 TopLeftPos
        {
            get { return _topLeftPos; }
            set { _topLeftPos = value; }
        }

        /// <summary>
        /// The <see cref="Control"/> has been moved, so update positions.
        /// </summary>
        protected override void OnMoved()
        {
            base.OnMoved();

            TopLeftPos = new Vector2(this.Position.X + this.Border.LeftWidth, this.Position.Y + this.Border.TopHeight);
        }

        /// <summary>
        /// Draws the <see cref="Control"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected override void DrawControl(ISpriteBatch sb)
        {
            base.DrawControl(sb);

            // If not visible, no point in drawing/updating anything
            if (!IsVisible)
                return;

            // Draw the scaled down version of the map
            ScaledMapGrh.Draw(sb, _topLeftPos);

            // Update the entities on the map
            UpdateMiniMap();

            // Draw all the entities we just found
            foreach (var grh in _grhEntities)
                grh.Item1.Draw(sb, _topLeftPos + grh.Item2);
        }

        private void UpdateMiniMap()
        {
            // Search the map for entities
            var entites = GamePlayScreen.Map.Spatial.GetMany<CharacterEntity>();

            // Clear the grh list so we don't hold duplicates, etc
            _grhEntities.Clear();

            // Add the various entities to our grh list, ready to be drawn
            foreach (var entity in entites)
            {
                if (entity is User)
                    AddEntityGrh(entity, _entityCharGrh);
                else if (entity is NPC)
                    AddEntityGrh(entity, _entityNPCGrh);
            }
        }

        private void AddEntityGrh(ISpatial entity, Grh grh)
        {
            // Find the scaling value so we can position the entities correctly
            var scaleX = GamePlayScreen.Map.Size.X / ScaledMapGrh.Size.X;
            var scaleY = GamePlayScreen.Map.Size.Y / ScaledMapGrh.Size.Y;
            var scaledPos = new Vector2((entity.Position.X / scaleX) - grh.Size.X / 2, (entity.Position.Y / scaleY) - grh.Size.Y / 2);
            // Add the grh as well as the position
            _grhEntities.Add(new Tuple<Grh, Vector2>(new Grh(grh.GrhData), scaledPos));
        }

        /// <summary>
        /// Handles when a map is changed and sets the new map to daw on the <see cref="MiniMapForm"/>.
        /// </summary>
        /// <param name="map">The map the user is on.</param>
        public void MapChanged(Map map)
        {
            // Show the map name in the form
            Text = "Mini Map: " + map.Name;

            // Assign the new map as a new grh
            ScaledMapGrh.SetGrh(GrhInfo.GetData("MiniMap", map.ID.ToString()));

            // Resize the form to match the new scaled map grh
            this.ClientSize = ScaledMapGrh.Size;
        }
    }
}