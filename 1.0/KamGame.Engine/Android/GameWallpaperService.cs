/*
 * Copyright (C) 2009 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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

            public static GameBase Game { get; private set; }
            public GameBase MyGame { get; private set; }

            public readonly WallpaperService Service;
            private ScreenReceiver screenReceiver;

            public static int Count;
            private LogWriter Log = new LogWriter("KamGame.GameWallpaper", (++Count) + "    ");

            public GameEngine(WallpaperService wall, Func<GameBase> newGame)
                : base(wall)
            {
                Log += "constructor";
                if (wall == null)
                    throw new ArgumentNullException("wall");
                if (newGame == null)
                    throw new ArgumentNullException("newGame");
                Service = wall;
                NewGame = newGame;
                Log--;
            }

            public override void OnCreate(ISurfaceHolder holder)
            {
                Log += "OnCreate";
                base.OnCreate(holder);

                //var filter = new IntentFilter();
                //filter.AddAction(Intent.ActionScreenOff);
                //filter.AddAction(Intent.ActionScreenOn);
                //filter.AddAction(Intent.ActionUserPresent);

                //screenReceiver = new ScreenReceiver();
                //Service.RegisterReceiver(screenReceiver, filter);

                //SetTouchEventsEnabled(true);
                Log--;
            }

            public override void OnDestroy()
            {
                Log += "OnDestroy";

                if (Game != MyGame)
                    MyGame = null;
                else
                    DestroyGame();

                base.OnDestroy();

                Log--;
            }

            public override void OnSurfaceCreated(ISurfaceHolder holder)
            {
                Log += "OnSurfaceCreated";

                if (Game != null)
                    DestroyGame();

                Xna.Game.Context = Service;
                Xna.Game.CustomHolder = SurfaceHolder;
                Game = MyGame = NewGame();
                Game.Run();

                base.OnSurfaceCreated(holder);
                Log--;
            }

            private void DestroyGame()
            {
                if (Game == null) return;
                Log += "Destroy Game";
                lock (Game)
                {
                    Pause();
                    Game.Exit();
                    Game.Dispose();
                    Game = MyGame = null;
                }
                Xna.Game.Context = null;
                Xna.Game.CustomHolder = null;
                Log--;
            }

            public override void OnSurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
            {
                if (MyGame == null || Game != MyGame) return;
                Log += "OnSurfaceChanged";
                Xna.Game.SurfaceWidth = width;
                Xna.Game.SurfaceHeight = height;
                base.OnSurfaceChanged(holder, format, width, height);
                Log--;
            }

            public override void OnSurfaceDestroyed(ISurfaceHolder holder)
            {
                if (MyGame == null || Game != MyGame) return;
                Log += "OnSurfaceDestroyed";
                if (Game != null)
                {
                    Log += "Destroy Game";
                    Game.Exit();
                    MyGame = Game = null;
                    Log--;
                }
                base.OnSurfaceDestroyed(holder);
                Log--;
            }

            public override void OnVisibilityChanged(bool visible)
            {
                if (MyGame == null || Game != MyGame) return;
                Log += "OnVisibilityChanged " + visible;
                if (visible)
                    Resume();
                else
                    Pause();
                Log--;
            }

            protected void Pause()
            {
                if (MyGame == null || Game != MyGame) return;
                Log &= "Pause";
                AndroidGameActivity.DoPaused();
            }

            protected void Resume()
            {
                if (MyGame == null || Game != MyGame) return;
                Log &= "Resume";
                AndroidGameActivity.DoResumed();
            }



            //public override void OnOffsetsChanged(float xOffset, float yOffset, float xOffsetStep, float yOffsetStep,
            //    int xPixelOffset, int yPixelOffset)
            //{
            //    //offset = xOffset;

            //    //DrawFrame();
            //}

            //// Store the position of the touch event so we can use it for drawing later
            //public override void OnTouchEvent(MotionEvent e)
            //{
            //    if (e.Action == MotionEventActions.Move)
            //        touch_point.Set(e.GetX(), e.GetY());
            //    else
            //        touch_point.Set(-1, -1);

            //    base.OnTouchEvent(e);
            //}

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