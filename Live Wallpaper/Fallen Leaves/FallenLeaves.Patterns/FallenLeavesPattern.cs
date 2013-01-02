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


        public Scene NewScene(int skyId, int layoutId, int windId, int fallenLeafsCount)
        {
            var scene = new Scene { Width = 3, };

            switch (skyId)
            {
                case 2:
                    scene.BlackColor = new Color(6, 6, 17);
                    scene.Layers.Add(sky2);
                    scene.Layers.Add(new Clouds(whiteClouds) { Density = 3, Speed = .5f, Top = 0, Bottom = 0.3f, MinScale = 0.2f, MaxScale = .7f, });
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


            scene.Layers.Add(winds[windId]);

            scene.Layers.Add(land6);

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
            }

            FallenLeafs.MaxEnterCountFactor = .5f + .5f * fallenLeafsCount;

            return scene;
        }

    }

}
