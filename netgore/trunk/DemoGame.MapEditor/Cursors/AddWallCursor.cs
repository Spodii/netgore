using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.MapEditor
{
    class AddWallCursor : EditorCursorBase
    {
        public override void MouseDown(ScreenForm screen, MouseEventArgs e)
        {
            // Switch to the wall editing tool
            screen.toolBarItem_Click(screen.picToolWalls, null);

            // Create the new wall
            WallEntity w = new WallEntity(screen.Camera.ToWorld(e.X, e.Y), Vector2.One);
            screen.Map.AddEntity(w);
            w.CollisionType = (CollisionType)screen.cmbWallType.SelectedItem;
            if (screen.chkSnapWallGrid.Checked)
                screen.Grid.Align(w);

            // Create the transformation boxes for the wall and select the bottom/right one
            screen.TransBoxes.Clear();
            TransBox.SurroundEntity(w, screen.TransBoxes);
            foreach (TransBox tBox in screen.TransBoxes)
            {
                if (tBox.TransType == TransBoxType.BottomRight)
                {
                    screen.SelectedTransBox = tBox;
                    break;
                }
            }

            screen.UpdateSelectedWallsList(new List<WallEntityBase>(1) { w });
        }
    }
}