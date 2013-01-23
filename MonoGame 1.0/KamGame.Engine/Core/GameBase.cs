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
        public bool UsePageOffset;

        public Vector2 CursorPosition;
        public Vector2 PriorCursorPosition;
        public Vector2 CursorOffset { get; private set; }
        public Vector2 CustomCursorOffset;
        public bool CursorIsDraged;
        public bool CursorIsClicked;
        public float PageOffset;
        public float PageOffsetStep;

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

        public void ClearInput()
        {
            CustomCursorOffset = Vector2.Zero;
            CursorIsDraged = false;
            CursorIsClicked = false;
            PriorAcceleration = Acceleration;
        }

        #endregion


        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            ClearInput();
        }

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

        private bool ScreenIsRotated;

        protected virtual void BeforeUpdate()
        {
#if DEBUG
            if (!ScreenIsRotated && Keyboard.GetState().IsKeyDown(Keys.F12))
            {
                var w = Graphics.PreferredBackBufferWidth;
                Graphics.PreferredBackBufferWidth = Graphics.PreferredBackBufferHeight;
                Graphics.PreferredBackBufferHeight = w;
                Graphics.ApplyChanges();
                ScreenIsRotated = true;
            }
            else
            {
                ScreenIsRotated = false;
            }
#endif

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

        public double Rand(double minValue, double maxValue)
        {
            return minValue >= maxValue ? minValue : minValue + (maxValue - minValue) * Random.NextDouble();
        }

        public bool RandBool() { return Random.Next(2) == 0; }
        public int RandSign() { return 1 - 2 * Rand(2); }
        public float RandAngle() { return (float)(2 * Math.PI * Random.NextDouble()); }

        public T Rand<T>(T[] array)
        {
            return array != null && array.Length > 0 ? array[Random.Next(array.Length)] : default(T);
        }


        public int[] RandSequence(int length)
        {
            var a = new int[length];
            for (var i = 0; i < length; i++)
            {
                a[i] = i;
            }
            for (var i = 0; i < length; i++)
            {
                var x = a[i];
                var j = Rand(length);
                a[i] = a[j];
                a[j] = x;
            }
            return a;
        }

        public T[] RandSequence<T>(int length, Func<int, T> select)
        {
            var a = new T[length];
            for (var i = 0; i < length; i++)
            {
                a[i] = select(i);
            }
            for (var i = 0; i < length; i++)
            {
                var x = a[i];
                var j = Rand(length);
                a[i] = a[j];
                a[j] = x;
            }
            return a;
        }

        #endregion


        #region Sin, Cos

        private static readonly float[] sin360 = NewSin360();
        //private static readonly float[] cos360 = NewCos360();

        static float[] NewSin360()
        {
            var a = new float[360];
            var p = 2 * Math.PI / 360;
            for (var i = 0; i < 360; i++)
            {
                a[i] = (float)Math.Sin(p * i);
            }
            return a;
        }

        static float[] NewCos360()
        {
            var a = new float[360];
            var p = 2 * Math.PI / 360;
            for (var i = 0; i < 360; i++)
            {
                a[i] = (float)Math.Cos(p * i);
            }
            return a;
        }

        public float Sin360(int angle)
        {
            angle = angle % 360;
            if (angle < 0) angle = 360 + angle;
            return sin360[angle];
        }
        public float Sin360(float angle)
        {
            return Sin360((int)angle);
        }

        #endregion

    }
}
