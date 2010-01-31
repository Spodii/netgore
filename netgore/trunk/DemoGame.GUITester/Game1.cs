using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.GUITester
{
    public class Game1 : Game
    {
        static readonly Random rnd = new Random();

        readonly GraphicsDeviceManager _graphics;
        SpriteFont _font;
        IGUIManager _gui;
        SpriteBatch _sb;
        TextBox _textBox;
        ControlBorder _topBorder;
        Form topForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game1"/> class.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        void b_OnMouseDown(object sender, MouseClickEventArgs e)
        {
            Window.Title = rnd.NextDouble().ToString();

            if (topForm.Border == ControlBorder.Empty)
                topForm.Border = _topBorder;
            else
                topForm.Border = ControlBorder.Empty;
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            _sb.BeginUnfiltered();
            _gui.Draw(_sb);
            _sb.End();

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            _sb = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>(ContentPaths.Build.Fonts.Join("Game"));
            GrhInfo.Load(ContentPaths.Build, Content);

            SkinManager skinManager = new SkinManager("Default");
            _gui = new GUIManager(_font, skinManager);

            topForm = new Form(_gui, new Vector2(5, 5), new Vector2(700, 550)) { Text = "Primary form" };
            topForm.MouseMoved += topForm_OnMouseMove;

            TextBox tb = new TextBox(topForm, new Vector2(10, 10), new Vector2(150, 300));

            _textBox = new TextBox(topForm, new Vector2(350, 10), new Vector2(200, 200))
            { Font = _font, Text = "abcdef\nghi\r\njklj\n" };

            for (int i = 0; i < 150; i++)
            {
                Color c = new Color((byte)rnd.Next(0, 256), (byte)rnd.Next(256), (byte)rnd.Next(256), 255);
                _textBox.Append(new StyledText(i + " ", c));
            }

            _textBox.KeyUp += delegate(object sender, KeyboardEventArgs e)
                                {
                                    if (e.Keys.Contains(Keys.F1))
                                        Debug.Fail(_textBox.Text);
                                    else if (e.Keys.Contains(Keys.F2))
                                        _textBox.Size += new Vector2(25, 0);
                                    else if (e.Keys.Contains(Keys.F3))
                                        _textBox.Size += new Vector2(-25, 0);
                                    else if (e.Keys.Contains(Keys.F4))
                                        _textBox.IsMultiLine = !_textBox.IsMultiLine;
                                };

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
            b.Clicked += b_OnMouseDown;

            new CheckBox(form, new Vector2(20, 200)) { Text = "Checkbox" };

            Form f2 = new Form(topForm, new Vector2(200, 250), new Vector2(275, 270)) { Text = "My form 2" };
            Form f3 = new Form(f2, Vector2.Zero, new Vector2(200, 200)) { Text = "form 3" };
            Form f4 = new Form(f3, Vector2.Zero, new Vector2(100, 100)) { Text = "form 4" };

            Label testLabelF4 = new Label(f4, Vector2.Zero) { Text = "Click me" };
            testLabelF4.Clicked += testLabelF4_OnClick;

            topForm.BeginDrag += OnDrag;
            topForm.EndDrag += OnDrag;
            form.BeginDrag += OnDrag;
            form.EndDrag += OnDrag;
            f2.BeginDrag += OnDrag;
            f2.EndDrag += OnDrag;
            f3.BeginDrag += OnDrag;
            f3.EndDrag += OnDrag;
            f4.BeginDrag += OnDrag;
            f4.EndDrag += OnDrag;

            // Set up the tooltips
            foreach (Control c in _gui.GetAllControls())
            {
                if (c.GetType() == typeof(Button))
                    c.Tooltip = Tooltip_Button;
                else if (c.GetType() == typeof(Label))
                    c.Tooltip += Tooltip_Label;
            }

            // Paged list
            var items = new List<string>();
            for (int i = 0; i < 100; i++)
                items.Add(i.ToString());

            var pl = new PagedList<string>(topForm, new Vector2(500, 250), new Vector2(100, 100)) { Items = items };
        }

        void OnDrag(Control sender)
        {
            TextControl s = (TextControl)sender;
            s.Text = s.Position.ToString();
            Window.Title = "Screen Position: " + s.ScreenPosition;
        }

        static void testLabelF4_OnClick(object sender, MouseClickEventArgs e)
        {
            Label source = (Label)sender;
            if (source.Text == "I was clicked!")
                source.Text = "Click me!";
            else
                source.Text = "I was clicked!";
        }

        static StyledText[] Tooltip_Button(Control sender, TooltipArgs args)
        {
            return new StyledText[]
            {
                new StyledText(
                    "hello-hello-hello-hello-hello-hello-hello-hello hello hello hello hello hello hello hello hello hello hello hello hello hello hello hello")
                , new StyledText("button", Color.LightBlue)
            };
        }

        static StyledText[] Tooltip_Label(Control sender, TooltipArgs args)
        {
            args.FontColor = Color.LightGreen;
            args.RefreshRate = 100;

            byte r = (byte)rnd.Next(100, 255);
            byte g = (byte)rnd.Next(100, 255);
            byte b = (byte)rnd.Next(100, 255);

            Color c = new Color(r, g, b, 255);
            return new StyledText[] { new StyledText("Text for a "), new StyledText("label", c) };
        }

        void topForm_OnMouseMove(object sender, MouseEventArgs e)
        {
            Control c = _gui.GetControlAtPoint(e.Location + topForm.Position);

            if (c == null)
            {
                Window.Title = "null";
                return;
            }

            Window.Title = c.ToString();
        }

        protected override void Update(GameTime gameTime)
        {
            int currentTime = gameTime.ToTotalMS();
            _gui.Update(currentTime);
            base.Update(gameTime);
        }
    }
}