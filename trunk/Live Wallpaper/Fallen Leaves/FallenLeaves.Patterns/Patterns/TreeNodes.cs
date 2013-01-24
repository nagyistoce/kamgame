using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;


namespace FallenLeaves
{
    partial class FallenLeavesPattern
    {
        public static TreeNode trunk1;
        public static TreeNode stick1;
        public static TreeNode leafs1;
        public static TreeNode leafs2;

        public static void CreateTreeNodes()
        {
            trunk1 = new TreeNode
            {
                maxAngle = .55f,
                K0 = .0015f,
                K0w = .00003f,
                K0p = 50,
                K1 = 0.00002f,
                K2 = .0005f,
                minK3 = 0.000015f,
                maxK3 = 0.000025f,
                minK3p = 500,
                maxK3p = 900,
                K4 = .015f,
                K5 = .0004f,
            };

            stick1 = new TreeNode
            {
                maxAngle = .55f,
                K0 = .0015f,
                K0w = .00003f,
                K0p = 50,
                K1 = 0.00004f,
                K2 = .0005f,
                minK3 = 0.000015f,
                maxK3 = 0.000025f,
                minK3p = 300,
                maxK3p = 500,
                K4 = .015f,
                K5 = .0004f,
            };

            leafs1 = new TreeNode
            {
                maxAngle = .55f,
                K0 = .0015f,
                K0w = .00003f,
                K0p = 50,
                K1 = 0.00008f,
                K2 = .0002f,
                minK3 = 0.000015f,
                maxK3 = 0.000025f,
                minK3p = 150,
                maxK3p = 200,
                K4 = .01f,
                K5 = .0002f,
            };

            leafs2 = new TreeNode
            {
                maxAngle = .25f,
                K0 = .0015f,
                K0w = .00003f,
                K0p = 50,
                K1 = 0.00008f,
                K2 = .0002f,
                minK3 = 0.000015f,
                maxK3 = 0.000025f,
                minK3p = 150,
                maxK3p = 200,
                K4 = .01f,
                K5 = .0002f,
            };
        }
    }
}
