﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KamGame.Wallpapers;


namespace FallenLeaves
{
    partial class FallenLeavesPattern
    {
        public Ground land5;
        public Ground land6;

        public void CreateGrounds()
        {
            land5 = new Ground
            {
                TextureNames = "ground/land5",
                RepeatX = 7,
                Heights = new[] { 85, 45, 90, 170, 170, 170, 170, 160, 170, 115, 78, 108, 170, 195, 185, 128 },
                Grasses = { grass11, grass12 },
            };
            land6 = new Ground
            {
                TextureNames = "ground/land6",
                RepeatX = 5,
                Heights = new[] { 175, 200, 200, 189, 177, 144, 84, 112, 176, 202, 180, 144, 156, 190, 208, 209 }.Select(a=> a).ToArray(),
                Grasses = { grass11, grass12 },
            };
        }
    }
}