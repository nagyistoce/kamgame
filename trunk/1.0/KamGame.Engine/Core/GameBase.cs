using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;


namespace KamGame
{
    public class GameBase : Game
    {
        public GameBase()
            : base()
        {
            Graphics = new GraphicsDeviceManager(this);
        }

        public readonly GraphicsDeviceManager Graphics;

        public float ScreenWidth { get { return GraphicsDevice.Viewport.Width; } }
        public float ScreenHeight { get { return GraphicsDevice.Viewport.Height; } }

        public GameTime GameTime { get; private set; }
        public Vector2 PriorCursorPosition;
        public Vector2 CursorOffset { get; private set; }
        public bool CursorIsDraged;

        public MouseState MouseState;
        public bool MouseIsMoved;
        public MouseState PrevMouseState;
        public readonly List<GestureSample> Gestures = new List<GestureSample>();

        public int FrameIndex;

        #region Stages

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            BeforeLoadContent();
            DoLoadContent();
            AfterLoadContent();
        }
        protected virtual void BeforeLoadContent()
        {
        }
        protected virtual void DoLoadContent()
        {
            base.LoadContent();
        }
        protected virtual void AfterLoadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            unchecked { ++FrameIndex; }
            BeforeUpdate();
            DoUpdate();
            AfterUpdate();
        }
        protected virtual void BeforeUpdate()
        {
            //if (ScreenWidth != Graphics.PreferredBackBufferWidth || ScreenHeight != Graphics.PreferredBackBufferHeight)
            //{
            //    Graphics.PreferredBackBufferWidth = ScreenWidth;
            //    Graphics.PreferredBackBufferHeight = ScreenHeight;
            //    Graphics.ApplyChanges();
            //}

            PrevMouseState = MouseState;
            MouseState = Mouse.GetState();
            MouseIsMoved = MouseState.X != PrevMouseState.X || MouseState.Y != PrevMouseState.Y;
            if (MouseIsMoved)
            {
                CursorOffset = new Vector2(MouseState.X - PrevMouseState.X, MouseState.Y - PrevMouseState.Y);
            }
            else
            {
                CursorOffset = Vector2.Zero;
                Gestures.Clear();
                while (TouchPanel.IsGestureAvailable)
                {
                    var g = TouchPanel.ReadGesture();
                    CursorOffset += g.Delta + g.Delta2;
                    Gestures.Add(g);
                }

                //if (Gestures.Count == 0)
                //    PriorCursorPosition = Vector2.Zero;
                //else
                //{
                //    var g = Gestures[Gestures.Count - 1];
                //    if (PriorCursorPosition != Vector2.Zero)
                //        CursorOffset = g.Position - PriorCursorPosition;
                //    PriorCursorPosition = g.Position;
                //}
            }

            CursorIsDraged =
                Math.Abs(CursorOffset.X) >= 1 &&
                (!MouseIsMoved || MouseState.LeftButton == ButtonState.Pressed);

        }
        protected virtual void DoUpdate()
        {
            base.Update(GameTime);
        }
        protected virtual void AfterUpdate()
        {
        }


        protected override void Draw(GameTime gameTime)
        {
            GameTime = gameTime;
            BeforeDraw();
            DoDraw();
            AfterDraw();
        }
        protected virtual void BeforeDraw()
        {
        }
        protected virtual void DoDraw()
        {
            base.Draw(GameTime);
        }
        protected virtual void AfterDraw()
        {
        }

        #endregion


        #region Load Utilites

        public XElement LoadXml(string fileName)
        {
#if ANDROID
            return XElement.Load(Activity.Assets.Open("Content/" + fileName.Replace(@"\", "/")));
#else
            return XElement.Load("Content/" + fileName);
#endif
        }

        #endregion


        #region Random

        public static readonly Random Random = new Random();

        public int RandInt() { return Random.Next(); }
        public int Rand(int maxValue) { return Random.Next(maxValue); }
        public int Rand(int minValue, int maxValue)
        {
            return minValue >= maxValue ? minValue : Random.Next(minValue, maxValue);
        }

        public double RandDouble() { return Random.NextDouble(); }
        public float Rand() { return (float)Random.NextDouble(); }
        public float Rand(float minValue, float maxValue)
        {
            return minValue >= maxValue ? minValue : minValue + (maxValue - minValue) * (float)Random.NextDouble();
        }

        public double Rand(double minValue) { return Random.NextDouble(); }
        public double Rand(double minValue, double maxValue)
        {
            return minValue >= maxValue ? minValue : minValue + (maxValue - minValue) * Random.NextDouble();
        }

        public int RandSign() { return 1 - 2 * Rand(2); }
        public float RandAngle() { return (float)(2 * Math.PI * Random.NextDouble()); }

        public T Rand<T>(T[] array)
        {
            return array != null && array.Length > 0 ? array[Random.Next(array.Length)] : default(T);
        }

        #endregion


    }
}
