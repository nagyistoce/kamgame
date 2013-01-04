using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Content;
using Android.Graphics;
using Android.Preferences;
using Android.Service.Wallpaper;
using Android.Views;
using Microsoft.Xna.Framework;
using Xna = Microsoft.Xna.Framework;


namespace KamGame
{
    //[Service(Label = "@string/ApplicationName", Permission = "android.permission.BIND_WALLPAPER")]
    //[IntentFilter(new[] { "android.service.wallpaper.WallpaperService" })]
    //[MetaData("android.service.wallpaper", Resource = "@xml/cube1")]
    public abstract class GameWallpaperService : WallpaperService
    {
        protected GameWallpaperService()
        {
        }

        public static bool PreferenceActivityIsActive;

        public override Engine OnCreateEngine()
        {
            return new GameEngine(this);
        }

        protected abstract Game2D NewGame();

        protected virtual void ApplyPreferences(ISharedPreferences p) { }



        public class GameEngine : Engine, ISharedPreferencesOnSharedPreferenceChangeListener
        {
            public readonly GameWallpaperService Service;
            protected LogWriter Log;

            public GameEngine(GameWallpaperService service)
                : base(service)
            {
#if DEBUG
                Log = new LogWriter("KamGame.GameWallpaper", () =>
                    InstanceIndex + "    " +
                    (MyGame == null ? " " : MyGame.InstanceIndex.ToString(CultureInfo.InvariantCulture)) + " / " +
                    GameBase.InstanceCount + "    "
                );
#endif
                Log += "constructor";
                if (service == null)
                    throw new ArgumentNullException("service");
                Service = service;
                Log--;
            }

            public static Game2D Game { get; private set; }
            public Game2D MyGame { get; private set; }

            public ISharedPreferences Preferences { get; private set; }
            private ScreenReceiver screenReceiver;


            protected static int InstanceLastIndex = -1;
            protected int InstanceIndex = ++InstanceLastIndex;

            public override void OnCreate(ISurfaceHolder holder)
            {
                Log += "OnCreate";
                base.OnCreate(holder);

                Preferences = PreferenceManager.GetDefaultSharedPreferences(Service);
                Preferences.RegisterOnSharedPreferenceChangeListener(this);

                var filter = new IntentFilter();
                filter.AddAction(Intent.ActionScreenOff);
                filter.AddAction(Intent.ActionScreenOn);
                filter.AddAction(Intent.ActionUserPresent);

                screenReceiver = new ScreenReceiver();
                Service.RegisterReceiver(screenReceiver, filter);

                SetTouchEventsEnabled(true);
                Log--;
            }

            public override void OnDestroy()
            {
                Log += "OnDestroy";

                if (Game != MyGame)
                    MyGame = null;
                else 
                    DestroyGame();

                if (screenReceiver != null)
                {
                    Service.UnregisterReceiver(screenReceiver);
                    screenReceiver = null;
                }

                Preferences.UnregisterOnSharedPreferenceChangeListener(this);

                base.OnDestroy();

                Log--;
            }


            private void CreateGame()
            {
                Log += "Create Game";
                Xna.Game.Context = Service;
                Xna.Game.CustomHolder = SurfaceHolder;

                Game = MyGame = Service.NewGame();
                Game.UseMouse = false;
                Game.UseTouch = false;
                Game.UseAccelerometer = true;

                Game.Run();
                Log--;
            }

            private void DestroyGame()
            {
                if (Game == null) return;
                Log += "Destroy Game";
                Game.Finish();
                Game = MyGame = null;
                Log--;
            }

            public bool IsCurrentGame
            {
                get
                {
                    //if (InstanceCount == 1)
                    //    MyGame = Game;
                    //return !MyGame.IsDisposed;
                    return MyGame != null && !MyGame.IsDisposed && Game == MyGame;
                }
            }


            public override void OnSurfaceCreated(ISurfaceHolder holder)
            {
                Log += "OnSurfaceCreated";
                DestroyGame();
                CreateGame();
                base.OnSurfaceCreated(holder);
                Log--;
            }

            public override void OnSurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
            {
                if (IsCurrentGame)
                {
                    Log += "OnSurfaceChanged";
                    Game.ClearInput();
                    Xna.Game.SurfaceWidth = width;
                    Xna.Game.SurfaceHeight = height;
                    base.OnSurfaceChanged(holder, format, width, height);
                    Log--;
                }
                else
                    base.OnSurfaceChanged(holder, format, width, height);
            }

            public override void OnSurfaceDestroyed(ISurfaceHolder holder)
            {
                if (IsCurrentGame)
                {
                    Log += "OnSurfaceDestroyed";
                    DestroyGame();
                    base.OnSurfaceDestroyed(holder);
                    Log--;
                }
                else
                    base.OnSurfaceDestroyed(holder);
            }


            public void OnSharedPreferenceChanged(ISharedPreferences p, string key)
            {
                if (!PreferenceActivityIsActive) return;
                Log += "OnSharedPreferenceChanged";
                Service.ApplyPreferences(p);
                AndroidGameActivity.DoResumed();
                Log--;
            }


            private bool IsFirstShowing = true;
            public override void OnVisibilityChanged(bool visible)
            {
                if (IsCurrentGame)
                {
                    Log += "OnVisibilityChanged " + visible;
                    if (visible)
                    {
                        if (IsFirstShowing)
                        {
                            Log += "FirstShowing";
                            Service.ApplyPreferences(Preferences);
                            IsFirstShowing = false;
                            Log--;
                        }
                        Game.StartFade();
                        AndroidGameActivity.DoResumed();
                    }
                    else
                        AndroidGameActivity.DoPaused();
                    Game.ClearInput();
                    base.OnVisibilityChanged(visible);
                    Log--;
                }
                else
                    base.OnVisibilityChanged(visible);
            }


            public override void OnOffsetsChanged(float xOffset, float yOffset, float xOffsetStep, float yOffsetStep,
                int xPixelOffset, int yPixelOffset)
            {
                if (xOffsetStep <= .01f) return;
                Game.ClearInput();
                Game.UsePageOffset = true;
                Game.PageOffset = xOffset;
                Game.PageOffsetStep = xOffsetStep;
            }

            private Vector2 touchPrior;
            // Store the position of the touch event so we can use it for drawing later
            public override void OnTouchEvent(MotionEvent e)
            {
                if (!IsCurrentGame || Game.UsePageOffset) return;

                if (e.Action == MotionEventActions.Move)
                {
                    var pos = new Vector2(e.GetX(), e.GetY());
                    Game.CustomCursorOffset = touchPrior != Vector2.Zero ? pos - touchPrior : Vector2.Zero;
                    touchPrior = pos;
                }
                else
                    touchPrior = Vector2.Zero;

                base.OnTouchEvent(e);
            }


        }

    }


    public class GameWallpaperService<TGame> : GameWallpaperService
        where TGame : Game2D, new()
    {
        public TGame Game { get; private set; }

        protected override Game2D NewGame()
        {
            return Game = new TGame();
        }
    }


}