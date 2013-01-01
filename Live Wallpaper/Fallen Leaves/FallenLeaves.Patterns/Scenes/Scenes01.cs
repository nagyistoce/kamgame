﻿using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{
    partial class FallenLeavesPattern
    {
        public Scene scene1;
        public Scene scene2;
        public Scene scene3;

        public void CreateScenes()
        {
            #region Scenes

            scene1 = new Scene
            {
                Width = 3,
                BlackColor = new Color(17, 6, 6),
                Layers =
                {
                    sky1,
                    new Clouds(grayClouds, farClouds) { Density = 4, Opacity = .8f },
                    new Clouds(grayClouds, nearClouds) { Density = 2, Opacity = .95f },
                    wind1,
                    land6,
                    new Tree(tree1) { Left = 1.3f, Right = 1.4f, Bottom = 0.04f, },
                    new Tree(tree2) { Left = 1.55f, Right = 1.15f, Bottom = 0.03f, },
                }
            };


            //scene2 = new Scene
            //{
            //    Width = 4,
            //    BlackColor = new Color(6, 6, 17),
            //    Layers =
            //    {
            //        new Sky {Width = 1.5f, TextureNames = "sky/back02", Stretch = true,},
            //        new Clouds(whiteClouds) {Density = 3, Speed = .5f, Top = -.15f, Bottom = 0.5f, MinScale = 0.2f, MaxScale = .5f,},
            //        wind1,
            //        land6,
            //        new Tree(tree1) {Left = 2.1f, Right = 1.4f, Bottom = 0.04f,},
            //        new Tree(tree2) {Left = 1.4f, Right = 2.1f, Bottom = 0.03f,},
            //    }
            //};

            //scene3 = new Scene
            //{
            //    Width = 4,
            //    BlackColor = new Color(18, 9, 0),
            //    Layers =
            //    {
            //        new Sky {Width = 1.5f, TextureNames = "sky/back03_1, sky/back03_2", RowCount = 2},
            //        new Clouds(darkClouds) {Density = 10, Speed = .3f, Top = 0.05f, Bottom = 0.9f, MinScale = .18f, MaxScale = .24f, Opacity = .8f},
            //        new Clouds(darkClouds) {Density = 3, Speed = .5f, Top = -.25f, Bottom = 1f, MinScale = .3f, MaxScale = .5f, Opacity = 1.0f},
            //        wind1,
            //        land5,
            //        new Tree(tree1) {Left = 1.7f, Right = 1.8f, Bottom = 0.04f,},
            //        new Tree(tree2) {Left = 1.95f, Right = 1.55f, Bottom = 0.03f,},
            //    }
            //};

            #endregion
        }
    }
}