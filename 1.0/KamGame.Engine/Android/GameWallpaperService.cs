using System;
using System.Globalization;
using Android.Content;
using Android.Graphics;
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
        public override Engine OnCreateEngine()
        {
            return new GameEngine(this, NewGame);
        }

        protected abstract GameBase NewGame();


        public class GameEngine : Engine
        {
            protected Func<GameBase> NewGame;
            public readonly WallpaperService Service;

            private LogWriter Log;

            public GameEngine(WallpaperService wall, Func<GameBase> newGame)
                : base(wall)
            {
#if DEBUG
                Log = new LogWriter("KamGame.GameWallpaper", () =>
                    (MyGame == null ? -1 : MyGame.InstanceIndex) + "/" + GameBase.InstanceCount + "    "
                );
#endif
                Log += "constructor";
                if (wall == null)
                    throw new ArgumentNullException("wall");
                if (newGame == null)
                    throw new ArgumentNullException("newGame");
                Service = wall;
                NewGame = newGame;
                Log--;
            }

            public static GameBase Game { get; private set; }
            public GameBase MyGame { get; private set; }
            public bool IsCurrentGame { get { return MyGame != null && !MyGame.IsDisposed && Game == MyGame; } }

            private ScreenReceiver screenReceiver;

            public override void OnCreate(ISurfaceHolder holder)
            {
                Log += "OnCreate";
                base.OnCreate(holder);

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
                base.OnDestroy();
                Log--;
            }


            private void CreateGame()
            {
                Log += "Create Game";
                Xna.Game.Context = Service;
                Xna.Game.CustomHolder = SurfaceHolder;

                Game = MyGame = NewGame();
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

            public override void OnVisibilityChanged(bool visible)
            {
                if (IsCurrentGame)
                {
                    Log += "OnVisibilityChanged " + visible;
                    if (visible)
                        AndroidGameActivity.DoResumed();
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

                Log &= e.Action.ToString();

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
        protected override GameBase NewGame()
        {
            return new TGame();
        }
    }


}