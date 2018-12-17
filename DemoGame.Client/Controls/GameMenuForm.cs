using System;
using System.Linq;
using NetGore;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// The main game form for while in the game.
    /// </summary>
    class GameMenuForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMenuForm"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public GameMenuForm(Control parent) : base(parent, Vector2.Zero, new Vector2(32))
        {
            var logOutLbl = new Label(this, new Vector2(3, 3)) { Text = "Log Out" };
            logOutLbl.Clicked += logOutLbl_Clicked;

            // Center on the parent
            Position = (Parent.ClientSize / 2f) - (Size / 2f);

            IsVisible = false;

            parent.KeyPressed += parent_KeyPressed;
        }

        /// <summary>
        /// Notifies listeners that the Log Out button has been clicked.
        /// </summary>
        public event EventHandler ClickedLogOut;

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            ResizeToChildren = true;
            Text = "Menu";
        }

        /// <summary>
        /// Handles the Clicked event of the logOutLbl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void logOutLbl_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button != Mouse.Button.Left)
                return;

            if (ClickedLogOut != null)
                ClickedLogOut.Raise(this, EventArgs.Empty);

            IsVisible = !IsVisible;
        }

        /// <summary>
        /// Handles the KeyPressed event of the parent control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.KeyEventArgs"/> instance containing the event data.</param>
        void parent_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
                IsVisible = !IsVisible;
        }
    }
}