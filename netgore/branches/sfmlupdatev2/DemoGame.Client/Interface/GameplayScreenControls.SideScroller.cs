#if TOPDOWN
#pragma warning disable 1587
#endif

#if !TOPDOWN

using System;
using System.Linq;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    partial class GameplayScreenControls
    {
        /// <summary>
        /// Creates the controls specific to side-scroller builds.
        /// </summary>
        void CreateControlsForPerspective()
        {
            CreateAndAdd(GameControlsKeys.Jump, _minMoveRate, () => UserChar.CanJump && CanUserMove(), HandleGameControl_Jump);
        }

        /// <summary>
        /// Handles the corresponding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void HandleGameControl_Jump(GameControl sender, EventArgs e)
        {
            using (var pw = ClientPacket.Jump())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }
    }
}

#endif