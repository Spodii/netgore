using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.World;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// Base class for targeting.
    /// </summary>
    public abstract class Targeter
    {
        readonly TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> _mouseOverAfterDrawHandler;
        readonly TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> _mouseOverBeforeDrawHandler;
        readonly TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> _targetAfterDrawHandler;
        readonly TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> _targetBeforeDrawHandler;
        readonly World _world;


        IDrawableTarget _mouseOver;
        IDrawableTarget _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="Targeter"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <exception cref="ArgumentNullException"><paramref name="world"/> is <c>null</c>.</exception>
        protected Targeter(World world)
        {
            if (world == null)
                throw new ArgumentNullException("world");

            _world = world;

            _mouseOverBeforeDrawHandler = MouseOver_BeforeDraw;
            _mouseOverBeforeDrawHandler = MouseOver_BeforeDraw;
            _mouseOverAfterDrawHandler = MouseOver_AfterDraw;
            _targetBeforeDrawHandler = Target_BeforeDraw;
            _targetAfterDrawHandler = Target_AfterDraw;

            _world.MapChanged -= World_MapChanged;
            _world.MapChanged += World_MapChanged;
        }

        /// <summary>
        /// Gets the <see cref="MapEntityIndex"/> of the <see cref="Targeter.Target"/>.
        /// </summary>
        public MapEntityIndex? TargetEntityIndex
        {
            get 
            { 
                if (_target == null) return null;

                return _target.MapEntityIndex;
            }
        }

        /// <summary>
        /// Gets the <see cref="MapEntityIndex"/> of the <see cref="Targeter.MouseOverTarget"/>.
        /// </summary>
        public MapEntityIndex? MouseOverTargetIndex
        {
            get
            {
                if (MouseOverTarget == null)
                    return null;

                return MouseOverTarget.MapEntityIndex;
            }
        }

        /// <summary>
        /// Occurs immediately after the <see cref="MouseOverTarget"/> is drawn.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{ISpriteBatch}"/> instance containing the event data.</param>
        protected virtual void MouseOver_AfterDraw(IDrawable sender, EventArgs<ISpriteBatch> e)
        {
        }

        /// <summary>
        /// Occurs immediately before the <see cref="MouseOverTarget"/> is drawn.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{ISpriteBatch}"/> instance containing the event data.</param>
        protected virtual void MouseOver_BeforeDraw(IDrawable sender, EventArgs<ISpriteBatch> e)
        {
        }

        /// <summary>
        /// Occurs immediately after the <see cref="Target"/> is drawn.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{ISpriteBatch}"/> instance containing the event data.</param>
        protected virtual void Target_AfterDraw(IDrawable sender, EventArgs<ISpriteBatch> e)
        {
        }

        /// <summary>
        /// Occurs immediately before the <see cref="Target"/> is drawn.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{ISpriteBatch}"/> instance containing the event data.</param>
        protected virtual void Target_BeforeDraw(IDrawable sender, EventArgs<ISpriteBatch> e)
        {
        }

        /// <summary>
        /// Gets the <see cref="IDrawableTarget"/> that is currently targeted.
        /// </summary>
        public IDrawableTarget Target
        {
            get { return _target; }
            private set {
                if (_target == value)
                    return;

                if (_target != null)
                {
                    _target.BeforeDraw -= _targetBeforeDrawHandler;
                    _target.AfterDraw -= _targetAfterDrawHandler;
                }

                _target = value;

                if (_target != null)
                {
                    _target.BeforeDraw -= _targetBeforeDrawHandler;
                    _target.BeforeDraw += _targetBeforeDrawHandler;

                    _target.AfterDraw -= _targetAfterDrawHandler;
                    _target.AfterDraw += _targetAfterDrawHandler;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="IDrawableTarget"/> the cursor is currently over.
        /// </summary>
        public IDrawableTarget MouseOverTarget
        {
            get { return _mouseOver; }
            private set 
            {
                if (_mouseOver == value)
                    return;

                if (_mouseOver != null)
                {
                    _mouseOver.BeforeDraw -= _mouseOverBeforeDrawHandler;
                    _mouseOver.AfterDraw -= _mouseOverAfterDrawHandler;
                }

                _mouseOver = value;

                if (_mouseOver != null)
                {
                    _mouseOver.BeforeDraw -= _mouseOverBeforeDrawHandler;
                    _mouseOver.BeforeDraw += _mouseOverBeforeDrawHandler;

                    _mouseOver.AfterDraw -= _mouseOverAfterDrawHandler;
                    _mouseOver.AfterDraw += _mouseOverAfterDrawHandler;
                }
            }
        }

        /// <summary>
        /// Updates the targeting.
        /// </summary>
        /// <param name="gui">THe <see cref="IGUIManager"/>.</param>
        public void Update(IGUIManager gui)
        {
            var cursorPos = gui.CursorPosition;

            MouseOverTarget = _world.Map.Spatial.Get<IDrawableTarget>(_world.Camera.ToWorld(cursorPos));

            if (gui.IsMouseButtonDown(Mouse.Button.Left))
            {
                if (MouseOverTarget != null)
                {
                    Target = MouseOverTarget == _world.UserChar ? null : MouseOverTarget;
                }
                else
                {
                    Target =  null;
                }
            }

            if (MouseOverTarget != null && MouseOverTarget.IsDisposed)
                MouseOverTarget = null;

            if (Target != null && Target.IsDisposed)
                Target = null;
        }


        /// <summary>
        /// Handles the map change event for targeting.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs{Map}"/> instance containing the event data.</param>
        void World_MapChanged(World sender, ValueChangedEventArgs<Map> e)
        {
            MouseOverTarget = null;
            _target = null;
        }
    }
}
