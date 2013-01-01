using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{
    partial class FallenLeavesGame
    {
        private Tree tree1;
        private Tree tree2;

        private void CreateTrees()
        {
            CreateTree1();
            CreateTree2();
        }

        private void CreateTree1()
        {
            tree1 = new Tree
            {
                Width = 0.5f,
                BaseHeight = 1024,
                Leafs = { Pattern = fallenLeafs1, EnterPoint = new Vector2(-160, 200), MaxEnterCount = 25, },
                Nodes =
                {
                    new TreeNode(trunk1)
                    {
                        TextureName = "tree/tree01/tree01_tree1",
                        BeginPoint = new Vector2(775, 890f),
                        Nodes =
                        {
                            // Ветка (левая) 
                            new TreeNode(stick1)
                            {
                                TextureName = "tree/tree01/tree01_stick1",
                                ParentPoint = new Vector2(310, 385f),
                                BeginPoint = new Vector2(785, 125f),
                                EndPoint = new Vector2(145, 195f),
                            },
                            // Ветка (левая)
                            new TreeNode(stick1)
                            {
                                TextureName = "tree/tree01/tree01_stick2",
                                ParentPoint = new Vector2(310, 385f),
                                BeginPoint = new Vector2(490, 235f),
                                EndPoint = new Vector2(65, 140f),
                            },
                            // Ветка (листья сверху)
                            new TreeNode(leafs1)
                            {
                                TextureName = "tree/tree01/tree01_leafs1",
                                ParentPoint = new Vector2(460, 490f),
                                BeginPoint = new Vector2(565, 725f),
                                EndPoint = new Vector2(390, 390f),
                            },
                            // Ветка (листья справа)
                            new TreeNode(leafs1)
                            {
                                TextureName = "tree/tree01/tree01_stick3",
                                ParentPoint = new Vector2(685, 515f),
                                BeginPoint = new Vector2(220, 450f),
                                EndPoint = new Vector2(335, 100f),
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
                BaseHeight = 1400,
                Leafs = {Pattern = fallenLeafs2, EnterPoint = new Vector2(50, 120f), MaxEnterCount = 40,},
                Nodes =
                {
                    new TreeNode(trunk1)
                    {
                        TextureName = "tree/tree02/tree02_tree2",
                        BeginPoint = new Vector2(150, 960),
                        Nodes =
                        {
                            new TreeNode(stick1)
                            {
                                TextureName = "tree/tree02/tree02_stick3",
                                ParentPoint = new Vector2(233, 740f),
                                BeginPoint = new Vector2(845, 885f),
                                EndPoint = new Vector2(625, 470f),
                            },
                            new TreeNode(stick1)
                            {
                                TextureName = "tree/tree02/tree02_stick6",
                                ParentPoint = new Vector2(395, 555f),
                                BeginPoint = new Vector2(305, 875f),
                                EndPoint = new Vector2(404, 745),
                                Nodes =
                                {
                                    new TreeNode(stick1)
                                    {
                                        TextureName = "tree/tree02/tree02_stick5",
                                        ParentPoint = new Vector2(600, 600f),
                                        BeginPoint = new Vector2(88, 67f),
                                        EndPoint = new Vector2(140, 135f),
                                    },
                                },
                            },
                            new TreeNode(leafs2)
                            {
                                TextureName = "tree/tree02/tree02_leafs",
                                ParentPoint = new Vector2(290, 380f),
                                BeginPoint = new Vector2(590, 765f),
                                EndPoint = new Vector2(465, 140f),
                            },
                        }
                    },
                },
            };
        }
    }
}
