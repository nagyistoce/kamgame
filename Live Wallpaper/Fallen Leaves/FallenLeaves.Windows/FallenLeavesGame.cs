#region Using Statements

using System;
using KamGame;
using KamGame.Wallpapers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#endregion


namespace FallenLeaves
{

    public partial class FallenLeavesGame : Game2D
    {
        public FallenLeavesGame()
        {
            TargetFramesPerSecond = 30;

#if WINDOWS
            //Graphics.PreferredBackBufferWidth = 1280; Graphics.PreferredBackBufferHeight = 800;
            Graphics.PreferredBackBufferWidth = 1000; Graphics.PreferredBackBufferHeight = 620;
            //Graphics.PreferredBackBufferWidth = 620; Graphics.PreferredBackBufferHeight = 1000;
#endif
#if ANDROID
            //Graphics.IsFullScreen = true,
            Graphics.SupportedOrientations =
                DisplayOrientation.LandscapeLeft
                | DisplayOrientation.LandscapeRight
                | DisplayOrientation.Portrait;
#endif
        }


        public Theme CurrentTheme;
        public Scene CurrentScene;
        private bool IsInitialized;

        protected override void Initialize()
        {
            if (UseTouch)
                TouchPanel.EnabledGestures = GestureType.FreeDrag;

            base.Initialize();
            if (CurrentScene != null)
                CurrentScene.Start();
            IsInitialized = true;
        }

        public void StartScene(string themeId, Scene scene)
        {
            if (CurrentScene != null)
            {
                CurrentScene.Stop();
                //CurrentScene = null;
                //CurrentTheme = null;
                //Content.Unload();
                //GC.Collect();
            }

            CurrentScene = scene;
            CurrentTheme = new Theme(this, themeId, CurrentScene);

            if (IsInitialized)
                CurrentScene.Start();

            Scene.UnloadTextures(this);
        }


        protected override void DoLoadContent()
        {
            DefaultFont = Content.Load<SpriteFont>("spriteFont1");
            base.DoLoadContent();
        }

        //protected override void DoUpdate()
        //{
        //    //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        //    //    Exit();

        //    if (CursorIsClicked && CursorPosition.Y < ScreenHeight / 4)
        //    {
        //        CurrentScene.Stop();
        //        CurrentScene = CurrentScene.Next();
        //        CurrentScene.Start();
        //    }

        //    base.DoUpdate();
        //}

        protected override void DoDraw()
        {
            GraphicsDevice.Clear(Color.Black);
            base.DoDraw();
        }

    }


}