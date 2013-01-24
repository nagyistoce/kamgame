using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;


namespace FallenLeaves
{
    partial class FallenLeavesPattern
    {
        public static FallenLeafs fallenLeafs1;
        public static FallenLeafs fallenLeafs2;
        public static LeafRegion smallLeafRegion;
        public static LeafRegion bigLeafRegion;

        public static void CreateFallenLeafs()
        {
            fallenLeafs1 = new FallenLeafs
            {
                TextureNames = "tree/tree01/leaf1, tree/tree01/leaf2, tree/tree01/leaf3, tree/tree01/leaf4, tree/tree01/leaf5",
                MinScale = .016f,
                MaxScale = .024f,
                SpeedX = 6f,
                AccelerationY = 7f,
                MinAngleSpeed = .005f,
                MaxAngleSpeed = .020f,
                MinSwirlRadius = 50f,
                MaxSwirlRadius = 150,
                Opacity = .9f,
                Windage = .75f,
            };

            fallenLeafs2 = new FallenLeafs
            {
                TextureNames = "tree/tree02/leaf11, tree/tree02/leaf12, tree/tree02/leaf13",
                MinScale = .013f,
                MaxScale = .018f,
                SpeedX = 6f,
                AccelerationY = 7f,
                MinAngleSpeed = .005f,
                MaxAngleSpeed = .020f,
                MinSwirlRadius = 5,
                MaxSwirlRadius = 100,
                Opacity = .9f,
                Windage = .85f,
            };

            smallLeafRegion = new LeafRegion { MaxEnterCount = 3, MinEnterPeriod = 200, MaxEnterPeriod = 1000 };
            bigLeafRegion = new LeafRegion { MaxEnterCount = 10, MaxEnterPeriod = 500 };
        }
    }
}
