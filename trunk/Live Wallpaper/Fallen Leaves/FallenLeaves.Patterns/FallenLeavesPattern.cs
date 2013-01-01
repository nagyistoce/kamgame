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
                    scene.Layers.Add(new Clouds(whiteClouds) { Density = 3, Speed = .5f, Top = -.15f, Bottom = 0.5f, MinScale = 0.2f, MaxScale = .5f, });
                    break;
                case 3:
                    scene.BlackColor = new Color(18, 9, 0);
                    scene.Layers.Add(sky3);
                    scene.Layers.Add(new Clouds(darkClouds) { Density = 10, Speed = .3f, Top = 0.05f, Bottom = 0.9f, MinScale = .18f, MaxScale = .24f, Opacity = .8f });
                    scene.Layers.Add(new Clouds(darkClouds) { Density = 3, Speed = .5f, Top = -.25f, Bottom = 1f, MinScale = .3f, MaxScale = .5f, Opacity = 1.0f });
                    break;
                case 4:
                    scene.BlackColor = new Color(17, 6, 6);
                    scene.Layers.Add(sky4);
                    scene.Layers.Add(new Clouds(grayClouds, farClouds) { Density = 4, Opacity = .8f });
                    scene.Layers.Add(new Clouds(grayClouds, nearClouds) { Density = 2, Opacity = .95f });
                    break;
            }


            scene.Layers.Add(wind1);
            //switch (windId)
            //{
            //    case 0: scene.Layers.Add(wind0); break;
            //    case 1: scene.Layers.Add(wind1); break;
            //    case 2: scene.Layers.Add(wind2); break;
            //    case 3: scene.Layers.Add(wind3); break;
            //    case 4: scene.Layers.Add(wind4); break;
            //    case 5: scene.Layers.Add(wind5); break;
            //}

            scene.Layers.Add(land6);

            switch (layoutId)
            {
                case 0:
                    scene.Layers.Add(new Tree(tree1) { Left = 1.3f, Right = 1.4f, Bottom = 0.04f, });
                    scene.Layers.Add(new Tree(tree2) { Left = 1.55f, Right = 1.15f, Bottom = 0.03f, });
                    break;
                case 1:
                    scene.Layers.Add(new Tree(tree2) { Left = 1.3f, Right = 1.4f, Bottom = 0.04f, });
                    scene.Layers.Add(new Tree(tree1) { Left = 1.55f, Right = 1.15f, Bottom = 0.03f, });
                    break;
                case 2:
                    scene.Layers.Add(new Tree(tree1) { Left = 1.0f, Right = 1.7f, Bottom = 0.04f, });
                    scene.Layers.Add(new Tree(tree2) { Left = 1.85f, Right = 0.85f, Bottom = 0.03f, });
                    break;
            }

            return scene;
        }

    }

}
