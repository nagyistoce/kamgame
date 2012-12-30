using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;


namespace FallenLeaves
{
    partial class FallenLeavesGame
    {
        private FallenLeafs fallenLeafs1;
        private FallenLeafs fallenLeafs2;

        private void CreateFallenLeafs()
        {
            fallenLeafs1 = new FallenLeafs
            {
                TextureNames = "tree/tree01/leaf1, tree/tree01/leaf2, tree/tree01/leaf3, tree/tree01/leaf4, tree/tree01/leaf5",
                MinScale = .018f,
                MaxScale = .024f,
                SpeedX = 6f,
                AccelerationY = 4f,
                MinAngleSpeed = .03f,
                MaxAngleSpeed = .06f,
                MinSwirlRadius = 10f,
                MaxSwirlRadius = 150,
                Opacity = .75f,
                Windage = .75f,
                EnterOpacityPeriod = 20,
                EnterRadius = 40,
            };

            fallenLeafs2 = new FallenLeafs
            {
                TextureNames = "tree/tree02/leaf11, tree/tree02/leaf12, tree/tree02/leaf13",
                MinScale = .015f,
                MaxScale = .019f,
                SpeedX = 6f,
                AccelerationY = 4f,
                MinAngleSpeed = .03f,
                MaxAngleSpeed = .06f,
                MinSwirlRadius = 10,
                MaxSwirlRadius = 100,
                Opacity = .75f,
                Windage = .85f,
                EnterOpacityPeriod = 20,
                EnterRadius = 60,
            };
        }
    }
}
