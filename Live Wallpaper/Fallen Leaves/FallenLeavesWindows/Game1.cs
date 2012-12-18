#region Using Statements
using System;
using System.Collections.Generic;
using KamGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace FallenLeaves
{
    public class Game1 : Game2D
    {

        public Game1(): base()
        {
            Content.RootDirectory = "Content";

#if WINDOWS
            Graphics.PreferredBackBufferWidth = 1000;
            Graphics.PreferredBackBufferHeight = 620;
            //Graphics.PreferredBackBufferWidth = 620;
            //Graphics.PreferredBackBufferHeight = 1000;
#endif
#if ANDROID
            //Graphics.IsFullScreen = true,
            Graphics.SupportedOrientations =
                DisplayOrientation.LandscapeLeft |
                    DisplayOrientation.LandscapeRight |
                    DisplayOrientation.Portrait;
#endif

        }

        protected override void Initialize()
        {
            TouchPanel.EnabledGestures = GestureType.FreeDrag;
            base.Initialize();

            var theme = Theme.Load(this, @"Autumn01\big");
            theme.StartScene("scene01");
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

            base.DoUpdate();
        }

        protected override void DoDraw()
        {
            GraphicsDevice.Clear(Color.Black);

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
