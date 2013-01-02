using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;


namespace FallenLeaves
{
    partial class FallenLeavesPattern
    {
        public FallenLeafs fallenLeafs1;
        public FallenLeafs fallenLeafs2;

        public void CreateFallenLeafs()
        {
            fallenLeafs1 = new FallenLeafs
            {
                TextureNames = "tree/tree01/leaf1, tree/tree01/leaf2, tree/tree01/leaf3, tree/tree01/leaf4, tree/tree01/leaf5",
                MinScale = .022f,
                MaxScale = .026f,
                SpeedX = 7f,
                AccelerationY = 4f,
                MinAngleSpeed = .005f,
                MaxAngleSpeed = .025f,
                MinSwirlRadius = 10f,
                MaxSwirlRadius = 150,
                Opacity = .9f,
                Windage = .75f,
                EnterOpacityPeriod = 20,
                EnterRadius = 40,
            };

            fallenLeafs2 = new FallenLeafs
            {
                TextureNames = "tree/tree02/leaf11, tree/tree02/leaf12, tree/tree02/leaf13",
                MinScale = .015f,
                MaxScale = .017f,
                SpeedX = 6f,
                AccelerationY = 4f,
                MinAngleSpeed = .005f,
                MaxAngleSpeed = .025f,
                MinSwirlRadius = 5,
                MaxSwirlRadius = 100,
                Opacity = .9f,
                Windage = .85f,
                EnterOpacityPeriod = 20,
                EnterRadius = 40,
            };
        }
    }
}
