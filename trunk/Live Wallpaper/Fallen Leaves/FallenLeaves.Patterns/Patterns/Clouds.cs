using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;


namespace FallenLeaves
{
    partial class FallenLeavesPattern
    {
        public static Clouds whiteClouds;
        public static Clouds grayClouds;
        public static Clouds darkClouds;
        public static Clouds farClouds;
        public static Clouds nearClouds;

        public static void CreateClouds()
        {
            whiteClouds = new Clouds
            {
                TextureNames = "cloud/white01/cloud01, cloud/white01/cloud02, cloud/white01/cloud03, cloud/white01/cloud04, cloud/white01/cloud05, cloud/white01/cloud06, cloud/white01/cloud07, cloud/white01/cloud08, cloud/white01/cloud09, cloud/white01/cloud10",
                BaseHeight = 256,
                MinScale = .5f,
                MaxScale = 1f,
                Width = 2f,
            };

            grayClouds = new Clouds
            {
                TextureNames = "cloud/gray01/cloud21, cloud/gray01/cloud22, cloud/gray01/cloud23, cloud/gray01/cloud24, cloud/gray01/cloud25, cloud/gray01/cloud26",
                BaseHeight = 256,
                MinScale = .5f,
                MaxScale = 1,
                Width = 2f,
            };

            darkClouds = new Clouds
            {
                TextureNames = "cloud/dark01/cloud31, cloud/dark01/cloud32, cloud/dark01/cloud33, cloud/dark01/cloud34, cloud/dark01/cloud35",
                BaseHeight = 256,
                MinScale = .5f,
                MaxScale = 1f,
                Width = 2f,
            };


            farClouds = new Clouds
            {
                Width = 1.7f,
                Speed = .3f,
                Top = .2f,
                Bottom = .7f,
                MinScale = .3f,
                MaxScale = .4f,
            };

            nearClouds = new Clouds
            {
                Width = 2f,
                Speed = .5f,
                Top = 0f,
                Bottom = .8f,
                MinScale = .6f,
                MaxScale = .7f,
            };
        }
    }
}
