using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{
    partial class FallenLeavesPattern
    {
        public Tree tree1;
        public Tree tree2;

        public void CreateTrees()
        {
            CreateTree1();
            CreateTree2();
        }

        public void CreateTree1()
        {
            tree1 = new Tree
            {
                Width = 0.5f,
                BaseHeight = 512,
                Leafs =
                {
                    Pattern = fallenLeafs1,
                    EnterPoint = new Vector2(-200, 200),
                    MaxEnterCount = 10,
                    EnterRadius = 30,
                    MinEnterPeriod = 50,
                    MaxEnterPeriod = 500,
                },
                Nodes =
                {
                    new TreeNode(trunk1)
                    {
                        TextureName = "tree/tree01/tree01_tree1",
                        BeginPoint = new Vector2(387, 445),
                        Nodes =
                        {
                            // Ветка (левая) 
                            new TreeNode(stick1)
                            {
                                TextureName = "tree/tree01/tree01_stick1",
                                ParentPoint = new Vector2(155, 192),
                                BeginPoint = new Vector2(392, 62),
                                EndPoint = new Vector2(72, 97),
                            },
                            // Ветка (левая)
                            new TreeNode(stick1)
                            {
                                TextureName = "tree/tree01/tree01_stick2",
                                ParentPoint = new Vector2(155, 192),
                                BeginPoint = new Vector2(245, 117),
                                EndPoint = new Vector2(33, 70),
                            },
                            // Ветка (левая нижняя)
                            new TreeNode(stick1)
                            {
                                TextureName = "tree/tree01/tree01_stick4",
                                ParentPoint = new Vector2(200, 250),
                                BeginPoint = new Vector2(220, 90),
                                EndPoint = new Vector2(80, 55),
                            },
                            // Ветка (листья сверху)
                            new TreeNode(leafs1)
                            {
                                TextureName = "tree/tree01/tree01_leafs1",
                                ParentPoint = new Vector2(230, 245),
                                BeginPoint = new Vector2(282, 362),
                                EndPoint = new Vector2(195, 195),
                            },
                            // Ветка (листья справа)
                            new TreeNode(leafs1)
                            {
                                TextureName = "tree/tree01/tree01_stick3",
                                ParentPoint = new Vector2(342, 257),
                                BeginPoint = new Vector2(110, 225),
                                EndPoint = new Vector2(167, 50),
                            },
                        },
                    },
                }
            };
        }

        private void CreateTree2()
        {
            tree2 = new Tree
            {
                Width = 0.5f,
                BaseHeight = 700,
                Leafs =
                {
                    Pattern = fallenLeafs2,
                    EnterPoint = new Vector2(50, 120f),
                    MaxEnterCount = 20,
                    EnterRadius = 30,
                    MinEnterPeriod = 50,
                    MaxEnterPeriod = 500,
                },
                Nodes =
                {
                    new TreeNode(trunk1)
                    {
                        TextureName = "tree/tree02/tree02_tree2",
                        BeginPoint = new Vector2(75, 480),
                        Nodes =
                        {
                            new TreeNode(stick1)
                            {
                                TextureName = "tree/tree02/tree02_stick3",
                                ParentPoint = new Vector2(116, 370),
                                BeginPoint = new Vector2(422, 442),
                                EndPoint = new Vector2(312, 235),
                            },
                            new TreeNode(stick1)
                            {
                                TextureName = "tree/tree02/tree02_stick6",
                                ParentPoint = new Vector2(197, 278),
                                BeginPoint = new Vector2(152, 437),
                                EndPoint = new Vector2(202, 372),
                                Nodes =
                                {
                                    new TreeNode(stick1)
                                    {
                                        TextureName = "tree/tree02/tree02_stick5",
                                        ParentPoint = new Vector2(300, 300),
                                        BeginPoint = new Vector2(44, 33),
                                        EndPoint = new Vector2(70, 62),
                                    },
                                },
                            },
                            new TreeNode(leafs2)
                            {
                                TextureName = "tree/tree02/tree02_leafs",
                                ParentPoint = new Vector2(145, 190),
                                BeginPoint = new Vector2(295, 382),
                                EndPoint = new Vector2(232, 70),
                            },
                        }
                    },
                },
            };
        }
    }
}
