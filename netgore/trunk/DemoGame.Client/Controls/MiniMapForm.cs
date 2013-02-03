using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> showing scaled down version of the map the user is on.
    /// </summary>
    public class MiniMapForm : Form
    {
        List<PictureBox> _picEntities = new List<PictureBox>();
        PictureBox _scaledMap;
        Grh _scaledMapGrh = new Grh(null);
        readonly Grh _entityCharGrh;
        readonly Grh _entityNPCGrh;
        GameplayScreen _gamePlayScreen;

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniMapForm"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="screen">The game play screen.</param>
        public MiniMapForm(Control parent, GameplayScreen screen) : base(parent, new Vector2(50, 50), new Vector2(250, 250))
        {
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
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Mini Map";
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(TickCount currentTime)
        {
            base.UpdateControl(currentTime);

            if (!IsVisible)
                return;

            UpdateMiniMap();
        }

        private void UpdateMiniMap()
        {
            _picEntities.Clear();

            if (ScaledMapGrh != null)
                _scaledMap = new PictureBox(this, Vector2.Zero, this.ClientSize) { CanFocus = false, CanDrag = false, Sprite = ScaledMapGrh };

            var entites = GamePlayScreen.Map.Entities.Where(x => !(x is WallEntity));
            foreach (var entity in entites)
            {
                if (entity is User)
                    DrawEntityGrh(entity, _entityCharGrh);
                else if (entity is NPC)
                    DrawEntityGrh(entity, _entityNPCGrh);
            }
        }

        private void DrawEntityGrh(Entity entity, Grh grh)
        {
            var scaleX = GamePlayScreen.Map.Size.X / ScaledMapGrh.Size.X;
            var scaleY = GamePlayScreen.Map.Size.Y / ScaledMapGrh.Size.Y;

            var scaledPos = new Vector2((entity.Position.X / scaleX) - grh.Size.X / 2, (entity.Position.Y / scaleY) - grh.Size.Y / 2);
            _picEntities.Add(new PictureBox(this, scaledPos, grh.Size) { CanFocus = false, CanDrag = false, Sprite = grh });
        }

        public void MapChanged(Map map)
        {
            Text = "Mini Map: " + map.Name;
            ScaledMapGrh.SetGrh(GrhInfo.GetData("MiniMap", map.ID.ToString()));
        }
    }
}