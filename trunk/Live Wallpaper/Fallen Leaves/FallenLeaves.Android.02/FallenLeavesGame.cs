#region Using Statements

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
            //Graphics.PreferredBackBufferWidth = 1280;
            //Graphics.PreferredBackBufferHeight = 800;

            Graphics.PreferredBackBufferWidth = 1000; Graphics.PreferredBackBufferHeight = 620;
            //Graphics.PreferredBackBufferWidth = 620; Graphics.PreferredBackBufferHeight = 1000;
#endif
#if ANDROID
    //Graphics.IsFullScreen = true,
            Graphics.SupportedOrientations =
                DisplayOrientation.LandscapeLeft |
                    DisplayOrientation.LandscapeRight |
                    DisplayOrientation.Portrait;
#endif
        }


        private Theme CurrentTheme;
        private Scene CurrentScene;

        protected override void Initialize()
        {
            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.Tap;
            base.Initialize();

            CreateWinds();
            CreateClouds();
            CreateFallenLeafs();
            CreateTreeNodes();
            CreateTrees();
            CreateGrasses();
            CreateGrounds();
            
            CreateScenes();

            CurrentTheme = new Theme(this, "Autumn01", scene1, scene2, scene3);
            CurrentScene = scene1;
            CurrentScene.Start();
        }


        protected override void DoLoadContent()
        {
            DefaultFont = Content.Load<SpriteFont>("spriteFont1");
            base.DoLoadContent();
        }
        
        protected override void DoUpdate()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (CursorIsClicked && CursorPosition.Y < ScreenHeight / 4)
            {
                CurrentScene.Stop();
                CurrentScene = CurrentScene.Next();
                CurrentScene.Start();
            }

            base.DoUpdate();
        }

        protected override void DoDraw()
        {
            GraphicsDevice.Clear(Color.Blue);

            DrawString("qqq", 100, 100);

            base.DoDraw();

            //DrawString(spriteFont, 
            //    "CursorOffset = " + CursorOffset + "\n"+
            //    "MouseState = " + MouseState.X + ", " + MouseState.Y + "\n" +
            //    "MouseIsMoved = " + MouseIsMoved
            //);

            //DrawString(spriteFont,
            //    "Gesture Count = " + Gestures.Count
            //    , Vector2.Zero, Color.Black
            //);

            //var i = 0;
            //foreach (var g in Gestures)
            //{
            //    DrawString(spriteFont,
            //        "Type = " + g.GestureType + "\r\n" +
            //        "Delta = (" + g.Delta.X + ", " + g.Delta.Y + ")\r\n" +
            //        "Delta2 = (" + g.Delta2.X + ", " + g.Delta2.Y + ")\r\n" +
            //        "Position = (" + g.Position.X + ", " + g.Position.Y + ")\r\n" +
            //        "Position2 = (" + g.Position2.X + ", " + g.Position2.Y + ")\r\n"
            //        , new Vector2(100 * i++, 30), Color.Black
            //    );
            //}
        }
    }


}