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
                ChangeSpeedPeriod = 1500,
                MaxSpeedFactor = 200,
                MinAmplitude = 0.1f,
                MaxAmplitude = .7f,
                AmplitureScatter = .3f,
                MinChangeAmplitudePeriod = 200,
                MaxChangeAmplitudePeriod = 700,
                AmplitudeStep = 0.005f
            };

            var wind1_max = new Wind(wind1)
            {
                ChangeSpeedPeriod = 10000,
                MaxSpeedFactor = 20,
                MinAmplitude = 1,
                MaxAmplitude = 1,
                AmplitureScatter = 0,
                MinChangeAmplitudePeriod = 10000,
                AmplitudeStep = 1000,
            };

            //Шаблон ветра, наследующийся от wind1. Более резко стабилизируется 
            var wind1_fast = new Wind(wind1)
            {
                AmplitudeStep = 0.02f
            };

            #endregion


            #region Clouds

            var whiteClouds = new Clouds
            {
                TextureNames = "cloud01,cloud02,cloud03,cloud04,cloud05,cloud06,cloud07,cloud08,cloud09,cloud10",
                BaseHeight = 256,
                MinScale = .5f,
                MaxScale = 1f
            };
            var grayClouds = new Clouds
            {
                TextureNames = "cloud21,cloud22,cloud23,cloud24,cloud25",
                BaseHeight = 256,
                MinScale = .5f,
                MaxScale = 1,
            };

            var darkClouds = new Clouds
            {
                TextureNames = "cloud31,cloud32,cloud33,cloud34,cloud35",
                BaseHeight = 256,
                MinScale = .5f,
                MaxScale = 1f,
            };


            var farClouds = new Clouds
            {
                Width = 1.7f,
                Speed = .3f,
                Top = .1f,
                Bottom = .7f,
                MinScale = .3f,
                MaxScale = .5f,
                Opacity = .7f,
            };

            var nearClouds = new Clouds
            {
                Width = 2f,
                Speed = .5f,
                Top = -.5f,
                Bottom = 1f,
                MinScale = .7f,
                MaxScale = .7f,
                Opacity = .9f
            };

            #endregion


            #region FallenLeafs

            var fallenLeafs1 = new FallenLeafs
            {
                TextureNames = "leaf1,leaf2,leaf3,leaf4,leaf5",
                minScale = .015f,
                maxScale = .020f,
                speedX = 6f,
                speedY = 4f,
                minAngleSpeed = .03f,
                maxAngleSpeed = .06f,
                MinSwirlRadius = 10f,
                MaxSwirlRadius = 150,
                opacity = .75f,
                Windage = .75f,
                EnterOpacityPeriod = 40f,
                EnterRadius = 40,
            };

            var fallenLeafs2 = new FallenLeafs
            {
                TextureNames = "leaf11,leaf12,leaf13",
                minScale = .013f,
                maxScale = .016f,
                speedX = 6f,
                speedY = 4f,
                minAngleSpeed = .03f,
                maxAngleSpeed = .06f,
                MinSwirlRadius = 10,
                MaxSwirlRadius = 100,
                opacity = .75f,
                Windage = .85f,
                EnterOpacityPeriod = 40,
                EnterRadius = 60,
            };

            #endregion


            #region TreeNodes

            var trunk1 = new TreeNode
            {
                maxAngle = .55f,
                K0 = .0015f,
                K0w = .00003f,
                K0p = 50,
                K1 = 0.00002f,
                K2 = .0005f,
                minK3 = 0.000015f,
                maxK3 = 0.000025f,
                minK3p = 500,
                maxK3p = 900,
                K4 = .015f,
                K5 = .0004f,
            };

            var stick1 = new TreeNode
            {
                maxAngle = .55f,
                K0 = .0015f,
                K0w = .00003f,
                K0p = 50,
                K1 = 0.00004f,
                K2 = .0005f,
                minK3 = 0.000015f,
                maxK3 = 0.000025f,
                minK3p = 300,
                maxK3p = 500,
                K4 = .015f,
                K5 = .0004f,
            };

            var leafs1 = new TreeNode
            {
                maxAngle = .55f,
                K0 = .0015f,
                K0w = .00003f,
                K0p = 50,
                K1 = 0.00008f,
                K2 = .0002f,
                minK3 = 0.000015f,
                maxK3 = 0.000025f,
                minK3p = 150,
                maxK3p = 200,
                K4 = .01f,
                K5 = .0002f,
            };

            var leafs2 = new TreeNode
            {
                maxAngle = .25f,
                K0 = .0015f,
                K0w = .00003f,
                K0p = 50,
                K1 = 0.00008f,
                K2 = .0002f,
                minK3 = 0.000015f,
                maxK3 = 0.000025f,
                minK3p = 150,
                maxK3p = 200,
                K4 = .01f,
                K5 = .0002f,
            };

            #endregion


            #region Trees

            var tree1 = new Tree
            {
                Width = 0.5f,
                BaseHeight = 1024,
                Leafs = { Pattern = fallenLeafs1, EnterPoint = new Vector2(-120, 200), MaxEnterCount = 20, },
                Nodes =
                {
                    new TreeNode(trunk1) {
                        TextureName = "tree01_tree1", 
                        BeginPoint = new Vector2(775, 890f),
                        Nodes = 
                        {
                            // Ветка (левая) 
                            new TreeNode(stick1)
                            {
                                TextureName = "tree01_stick1",
                                ParentPoint = new Vector2(310, 385f),
                                BeginPoint = new Vector2(785, 125f),
                                EndPoint = new Vector2(145, 195f),
                            },
                            // Ветка (левая)
                            new TreeNode(stick1)
                            {
                                TextureName = "tree01_stick2",
                                ParentPoint = new Vector2(310, 385f),
                                BeginPoint = new Vector2(490, 235f),
                                EndPoint = new Vector2(65, 140f),
                            },
                            // Ветка (листья сверху)
                            new TreeNode(leafs1)
                            {
                                TextureName = "tree01_leafs1",
                                ParentPoint = new Vector2(460, 490f),
                                BeginPoint = new Vector2(565, 725f),
                                EndPoint = new Vector2(390, 390f),
                            },
                            // Ветка (листья справа)
                            new TreeNode(leafs1)
                            {
                                TextureName = "tree01_stick3",
                                ParentPoint = new Vector2(685, 515f),
                                BeginPoint = new Vector2(220, 450f),
                                EndPoint = new Vector2(335, 100f),
                            },
                        },
                    },
                }
            };

            var tree2 = new Tree
            {
                Width = 0.5f,
                BaseHeight = 1400,
                Leafs = { Pattern = fallenLeafs2, EnterPoint = new Vector2(50, 150f), MaxEnterCount = 40, },
                Nodes =
                {
                    new TreeNode(trunk1) 
                    { 
                        TextureName = "tree02_tree2", 
                        BeginPoint = new Vector2(150, 960), 
                        Nodes =
                        {
                            new TreeNode(stick1)
                            {
                                TextureName = "tree02_stick3", 
                                ParentPoint = new Vector2(233, 740f), 
                                BeginPoint = new Vector2(845, 885f), 
                                EndPoint = new Vector2(625, 470f),
                            },
                            new TreeNode(stick1)
                            {
                                TextureName = "tree02_stick6", 
                                ParentPoint = new Vector2(395, 555f), 
                                BeginPoint = new Vector2(305, 875f), 
                                EndPoint = new Vector2(404, 745),
                                Nodes =
                                {
                                    new TreeNode(stick1)
                                    {
                                        TextureName = "tree02_stick5", 
                                        ParentPoint = new Vector2(600, 600f), 
                                        BeginPoint = new Vector2(88, 67f), 
                                        EndPoint = new Vector2(140,135f),
                                    },    
                                },
                            },
                            new TreeNode(leafs2)
                            {
                                TextureName = "tree02_leafs", 
                                ParentPoint = new Vector2(290, 380f), 
                                BeginPoint = new Vector2(590, 765f), 
                                EndPoint = new Vector2(465, 140f),
                            },
                        }
                    },
                },
            };


            #endregion


            #region Grasses

            var grass1 = new Grass
            {
                minRotation = -.175f,
                maxRotation = .45f,
                maxAngle = .45f,
                opacity = 1f,
                K0 = 0f,
                K0w = .004f,
                K0p = 15,
                minK1 = .004f,
                minK2 = .15f,
                minK3 = .00055f,
                minK3p = 4,
                minK4 = .022f,
                minK5 = .025f,
                maxK1 = .006f,
                maxK2 = .25f,
                maxK3 = .00075f,
                maxK3p = 6,
                maxK4 = .026f,
                maxK5 = .035f,
            };

            var grass11 = new Grass
            {
                Pattern = grass1,
                TextureNames = "grass1a, grass1b, grass1c",
                Density = 100,
                BeginPoint = new Vector2(12, 120),
                MinScale = 0.075f,
                MaxScale = 0.1f,
            };

            var grass12 = new Grass
            {
                Pattern = grass1,
                TextureNames = "grass2, grass3, grass4, grass5",
                Density = 100,
                BeginPoint = new Vector2(32, 125),
                MinScale = 0.09f,
                MaxScale = 0.15f,
            };


            #endregion


            #region Grounds

            var land5 = new Ground
            {
                TextureNames = "land5",
                RepeatX = 7,
                Heights = new[] { 85, 45, 90, 170, 170, 170, 170, 160, 170, 115, 78, 108, 170, 195, 185, 128 },
                Grasses = { grass11, grass12 },
            };
            var land6 = new Ground
            {
                TextureNames = "land6",
                RepeatX = 7,
                Heights = new[] { 175, 200, 200, 189, 177, 144, 84, 112, 176, 202, 180, 144, 156, 190, 208, 209 },
                Grasses = { grass11, grass12 },
            };

            #endregion

            #endregion


            #region Scene 1

            var scene1 = new Scene
            {
                Width = 4,
                Layers =
                {
                    new Sky { Width = 1.5f, TextureNames = "back04" },
                    new Clouds(grayClouds, farClouds) { Density = 4, Opacity = .7f },
                    new Clouds(grayClouds, nearClouds) { Density = 4, Opacity = .9f },
                    wind1,
                    land6,
                    new Tree(tree1) { Left = 1.7f, Right = 1.8f, Bottom = 0.04f, },
                    new Tree(tree2) { Left = 1.95f, Right = 1.55f, Bottom = 0.03f, },
                }
            };

            #endregion


            CurrentTheme = new Theme(this, "Autumn01/big", scene1);
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