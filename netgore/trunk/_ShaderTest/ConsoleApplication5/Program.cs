using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace ConsoleApplication5
{
    class Program
    {
        static readonly List<IFx> _fx = new List<IFx>();

        static Image _imgBg;
        static Image _imgWave;
        static Image _imgBoom;
        static Image _imgGlass;
        static Image _imgNoise;
        static Image _imgWater;
        static Sprite _sprite;
        static Shader _shReflect;
        static Shader _shSpriteShader;
        static Shader _shReflectDrawWater;
        static Shader _shReflectDrawExplosion;
        static Vector2 _mousePos;
        static RenderWindow _rw;
        static RenderImage _bumpMap;
        static RenderImage _buffer;

        static void Main(string[] args)
        {
            int fpsCount = 0;
            int lastFpsTime = int.MinValue;

            if (!Shader.IsAvailable)
                throw new Exception("No shader support, asshole.");

            var rw = new RenderWindow(VideoMode.DesktopMode, "Test", Styles.Resize | Styles.Titlebar);
            _rw = rw;
            rw.SetSize(800, 600);
            rw.DefaultView.Size = new Vector2(800, 600);
            rw.DefaultView.Center = rw.DefaultView.Size / 2f;
            rw.CurrentView = rw.DefaultView;
            rw.Show(true);
            rw.ShowMouseCursor(true);
            rw.SetActive(true);
            rw.UseVerticalSync(false);

            _bumpMap = new RenderImage(rw.Width, rw.Height);
            _buffer = new RenderImage(rw.Width, rw.Height);
       
            _sprite = new Sprite();
            _imgBg = new Image("bg.bmp");
            _imgBoom = new Image("boom.png");
            _imgBoom.CreateMaskFromColor(new Color(0, 0, 0));
            _imgWave = new Image("wave.jpg");
            _imgGlass = new Image("glass.png");
            _imgNoise = new Image("noise.jpg");
            _imgWater = new Image("water.png");

            rw.MouseMoved += rw_MouseMoved;
            rw.MouseButtonPressed += rw_MouseButtonPressed;
            rw.MouseButtonReleased += rw_MouseButtonReleased;
            rw.KeyPressed += rw_KeyPressed;

            _shReflect = new Shader("reflect.sfx");

            _shReflectDrawWater = new Shader("reflect_draw_water.sfx");
            _shReflectDrawWater.SetTexture("WaveNoiseTexture", _imgNoise);

            _shReflectDrawExplosion = new Shader("reflect_draw_explosion.sfx");
            _shReflectDrawExplosion.SetTexture("NoiseTexture", _imgBoom);

            _shSpriteShader = new Shader("sprite_shader.sfx");

            _fx.Add(new Water(new Vector2(0, 300), new Vector2(800, 300)));

            //Text fpsText = new Text { Font = Font.DefaultFont, Color = Color.White, Position = new Vector2(0, 0) };

            int lastUpdateTime = int.MinValue;

            while (rw.IsOpened())
            {
                _shReflectDrawWater.SetParameter("Time", Environment.TickCount / 100f);

                if (lastFpsTime + 1000 < Environment.TickCount)
                {
                    lastFpsTime = Environment.TickCount;
                    //fpsText.DisplayedString = fpsCount.ToString();
                    fpsCount = 0;
                }
                fpsCount++;

                if (lastUpdateTime + 10 < Environment.TickCount)
                {
                    lastUpdateTime = Environment.TickCount;
                    foreach (var fx in _fx)
                        fx.Update();
                }

                rw.DispatchEvents();

                rw.Clear();
                _bumpMap.Clear(new Color(127,127,127,0));
                _buffer.Clear();

                Render();

                _bumpMap.Display();
                _buffer.Display();

                InitSprite(_buffer.Image, new Vector2(0, 0));
                rw.Draw(_sprite);

                if (_fx.Count > 0)
                {
                    if (!rw.Input.IsKeyDown(KeyCode.Tilde))
                    {
                        _shReflect.SetTexture("ColorMap", _buffer.Image);
                        _shReflect.SetTexture("NoiseMap", _bumpMap.Image);

                        InitSprite(_bumpMap.Image, new Vector2(0, 0));

                        _sprite.BlendMode = BlendMode.None;
                        if (!rw.Input.IsKeyDown(KeyCode.Num1))
                        {
                            rw.Draw(_sprite, _shReflect);
                        }
                        else
                        {
                            rw.Draw(_sprite);
                        }
                    }
                }

                //rw.Draw(fpsText);

                rw.Display();
            }
        }

        static void rw_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
        }

        static void rw_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == KeyCode.Escape)
                _rw.Close();
        }

        static void rw_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
                _fx.Add(new Explosion(_mousePos));
        }

        static void rw_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            _mousePos = new Vector2(e.X, e.Y);
        }

        static void InitSprite(Image i, Vector2 pos)
        {
            _sprite.Position = pos;
            _sprite.Image = i;
            _sprite.Width = i.Width;
            _sprite.Height = i.Height;
            _sprite.SubRect = new IntRect(0, 0, (int)i.Width, (int)i.Height);
            _sprite.Scale = new Vector2(1, 1);
            _sprite.BlendMode = BlendMode.Alpha;
            _sprite.Color = new Color(255, 255, 255, 255);
        }

        static void InitSprite(Image i)
        {
            InitSprite(i, new Vector2(0, 0));
        }

        static void Render()
        {
            _shSpriteShader.Bind();
            InitSprite(_imgBg);
            _buffer.Draw(_sprite);
            _shSpriteShader.Unbind();

            _fx.Sort((x, y) => x.GetType() == typeof(Explosion) ? 0 : 1);

            for (int i = 0; i < _fx.Count; i++)
            {
                if (_fx[i].IsDead)
                {
                    _fx.RemoveAt(i);
                    i--;
                }
                else
                {
                    _fx[i].Draw(_bumpMap);
                }
            }
        }

        interface IFx
        {
            void Draw(RenderTarget rt);

            void Update();

            bool IsDead { get; }
        }

        class Water : IFx
        {
            readonly Vector2 _pos;
            readonly Vector2 _size;

            public Water(Vector2 pos, Vector2 size)
            {
                _pos = pos;
                _size = size;
            }

            public void Draw(RenderTarget rt)
            {
                InitSprite(_imgWater);
                _sprite.Position = _pos;
                _sprite.Width = _size.X;
                _sprite.Height = _size.Y;
                _sprite.BlendMode = BlendMode.None;

                rt.Draw(_sprite, _shReflectDrawWater);
            }

            public void Update()
            {
            }

            public bool IsDead
            {
                get { return false; }
            }
        }

        class Explosion : IFx
        {
            readonly Vector2 _pos;

            float _expandTimer = 0.1f;

            public Explosion(Vector2 pos)
            {
                _pos = pos;
            }

            public void Draw(RenderTarget rt)
            {
                InitSprite(_imgBoom);

                _sprite.Scale = new Vector2(_expandTimer, _expandTimer);
                _sprite.Position = _pos - new Vector2(_sprite.Width / 2f, _sprite.Height / 2f);
                _sprite.BlendMode = BlendMode.Add;

                _shReflectDrawExplosion.SetParameter("MaxAge", 50.0f);
                _shReflectDrawExplosion.SetParameter("Age", _expandTimer);

                rt.Draw(_sprite, _shReflectDrawExplosion);
            }

            public void Update()
            {
                _expandTimer += 0.72f;
            }

            public bool IsDead
            {
                get { return _expandTimer > 50.0f; }
            }
        }
    }
}
