using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.GUITester
{
    public class Game1 : Game
    {
        readonly GraphicsDeviceManager _graphics;
        readonly Random r = new Random();
        SpriteFont _font;
        GUIManagerBase _gui;
        SpriteBatch _sb;
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
            Window.Title = r.NextDouble().ToString();

            if (topForm.Border == null)
                topForm.Border = _topBorder;
            else
                topForm.Border = null;
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            _sb.Begin();
            _gui.Draw(_sb);
            _sb.End();

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            _sb = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("SpriteFont1");
            GrhInfo.Load(ContentPaths.Build.Data.Join("grhdata.xml"), Content);

            _gui = new GUIManager(_font);

            topForm = new Form(_gui, "Primary form", new Vector2(5, 5), new Vector2(500, 500));
            topForm.OnMouseMove += topForm_OnMouseMove;

            TextBoxMultiLineLocked tbmll = new TextBoxMultiLineLocked(string.Empty, new Vector2(10, 10), new Vector2(150, 300),
                                                                      topForm);

            var styledTexts = new List<StyledText>
                              {
                                  new StyledText("Black ", Color.Black), new StyledText("Red ", Color.Red),
                                  new StyledText("Green ", Color.Green), new StyledText("Yellow ", Color.Yellow),
                                  new StyledText("Voilet ", Color.Violet), new StyledText("Orange ", Color.Orange),
                                  new StyledText("Tomato ", Color.Tomato), new StyledText("DarkRed ", Color.DarkRed),
                              };
            tbmll.Append(styledTexts);

            _topBorder = topForm.Border;

            Form form = new Form(_gui, "My form", new Vector2(50, 50), new Vector2(200, 200), topForm);
            TextBoxSingleLine tb = new TextBoxSingleLine("My textboxadsfasdf sd", new Vector2(20, 90), new Vector2(150, 50), form);

            Button b = new Button("Press me", new Vector2(20, 20), new Vector2(80, 30), form);
            b.OnClick += b_OnMouseDown;

            new CheckBox("Checkbox", new Vector2(20, 200), form);

            Form f2 = new Form(_gui, "My form 2", new Vector2(200, 200), new Vector2(300, 300), topForm);
            Form f3 = new Form(_gui, "form 3", Vector2.Zero, new Vector2(200, 200), f2);
            Form f4 = new Form(_gui, "form 4", Vector2.Zero, new Vector2(100, 100), f3);

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
        }

        void OnDrag(Control sender)
        {
            TextControl s = (TextControl)sender;
            s.Text = s.Position.ToString();
            Window.Title = "Screen Position: " + s.ScreenPosition;
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
            _gui.Update();
            base.Update(gameTime);
        }
    }
}