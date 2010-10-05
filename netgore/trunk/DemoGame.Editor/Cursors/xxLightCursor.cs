using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.Editor;
using NetGore.Graphics;
using SFML.Graphics;
using Image = System.Drawing.Image;

namespace DemoGame.Editor
{
    sealed class xxLightCursor : EditorCursor<EditMapForm>
    {
        ILight _selectedLight;
        Vector2 _selectedLightOffset;
        string _toolTip = string.Empty;
        object _toolTipObj = null;
        Vector2 _toolTipPos;

        /// <summary>
        /// Gets the cursor's <see cref="System.Drawing.Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_lights; }
        }

        /// <summary>
        /// Property to access the MSC. Provided purely for the means of shortening the
        /// code
        /// </summary>
        MapScreenControl MSC
        {
            get { return Container.MapScreenControl; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public override string Name
        {
            get { return "Select Light"; }
        }

        /// <summary>
        /// Property to access the <see cref="SelectedObjectsManager{T}"/>. Provided purely for convenience.
        /// </summary>
        static SelectedObjectsManager<object> SOM
        {
            get { return GlobalState.Instance.Map.SelectedObjsManager; }
        }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        public override int ToolbarPriority
        {
            get { return 40; }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the cursor becomes the active cursor.
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            ClearState();
        }

        /// <summary>
        /// Completely clears the state of the cursor.
        /// </summary>
        void ClearState()
        {
            _selectedLight = null;
            _selectedLightOffset = Vector2.Zero;
            _toolTip = string.Empty;
            _toolTipObj = null;
            _toolTipPos = Vector2.Zero;
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the cursor is no longer the active cursor.
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();

            ClearState();
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public override void DrawInterface(ISpriteBatch spriteBatch)
        {
            // If we have a light under the cursor or selected, use the SizeAll cursor
            if (_selectedLight != null || FindMouseOverLight() != null)
                Container.Cursor = Cursors.SizeAll;

            // Draw the tooltip
            if (_toolTipObj != null && !string.IsNullOrEmpty(_toolTip))
                spriteBatch.DrawStringShaded(GlobalState.Instance.DefaultRenderFont, _toolTip, _toolTipPos, Color.White,
                                             Color.Black);
        }

        /// <summary>
        /// Finds the <see cref="ILight"/> under the cursor.
        /// </summary>
        /// <returns>The <see cref="ILight"/> under the cursor, or null if no <see cref="ILight"/> is under the cursor.</returns>
        ILight FindMouseOverLight()
        {
            var cursorPos = MSC.CursorPos;

            var closestLight = MSC.Map.Lights.MinElementOrDefault(x => cursorPos.QuickDistance(x.Center));
            if (closestLight == null)
                return null;

            if (cursorPos.QuickDistance(closestLight.Position) > 5)
                return null;

            return closestLight;
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(MouseEventArgs e)
        {
            _selectedLight = FindMouseOverLight();
            if (_selectedLight != null)
                _selectedLightOffset = MSC.CursorPos - _selectedLight.Center;

            SOM.SetSelected(_selectedLight);

            _toolTipObj = null;
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(MouseEventArgs e)
        {
            if (_selectedLight != null)
            {
                // Move the light if dragging one
                _selectedLight.Teleport(MSC.CursorPos - _selectedLightOffset);

                _toolTipObj = null;
            }
            else
                UpdateToolTip();
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been released.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseUp(MouseEventArgs e)
        {
            _selectedLight = null;
            UpdateToolTip();
        }

        /// <summary>
        /// When overridden in the derived class, handles when the delete button has been pressed.
        /// </summary>
        public override void PressDelete()
        {
            var light = SOM.Focused as ILight;
            if (light != null && light.Tag == MSC.Map)
            {
                MSC.Map.RemoveLight(light);
                MSC.DrawingManager.LightManager.Remove(light);
                SOM.Clear();
            }
        }

        void UpdateToolTip()
        {
            if (_selectedLight != null)
            {
                _toolTipObj = null;
                _toolTip = null;
                return;
            }

            // Display the tooltip for the light under the cursor
            var light = FindMouseOverLight();
            if (_toolTipObj == light)
                return;

            _toolTipObj = light;
            if (light == null)
                return;

            _toolTip = string.Format("{0}\n{1} ({2}x{3})", light, light.Position, light.Size.X, light.Size.Y);
            _toolTipPos = xxEntityCursor.GetToolTipPos(GlobalState.Instance.DefaultRenderFont, _toolTip, light);
            _toolTipPos.X = light.Position.X + 5;
        }
    }
}