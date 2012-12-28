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
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
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

        protected abstract Game NewGame();

        public class GameEngine : Engine
        {
            protected Func<Game> NewGame;
            public Game Game { get; private set; }
            public readonly WallpaperService Service;
            private ScreenReceiver screenReceiver;

            public GameEngine(WallpaperService wall, Func<Game> newGame)
                : base(wall)
            {
                if (wall == null)
                    throw new ArgumentNullException("wall");
                if (newGame == null)
                    throw new ArgumentNullException("newGame");
                Service = wall;
                NewGame = newGame;
            }

            public override void OnCreate(ISurfaceHolder holder)
            {
                base.OnCreate(holder);

                var filter = new IntentFilter();
                filter.AddAction(Intent.ActionScreenOff);
                filter.AddAction(Intent.ActionScreenOn);
                filter.AddAction(Intent.ActionUserPresent);

                screenReceiver = new ScreenReceiver();
                Service.RegisterReceiver(screenReceiver, filter);

                //SetTouchEventsEnabled(true);
            }

            public override void OnDestroy()
            {
                Service.UnregisterReceiver(screenReceiver);
                Game.Context = null;
                Game.CustomHolder = null;
                base.OnDestroy();
            }

            public override void OnVisibilityChanged(bool visible)
            {
                if (visible)
                    Resume();
                else
                    Pause();
            }

            public override void OnSurfaceCreated(ISurfaceHolder holder)
            {
                if (Game == null)
                {
                    Game.Context = Service;
                    Game.CustomHolder = SurfaceHolder;
                    Game = NewGame();
                    Game.Run();
                    Resume();
                }

                base.OnSurfaceCreated(holder);
            }

            public override void OnSurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
            {
                base.OnSurfaceChanged(holder, format, width, height);
                //center.Set(width / 2.0f, height / 2.0f);
                //center.Set(width / 2.0f, height / 2.0f);
                //DrawFrame();
            }

            public override void OnSurfaceDestroyed(ISurfaceHolder holder)
            {
                Game.Exit();
                Game = null;
                base.OnSurfaceDestroyed(holder);
            }

            protected void Pause()
            {
                AndroidGameActivity.DoPaused();
            }

            protected void Resume()
            {
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
        where TGame : Game, new()
    {
        protected override Game NewGame()
        {
            return new TGame();
        }
    }


}