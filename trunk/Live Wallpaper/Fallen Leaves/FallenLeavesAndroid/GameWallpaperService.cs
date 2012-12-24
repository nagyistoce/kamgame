﻿/*
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
using Android.Graphics;
using Android.OS;
using Android.Service.Wallpaper;
using Android.Views;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{
    [Service(Label = "@string/ApplicationName", Permission = "android.permission.BIND_WALLPAPER")]
    [IntentFilter(new[] { "android.service.wallpaper.WallpaperService" })]
    [MetaData("android.service.wallpaper", Resource = "@xml/cube1")]
    internal class GameWallpaperService : WallpaperService
    {
        public override Engine OnCreateEngine() { return new GameEngine(this); }

        public class GameEngine : Engine
        {

            //private readonly Handler mHandler = new Handler();

            //private readonly Paint paint = new Paint();
            //private readonly PointF center = new PointF();
            //private readonly PointF touch_point = new PointF(-1, -1);
            //private float offset;
            //private readonly long start_time;

            //private readonly Action mDrawCube;
            //private bool is_visible;

            Game Game;
            public WallpaperService Service { get; private set; }

            public GameEngine(WallpaperService wall): base(wall) { Service = wall; }

            public override void OnCreate(ISurfaceHolder holder)
            {
                base.OnCreate(holder);
                // By default we don't get touch events, so enable them.
                //SetTouchEventsEnabled(true);
            }

            public override void OnDestroy()
            {
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
                    AndroidGameWindow.CustomHolder = SurfaceHolder;
                    Game = new Game1();
                    Game.Run();
                    Resume();
                }

                base.OnSurfaceCreated(holder);
            }

            public override void OnSurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
            {
                base.OnSurfaceChanged(holder, format, width, height);

                //// store the center of the surface, so we can draw the cube in the right spot
                //center.Set(width / 2.0f, height / 2.0f);

                // store the center of the surface, so we can draw the cube in the right spot
                //center.Set(width / 2.0f, height / 2.0f);

                //DrawFrame();
            }

            public override void OnSurfaceDestroyed(ISurfaceHolder holder)
            {
                Game.Exit();
                Game = null;
                base.OnSurfaceDestroyed(holder);
            }

            protected  void Pause()
            {
                AndroidGameActivity.DoPaused();
            }


            protected void Resume()
            {
                AndroidGameActivity.DoResumed();

                //var deviceManager = (IGraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager));
                //if (deviceManager == null)
                //    return;
                //var graphicsDeviceManager = deviceManager as GraphicsDeviceManager;
                //if (graphicsDeviceManager != null)
                //    graphicsDeviceManager.ForceSetFullScreen();
                //Game.Window.RequestFocus();
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

            // Draw one frame of the animation. This method gets called repeatedly
            // by posting a delayed Runnable. You can do any drawing you want in
            // here. This example draws a wireframe cube.
            //private void DrawFrame()
            //{
            //    //var holder = this.SurfaceHolder;
            //    //holder.


            //    //var holder = SurfaceHolder;

            //    //Canvas c = null;

            //    //try
            //    //{
            //    //    c = holder.LockCanvas();

            //    //    if (c != null)
            //    //    {
            //    //        //DrawCube(c);
            //    //        DrawTouchPoint(c);
            //    //    }
            //    //}
            //    //finally
            //    //{
            //    //    if (c != null)
            //    //        holder.UnlockCanvasAndPost(c);
            //    //}

            //    //// Reschedule the next redraw
            //    //mHandler.RemoveCallbacks(mDrawCube);

            //    //if (is_visible)
            //    //    mHandler.PostDelayed(mDrawCube, 1000 / 25);
            //}

            //// Draw a wireframe cube by drawing 12 3 dimensional lines between
            //// adjacent corners of the cube
            //private void DrawCube(Canvas c)
            //{
            //    c.Save();
            //    c.Translate(center.X, center.Y);
            //    c.DrawColor(Color.Black);

            //    DrawLine(c, -400, -400, -400, 400, -400, -400);
            //    DrawLine(c, 400, -400, -400, 400, 400, -400);
            //    DrawLine(c, 400, 400, -400, -400, 400, -400);
            //    DrawLine(c, -400, 400, -400, -400, -400, -400);

            //    DrawLine(c, -400, -400, 400, 400, -400, 400);
            //    DrawLine(c, 400, -400, 400, 400, 400, 400);
            //    DrawLine(c, 400, 400, 400, -400, 400, 400);
            //    DrawLine(c, -400, 400, 400, -400, -400, 400);

            //    DrawLine(c, -400, -400, 400, -400, -400, -400);
            //    DrawLine(c, 400, -400, 400, 400, -400, -400);
            //    DrawLine(c, 400, 400, 400, 400, 400, -400);
            //    DrawLine(c, -400, 400, 400, -400, 400, -400);

            //    c.Restore();
            //}

            //// Draw a 3 dimensional line on to the screen
            //private void DrawLine(Canvas c, int x1, int y1, int z1, int x2, int y2, int z2)
            //{
            //    var now = SystemClock.ElapsedRealtime();
            //    var xrot = ((float)(now - start_time)) / 1000;
            //    var yrot = (0.5f - offset) * 2.0f;
            //    //float zrot = 0;

            //    // 3D transformations

            //    // rotation around X-axis
            //    var newy1 = (float)(Math.Sin(xrot) * z1 + Math.Cos(xrot) * y1);
            //    var newy2 = (float)(Math.Sin(xrot) * z2 + Math.Cos(xrot) * y2);
            //    var newz1 = (float)(Math.Cos(xrot) * z1 - Math.Sin(xrot) * y1);
            //    var newz2 = (float)(Math.Cos(xrot) * z2 - Math.Sin(xrot) * y2);

            //    // rotation around Y-axis
            //    var newx1 = (float)(Math.Sin(yrot) * newz1 + Math.Cos(yrot) * x1);
            //    var newx2 = (float)(Math.Sin(yrot) * newz2 + Math.Cos(yrot) * x2);
            //    newz1 = (float)(Math.Cos(yrot) * newz1 - Math.Sin(yrot) * x1);
            //    newz2 = (float)(Math.Cos(yrot) * newz2 - Math.Sin(yrot) * x2);

            //    // 3D-to-2D projection
            //    var startX = newx1 / (4 - newz1 / 400);
            //    var startY = newy1 / (4 - newz1 / 400);
            //    var stopX = newx2 / (4 - newz2 / 400);
            //    var stopY = newy2 / (4 - newz2 / 400);

            //    c.DrawLine(startX, startY, stopX, stopY, paint);
            //}

            // Draw a circle around the current touch point, if any.
            //private void DrawTouchPoint(Canvas c)
            //{
            //    if (touch_point.X >= 0 && touch_point.Y >= 0)
            //        c.DrawCircle(touch_point.X, touch_point.Y, 80, paint);
            //}
        }
    }
}