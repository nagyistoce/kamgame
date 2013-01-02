using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KamGame.Wallpapers;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{

    public partial class FallenLeavesPattern
    {

        public FallenLeavesPattern()
        {
            CreateSkys();
            CreateWinds();
            CreateClouds();
            CreateFallenLeafs();
            CreateTreeNodes();
            CreateTrees();
            CreateGrasses();
            CreateGrounds();
        }


        public Scene NewScene(
            int skyId = 4, float cloudsCount = 1,
            int windId = 0, int windDirection = 0, bool windShow = false,
            float grassCount = 1,
            int layoutId = 0,
            float fallenLeafsCount = 1, float fallenLeafsScale = 1
        )
        {
            var scene = new Scene { Width = 3, };

            switch (skyId)
            {
                case 2:
                    scene.BlackColor = new Color(6, 6, 17);
                    scene.Layers.Add(sky2);
                    scene.Layers.Add(new Clouds(whiteClouds) { Density = 5, Speed = .5f, Top = .2f, Bottom = 0.3f, MinScale = 0.2f, MaxScale = .7f, });
                    break;
                case 3:
                    scene.BlackColor = new Color(18, 9, 0);
                    scene.Layers.Add(sky3);
                    scene.Layers.Add(new Clouds(darkClouds, farClouds)
                    {
                        Density = 10,
                        MinScale = .2f,
                        MaxScale = .3f,
                    });
                    scene.Layers.Add(new Clouds(darkClouds, nearClouds)
                    {
                        Density = 3,
                        MinScale = .4f,
                        MaxScale = .5f,
                    });
                    break;
                case 4:
                    scene.BlackColor = new Color(17, 6, 6);
                    scene.Layers.Add(sky4);
                    scene.Layers.Add(new Clouds(grayClouds, farClouds) { Density = 4, Opacity = .9f });
                    scene.Layers.Add(new Clouds(grayClouds, nearClouds) { Density = 2, Opacity = .95f });
                    break;
            }

            Clouds.DensityFactor = cloudsCount;

            scene.Layers.Add(new Wind(winds[windId]) { Direction = windDirection });
            Wind.ShowBar = windShow;

            scene.Layers.Add(land6);
            Grass.DensityFactor = grassCount;

            switch (layoutId)
            {
                case 0:
                    scene.Layers.Add(new Tree(tree1) { Left = 1.3f, Right = 1.4f, Bottom = 0.04f, });
                    scene.Layers.Add(new Tree(tree2) { Left = 1.55f, Right = 1.15f, Bottom = 0.03f, });
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
                    scene.Layers.Add(new Tree(tree1) { Left = 1.0f, Right = 1.7f, Bottom = 0.03f, BaseHeight = 1024 });
                    scene.Layers.Add(new Tree(tree2) { Left = 1.1f, Right = 1.6f, Bottom = 0.03f, });
                    scene.Layers.Add(new Tree(tree2) { Left = 1.6f, Right = 1.1f, Bottom = 0.03f, UseFlip = true });
                    scene.Layers.Add(new Tree(tree2) { Left = 1.4f, Right = 1.3f, Bottom = 0.03f, BaseHeight = 1300, UseFlip = true });
                    scene.Layers.Add(new Tree(tree1) { Left = 1.7f, Right = 1.0f, Bottom = 0.03f, BaseHeight = 1024, UseFlip = true, });
                    break;
            }

            FallenLeafs.EnterCountFactor = fallenLeafsCount;
            FallenLeafs.ScaleFactor = fallenLeafsScale;

            return scene;
        }

    }

}
