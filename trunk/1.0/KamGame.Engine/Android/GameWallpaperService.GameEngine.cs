using System;
using System.Globalization;
using Android.Content;
using Android.Graphics;
using Android.Preferences;
using Android.Views;
using Microsoft.Xna.Framework;
using Xna = Microsoft.Xna.Framework;


namespace KamGame
{


    partial class GameWallpaperService
    {

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

            protected static int InstanceCount;
            protected static int InstanceLastIndex = -1;
            protected int InstanceIndex = ++InstanceLastIndex;

            public override void OnCreate(ISurfaceHolder holder)
            {
                InstanceCount++;
                Log.Try("OnCreate", () =>
                {
                    base.OnCreate(holder);
                    Preferences = PreferenceManager.GetDefaultSharedPreferences(Service);
                    Preferences.RegisterOnSharedPreferenceChangeListener(this);
                    SetTouchEventsEnabled(true);
                });
            }

            public override void OnDestroy()
            {
                Log.Try("OnDestroy", () =>
                {
                    if (Game != MyGame)
                        MyGame = null;
                    else
                        DestroyGame();

                    Preferences.UnregisterOnSharedPreferenceChangeListener(this);

                    base.OnDestroy();

                    InstanceCount--;
                });
            }


            private void CreateGame()
            {
                Log.Try("Create Game", () =>
                {
                    Xna.Game.Context = Service;
                    Xna.Game.CustomHolder = SurfaceHolder;

                    Game = MyGame = Service.NewGame();
                    Game.UseMouse = false;
                    Game.UseTouch = false;
                    Game.UseAccelerometer = true;

                    Game.Run();
                });
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
                    var result = MyGame != null && !MyGame.IsDisposed && Game == MyGame;
                    //Log &= "IsCurrentGame: " + result;
                    return result;
                }
            }


            public override void OnSurfaceCreated(ISurfaceHolder holder)
            {
                Log.Try("OnSurfaceCreated", () =>
                {
                    DestroyGame();
                    CreateGame();
                    base.OnSurfaceCreated(holder);
                });
            }

            public override void OnSurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
            {
                Log.Try("OnSurfaceChanged", () =>
                {
                    if (IsCurrentGame)
                    {
                        Game.ClearInput();
                        Xna.Game.SurfaceWidth = width;
                        Xna.Game.SurfaceHeight = height;
                        base.OnSurfaceChanged(holder, format, width, height);
                    }
                    else
                        base.OnSurfaceChanged(holder, format, width, height);
                });
            }

            public override void OnSurfaceDestroyed(ISurfaceHolder holder)
            {
                Log.Try("OnSurfaceDestroyed", () =>
                {
                    if (IsCurrentGame)
                        DestroyGame();
                    base.OnSurfaceDestroyed(holder);
                });
            }


            public void OnSharedPreferenceChanged(ISharedPreferences p, string key)
            {
                Log.Try("OnSharedPreferenceChanged", () =>
                {
                    if (!PreferenceActivityIsActive || Game == null) return;
                    Service.ApplyPreferences(p);
                    AndroidGameActivity.DoResumed();
                });
            }


            private bool IsFirstShowing = true;
            public override void OnVisibilityChanged(bool visible)
            {
                Log.Try("OnVisibilityChanged " + visible, () =>
                {
                    if (!visible)
                    {
                        if (IsCurrentGame)
                            AndroidGameActivity.DoPaused();
                    }
                    else if (MyGame != Game)
                    {
                        //Log.Try("OnSurfaceCreated", () => OnSurfaceCreated(SurfaceHolder));
                        //Log.Try("Window.SurfaceCreated", () => ((ISurfaceHolderCallback)Game.Window).SurfaceCreated(SurfaceHolder));
                        //return;

                        //Log.Try("SurfaceHolder.Surface.Hide", () => SurfaceHolder.Surface.Hide());
                        //Log.Try("SurfaceHolder.Surface.Show", () => SurfaceHolder.Surface.Show());

                        throw new Exception();
                    }
                    else
                    {
                        if (IsFirstShowing)
                        {
                            //Log += "FirstShowing";
                            Service.ApplyPreferences(Preferences);
                            IsFirstShowing = false;
                            //Log--;
                        }
                        Game.ClearInput();
                        Game.StartFade();
                        AndroidGameActivity.DoResumed();
                        Log &= "Active: " + Game.IsActive;
                    }

                    base.OnVisibilityChanged(visible);
                });
            }


            private TimeSpan priorTapTime;
            private int tapCount;
            public override Android.OS.Bundle OnCommand(string action, int x, int y, int z, Android.OS.Bundle extras, bool resultRequested)
            {
                if (action == "android.wallpaper.tap")
                {
                    var now = Game.GameTime.TotalGameTime;
                    if ((now - priorTapTime).TotalMilliseconds < 300)
                        tapCount++;
                    else
                        tapCount = 1;
                    priorTapTime = Game.GameTime.TotalGameTime;

                    if (tapCount >= 3)
                    {
                        tapCount = 0;
                        Log.Try("Edit Preferences", () => Service.ShowSettings());
                    }
                }
                return base.OnCommand(action, x, y, z, extras, resultRequested);
            }


            public override void OnOffsetsChanged(float xOffset, float yOffset, float xOffsetStep, float yOffsetStep, int xPixelOffset, int yPixelOffset)
            {
                if (xOffsetStep <= .01f) return;
                //Log.Try("OnOffsetsChanged", () =>
                //{
                if (Game == null) return;
                Game.ClearInput();
                Game.UsePageOffset = true;
                Game.PageOffset = xOffset;
                Game.PageOffsetStep = xOffsetStep;
                //});
            }

            private Vector2 touchPrior;
            // Store the position of the touch event so we can use it for drawing later
            public override void OnTouchEvent(MotionEvent e)
            {
                if (Game == null || Game.UsePageOffset || !IsCurrentGame) return;

                //Log.Try("OnTouchEvent", () =>
                //{
                if (e.Action == MotionEventActions.Move)
                {
                    var pos = new Vector2(e.GetX(), e.GetY());
                    Game.CustomCursorOffset = touchPrior != Vector2.Zero ? pos - touchPrior : Vector2.Zero;
                    touchPrior = pos;
                }
                else
                    touchPrior = Vector2.Zero;

                base.OnTouchEvent(e);
                //});
            }


        }

    }


}
