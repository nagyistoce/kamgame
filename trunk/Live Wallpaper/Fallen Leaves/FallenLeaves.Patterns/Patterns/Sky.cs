using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{
    partial class FallenLeavesPattern
    {
        public static SortedDictionary<string, Sky> Skys = new SortedDictionary<string, Sky>();

        public static void CreateSkys()
        {
            var blueSky2 = new Sky
            {
                BlackColor = new Color(6, 6, 17),
                CloudColor = new Color(235, 232, 213),
                Clouds = { new Clouds(whiteClouds) { Density = 8, Speed = .5f, Top = 0f, Bottom = 0.7f, MinScale = 0.2f, MaxScale = .7f, } },
            };
            var orangeSky1 = new Sky
            {
                BlackColor = new Color(17, 6, 6),
                Clouds =
                {
                    new Clouds(grayClouds, farClouds) { Density = 4 },
                    new Clouds(grayClouds, nearClouds) { Density = 2 },
                },
            };
            var darkSky3 = new Sky
            {
                BlackColor = new Color(18, 9, 0),
                Clouds =
                {
                    new Clouds(darkClouds, farClouds) { Density = 10, MinScale = .2f, MaxScale = .3f, },
                    new Clouds(darkClouds, nearClouds) { Density = 3, MinScale = .4f, MaxScale = .5f, },
                },
            };

            var darkBlueSky1 = new Sky
            {
                BlackColor = new Color(6, 6, 17),
                CloudColor = new Color(20, 50, 90),
                Clouds =
                {
                    new Clouds(whiteClouds, farClouds) { Density = 10, },
                    new Clouds(whiteClouds, nearClouds) { Density = 3, },
                },
            };



            Skys["sky2"] = new Sky(blueSky2) { Width = 1.5f, TextureNames = "sky/back02", BaseVScale = 1.5f, };
            Skys["sky3"] = new Sky(darkSky3) { Width = 1.5f, TextureNames = "sky/back03", BaseVScale = 1.5f };
            Skys["sky4"] = new Sky(orangeSky1) { Width = 1.5f, TextureNames = "sky/back04", BaseVScale = 1.5f };
            Skys["sky4a"] = new Sky(orangeSky1) { Width = 1.5f, TextureNames = "sky/back04a", BaseVScale = 1.5f };
            Skys["sky5"] = new Sky(orangeSky1) { Width = 1.5f, TextureNames = "sky/back05", BaseVScale = 1.5f };
            Skys["sky6"] = new Sky(darkBlueSky1) { Width = 1.5f, TextureNames = "sky/back06", BaseVScale = 1.5f };
            Skys["sky7"] = new Sky
            {
                Width = 1.5f, TextureNames = "sky/back07", BaseVScale = 1.5f,
                BlackColor = new Color(6, 6, 17),
                CloudColor = new Color(220, 229, 243),
                Clouds = { new Clouds(whiteClouds, farClouds) { Density = 10, Top= -.1f }, },
            };
            Skys["sky8"] = new Sky(darkSky3) { Width = 1.5f, TextureNames = "sky/back08", BaseVScale = 1.5f, };
        }
    }
}
