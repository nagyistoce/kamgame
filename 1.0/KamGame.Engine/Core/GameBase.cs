using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;


namespace KamGame
{
    public class GameBase : Game
    {
        public GameBase()
        {
            InstanceCount++;
            InstanceIndex = NextInstanceIndex++;
            Content.RootDirectory = "Content";
            Graphics = new GraphicsDeviceManager(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            InstanceCount--;
        }

        public static int NextInstanceIndex;
        public static int InstanceCount;
        public int InstanceIndex;
        public override string ToString()
        {
            return base.ToString() + " [" + InstanceIndex + "/" + InstanceCount + "]";
        }


        public readonly GraphicsDeviceManager Graphics;

        public float ScreenWidth { get { return GraphicsDevice.Viewport.Width; } }
        public float ScreenHeight { get { return GraphicsDevice.Viewport.Height; } }
        public float LandscapeWidth { get; protected set; }
        public float LandscapeHeight { get; protected set; }


        public GameTime GameTime { get; private set; }


        private long defaultTargetElapsedTicks;
        public int TargetFramesPerSecond
        {
            set
            {
                if (defaultTargetElapsedTicks == 0)
                    defaultTargetElapsedTicks = TargetElapsedTime.Ticks;
                if (value > 0)
                    TargetElapsedTime = TimeSpan.FromSeconds(1.0 / value);
                GameTimeScale = defaultTargetElapsedTicks / (float)TargetElapsedTime.Ticks;
                GameSpeedScale = 1 / GameTimeScale;
                GameAccelerateScale = GameSpeedScale * GameSpeedScale;
            }
        }

        /// <summary>
        /// Масштаб времени. Чем больше, тем быстрее проходит время. Чтоб отмасштабировать скорость необходимо её разделить на масштаб. 
        /// </summary>
        public float GameTimeScale { get; private set; }
        /// <summary>
        /// Масштаб скорости (обратен масштабу времени). Чтоб отмасштабировать скорость необходимо её умножить на масштаб. 
        /// </summary>
        public float GameSpeedScale { get; private set; }
        /// <summary>
        /// Масштаб ускорения (равен квадрату масштаба скорости). Чтоб отмасштабировать ускорение необходимо её умножить на масштаб. 
        /// </summary>
        public float GameAccelerateScale { get; private set; }



        public Vector2 CursorPosition;
        public Vector2 PriorCursorPosition;
        public Vector2 CursorOffset { get; private set; }
        public Vector2 CustomCursorOffset;
        public bool CursorIsDraged;
        public bool CursorIsClicked;

        public MouseState MouseState;
        public bool MouseIsMoved;
        public MouseState PrevMouseState;
        public readonly List<GestureSample> Gestures = new List<GestureSample>();

        public int FrameIndex;

        #region Stages

        //protected override void Initialize()
        //{
        //    base.Initialize();
        //}

        protected override void LoadContent()
        {
            BeforeLoadContent();
            DoLoadContent();
            AfterLoadContent();
        }
        protected virtual void BeforeLoadContent()
        {
            LandscapeWidth = Math.Max(ScreenWidth, ScreenHeight);
            LandscapeHeight = Math.Min(ScreenWidth, ScreenHeight);
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

        private bool startClick;

        protected virtual void BeforeUpdate()
        {
            //if (ScreenWidth != Graphics.PreferredBackBufferWidth || ScreenHeight != Graphics.PreferredBackBufferHeight)
            //{
            //    Graphics.PreferredBackBufferWidth = ScreenWidth;
            //    Graphics.PreferredBackBufferHeight = ScreenHeight;
            //    Graphics.ApplyChanges();
            //}

            if (CustomCursorOffset != Vector2.Zero)
            {
                CursorOffset = CustomCursorOffset;
                CursorIsDraged = true;
                return;
            }

            PrevMouseState = MouseState;
            MouseState = Mouse.GetState();
            MouseIsMoved = MouseState.X != PrevMouseState.X || MouseState.Y != PrevMouseState.Y;
            CursorIsClicked = false;
            if (MouseIsMoved)
            {
                CursorPosition.X = MouseState.X;
                CursorPosition.Y = MouseState.Y;
                CursorOffset = new Vector2(MouseState.X - PrevMouseState.X, MouseState.Y - PrevMouseState.Y);
            }
            else
            {
                CursorOffset = Vector2.Zero;
                Gestures.Clear();
                while (TouchPanel.IsGestureAvailable)
                {
                    var g = TouchPanel.ReadGesture();
                    if (g.GestureType == GestureType.Tap)
                        CursorIsClicked = true;
                    else
                        CursorOffset += g.Delta + g.Delta2;
                    CursorPosition = g.Position;
                    Gestures.Add(g);
                }
            }

            CursorIsDraged =
                Math.Abs(CursorOffset.X) >= 1 &&
                (!MouseIsMoved || MouseState.LeftButton == ButtonState.Pressed);

            if (MouseState.LeftButton == ButtonState.Pressed)
            {
                startClick = true;
                CursorIsClicked = false;
            }
            else if (startClick)
            {
                CursorIsClicked = true;
                startClick = false;
            }

        }
        protected virtual void DoUpdate()
        {
            base.Update(GameTime);
        }
        protected virtual void AfterUpdate()
        {
            CustomCursorOffset = Vector2.Zero;
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

        //        public XElement LoadXml(string fileName)
        //        {
        //#if ANDROID
        //            return XElement.Load(Activity.Assets.Open("Content/" + fileName.Replace(@"\", "/")));
        //#else
        //            return XElement.Load("Content/" + fileName);
        //#endif
        //        }

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
