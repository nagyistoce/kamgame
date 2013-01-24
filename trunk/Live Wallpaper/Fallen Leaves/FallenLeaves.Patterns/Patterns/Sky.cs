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
            var blueSky1 = new Sky
            {
                BlackColor = new Color(6, 6, 17),
                Clouds= {new Clouds(whiteClouds) { Density = 5, Speed = .5f, Top = .2f, Bottom = 0.3f, MinScale = 0.2f, MaxScale = .7f, }},
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
            var darkSky1 = new Sky
            {
                BlackColor = new Color(18, 9, 0),
                Clouds =
                {
                    new Clouds(darkClouds, farClouds) { Density = 10, MinScale = .2f, MaxScale = .3f, },
                    new Clouds(darkClouds, nearClouds) { Density = 3, MinScale = .4f, MaxScale = .5f, },
                },
            };


            Skys["sky2"] = new Sky(blueSky1) { Width = 1.5f, TextureNames = "sky/back02", Stretch = true, };
            Skys["sky3"] = new Sky(darkSky1) { Width = 1.5f, TextureNames = "sky/back03", BaseVScale = 1.5f };
            Skys["sky4"] = new Sky(orangeSky1) { Width = 1.5f, TextureNames = "sky/back04", BaseVScale = 1.5f };
            Skys["sky4a"] = new Sky(orangeSky1) { Width = 1.5f, TextureNames = "sky/back04a", BaseVScale = 1.5f };
            Skys["sky5"] = new Sky(orangeSky1) { Width = 1.5f, TextureNames = "sky/back05", BaseVScale = 1.5f };
            Skys["sky6"] = new Sky(orangeSky1) { Width = 1.5f, TextureNames = "sky/back06", BaseVScale = 1.5f };
        }
    }
}
