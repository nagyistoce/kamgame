using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KamGame;
using KamGame.Wallpapers;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{

    public partial class FallenLeavesPattern
    {
        private static bool IsCreated;
        public static void CreateAll()
        {
            CreateWinds();
            CreateClouds();
            CreateSkys();
            CreateFallenLeafs();
            CreateTreeNodes();
            CreateTrees();
            CreateGrasses();
            CreateGrounds();
            IsCreated = true;
        }


        public static Action Updating;
        public static Action RecreateScene;
        public static TimeSpan Auto1_NextTime;
        public static TimeSpan[] Auto1_Times =
        {
            //new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 10), new TimeSpan(0, 0, 16), new TimeSpan(0, 0, 22)
            new TimeSpan(5, 0, 0), new TimeSpan(10, 0, 0), new TimeSpan(16, 0, 0), new TimeSpan(22, 0, 0)
        };

        public static string[][] Auto1_Skys =
        {
            new[] { "sky6" }, new[] { "sky4", "sky5" }, new[] { "sky2" }, new[] { "sky3" }
        };

        public static Scene NewScene(
            int textureQuality = 0,
            string skyId = "sky4", float cloudsCount = 1,
            int windId = 0, int windDirection = 0,
            float grassCount = 1,
            int layoutId = 0,
            float fallenLeafsCount = 1, float fallenLeafsScale = 1
        )
        {

            if (textureQuality != Scene.TextureQuality)
            {
                IsCreated = false;
                Scene.TextureQuality = textureQuality;
                TreeSizeFactor = textureQuality == 1 ? 2 : 1;
                GrassSizeFactor = textureQuality == 1 ? 1 : .5f;
            }

            if (!IsCreated)
                CreateAll();

            var scene = new Scene { Width = 3, };

            if (skyId == "sky4a")
                skyId = "sky4";
            else if (skyId == "auto1")
            {
                var now = DateTime.Now.TimeOfDay;
                //now = new TimeSpan(0, 0, now.Seconds % 24);
                skyId = Auto1_Skys[0][GameBase.Random.Next(Auto1_Skys[0].Length)];
                Auto1_NextTime = Auto1_Times[0];
                for (int i = 0, len = Auto1_Times.Length; i < len; i++)
                {
                    if (now >= Auto1_Times[i]) continue;
                    skyId = Auto1_Skys[i][GameBase.Random.Next(Auto1_Skys[i].Length)];
                    Auto1_NextTime = Auto1_Times[i];
                    break;
                }

                Updating = () =>
                {
                    var now2 = DateTime.Now.TimeOfDay;
                    //now2 = new TimeSpan(0, 0, now2.Seconds % 24);
                    if (RecreateScene != null && now2 > Auto1_NextTime)
                    {
                        RecreateScene();
                    }
                };
            }

            var sky = Skys[skyId];
            scene.BlackColor = sky.BlackColor;
            scene.Layers.Add(sky);
            // ReSharper disable RedundantEnumerableCastCall
            scene.Layers.AddRange(sky.Clouds.Cast<Layer>());
            // ReSharper restore RedundantEnumerableCastCall

            Clouds.DensityFactor = cloudsCount;

            scene.Layers.Add(new Wind(winds[windId]) { Direction = windDirection });

            scene.Layers.Add(land6);
            Grass.DensityFactor = grassCount;

            switch (layoutId)
            {
                case 0:
                    scene.Layers.Add(new Tree(tree1) { Left = 1.3f, Right = 1.4f, Bottom = 0.04f, });
                    //scene.Layers.Add(new Tree(tree1) { Left = 1.3f, Right = 1.4f, Bottom = 0.04f, Width = .25f, });
                    scene.Layers.Add(new Tree(tree2) { Left = 1.55f, Right = 1.15f, Bottom = 0.03f, });
                    //scene.Layers.Add(new Stone(stone1) { Left = .5f, Right = 2.5f, Bottom = 0.03f, });
                    //scene.Layers.Add(new Stone(stone2) { Left = 1.0f, Right = 2.0f, Bottom = 0.03f, });
                    //scene.Layers.Add(new Stone(stone3) { Left = 1.5f, Right = 1.5f, Bottom = 0.03f, });
                    //scene.Layers.Add(new Stone(stone4) { Left = 2.0f, Right = 1.0f, Bottom = 0.03f, });
                    break;
                case 1:
                    scene.Layers.Add(new Tree(tree2) { Left = 1.1f, Right = 1.6f, Bottom = 0.04f, });
                    scene.Layers.Add(new Tree(tree1) { Left = 1.8f, Right = 0.9f, Bottom = 0.03f, });
                    break;
                case 2:
                    scene.Layers.Add(new Tree(tree2) { Left = .3f, Right = 2.4f, Bottom = 0.04f, });
                    scene.Layers.Add(new Tree(tree1) { Left = 2.4f, Right = 0.3f, Bottom = 0.03f, });
                    break;
                case 3:
                    scene.Layers.Add(new Tree(tree1) { Left = 1.0f, Right = 1.7f, Bottom = 0.03f, Width = .25f });
                    scene.Layers.Add(new Tree(tree2) { Left = 1.1f, Right = 1.6f, Bottom = 0.03f, });
                    scene.Layers.Add(new Tree(tree2) { Left = 1.6f, Right = 1.1f, Bottom = 0.03f, UseFlip = true });
                    scene.Layers.Add(new Tree(tree2) { Left = 1.4f, Right = 1.3f, Bottom = 0.03f, Width = .25f, UseFlip = true });
                    scene.Layers.Add(new Tree(tree1) { Left = 1.7f, Right = 1.0f, Bottom = 0.03f, Width = .25f, UseFlip = true, });
                    break;
            }

            FallenLeafs.EnterCountFactor = fallenLeafsCount;
            FallenLeafs.ScaleFactor = fallenLeafsScale;

            return scene;
        }

    }

}
