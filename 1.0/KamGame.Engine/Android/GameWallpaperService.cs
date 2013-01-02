using System;
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
        protected static LogWriter Log;

        public override Engine OnCreateEngine()
        {
            return new GameEngine(this);
        }

        protected abstract GameBase NewGame();

        protected bool PreferencesIsChanged = true;
        public virtual void OnPreferenceChanged(ISharedPreferences p, string key) { }
        protected virtual void ApplyPreferences(ISharedPreferences p) { }

        public class GameEngine : Engine, ISharedPreferencesOnSharedPreferenceChangeListener
        {
            public readonly GameWallpaperService Service;

            public GameEngine(GameWallpaperService service)
                : base(service)
            {
#if DEBUG
                Log = new LogWriter("KamGame.GameWallpaper", () =>
                    (MyGame == null ? -1 : MyGame.InstanceIndex) + "/" + GameBase.InstanceCount + "    "
                );
#endif
                Log += "constructor";
                if (service == null)
                    throw new ArgumentNullException("service");
                Service = service;
                Log--;
            }

            public static GameBase Game { get; private set; }
            public GameBase MyGame { get; private set; }
            public bool IsCurrentGame { get { return MyGame != null && !MyGame.IsDisposed && Game == MyGame; } }

            public ISharedPreferences Preferences { get; private set; }
            private ScreenReceiver screenReceiver;


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
                Service.PreferencesIsChanged = true;
                Service.OnPreferenceChanged(p, key);
            }

            public override void OnVisibilityChanged(bool visible)
            {
                if (IsCurrentGame)
                {
                    Log += "OnVisibilityChanged " + visible;
                    if (visible)
                    {
                        if (Service.PreferencesIsChanged)
                        {
                            Service.ApplyPreferences(Preferences);
                            Service.PreferencesIsChanged = false;
                        }
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
        where TGame : GameBase, new()
    {
        public TGame Game { get; private set; }

        protected override GameBase NewGame()
        {
            return Game = new TGame();
        }
    }


}