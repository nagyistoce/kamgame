#region Using Statements
using System;
using System.Collections.Generic;
using KamGame;
using KamGame.Wallpaper;
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
    public class FallenLeavesGame : Game2D
    {

        public FallenLeavesGame()
            : base()
        {
            // TODO TargetElapsedTime = TimeSpan.FromTicks(333333);
            Content.RootDirectory = "Content";

#if WINDOWS
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 800;

            //Graphics.PreferredBackBufferWidth = 1000;
            //Graphics.PreferredBackBufferHeight = 620;
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


        private Theme CurrentTheme;
        private Scene CurrentScene;

        protected override void Initialize()
        {
            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.Tap;
            base.Initialize();

            //CurrentTheme = Theme.Load(this, @"Autumn01/big");
            //CurrentScene = CurrentTheme.Scenes[0];
            //CurrentScene.Start();


            #region Patterns


            #region Winds

            var wind1 = new Wind
            {
                changeSpeedPeriod = 1500,
                maxSpeedFactor = 200,
                minAmplitude = 0.1f,
                maxAmplitude = .7f,
                amplitureScatter = .3f,
                minChangeAmplitudePeriod = 200,
                maxChangeAmplitudePeriod = 700,
                amplitudeStep = 0.005f
            };
            var wind1_max = new Wind
            {
                Pattern = wind1,
                changeSpeedPeriod = 10000,
                maxSpeedFactor = 20,
                minAmplitude = 1,
                maxAmplitude = 1,
                amplitureScatter = 0,
                minChangeAmplitudePeriod = 10000,
                amplitudeStep = 1000,
            };

            //Шаблон ветра, наследующийся от wind1. Более резко стабилизируется 
            var wind1_fast = new Wind { Pattern = wind1, amplitudeStep = 0.02f };

            #endregion


            var whiteClouds = new Clouds
            {
                TextureNames = "cloud01,cloud02,cloud03,cloud04,cloud05,cloud06,cloud07,cloud08,cloud09,cloud10",
                Width = 2,
                BaseHeight = 256,
                MinScale = .5f,
                MaxScale = 1f
            };
            var grayClouds = new Clouds
            {
                TextureNames = "cloud21,cloud22,cloud23,cloud24,cloud25",
                Width = 2,
                BaseHeight = 256,
                MinScale = .5f,
                MaxScale = 1,
            };

            #endregion


            #region Scene 1

            var scene1 = new Scene
            {
                Width = 4,
                Layers =
                {
                    new Sky { Width = 1.5f, TextureNames = "back04" },
                    new Clouds
                    {
                        Pattern = grayClouds, 
                        Density = 4, 
                        Speed = .3f, 
                        Top = -.3f, Bottom = .8f, 
                        MinScale = .3f, MaxScale=.5f, 
                        Opacity=.8f
                    },
                    new Clouds
                    {
                        Pattern = grayClouds, 
                        Density = 4, 
                        Speed = .5f, 
                        Top = -.5f, Bottom = .9f, 
                        MinScale = .7f, MaxScale=.7f, 
                        Opacity = .8f
                    },
                }
            };

            #endregion


            CurrentTheme = new Theme(this, "Autumn01/big", scene1);

            scene1.Start();
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
