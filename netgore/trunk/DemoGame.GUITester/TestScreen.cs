using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.GUITester
{
    sealed class TestScreen : GameScreen
    {
        static readonly SafeRandom rnd = new SafeRandom();

        Label _dragLbl;
        TextBox _textBox;
        ControlBorder _topBorder;
        Form topForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="screenManager"/> is null.</exception>
        public TestScreen(IScreenManager screenManager) : base(screenManager, "TestScreen")
        {
        }

        void b_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (topForm.Border == ControlBorder.Empty)
                topForm.Border = _topBorder;
            else
                topForm.Border = ControlBorder.Empty;
        }

        void DragControl(Control sender)
        {
            TextControl s = (TextControl)sender;
            s.Text = s.Position.ToString();

            _dragLbl.Text = "Screen Position: " + s.ScreenPosition;
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
            topForm.MouseMoved += topForm_MouseMoved;

            TextBox tb = new TextBox(topForm, new Vector2(10, 10), new Vector2(150, 300));

            _textBox = new TextBox(topForm, new Vector2(350, 10), new Vector2(200, 200))
            { Font = GUIManager.Font, Text = "abcdef\nghi\r\njklj\n" };

            for (int i = 0; i < 150; i++)
            {
                Color c = new Color((byte)rnd.Next(0, 256), (byte)rnd.Next(256), (byte)rnd.Next(256), 255);
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

            _topBorder = topForm.Border;

            Form form = new Form(topForm, new Vector2(50, 50), new Vector2(200, 200)) { Text = "My form" };

            Button b = new Button(form, new Vector2(20, 20), new Vector2(80, 30)) { Text = "Press me" };
            b.Clicked += b_Clicked;

            new CheckBox(form, new Vector2(20, 200)) { Text = "Checkbox" };

            Form f2 = new Form(topForm, new Vector2(200, 250), new Vector2(275, 270)) { Text = "My form 2" };
            Form f3 = new Form(f2, Vector2.Zero, new Vector2(200, 200)) { Text = "form 3" };
            Form f4 = new Form(f3, Vector2.Zero, new Vector2(100, 100)) { Text = "form 4" };

            Label testLabelF4 = new Label(f4, Vector2.Zero) { Text = "Click me" };
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
            foreach (Control c in GUIManager.GetAllControls())
            {
                if (c.GetType() == typeof(Button))
                    c.Tooltip = Tooltip_Button;
                else if (c.GetType() == typeof(Label))
                    c.Tooltip += Tooltip_Label;
            }

            // Paged list
            var items = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                items.Add(i.ToString());
            }

            new ListBox<string>(topForm, new Vector2(500, 250), new Vector2(100, 100)) { Items = items, ShowPaging = true };
        }

        void testLabelF4_Clicked(object sender, MouseButtonEventArgs e)
        {
            Label source = (Label)sender;
            if (source.Text == "I was clicked!")
                source.Text = "Click me!";
            else
                source.Text = "I was clicked!";

            var msgBox = new MessageBox(GUIManager, "My message box",
                                        "asdlkf aslfdkj sadflkj asdflkj was fadjlkjfsalkaj sfdlksadjf asfdjlalksdfj asdfsdfa eklrj afek jasdlfkj asdflkj asdflkj woieur klasdf\nasdflkj\nasdf\nadsf",
                                        MessageBoxButton.YesNoCancel);
            msgBox.OptionSelected += (x, y) => new MessageBox(GUIManager, "Hello", "You selected: " + y, MessageBoxButton.Ok);
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

            byte r = (byte)rnd.Next(100, 255);
            byte g = (byte)rnd.Next(100, 255);
            byte b = (byte)rnd.Next(100, 255);

            Color c = new Color(r, g, b, 255);
            return new StyledText[] { new StyledText("Text for a "), new StyledText("label", c) };
        }

        void topForm_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            Control c = GUIManager.GetControlAtPoint(new Vector2(e.X, e.Y) + topForm.Position);

            if (c == null)
                return;

            // TODO: ## Window.Title = c.ToString();
        }
    }
}