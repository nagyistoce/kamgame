using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#if ANDROID
using Microsoft.Devices.Sensors;
#endif

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
#if ANDROID
            if (accelSensor != null)
            {
                accelSensor.Dispose();
                accelSensor = null;
            }
#endif
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

        public int FrameIndex;


        #region Inputs

        public bool UseMouse = true;
        public bool UseTouch = true;
        public bool UseAccelerometer = true;

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

        public Vector3 Acceleration, PriorAcceleration;
        /// <summary>
        /// Квадрат скаляра ускорения
        /// </summary>
        public float AccelerationDistance2;
#if ANDROID
        private Accelerometer accelSensor;
#endif


        private bool startClick;

        protected void HandleInput()
        {
            if (CustomCursorOffset != Vector2.Zero)
            {
                CursorOffset = CustomCursorOffset;
                CursorIsDraged = true;
                return;
            }

#if ANDROID
            if (UseAccelerometer)
            {
                if (accelSensor == null)
                {
                    accelSensor = new Accelerometer();
                    accelSensor.CurrentValueChanged += (sender, e) =>
                    {
                        Acceleration = e.SensorReading.Acceleration;
                        if (Window.CurrentOrientation == DisplayOrientation.Portrait)
                        {
                            Acceleration.X = -Acceleration.X;
                        }
                        else
                        {
                            var x = Acceleration.X;
                            Acceleration.X = Acceleration.Y;
                            Acceleration.Y = x;
                        }

                        //Android.Util.Log.Debug("ACCEL", Acceleration.ToString());
                    };
                    accelSensor.Start();
                }
                AccelerationDistance2 = PriorAcceleration == Vector3.Zero ? 0 : Vector3.DistanceSquared(PriorAcceleration, Acceleration);
            }
#endif

            if (!UseMouse && !UseTouch) return;

            if (UseMouse)
            {
                PrevMouseState = MouseState;
                MouseState = Mouse.GetState();
                MouseIsMoved = MouseState.X != PrevMouseState.X || MouseState.Y != PrevMouseState.Y;
                CursorIsClicked = false;
            }

            if (MouseIsMoved)
            {
                CursorPosition.X = MouseState.X;
                CursorPosition.Y = MouseState.Y;
                CursorOffset = new Vector2(MouseState.X - PrevMouseState.X, MouseState.Y - PrevMouseState.Y);
            }
            else if (UseTouch)
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

        protected void ClearInput()
        {
            CustomCursorOffset = Vector2.Zero;
            CursorIsDraged = false;
            CursorIsClicked = false;
            PriorAcceleration = Acceleration;
        }

        #endregion



        #region Stages

        //protected override void Initialize()
        //{
        //    base.Initialize();
        //}


        #region LoadContent

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

        protected virtual void AfterLoadContent() { }

        #endregion


        #region Update

        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            unchecked
            {
                ++FrameIndex;
            }
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

            HandleInput();
        }

        protected virtual void DoUpdate()
        {
            base.Update(GameTime);
        }

        protected virtual void AfterUpdate()
        {
        }

        #endregion


        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            GameTime = gameTime;
            BeforeDraw();
            DoDraw();
            AfterDraw();
        }

        protected virtual void BeforeDraw() { }

        protected virtual void DoDraw()
        {
            base.Draw(GameTime);
        }

        protected virtual void AfterDraw()
        {
            ClearInput();
        }

        #endregion


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
