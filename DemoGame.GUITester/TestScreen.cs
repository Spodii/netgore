using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DemoGame.Client;
using NetGore;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.GUITester
{
    sealed class TestScreen : GameMenuScreenBase
    {
        static readonly SafeRandom rnd = new SafeRandom();

        Label _dragLbl;
        TextBox _textBox;
        Form topForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="screenManager"/> is null.</exception>
        public TestScreen(IScreenManager screenManager) : base(screenManager, "TestScreen", "Test Screen")
        {
        }

        /// <summary>
        /// Handles the Drag event of a <see cref="Control"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void DragControl(Control sender, EventArgs e)
        {
            var s = (TextControl)sender;
            s.Text = s.Position.ToString();

            _dragLbl.Text = "Screen Position: " + s.ScreenPosition;
        }

        /// <summary>
        /// Gets the <see cref="Font"/> to use as the default font for the <see cref="IGUIManager"/> for this
        /// <see cref="GameScreen"/>.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> for this screen.</param>
        /// <returns>The <see cref="Font"/> to use for this <see cref="GameScreen"/>. If null, the
        /// <see cref="IScreenManager.DefaultFont"/> for this <see cref="GameScreen"/> will be used instead.</returns>
        protected override Font GetScreenManagerFont(IScreenManager screenManager)
        {
            return GameScreenHelper.DefaultGameGUIFont;
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            topForm = new Form(GUIManager, new Vector2(5, 5), new Vector2(700, 550)) { Text = "Primary form" };

            var tb = new TextBox(topForm, new Vector2(10, 10), new Vector2(150, 300));

            _textBox = new TextBox(topForm, new Vector2(350, 10), new Vector2(200, 200))
            { Font = GUIManager.Font, Text = "abcdef\nghi\r\njklj\n" };

            for (var i = 0; i < 150; i++)
            {
                var c = new Color((byte)rnd.Next(0, 256), (byte)rnd.Next(256), (byte)rnd.Next(256), 255);
                _textBox.Append(new StyledText(i + " ", c));
            }

            var styledTexts = new List<StyledText>
            {
                new StyledText("Black ", Color.Black),
                new StyledText("Red ", Color.Red),
                new StyledText("Green ", Color.Green),
                new StyledText("Yellow ", Color.Yellow),
                new StyledText("Voilet ", Color.Violet),
                new StyledText("Orange ", Color.Orange),
                new StyledText("Tomato ", Color.Tomato),
                new StyledText("DarkRed ", Color.DarkRed),
            };

            tb.Append(styledTexts);

            var form = new Form(topForm, new Vector2(50, 50), new Vector2(200, 200)) { Text = "My form" };

            var b = new Button(form, new Vector2(20, 20), new Vector2(80, 30)) { Text = "Press me" };
            b.Clicked += b_Clicked;

            new CheckBox(form, new Vector2(20, 200)) { Text = "Checkbox" };

            var f2 = new Form(topForm, new Vector2(200, 250), new Vector2(275, 270)) { Text = "My form 2" };
            var f3 = new Form(f2, Vector2.Zero, new Vector2(200, 200)) { Text = "form 3" };
            var f4 = new Form(f3, Vector2.Zero, new Vector2(100, 100)) { Text = "form 4" };

            var testLabelF4 = new Label(f4, Vector2.Zero) { Text = "Click me" };
            testLabelF4.Clicked += testLabelF4_Clicked;

            _dragLbl = new Label(topForm, topForm.Size - new Vector2(75, 30));

            topForm.BeginDrag += DragControl;
            topForm.EndDrag += DragControl;
            form.BeginDrag += DragControl;
            form.EndDrag += DragControl;
            f2.BeginDrag += DragControl;
            f2.EndDrag += DragControl;
            f3.BeginDrag += DragControl;
            f3.EndDrag += DragControl;
            f4.BeginDrag += DragControl;
            f4.EndDrag += DragControl;

            // Set up the tooltips
            foreach (var c in GUIManager.GetAllControls())
            {
                if (c.GetType() == typeof(Button))
                    c.Tooltip = Tooltip_Button;
                else if (c.GetType() == typeof(Label))
                    c.Tooltip += Tooltip_Label;
            }

            // Paged list
            var items = new List<string>();
            for (var i = 0; i < 100; i++)
            {
                items.Add(i.ToString());
            }

            new ListBox<string>(topForm, new Vector2(500, 250), new Vector2(100, 100)) { Items = items, ShowPaging = true, CanSelect = true };
        }

        static StyledText[] Tooltip_Button(Control sender, TooltipArgs args)
        {
            return new StyledText[]
            {
                new StyledText(
                    "hello-hello-hello-hello-hello-hello-hello-hello hello hello hello hello hello hello hello hello hello hello hello hello hello hello hello")
                , new StyledText("button", new Color(50, 50, 190))
            };
        }

        static StyledText[] Tooltip_Label(Control sender, TooltipArgs args)
        {
            args.FontColor = new Color(50, 190, 50);
            args.RefreshRate = 100;

            var r = (byte)rnd.Next(100, 255);
            var g = (byte)rnd.Next(100, 255);
            var b = (byte)rnd.Next(100, 255);

            var c = new Color(r, g, b, 255);
            return new StyledText[] { new StyledText("Text for a "), new StyledText("label", c) };
        }

        void b_Clicked(object sender, MouseButtonEventArgs e)
        {
            //var inBox = new InputBox(GUIManager, "Input", "Enter some number:", MessageBoxButton.OkCancel);
            var msgBox = new MessageBox(GUIManager, ":o", "You clicked the magical button!", MessageBoxButton.YesNoCancel);
            msgBox.OptionSelected += msgBox_OptionSelected;
        }

        /// <summary>
        /// Handles the corresponding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{MessageBoxButton}"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "NetGore.Graphics.GUI.MessageBox")]
        static void msgBox_OptionSelected(Control sender, EventArgs<MessageBoxButton> e)
        {
            var senderAsMsgBox = sender as MessageBox;
            if (senderAsMsgBox != null)
                senderAsMsgBox.OptionSelected -= msgBox_OptionSelected;

            switch (e.Item1)
            {
                case MessageBoxButton.Yes:
                    new MessageBox(sender.GUIManager, ":|", "Yes? What do you mean yes? I didn't even ask you a question!",
                        MessageBoxButton.Ok);
                    break;

                case MessageBoxButton.No:
                    string message;

                    if (senderAsMsgBox != null && senderAsMsgBox.Message.StartsWith("NO!"))
                        message = senderAsMsgBox.Message + "!!!";
                    else
                        message = "NO!";

                    var msgBox = new MessageBox(sender.GUIManager, "No", message, MessageBoxButton.YesNo);
                    msgBox.OptionSelected += msgBox_OptionSelected;
                    break;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "NetGore.Graphics.GUI.MessageBox")]
        void testLabelF4_Clicked(object sender, MouseButtonEventArgs e)
        {
            var source = (Label)sender;
            if (source.Text == "I was clicked!")
                source.Text = "Click me!";
            else
                source.Text = "I was clicked!";

            var msgBox = new MessageBox(GUIManager, "My message box",
                "asdlkf aslfdkj sadflkj asdflkj was fadjlkjfsalkaj sfdlksadjf asfdjlalksdfj asdfsdfa eklrj afek jasdlfkj asdflkj asdflkj woieur klasdf\nasdflkj\nasdf\nadsf",
                MessageBoxButton.YesNoCancel);
            msgBox.OptionSelected += (x, y) => new MessageBox(GUIManager, "Hello", "You selected: " + y, MessageBoxButton.Ok);
        }
    }
}