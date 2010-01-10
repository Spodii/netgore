using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.MapEditor.Properties;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.EditorTools;

namespace DemoGame.MapEditor
{
    sealed class AddEntityCursor : MapEditorCursorBase<ScreenForm>
    {
        readonly ContextMenu _contextMenu;
        readonly MenuItem[] _menuEntityTypeChild;
        readonly MenuItem _mnuEntityType;

        Type _selectedType;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEntityCursor"/> class.
        /// </summary>
        public AddEntityCursor()
        {
            var types = MapFileEntityAttribute.GetTypes().OrderBy(x => x.Name);
            _menuEntityTypeChild = types.Select(x => new MenuItem(x.Name, Menu_EntityTypeChild_Click) { Tag = x }).ToArray();

            _mnuEntityType = new MenuItem("Entity type", _menuEntityTypeChild);
            _contextMenu = new ContextMenu(new MenuItem[] { _mnuEntityType });

            _selectedType = types.FirstOrDefault();
        }

        /// <summary>
        /// Gets the cursor's <see cref="Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_entitiesadd; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public override string Name
        {
            get { return "Add Entity"; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="ContextMenu"/> used by this cursor
        /// to display additional functions and settings.
        /// </summary>
        /// <param name="cursorManager">The cursor manager.</param>
        /// <returns>
        /// The <see cref="ContextMenu"/> used by this cursor to display additional functions and settings,
        /// or null for no <see cref="ContextMenu"/>.
        /// </returns>
        public override ContextMenu GetContextMenu(MapEditorCursorManager<ScreenForm> cursorManager)
        {
            return _contextMenu;
        }

        void Menu_EntityTypeChild_Click(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var type = (Type)mi.Tag;

            foreach (var x in _menuEntityTypeChild)
            {
                x.Checked = false;
            }

            mi.Checked = true;
            _selectedType = type;
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(ScreenForm screen, MouseEventArgs e)
        {
            if (_selectedType == null)
                return;

            // Create the Entity
            Entity entity = (Entity)Activator.CreateInstance(_selectedType);
            screen.Map.AddEntity(entity);

            // Move to the center of the screen
            entity.Size = new Vector2(64);
            entity.Position = screen.CursorPos - (entity.Size / 2f);
        }
    }
}