using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{
    partial class FallenLeavesGame
    {
        private Grass grass1;
        private Grass grass11;
        private Grass grass12;

        private void CreateGrasses()
        {
            grass1 = new Grass
            {
                MinRotation = -.175f,
                MaxRotation = .45f,
                MaxAngle = .45f,
                Opacity = 1f,
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

            grass11 = new Grass
            {
                Pattern = grass1,
                TextureNames = @"grass\grass1a, grass\grass1b, grass\grass1c",
                Density = 100,
                BeginPoint = new Vector2(12, 120),
                MinScale = 0.075f,
                MaxScale = 0.1f,
            };

            grass12 = new Grass
            {
                Pattern = grass1,
                TextureNames = @"grass\grass2, grass\grass3, grass\grass4, grass\grass5",
                Density = 100,
                BeginPoint = new Vector2(32, 125),
                MinScale = 0.09f,
                MaxScale = 0.15f,
            };
        }
    }
}
