using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    class AddGrhCursor : EditorCursorBase
    {
        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(ScreenForm screen, MouseEventArgs e)
        {
            if (screen.chkSnapGrhGrid.Checked)
                MouseUp(screen, e);
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been released.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseUp(ScreenForm screen, MouseEventArgs e)
        {
            Vector2 cursorPos = screen.CursorPos;

            // On left-click place the Grh on the map
            if (e.Button == MouseButtons.Left)
            {
                // Check for a valid MapGrh
                if (screen.SelectedGrh.GrhData == null)
                    return;

                // Find the position the MapGrh will be created at
                Vector2 drawPos;
                if (screen.chkSnapGrhGrid.Checked)
                    drawPos = screen.Grid.AlignDown(cursorPos);
                else
                    drawPos = cursorPos;

                // Check if a MapGrh of the same type already exists at the location
                foreach (MapGrh grh in screen.Map.MapGrhs)
                {
                    if (grh.Destination == drawPos && grh.Grh.GrhData.GrhIndex == screen.SelectedGrh.GrhData.GrhIndex)
                        return;
                }

                // Add the MapGrh to the map
                Grh g = new Grh(screen.SelectedGrh.GrhData.GrhIndex, AnimType.Loop, screen.GetTime());
                screen.Map.AddMapGrh(new MapGrh(g, drawPos, screen.chkForeground.Checked));
            }
            else if (e.Button == MouseButtons.Right)
            {
                // On right-click delete any Grhs under the cursor
                while (true)
                {
                    MapGrh grh = screen.Map.GetMapGrh(cursorPos);
                    if (grh == null)
                        break;
                    screen.Map.RemoveMapGrh(grh);
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public override string Name
        {
            get { return "Add Grh"; }
        }
    }
}