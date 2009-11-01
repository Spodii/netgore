using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.GUITester
{
    public class Game1 : Game
    {
        static readonly Random rnd = new Random();

        readonly GraphicsDeviceManager _graphics;
        SpriteFont _font;
        GUIManagerBase _gui;
        SpriteBatch _sb;
        TextBox _textBox;
        ControlBorder _topBorder;
        Form topForm;

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

            if (topForm.Border == null)
                topForm.Border = _topBorder;
            else
                topForm.Border = null;
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

            _gui = new GUIManager(_font);

            topForm = new Form(_gui, "Primary form", new Vector2(5, 5), new Vector2(700, 550));
            topForm.OnMouseMove += topForm_OnMouseMove;

            TextBoxMultiLineLocked tbmll = new TextBoxMultiLineLocked(string.Empty, new Vector2(10, 10), new Vector2(150, 300),
                                                                      topForm);

            _textBox = new TextBox(_gui, _font, new Vector2(350, 10), new Vector2(200, 200), topForm);
            _textBox.Append("abcdef\nghi\r\njklj\n");

            for (int i = 0; i < 150; i++)
            {
                Color c = new Color((byte)rnd.Next(0, 256), (byte)rnd.Next(256), (byte)rnd.Next(256), 255);
                _textBox.Append(new StyledText(i + " ", c));
            }

            _textBox.OnKeyUp += delegate(object sender, KeyboardEventArgs e)
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
            tbmll.Append(styledTexts);

            _topBorder = topForm.Border;

            Form form = new Form(_gui, "My form", new Vector2(50, 50), new Vector2(200, 200), topForm);

            Button b = new Button("Press me", new Vector2(20, 20), new Vector2(80, 30), form);
            b.OnClick += b_OnMouseDown;

            new CheckBox("Checkbox", new Vector2(20, 200), form);

            Form f2 = new Form(_gui, "My form 2", new Vector2(200, 250), new Vector2(275, 270), topForm);
            Form f3 = new Form(_gui, "form 3", Vector2.Zero, new Vector2(200, 200), f2);
            Form f4 = new Form(_gui, "form 4", Vector2.Zero, new Vector2(100, 100), f3);

            Label testLabelF4 = new Label("Click me", Vector2.Zero, f4);
            testLabelF4.OnClick += testLabelF4_OnClick;

            topForm.OnBeginDrag += OnDrag;
            topForm.OnEndDrag += OnDrag;
            form.OnBeginDrag += OnDrag;
            form.OnEndDrag += OnDrag;
            f2.OnBeginDrag += OnDrag;
            f2.OnEndDrag += OnDrag;
            f3.OnBeginDrag += OnDrag;
            f3.OnEndDrag += OnDrag;
            f4.OnBeginDrag += OnDrag;
            f4.OnEndDrag += OnDrag;

            // Set up the tooltips
            foreach (Control c in _gui.GetAllControls())
            {
                if (c.GetType() == typeof(Button))
                    c.Tooltip = Tooltip_Button;
                else if (c.GetType() == typeof(Label))
                    c.Tooltip += Tooltip_Label;
            }
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
            int currentTime = (int)gameTime.TotalRealTime.TotalMilliseconds;
            _gui.Update(currentTime);
            base.Update(gameTime);
        }
    }
}